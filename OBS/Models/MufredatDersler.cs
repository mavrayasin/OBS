//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OBS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MufredatDersler
    {
        public int ID { get; set; }
        public int MufredatID { get; set; }
        public int DersID { get; set; }
    
        public virtual Dersler Dersler { get; set; }
        public virtual Mufredat Mufredat { get; set; }
    }
}
