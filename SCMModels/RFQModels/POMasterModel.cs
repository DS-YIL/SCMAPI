using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class POMasterModel
    {
        public POMasterModel()
        {
            poitems = new List<POItem>();
        }
        public string pono { get; set; }
        public DateTime podate { get; set; }
        public string preparedby { get; set; }
        public DateTime prepareddate { get; set; }
        public string poremarks { get; set; }
        public string poterms { get; set; }
        public string insurance { get; set; }
        public string potype { get; set; }
        public string purchasetype { get; set; }
        public decimal collectiveno{ get; set; }
        public decimal priorvendor { get; set; }
        public DateTime Reqdeliverydate { get; set; }
        public string YGSpono { get; set; }
        public string YGSpodate { get; set; }
        public DateTime downloadedon { get; set; }
        public DateTime downloadedby { get; set; }
        public DateTime completedon { get; set; }
        public string completedby { get; set; }
        public int departmentid { get; set; }
        public int BuyerGroupID { get; set; }
        public string scmpoconfirmation { get; set; }
        public string itemtype { get; set; }
        public List<POItem> poitems { get; set; }
        public string VendorCode { get; set; }
    }
    public class POItem
    {
        public int paitemid { get; set; }
    }
    public class posearchmodel
    {
        public int poid { get; set; }
        public int Buyergroupid { get; set; }
        public int DepartmentId { get; set; }
        public string potype { get; set; }
        public string VendorId { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
    }
}
