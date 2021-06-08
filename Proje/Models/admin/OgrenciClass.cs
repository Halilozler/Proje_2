using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models.admin
{
    public class OgrenciClass
    {
        public List<tbl_Ogrenci> List_Ogrenci { get; set; }
        public tbl_Ogrenci Ogrenci { get; set; }
        public List<tbl_AlınanDersler> List_Ders { get; set; }
        public tbl_Ders Ders { get; set; }
        public tbl_AlınanDersler A_Ders { get; set; }
        public string No { get; set; }
    }
}