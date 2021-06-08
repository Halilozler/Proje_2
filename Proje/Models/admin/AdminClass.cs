using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models.admin
{
    public class AdminClass
    {
        public List<tbl_YapılacakDers> yapılacakders { get; set; }
        public List<tbl_YapılacakDers> IptalDersler { get; set; }
        public List<tbl_OnlineDers> onlineDers { get; set; }
        public AdminClass(List<tbl_YapılacakDers> yapılacakders, List<tbl_YapılacakDers> IptalDersler, List<tbl_OnlineDers> onlineDers)
        {
            this.yapılacakders = yapılacakders;
            this.IptalDersler = IptalDersler;
            this.onlineDers = onlineDers;
        }
    }
}