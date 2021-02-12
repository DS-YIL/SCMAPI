/*
          Name of Class : <<RFQ.cs>>  Author :<<Fayaz>>  
          Date of Creation <<16-1-2021>>
          Purpose : <<Returns list of mapped and not mapped revisions>>
          Review Date :<<>>   Reviewed By :<<>>
         Version : 0.1 <change version only if there is major change - new release etc>
          Sourcecode Copyright : Yokogawa India Limited
     */
using System;
namespace SCMModels.Models
{
    public class RFQ
    {
        public int rfqRevisionId { get; set; }
        public bool ActiveRevision { get; set; }
        public int RevisionNo { get; set; }
        public int VendorId { get; set; }
        public int? MprRevisionId { get; set; }
        public int RevisionId { get; set; }
        public string RFQNo { get; set; }
        public int? statusId { get; set; }
        public bool Mapping { get; set; }

    }                    
}
