using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models.OgretmenRandevu
{
    public class Ogretmen
    {
        public string Ad { get; set; }
        public string Tarih { get; set; }
        public string Saat { get; set; }
        public int Id { get; set; }
        public string Link { get; set; }
        public DateTime BirleşikTarih { get; set; }

    }
}