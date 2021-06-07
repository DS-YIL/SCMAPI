using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class ScrapRegisterMasterModel
    {
        public ScrapRegisterMasterModel()
        {
            scrapitems = new List<ScrapItems>();
            documents = new List<ScapRegisterDocumentModel>();
            statustarck = new List<ScrapStatustarckModel>();
        }
        public string employeeno { get; set; }
        public int scrapid { get; set; }
        public string TruckNo { get; set; }
        public System.DateTime DateOfEntry { get; set; }
        public string DepartmentName { get; set; }
        public int RequesterDepartmentID { get; set; }
        public string RequestedBY { get; set; }
        public string RequestedName { get; set; }
        public string PreparedBY { get; set; }
        public string PreparedDate { get; set; }
        public string ApprovalBy { get; set; }
        public string ApprovalName { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string Approvalstatus { get; set; }
        public string SONO { get; set; }
        public Nullable<System.DateTime> SoDate { get; set;  }
        public string Soupdatedby { get; set; }
        public Nullable<System.DateTime> Soupdateddate { get; set; }
        public string VATInvoiceno { get; set; }
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }
        public string VATInvoiceupdatedby { get; set; }
        public Nullable<System.DateTime> VatInvoiceUpdatedDate { get; set; }
        public bool VatInvoiceDocumentUploaded { get; set; }
        public string GatePAssNo { get; set; }
        public Nullable<System.DateTime> GatePassDate { get; set; }
        public string GatePassupdateby { get; set; }
        public Nullable<System.DateTime> GatePassupdateddate { get; set; }
        public bool GatePassDocumentUploaded { get; set; }
        public string TaxInvoiceNo { get; set; }
        public Nullable<System.DateTime> TaxInvoiceDate { get; set; }
        public string TaxInvoiceUpdatedY { get; set; }
        public Nullable<System.DateTime> TaxInvoiceUpdatedDate { get; set; }
        public string TaxInvoiceDocumentUpdated { get; set; }
        public List<ScrapItems> scrapitems { get; set; }
        public int ScrapStatusId { get; set; }
        public List<ScapRegisterDocumentModel> documents { get; set; }
        public List<ScrapStatustarckModel> statustarck { get; set; }
        public string scraptype { get; set; }
        public string Vendor { get; set; }
        public string Vendorcode { get; set; }
        public string Verifier { get; set; }
        public string VerifierRemarks { get; set; }
        public decimal fundavailablewithvendor { get; set; }
        public string fundavendorremarks { get; set; }
    }
    public class ScrapItems
    {
        public int Scratypeid { get; set; }
        public string ItemId { get; set; }
        public int PriceType { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public string UOM { get; set; }
        public decimal BAsicPrice { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> tcs { get; set; }
        public Nullable<decimal> sgstamount { get; set; }
        public Nullable<decimal> cgstamount { get; set; }
        public Nullable<decimal> igstamount { get; set; }
        public string employeeno { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string Scraptype { get; set; }
        public string Itemcode { get; set; }
        public int ScrapItemid { get; set; }
    }
    public class ScapRegisterDocumentModel
    {
        public int scrapdocid { get; set; }
        public Nullable<int> Scrapentryid { get; set; }
        public string DocumentNAme { get; set; }
        public string uploadedBy { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public string path { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DocumentType { get; set; }
        //public virtual ScrapEntryMaster ScrapEntryMaster { get; set; }
        //public virtual ScrapRegisterDocumentTypeMaster ScrapRegisterDocumentTypeMaster { get; set; }
    }
    public class ScrapStatustarckModel
    {
        public int stausid { get; set; }
        public string Status { get; set; }
        public int scrapid { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class ScrapflowModel
    {
        public string Scrapflow { get; set; }
        public string Incharge { get; set; }
        public string createdby { get; set; }
        public string Inchargename { get; set; }
    }
    public class scrapsearchmodel
    {
        public string scraptype { get; set; }
        public string truckno { get; set; }
        public string employeeno { get; set; }
        public List<string> scrapflow { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Vendorid { get; set; }
        public string VendorName { get; set; }
        public Nullable<DateTime> scrapfrom { get; set; }
        public Nullable<DateTime> scrapto { get; set; }
        public string scraptypepending { get; set; }
        public string scraptypeapprove { get; set; }
        public string Datetype { get; set; }
    }
}
