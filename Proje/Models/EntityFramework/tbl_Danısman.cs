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
    
    public partial class tbl_Danısman
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Danısman()
        {
            this.tbl_Mesaj = new HashSet<tbl_Mesaj>();
        }
    
        public int Danısman_id { get; set; }
        public Nullable<int> Ogrenci_id { get; set; }
        public Nullable<int> Ogretmen_id { get; set; }
    
        public virtual tbl_Ogrenci tbl_Ogrenci { get; set; }
        public virtual tbl_Ogretmen tbl_Ogretmen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Mesaj> tbl_Mesaj { get; set; }
    }
}
