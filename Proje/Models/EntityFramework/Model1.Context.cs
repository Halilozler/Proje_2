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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OnlineEğitimEntities : DbContext
    {
        public OnlineEğitimEntities()
            : base("name=OnlineEğitimEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Admin> tbl_Admin { get; set; }
        public virtual DbSet<tbl_AlınanDersler> tbl_AlınanDersler { get; set; }
        public virtual DbSet<tbl_Bolum> tbl_Bolum { get; set; }
        public virtual DbSet<tbl_Danısman> tbl_Danısman { get; set; }
        public virtual DbSet<tbl_Ders> tbl_Ders { get; set; }
        public virtual DbSet<tbl_Fakulte> tbl_Fakulte { get; set; }
        public virtual DbSet<tbl_Mesaj> tbl_Mesaj { get; set; }
        public virtual DbSet<tbl_Ogrenci> tbl_Ogrenci { get; set; }
        public virtual DbSet<tbl_Ogretim> tbl_Ogretim { get; set; }
        public virtual DbSet<tbl_Ogretmen> tbl_Ogretmen { get; set; }
        public virtual DbSet<tbl_OnlineDers> tbl_OnlineDers { get; set; }
        public virtual DbSet<tbl_YapılacakDers> tbl_YapılacakDers { get; set; }
        public virtual DbSet<tbl_YapılanDersLog> tbl_YapılanDersLog { get; set; }
        public virtual DbSet<tbl_AdminLog> tbl_AdminLog { get; set; }
        public virtual DbSet<tbl_İşlem> tbl_İşlem { get; set; }
    }
}
