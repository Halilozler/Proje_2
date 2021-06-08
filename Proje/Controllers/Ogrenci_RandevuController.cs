using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proje.Models.EntityFramework;
using Proje.Models;

namespace Proje.Controllers
{
    public class Ogrenci_RandevuController : Controller
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        // GET: Ogrenci_Randevu
        public ActionResult Index()
        {
            if (Degerler.kontrol == 3)
            {

                var b = from yd in entities.tbl_YapılacakDers.Where(x => x.Durum == true && x.tbl_Ders.Durum == true)
                        join ad in entities.tbl_AlınanDersler.Where(x => x.Ogrenci_id == Degerler.id && x.Durum == true)
                        on yd.Ders_id equals ad.Ders_id
                        select new
                        {
                            ders_ad = yd.tbl_Ders.Ders_Adı,
                            tarih = (DateTime)yd.Tarih_Saat,
                            link = yd.Join_Link,
                        };
                b = b.OrderBy(x => x.tarih);

                List<tiste> liste = new List<tiste>();
                foreach (var item in b)
                {
                    tiste tiste = new tiste()
                    {
                        Ad = item.ders_ad,
                        Zaman = item.tarih.ToLongDateString(),
                        Saat = item.tarih.ToShortTimeString(),
                        link = item.link,
                        Tarih = item.tarih,
                    };
                    liste.Add(tiste);
                }
               
                return View(liste);

            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }
        
        
    }
}