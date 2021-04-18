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
        }
        public string TruckNo { get; set; }
        public System.DateTime DateOfEntry { get; set; }
        public string DepartmentName { get; set; }
        public int RequesterDepartmentID { get; set; }
        public string RequestedBY { get; set; }
        public string PreparedBY { get; set; }
        public string PreparedDate { get; set; }
        public string ApprovalBy { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string Approvalstatus { get; set; }
        public string SONO { get; set; }
        public string SoDate { get; set; }
        public string Soupdatedby { get; set; }
        public DateTime Soupdateddate { get; set; }
        public string VATInvoiceno { get; set; }
        public DateTime VATInvoiceDate { get; set; }
        public string VATInvoiceupdatedby { get; set; }
        public DateTime VatInvoiceUpdatedDate { get; set; }
        public bool VatInvoiceDocumentUploaded { get; set; }
        public string GatePAssNo { get; set; }
        public string GatePassDate { get; set; }
        public string GatePassupdateby { get; set; }
        public string GatePassupdateddate { get; set; }
        public bool GatePassDocumentUploaded { get; set; }
        public string TaxInvoiceNo { get; set; }
        public DateTime TaxInvoiceDate { get; set; }
        public string TaxInvoiceUpdatedY { get; set; }
        public DateTime TaxInvoiceUpdatedDate { get; set; }
        public string TaxInvoiceDocumentUpdated { get; set; }
        public List<ScrapItems> scrapitems { get; set; }
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
        public string tcs { get; set; }
        public int sgstamount { get; set; }
        public int cgstamount { get; set; }
        public int igstamount { get; set; }
    }
}
