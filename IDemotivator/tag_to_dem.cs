//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDemotivator
{
    using System;
    using System.Collections.Generic;
    
    public partial class tag_to_dem
    {
        public int tagId { get; set; }
        public int DemotivatorId { get; set; }
        public int Id { get; set; }
    
        public virtual tag tag { get; set; }
        public virtual Demotivator Demotivator { get; set; }
    }
}
