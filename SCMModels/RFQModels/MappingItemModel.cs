using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class MappingItemModel
    {
        public int mprrevisionid { get; set; }
        public int Itemdetailsid { get; set; }
        public int DocumentNo { get; set; }
        public int RFQNo { get; set; }
        public int itemid { get; set; }
        public int RFQItemsId { get; set; }
        public int RFQSplitItemId { get; set; }
        public int previousitemdetails { get; set; }
        public int itemrevision { get; set; }
    }
}
