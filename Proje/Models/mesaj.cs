using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models
{
    public class mesaj
    {
        public List<tbl_Mesaj> Gelenler{ get; set; }
        public List<tbl_Mesaj> Yanıtlananlar { get; set; }
        public string m { get; set; }

    }
}