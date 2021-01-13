using DALayer.Common;
using DALayer.Emails;
using DALayer.MPR;
using SCMModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;

namespace DALayer.ASN
{
	public class ASNDA : IASNDA
	{
		private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
		private IMPRDA mprDA = default(IMPRDA);
		private ErrorLog log = new ErrorLog();
		public ASNDA(IEmailTemplateDA EmailTemplateDA, IMPRDA mprDA)
		{
			this.emailTemplateDA = EmailTemplateDA;
			this.mprDA = mprDA;
		}

		VSCMEntities vscm = new VSCMEntities();
		YSCMEntities obj = new YSCMEntities();

		/*Name of Function : <<ASNInitiate>>  Author :<<Prasanna>>  
		Date of Creation <<18-12-2020>>
		Purpose : <<function is used to  get getAsnList>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public bool ASNInitiate(ASNInitiation asnASNInitiation)
		{
			try
			{
				int vendorid = Convert.ToInt32(asnASNInitiation.VendorId);
				//check vuserid exist or not if not exist adding credentials  in vendorusermaster tables
				List<string> EmailList = asnASNInitiation.VendorEmailId.Split(new char[] { ',' }).ToList();
				foreach (var item in EmailList)
				{
					Int32 sequenceNo = 0;
					string password = "";
					var value = "";
					RemoteVendorUserMaster vendorUsermaster = vscm.RemoteVendorUserMasters.Where(li => li.Vuserid == item && li.VendorId == asnASNInitiation.VendorId).FirstOrDefault();

					//need to implement vUniqueId value
					if (vendorUsermaster == null && !string.IsNullOrEmpty(item))
					{
						RemoteVendorUserMaster vendorUsermasters = new RemoteVendorUserMaster();
						sequenceNo = Convert.ToInt32(vscm.RemoteVendorUserMasters.Max(li => li.SequenceNo));
						if (sequenceNo == null || sequenceNo == 0)
							sequenceNo = 1;
						else
						{
							sequenceNo = sequenceNo + 1;
						}
						value = obj.SP_sequenceNumber(sequenceNo).FirstOrDefault();
						vendorUsermasters.VuniqueId = "C" + value;
						vendorUsermasters.SequenceNo = sequenceNo;
						vendorUsermasters.Vuserid = item.Replace(" ", String.Empty);
						password = this.mprDA.GeneratePassword();
						vendorUsermasters.pwd = password;
						vendorUsermasters.ContactNumber = null;
						vendorUsermasters.ContactPerson = null;
						vendorUsermasters.VendorId = vendorid;
						vendorUsermasters.Active = true;
						vendorUsermasters.SuperUser = true;
						vendorUsermasters.UpdatedBy = asnASNInitiation.InitiateFrom;
						vendorUsermasters.UpdatedOn = DateTime.Now;
						vscm.RemoteVendorUserMasters.Add(vendorUsermasters);
						vscm.SaveChanges();
					}
					VendorUserMaster Localvenmaster = obj.VendorUserMasters.Where(li => li.Vuserid == item && li.VendorId == vendorid).FirstOrDefault<VendorUserMaster>();
					if (Localvenmaster == null && !string.IsNullOrEmpty(item))
					{
						RemoteVendorUserMaster venmaster = vscm.RemoteVendorUserMasters.Where(li => li.Vuserid == item && li.VendorId == vendorid).FirstOrDefault<RemoteVendorUserMaster>();
						VendorUserMaster vendorUsermasters = new VendorUserMaster();
						vendorUsermasters.Vuserid = venmaster.Vuserid;
						vendorUsermasters.pwd = venmaster.pwd;
						vendorUsermasters.VendorId = venmaster.VendorId;
						vendorUsermasters.ContactNumber = venmaster.ContactNumber;
						vendorUsermasters.ContactPerson = venmaster.ContactPerson;
						vendorUsermasters.Active = true;
						vendorUsermasters.SuperUser = true;
						vendorUsermasters.VuniqueId = venmaster.VuniqueId;
						vendorUsermasters.SequenceNo = venmaster.SequenceNo;
						vendorUsermasters.UpdatedBy = venmaster.UpdatedBy;
						vendorUsermasters.UpdatedOn = DateTime.Now;
						obj.VendorUserMasters.Add(vendorUsermasters);
						obj.SaveChanges();
					}

				}
				ASNInitiation asnIniLocal = new ASNInitiation();
				asnIniLocal.VendorId = vendorid;
				asnIniLocal.VendorEmailId = asnASNInitiation.VendorEmailId;
				asnIniLocal.Remarks = asnASNInitiation.Remarks;
				asnIniLocal.InitiateFrom = asnASNInitiation.InitiateFrom;
				asnIniLocal.CreatedDate = DateTime.Now;
				obj.ASNInitiations.Add(asnIniLocal);
				obj.SaveChanges();
				this.emailTemplateDA.sendASNInitiationEmail(asnIniLocal);
			}
			catch (DbEntityValidationException e)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						log.ErrorMessage("ASNDA", "ASNInitiate", ve.ErrorMessage);
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}

			}
			return true;
		}

		/*Name of Function : <<getAsnList>>  Author :<<Prasanna>>  
		Date of Creation <<07-12-2020>>
		Purpose : <<function is used to  get getAsnList>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public List<ASNShipmentHeader> getAsnList(ASNfilters ASNfilters)
		{
			List<ASNShipmentHeader> asnList = new List<ASNShipmentHeader>();
			try
			{
				using (YSCMEntities Context = new YSCMEntities())
				{
					Context.Configuration.ProxyCreationEnabled = false;
					var query = default(string);
					query = "select * from ASNShipmentHeader where ( Deleteflag=1 or Deleteflag is null)";
					if (!string.IsNullOrEmpty(ASNfilters.ToDate))
						query += " and CreatedDate <= '" + ASNfilters.ToDate + "'";
					if (!string.IsNullOrEmpty(ASNfilters.FromDate))
						query += "  and CreatedDate >= '" + ASNfilters.FromDate + "'";
					if (!string.IsNullOrEmpty(ASNfilters.Vendorid))
						query += "  and VendorId = '" + ASNfilters.Vendorid + "'";
					if (!string.IsNullOrEmpty(ASNfilters.VendorName))
						query += "  and VendorName like'%" + ASNfilters.VendorName + "%'";
					if (!string.IsNullOrEmpty(ASNfilters.ASNNo))
						query += "  and ASNNo = '" + ASNfilters.ASNNo + "'";
					query += " order by ASNId desc ";
					asnList = Context.ASNShipmentHeaders.SqlQuery(query).ToList<ASNShipmentHeader>();
				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("ASNDA", "getAsnList", ex.Message + "; " + ex.StackTrace.ToString());
			}
			return asnList;
		}
		/*Name of Function : <<getAsnDetailsByAsnNo>>  Author :<<Prasanna>>  
		Date of Creation <<07-12-2020>>
		Purpose : <<function is used to  get getAsnDetailsByAsnNo>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public ASNShipmentHeader getAsnDetailsByAsnNo(int ASNId)
		{
			ASNShipmentHeader ASNDetails = new ASNShipmentHeader();
			try
			{
				ASNDetails = obj.ASNShipmentHeaders.Where(li => li.ASNId == ASNId).FirstOrDefault();
				foreach (ASNCommunication item in ASNDetails.ASNCommunications)
				{
					item.Employee = obj.VendorEmployeeViews.Where(li => li.EmployeeNo == item.RemarksFrom).FirstOrDefault();
				}

			}
			catch (Exception ex)
			{
				log.ErrorMessage("ASNDA", "getAsnDetailsByAsnNo", ex.Message + "; " + ex.StackTrace.ToString());
			}
			return ASNDetails;
		}

		/*Name of Function : <<GetInvoiceDetails>>  Author :<<Prasanna>>  
		Date of Creation <<14-12-2020>>
		Purpose : <<function is used GetI nvoiceDetails>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public InvoiceDetail GetInvoiceDetails(InvoiceDetail invoicedetails)
		{
			InvoiceDetail invDetails = new InvoiceDetail();
			try
			{
				invDetails = obj.InvoiceDetails.Where(li => li.InvoiceNo == invoicedetails.InvoiceNo && li.ASNId == invoicedetails.ASNId).FirstOrDefault();
				// invoiceDetails.

			}
			catch (Exception ex)
			{
				log.ErrorMessage("ASNDA", "GetInvoiceDetails", ex.Message + "; " + ex.StackTrace.ToString());
			}
			return invDetails;
		}
		/*Name of Function : <<updateASNComminications>>  Author :<<Prasanna>>  
		Date of Creation <<07-12-2020>>
		Purpose : <<function is used updateASNComminications>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public bool updateASNComminications(ASNCommunication asncom)
		{
			try
			{
				int ASNCCId = 0;
				//insert in vscm
				RemoteASNCommunication com = vscm.RemoteASNCommunications.Where(li => li.ASNCCId == asncom.ASNCCId).FirstOrDefault();
				if (com == null)
				{
					RemoteASNCommunication asnComRemote = new RemoteASNCommunication();
					asnComRemote.ASNId = asncom.ASNId;
					asnComRemote.Remarks = asncom.Remarks;
					asnComRemote.RemarksFrom = asncom.RemarksFrom;
					asnComRemote.RemarksDate = DateTime.Now;
					vscm.RemoteASNCommunications.Add(asnComRemote);
					vscm.SaveChanges();
					ASNCCId = asnComRemote.ASNCCId;
				}

				//inert in yscm
				ASNCommunication comLocal = obj.ASNCommunications.Where(li => li.ASNCCId == ASNCCId).FirstOrDefault();
				if (comLocal == null)
				{
					ASNCommunication asnComLocal = new ASNCommunication();
					asnComLocal.ASNCCId = ASNCCId;
					asnComLocal.ASNId = asncom.ASNId;
					asnComLocal.Remarks = asncom.Remarks;
					asnComLocal.RemarksFrom = asncom.RemarksFrom;
					asnComLocal.RemarksDate = DateTime.Now;
					obj.ASNCommunications.Add(asnComLocal);
					obj.SaveChanges();
				}
				this.emailTemplateDA.sendASNCommunicationMail(asncom.ASNId, asncom.Remarks, asncom.RemarksFrom);
			}
			catch (Exception ex)
			{
				log.ErrorMessage("ASNDA", "updateASNComminications", ex.Message + "; " + ex.StackTrace.ToString());
			}
			return true;
		}

	}
}
