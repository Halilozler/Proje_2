//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proje.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_YapılacakDers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_YapılacakDers()
        {
            this.tbl_OnlineDers = new HashSet<tbl_OnlineDers>();
            this.tbl_AdminLog = new HashSet<tbl_AdminLog>();
        }
    
        public int YapılacakDers_id { get; set; }
        public Nullable<int> Ders_id { get; set; }
        public Nullable<System.DateTime> Tarih_Saat { get; set; }
        public Nullable<System.DateTime> Kapama { get; set; }
        public Nullable<bool> Durum { get; set; }
        public string Host_Link { get; set; }
        public string Join_Link { get; set; }
    
        public virtual tbl_Ders tbl_Ders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_OnlineDers> tbl_OnlineDers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AdminLog> tbl_AdminLog { get; set; }
    }
}
