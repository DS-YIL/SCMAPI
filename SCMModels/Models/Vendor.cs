/*
            Name of Class : <<Vendor>>  Author :<<Fayaz>>  
            Date of Creation <<20-1-2021>>
            Purpose : <<Returning particular vendor data>>
            Review Date :<<>>   Reviewed By :<<>>
           Version : 0.1 <change version only if there is major change - new release etc>
            Sourcecode Copyright : Yokogawa India Limited
       */


using System;

namespace SCMModels.Models
{
    public class Vendor
    {
        public int Vendorid { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string OldVendorCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string AuGr { get; set; }
        public string PaymentTermCode { get; set; }
        public string Blocked { get; set; }
        public string Emailid { get; set; }
        public string ContactNo { get; set; }
        public Nullable<bool> Deleteflag { get; set; }
    }
}
