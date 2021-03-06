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
    
    public partial class RFQDocument
    {
        public int RfqDocId { get; set; }
        public int rfqRevisionId { get; set; }
        public Nullable<int> rfqItemsid { get; set; }
        public string DocumentName { get; set; }
        public int DocumentType { get; set; }
        public string Path { get; set; }
        public string UploadedBy { get; set; }
        public System.DateTime uploadedDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public string StatusBy { get; set; }
        public bool DeleteFlag { get; set; }
    
        public virtual RFQItem RFQItem { get; set; }
        public virtual RFQRevision RFQRevision { get; set; }
    }
}
