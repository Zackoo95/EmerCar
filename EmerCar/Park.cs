//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmerCar
{
    using System;
    using System.Collections.Generic;
    
    public partial class Park
    {
        public int Park1 { get; set; }
        public int UserId { get; set; }
        public System.DateTime Park_Date { get; set; }
        public float Location_Lat { get; set; }
        public float Location_Long { get; set; }
    
        public virtual User User { get; set; }
    }
}
