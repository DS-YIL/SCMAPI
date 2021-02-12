﻿using BALayer.MPR;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;
using System.Configuration;
using SCMModels.RFQModels;
using System.Data.OleDb;
using System.Data;
using System.Globalization;
using System.Linq;
using DALayer.Emails;
using System.Data.Entity.Validation;
using DALayer.Common;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/MPR")]
	public class MPRController : ApiController
	{
		private readonly IMPRBA _mprBusenessAcess;
		private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
		private ErrorLog log = new ErrorLog();
		public MPRController(IMPRBA mprBA, IEmailTemplateDA EmailTemplateDA)
		{
			this._mprBusenessAcess = mprBA;
			this.emailTemplateDA = EmailTemplateDA;
		}

		[HttpPost]
		[Route("getDBMastersList")]
		public IHttpActionResult getDBMastersList([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.getDBMastersList(Result));
		}

		[HttpPost]
		[Route("addDataToDBMasters")]
		public IHttpActionResult addDataToDBMasters([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.addDataToDBMasters(Result));
		}

		[HttpPost]
		[Route("updateDataToDBMasters")]
		public IHttpActionResult updateDataToDBMasters([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.updateDataToDBMasters(Result));
		}

		[HttpPost]
		[Route("GetListItems")]
		public IHttpActionResult GetListItems([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.GetListItems(Result));
		}

		[HttpPost]
		[Route("UpdateMPR")]
		public IHttpActionResult updateMPR([FromBody] MPRRevision mpr)
		{
			return Ok(this._mprBusenessAcess.updateMPR(mpr));
		}

		[HttpPost]
		[Route("copyMprRevision/{repeatOrder}/{revise}")]
		public IHttpActionResult copyMprRevision([FromBody] MPRRevision mpr, bool repeatOrder, bool revise)
		{
			return Ok(this._mprBusenessAcess.copyMprRevision(mpr, repeatOrder, revise));
		}
		[HttpPost]
		[Route("addNewVendor")]
		public IHttpActionResult addNewVendor([FromBody] VendormasterModel vendor)
		{
			return Ok(this._mprBusenessAcess.addNewVendor(vendor));
		}
		[HttpPost]
		[Route("deleteMPRDocument")]
		public IHttpActionResult deleteMPRDocument([FromBody] MPRDocument mprDocument)
		{
			return Ok(this._mprBusenessAcess.deleteMPRDocument(mprDocument));
		}

		[HttpPost]
		[Route("deleteMPRItemInfo")]
		public IHttpActionResult deleteMPRItemInfo([FromBody] MPRItemInfo mprItemInfo)
		{
			return Ok(this._mprBusenessAcess.deleteMPRItemInfo(mprItemInfo));
		}

		[HttpPost]
		[Route("deleteMPRVendor")]
		public IHttpActionResult deleteMPRVendor([FromBody] MPRVendorDetail mprVendor)
		{
			return Ok(this._mprBusenessAcess.deleteMPRVendor(mprVendor));
		}

		[HttpPost]
		[Route("deleteMPRDocumentation")]
		public IHttpActionResult deleteMPRDocumentation([FromBody] MPRDocumentation MPRDocumentation)
		{
			return Ok(this._mprBusenessAcess.deleteMPRDocumentation(MPRDocumentation));
		}

		[HttpGet]
		[Route("getMPRRevisionDetails/{RevisionId}")]
		public IHttpActionResult getMPRRevisionDetails(int RevisionId)
		{
			return Ok(this._mprBusenessAcess.getMPRRevisionDetails(RevisionId));
		}

		[HttpPost]
		[Route("getMPRList")]
		public IHttpActionResult getMPRList(mprFilterParams mprfilterparams)
		{
			return Ok(this._mprBusenessAcess.getMPRList(mprfilterparams));
		}

		[HttpPost]
		[Route("getSavingsReport")]
		public IHttpActionResult getSavingsReport(mprFilterParams mprfilterparams)
		{
			return Ok(this._mprBusenessAcess.getSavingsReport(mprfilterparams));
		}

		[HttpPost]
		[Route("getMPRPendingListCnt/{preparedBy}")]
		public IHttpActionResult getMPRPendingListCnt(string preparedBy)
		{
			return Ok(this._mprBusenessAcess.getMPRPendingListCnt(preparedBy));
		}
		[HttpGet]
		[Route("getEmployeeList")]
		public IHttpActionResult getEmployeeList()
		{
			return Ok(this._mprBusenessAcess.getEmployeeList());
		}

		[HttpGet]
		[Route("getMPRRevisionList/{RequisitionId}")]
		public IHttpActionResult getMPRRevisionList(int RequisitionId)
		{
			return Ok(this._mprBusenessAcess.getMPRRevisionList(RequisitionId));
		}
		[HttpPost]
		[Route("statusUpdate")]
		public IHttpActionResult statusUpdate(MPRStatusUpdate mprStatus)
		{
			return Ok(this._mprBusenessAcess.statusUpdate(mprStatus));
		}
		[HttpGet]

		[Route("getStatusList")]
		public IHttpActionResult getStatusList()
		{
			return Ok(this._mprBusenessAcess.getStatusList());
		}
		[HttpGet]
		[Route("getAccessList/{RoleId}")]
		public IHttpActionResult getAccessList(int RoleId)
		{
			return Ok(this._mprBusenessAcess.getAccessList(RoleId));
		}
		[HttpPost]
		[Route("updateMPRVendor/{RevisionId}")]
		public IHttpActionResult updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId)
		{
			var result = this._mprBusenessAcess.updateMPRVendor(MPRVendorDetails, RevisionId);

			return Ok(result);

		}
		[HttpPost]
		[Route("deleteMPR")]
		public IHttpActionResult deleteMPR(DeleteMpr deleteMprInfo)
		{
			return Ok(this._mprBusenessAcess.deleteMPR(deleteMprInfo));
		}

		[HttpPost]
		[Route("sendMailtoVendor")]
		public IHttpActionResult sendMailtoVendor([FromBody] sendMailObj mailObj)
		{
			return Ok(this.emailTemplateDA.sendMailtoVendor(mailObj));
		}
		[HttpPost]
		[Route("addMprItemInfo")]
		public IHttpActionResult addMprItemInfo([FromBody] MPRItemInfo mprIteminfos)
		{
			return Ok(this._mprBusenessAcess.addMprItemInfo(mprIteminfos));
		}

		[HttpPost]
		[Route("updateItemId")]
		public IHttpActionResult updateItemId([FromBody] materialUpdate mprIteminfos)
		{
			return Ok(this._mprBusenessAcess.updateItemId(mprIteminfos));
		}

		[HttpPost]
		[Route("updateVendorRegProcess/{typeOfuser}")]
		public IHttpActionResult updateVendorRegProcess([FromBody] VendorRegApprovalProcessData model, string typeOfuser)
		{
			return Ok(this._mprBusenessAcess.updateVendorRegProcess(model, typeOfuser));
		}
		[HttpPost]
		[Route("getVendorReqList")]
		public IHttpActionResult getVendorReqList(vendorRegfilters vendorRegfilters)
		{
			return Ok(this._mprBusenessAcess.getVendorReqList(vendorRegfilters));
		}

		[HttpGet]
		[Route("GetStateList")]
		public IHttpActionResult GetStateNameList()
		{
			return Ok(this._mprBusenessAcess.StateNameList());
		}
		[HttpGet]
		[Route("GetDocumentTypeList")]
		public IHttpActionResult GetDocumentsList()
		{
			return Ok(this._mprBusenessAcess.DocumentMasterList());
		}
		[HttpGet]
		[Route("GetNaturOfBusiness")]
		public IHttpActionResult GetNatureOfBusiness()
		{
			return Ok(this._mprBusenessAcess.natureOfBusinessesList());
		}
		[HttpGet]
		[Route("Getvendordetails/{vendorid}")]
		public IHttpActionResult getvendordetails(int vendorid)
		{
			return Ok(this._mprBusenessAcess.GetVendorDetails(vendorid));
		}
		[HttpPost]
		[Route("SaveVendorDetails")]
		public IHttpActionResult SaveVendorData(VendorRegistrationModel model)
		{
			return Ok(this._mprBusenessAcess.SaveVendorDetails(model));
		}
		[HttpPost]
		[Route("deleteRegAttachedfile")]
		public IHttpActionResult deleteRegAttachedfile(VendorRegisterDocumentDetail model)
		{
			return Ok(this._mprBusenessAcess.DeletefileAttached(model));
		}


		[HttpGet]
		[Route("GetYILTermGroups")]
		public IHttpActionResult GetYILTermGroups()
		{
			return Ok(this._mprBusenessAcess.GetYILTermGroups());
		}
		[HttpPost]
		[Route("UpdateYILTermsGroup")]
		public IHttpActionResult UpdateYILTermsGroup(YILTermsGroup model)
		{
			return Ok(this._mprBusenessAcess.UpdateYILTermsGroup(model));
		}
		[HttpPost]
		[Route("UpdateYILTermsAndConditions")]
		public IHttpActionResult UpdateYILTermsAndConditions(YILTermsandCondition model)
		{
			return Ok(this._mprBusenessAcess.UpdateYILTermsAndConditions(model));
		}
		[HttpGet]
		[Route("DeleteTermGroup/{TermGroupId}/{DeletedBy}")]
		public IHttpActionResult DeleteTermGroup(int TermGroupId, string DeletedBy)
		{
			return Ok(this._mprBusenessAcess.DeleteTermGroup(TermGroupId,  DeletedBy));
		}
		[HttpGet]
		[Route("DeleteTermsAndConditions/{TermId}/{DeletedBy}")]
		public IHttpActionResult DeleteTermsAndConditions(int TermId, string DeletedBy)
		{
			return Ok(this._mprBusenessAcess.DeleteTermsAndConditions(TermId, DeletedBy));
		}

		[Route("UploadFile")]
		[HttpPost]
		public IHttpActionResult UploadFile()
		{
			try
			{
				var httpRequest = HttpContext.Current.Request;
				var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
				string parsedFileName = "";
				var revisionId = httpRequest.Files.AllKeys[0];
				if (httpRequest.Files.Count > 0)
				{
					foreach (string file in httpRequest.Files)
					{
						var postedFile = httpRequest.Files[file];
						byte[] fileData = null;
						using (var binaryReader = new BinaryReader(postedFile.InputStream))
						{
							fileData = binaryReader.ReadBytes(postedFile.ContentLength);
						}

						GC.Collect();
						parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + revisionId + "\\" + ToValidFileName(postedFile.FileName));
						serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + revisionId;
						var path = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
						if (!Directory.Exists(serverPath))
							Directory.CreateDirectory(serverPath);
						var memory = new MemoryStream();
						FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
						var updatedStream = new MemoryStream(fileData);
						updatedStream.Seek(0, SeekOrigin.Begin);
						updatedStream.CopyToAsync(fs).Wait();
						fs.Flush();
						GC.Collect();
					}
				}
				return Ok(parsedFileName);
			}
			catch (Exception e)
			{
				log.ErrorMessage("MPRController", "UploadFile", e.Message.ToString());
				return Ok(e);
			}

		}

		[HttpPost]
		[Route("uploadVendorData")]
		public IHttpActionResult uploadVendorData()
		{
			try
			{
				var httpRequest = HttpContext.Current.Request;
				var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
				string parsedFileName = "";
				if (httpRequest.Files.Count > 0)
				{
					var Id = httpRequest.Files.AllKeys[0];
					var postedFile = httpRequest.Files[0];
					parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + Id + "\\" + ToValidFileName(postedFile.FileName));
					serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + Id;
					var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
					if (!Directory.Exists(serverPath))
						Directory.CreateDirectory(serverPath);
					postedFile.SaveAs(filePath);


					DataTable dtexcel = new DataTable();
					bool hasHeaders = false;
					string HDR = hasHeaders ? "Yes" : "No";
					string strConn;
					if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
						strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
					else
						strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";

					OleDbConnection conn = new OleDbConnection(strConn);
					conn.Open();
					DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

					DataRow schemaRow = schemaTable.Rows[0];
					string sheet = schemaRow["TABLE_NAME"].ToString();
					if (!sheet.EndsWith("_"))
					{
						string query = "SELECT  * FROM [Sheet1$]";
						OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
						dtexcel.Locale = CultureInfo.CurrentCulture;
						daexcel.Fill(dtexcel);
					}

					conn.Close();
					int iSucceRows = 0;
					YSCMEntities obj = new YSCMEntities();
					foreach (DataRow row in dtexcel.Rows)
					{
						var vendorcode = row["Vendor Code"].ToString();
						VendorMaster vendorMaster = obj.VendorMasters.Where(li => li.VendorCode == vendorcode).FirstOrDefault();
						if (vendorMaster != null)
						{
							VendormasterModel vendorModel = new VendormasterModel();
							vendorModel.Vendorid = vendorMaster.Vendorid;
							vendorModel.VendorName = row["Vendor Name"].ToString();
							string Emailids = "";
							if (!string.IsNullOrEmpty(row["Email Id 1"].ToString()))
								Emailids = row["Email Id 1"].ToString();
							if (!string.IsNullOrEmpty(row["Email Id 2"].ToString()))
								Emailids += "," + row["Email Id 2"].ToString();
							if (!string.IsNullOrEmpty(row["Email Id 3"].ToString()))
								Emailids += "," + row["Email Id 3"].ToString();
							if (!string.IsNullOrEmpty(row["Email Id 4"].ToString()))
								Emailids += "," + row["Email Id 4"].ToString();

							vendorModel.Emailid = Emailids;
							vendorModel.ContactNumber = row["contact number"].ToString();
							vendorModel.ContactPerson = row["contact person"].ToString();
							this._mprBusenessAcess.addNewVendor(vendorModel);
						}
					}


					int succRecs = iSucceRows;
				}
				return Ok(parsedFileName);

			}
			catch (Exception e)
			{
				log.ErrorMessage("MPRController", "uploadVendorData", e.Message.ToString());
				return Ok(e);
			}

		}
		[HttpPost]
		[Route("uploadExcel")]
		public IHttpActionResult uploadExcel()
		{

			var revisionId = "";
			var httpRequest = HttpContext.Current.Request;
			var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
			string parsedFileName = "";
			if (httpRequest.Files.Count > 0)
			{
				revisionId = httpRequest.Files.AllKeys[0];
				var postedFile = httpRequest.Files[0];
				parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + revisionId + "\\" + ToValidFileName(postedFile.FileName));
				serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + revisionId;
				var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
				if (!Directory.Exists(serverPath))
					Directory.CreateDirectory(serverPath);
				postedFile.SaveAs(filePath);

				DataTable dtexcel = new DataTable();

				bool hasHeaders = false;
				string HDR = hasHeaders ? "Yes" : "No";
				string strConn;
				if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
					strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
				else
					strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";

				OleDbConnection conn = new OleDbConnection(strConn);
				conn.Open();
				DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

				DataRow schemaRow = schemaTable.Rows[0];
				string sheet = schemaRow["TABLE_NAME"].ToString();
				if (!sheet.EndsWith("_"))
				{
					string query = "SELECT  * FROM [Sheet1$A4:M]";
					OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
					dtexcel.Locale = CultureInfo.CurrentCulture;
					daexcel.Fill(dtexcel);
				}

				conn.Close();
				int iSucceRows = 0;
				try
				{
					YSCMEntities entities = new YSCMEntities();
					foreach (DataRow row in dtexcel.Rows)
					{

						MPRItemInfo mprIteminfos = new MPRItemInfo();
						if (row[1].ToString() != "" && row[2].ToString() != "")
						{
							string unitname = "";
							UnitMaster data = new UnitMaster();
							if (!string.IsNullOrEmpty(row[3].ToString()))
								unitname = row[3].ToString();
							if (!string.IsNullOrEmpty(unitname))
								data = entities.UnitMasters.Where(x => x.UnitName == unitname).FirstOrDefault();
							if (!string.IsNullOrEmpty(row[1].ToString()))
								mprIteminfos.ItemDescription = row[1].ToString();
							mprIteminfos.RevisionId = Convert.ToInt32(revisionId);
							if (!string.IsNullOrEmpty(row[2].ToString()))
								mprIteminfos.Quantity = Convert.ToInt32(row[2]);
							if (!string.IsNullOrEmpty(row[4].ToString()))
								mprIteminfos.SOLineItemNo = row[4].ToString();
							if (!string.IsNullOrEmpty(row[5].ToString()))
								mprIteminfos.TargetSpend = Convert.ToInt32(row[5]);
							if (!string.IsNullOrEmpty(row[6].ToString()))
								mprIteminfos.MfgPartNo = row[6].ToString();
							if (!string.IsNullOrEmpty(row[7].ToString()))
								mprIteminfos.MfgModelNo = row[7].ToString();
							if (!string.IsNullOrEmpty(row[8].ToString()))
								mprIteminfos.ReferenceDocNo = row[8].ToString();
							if (!string.IsNullOrEmpty(row[9].ToString()))
								mprIteminfos.YGSMaterialCode = row[9].ToString();
							if (!string.IsNullOrEmpty(row[11].ToString()))
								mprIteminfos.WBS = row[11].ToString();
							if (!string.IsNullOrEmpty(row[10].ToString()))
								mprIteminfos.ProjectDefinition = row[10].ToString();
							if (!string.IsNullOrEmpty(row[12].ToString()))
								mprIteminfos.SystemModel = row[12].ToString();
                            if (data != null)
                                mprIteminfos.UnitId = data.UnitId;
							if (row[9].ToString() == "")
								mprIteminfos.Itemid = "NewItem";
							else
							{
								mprIteminfos.Itemid = row[9].ToString();
								if (row[9].ToString() == "NewItem" || row[9].ToString() == "0000")
									mprIteminfos.Itemid = "NewItem";
							}
							this._mprBusenessAcess.addMprItemInfo(mprIteminfos);
							//entities.MPRItemInfoes.Add(mprIteminfos);
							//entities.SaveChanges();
						}
						iSucceRows++;
					}

					//string unitname = row["UnitId"].ToString();
					//var data = entities.UnitMasters.Where(x => x.UnitName == unitname).FirstOrDefault();

					//MPRItemInfo mprIteminfos = new MPRItemInfo();
					//if (row["ItemDescription"].ToString() != "" && row["Quantity"].ToString() != "")
					//{
					//	mprIteminfos.ItemDescription = row["ItemDescription"].ToString();
					//	mprIteminfos.RevisionId = Convert.ToInt32(revisionId);
					//	mprIteminfos.Quantity = Convert.ToInt32(row["Quantity"]);
					//	mprIteminfos.SOLineItemNo = row["SOLineItemNo"].ToString();
					//	mprIteminfos.TargetSpend = Convert.ToInt32(row["TargetSpend"]);
					//	mprIteminfos.MfgPartNo = row["MfgPartNo"].ToString();
					//	mprIteminfos.MfgModelNo = row["MfgModelNo"].ToString();
					//	mprIteminfos.ReferenceDocNo = row["ReferenceDocNo"].ToString();
					//	if(data!=null)
					//	mprIteminfos.UnitId = data.UnitId;
					//	if (row["YGSMaterialCode"].ToString() == "")
					//		mprIteminfos.Itemid = "0000";
					//	else
					//	{
					//		mprIteminfos.Itemid = row["YGSMaterialCode"].ToString();
					//		if (row["YGSMaterialCode"].ToString() == "NewItem")
					//			mprIteminfos.Itemid = "0000";
					//	}
					//	this._mprBusenessAcess.addMprItemInfo(mprIteminfos);
					//	//entities.MPRItemInfoes.Add(mprIteminfos);
					//	//entities.SaveChanges();
					//}
					//iSucceRows++;
					//}
					int succRecs = iSucceRows;
				}
				catch (Exception e)
				{
					log.ErrorMessage("MPRController", "uploadExcel", e.Message.ToString());
					return Ok(e);
				}
			}
			return Ok(parsedFileName);

		}
		[HttpGet]
		[Route("Loadstoragelocationsbydepartment")]
		public IHttpActionResult Loadstoragelocationsbydepartment()
		{
			return Ok(this._mprBusenessAcess.Loadstoragelocationsbydepartment());
		}
		[HttpGet]
		[Route("LoadJobCodesbysaleorder/{saleorder}")]
		public IHttpActionResult LoadJobCodesbysaleorder(string saleorder)
		{
			return Ok(this._mprBusenessAcess.LoadJobCodesbysaleorder(saleorder));
		}
		private static string ToValidFileName(string fileName)
		{
			fileName = fileName.ToLower().Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("&", "_").Replace("*", "_").Replace("-", "_").Replace("+", "_");
			return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
		}


		//api methods to save files from vendor portal
		[Route("UploadVscmFile")]
		[HttpPost]
		public IHttpActionResult UploadVscmFile()
		{
			//log.ErrorMessage("MPRController", "UploadVscmFile", "SCM Api called");
			var filePath = "";
			string dbfilePath = "";
			try
			{
				var httpRequest = HttpContext.Current.Request;
				var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
				if (httpRequest.Files.Count > 0)
				{
					string filename = string.Empty;
					string[] listdata;
					string VUniqueId, docid, vendorid, docType;
					foreach (string file in httpRequest.Files)
					{
						filename = file;
						break;
					}
					listdata = filename.Split('_');
					VUniqueId = listdata[0];
					vendorid = listdata[1];
					docid = listdata[2];
					docType = listdata[3];
					for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
					{
						var postedFile = httpRequest.Files[i];
						filePath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + VUniqueId + "_" + vendorid + "_" + docid + "_" + docType);
						if (!Directory.Exists(filePath))
							Directory.CreateDirectory(filePath);
						filePath = Path.Combine(filePath, ToValidFileName(postedFile.FileName));
						postedFile.SaveAs(filePath);


					}
				}
			}
			catch (Exception e)
			{
				log.ErrorMessage("MPRController", "UploadVscmFile", e.Message.ToString());
				return Ok(e);
			}
			return Ok(true);
		}


		[Route("InsertVscmVendorQuoteDocument")]
		[HttpPost]
		public IHttpActionResult InsertVscmVendorQuoteDocument()
		{
			//log.ErrorMessage("MPRController", "InsertVscmVendorQuoteDocument", "SCM Api called");
			var filePath = "";
			var fileserverpath = "";
			var httpRequest = HttpContext.Current.Request;
			try
			{
				if (httpRequest.Files.Count > 0)
				{
					string filename = string.Empty;
					string[] listdata;
					string RFQRevId, RFQItemId, updatedBy, updatedRevisionId, pageType;
					foreach (string file in httpRequest.Files)
					{
						filename = file;
						break;
					}

					listdata = filename.Split('_');
					RFQRevId = listdata[0];
					RFQItemId = listdata[1];
					updatedBy = listdata[2];
					pageType = listdata[3];

					for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
					{
						var postedFile = httpRequest.Files[i];
						filePath = HttpContext.Current.Server.MapPath("~/SCMDocs") + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + RFQRevId + "_" + RFQItemId + "_" + updatedBy + "_" + pageType;
						if (!Directory.Exists(filePath))
							Directory.CreateDirectory(filePath);
						filePath = Path.Combine(filePath, ToValidFileName(postedFile.FileName));
						postedFile.SaveAs(filePath);
					}

				}
			}
			catch (Exception e)
			{
				log.ErrorMessage("MPRController", "InsertVscmVendorQuoteDocument", e.Message.ToString());
				return Ok(e);
			}
			return Ok(true);
		}

        [HttpPost]
        [Route("Getoldrevisionitems")]
        public IHttpActionResult Getoldrevisionitems(List<int> itemdetailsid)
        {
            return Ok(this._mprBusenessAcess.Getoldrevisionitems(itemdetailsid));
        }
		[HttpGet]
		[Route("GetTokuchuinformation/{mprrevisionid}")]
		public IHttpActionResult GetTokuchuinformation(int mprrevisionid)
        {
			return Ok(this._mprBusenessAcess.GetTokuchuinformation(mprrevisionid));
		}
		[HttpGet]
		[Route("getVendorList")]
		public IHttpActionResult GetVendorList()
		{
			return Ok(this._mprBusenessAcess.VendorList());
		}



		[HttpGet]
		[Route("getVendorInfo/{vendorId}")]
		public IHttpActionResult GetVendorInfo(int vendorid)
		{
			return Ok(this._mprBusenessAcess.GetVendorInfo(vendorid));
		}



		[HttpGet]
		[Route("getVendorUserInfo/{vendorId}")]
		public IHttpActionResult GetVendorUserInfo(int vendorid)
		{
			return Ok(this._mprBusenessAcess.GetVendorUserInfo(vendorid));
		}



		[HttpPost]
		[Route("deactivateVendor")]
		public IHttpActionResult DeactivateVendor([FromBody] VendorUserMaster vendorUserMaster)
		{
			return Ok(this._mprBusenessAcess.DeactivateVendor(vendorUserMaster));
		}



		[HttpPost]
		[Route("AddVendorUser")]
		public IHttpActionResult AddVendorUser([FromBody] VendorUserMaster vendor)
		{
			return Ok(this._mprBusenessAcess.AddVendorUser(vendor));
		}



		[HttpDelete]
		[Route("DeactivateItem/{rfqrevisionId}/{mprRevisionid}")]
		[AcceptVerbs("DELETE")]
		public IHttpActionResult SetFlag(int rfqrevisionId, int mprRevisionid)
		{
			return Ok(this._mprBusenessAcess.DeteteRfqItems(rfqrevisionId, mprRevisionid));
		}



		[HttpGet]
		[Route("GetMappedNotMappedRfqItems/{mprRevisionId}")]
		public IHttpActionResult CheckMprRevision(int mprRevisionId)
		{
			return Ok(this._mprBusenessAcess.CheckMprRevision(mprRevisionId));
		}
	}

}
