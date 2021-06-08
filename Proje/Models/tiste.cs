using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proje.Models
{
    //Öğrencinin aldığı dersleri ders olanlarını gönderdiğimiz class
    public class tiste
    {
        public string Ad { get; set; }
        public string Zaman { get; set; }
        public string Saat { get; set; }
        public string link { get; set; }
        public DateTime Tarih { get; set; }
        public void SaatAyrıstırma(DateTime tarih)
        {
            int saat = tarih.Hour;
            int dakika = tarih.Minute;
            this.Zaman = saat + ":" + dakika;
        }
    }
}