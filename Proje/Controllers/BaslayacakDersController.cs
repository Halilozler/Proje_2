                                                            

                                                        //İPTAL EDİLDİ
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proje.Models.EntityFramework;
using Proje.Models;

namespace Proje.Controllers
{
    public class BaslayacakDersController : Controller
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        
        public ActionResult Ogrenci()
        {
            if (Degerler.kontrol == 3)
            {
                var ders = from yd in entities.tbl_OnlineDers.Where(x => x.tbl_YapılacakDers.Durum == true && x.tbl_YapılacakDers.tbl_Ders.Durum == true)
                           join ad in entities.tbl_AlınanDersler.Where(x => x.Ogrenci_id == Degerler.id && x.Durum == true)     //dersler geliyor
                           on yd.tbl_YapılacakDers.Ders_id equals ad.Ders_id
                           select new
                           {
                               ders_ad = yd.tbl_YapılacakDers.tbl_Ders.Ders_Adı,
                               tarih = (DateTime)yd.tbl_YapılacakDers.Tarih_Saat,
                               link = yd.tbl_YapılacakDers.Join_Link,
                           };

                List<tiste> liste = new List<tiste>();
                foreach (var item in ders)
                {
                    tiste tiste = new tiste()
                    {
                        Ad = item.ders_ad,
                        link = item.link,
                    };
                    tiste.SaatAyrıstırma(item.tarih);
                    liste.Add(tiste);
                }

                return View(liste);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }

        }
        public ActionResult Ogretmen()
        {
            if (Degerler.kontrol == 2)
            {
                var ders = entities.tbl_OnlineDers.Where(x => x.tbl_YapılacakDers.tbl_Ders.Ogretmen_id == Degerler.id 
                && x.tbl_YapılacakDers.Durum == true
                && x.tbl_YapılacakDers.tbl_Ders.Durum == true);
                return View(ders.ToList());
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
            
        }
    }
}