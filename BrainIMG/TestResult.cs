//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrainIMG
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestResult
    {
        public int ID { get; set; }
        public string Value1 { get; set; }
        public Nullable<int> Value2 { get; set; }
        public string Value3 { get; set; }
        public int RecordID { get; set; }
    
        public virtual TestRecord TestRecord { get; set; }
    }
}