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
    
    public partial class Test_ItemAttributesMaster
    {
        public int ItemAttributeid { get; set; }
        public Nullable<int> itemid { get; set; }
        public Nullable<int> Attributeid { get; set; }
        public string description { get; set; }
    
        public virtual Test_AttributeMaster Test_AttributeMaster { get; set; }
        public virtual Test_ItemMaster Test_ItemMaster { get; set; }
    }
}