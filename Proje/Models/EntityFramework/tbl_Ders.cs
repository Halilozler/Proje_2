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
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_Ders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Ders()
        {
            this.tbl_AlınanDersler = new HashSet<tbl_AlınanDersler>();
            this.tbl_YapılacakDers = new HashSet<tbl_YapılacakDers>();
            this.tbl_YapılanDersLog = new HashSet<tbl_YapılanDersLog>();
            this.tbl_AdminLog = new HashSet<tbl_AdminLog>();
        }
    
        public int Ders_id { get; set; }
        [Required]
        public string Ders_Adı { get; set; }
        public Nullable<int> Ogretmen_id { get; set; }
        public Nullable<byte> Ogretim_id { get; set; }
        public Nullable<bool> Durum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AlınanDersler> tbl_AlınanDersler { get; set; }
        public virtual tbl_Ogretim tbl_Ogretim { get; set; }
        public virtual tbl_Ogretmen tbl_Ogretmen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_YapılacakDers> tbl_YapılacakDers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_YapılanDersLog> tbl_YapılanDersLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AdminLog> tbl_AdminLog { get; set; }
    }
}
