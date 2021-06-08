using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models.admin
{
    public class OgretmenClass
    {
        public List<tbl_Ogretmen> List_Ogretmen { get; set; }
        public tbl_Ogretmen ogretmen { get; set; }
        public tbl_Ders ders { get; set; }
        public List<tbl_Ders> List_ders { get; set; }
        
        
    }
}