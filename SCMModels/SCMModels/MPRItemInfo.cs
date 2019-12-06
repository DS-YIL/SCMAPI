//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCMModels.SCMModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class MPRItemInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MPRItemInfo()
        {
            this.MPRDocuments = new HashSet<MPRDocument>();
            this.RFQItems = new HashSet<RFQItem>();
        }
    
        public int Itemdetailsid { get; set; }
        public Nullable<int> RevisionId { get; set; }
        public int Itemid { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<byte> UnitId { get; set; }
        public string SaleOrderNo { get; set; }
        public string SOLineItemNo { get; set; }
        public Nullable<decimal> TargetSpend { get; set; }
        public string ReferenceDocNo { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRDocument> MPRDocuments { get; set; }
        public virtual MPRItemInfo MPRItemInfo1 { get; set; }
        public virtual MPRItemInfo MPRItemInfo2 { get; set; }
        public virtual MPRRevision MPRRevision { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RFQItem> RFQItems { get; set; }
    }
}