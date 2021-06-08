using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proje.Models.EntityFramework;
using Proje.Models;
using Proje.Models.OgretmenRandevu;

namespace Proje.Controllers
{
    public class Ogretmen_RandevuController : Controller
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        // GET: Ogretmen_Randevu
        public ActionResult Index()
        {
            if (Degerler.kontrol == 2)
            {
                var dersler = entities.tbl_YapılacakDers.Where(x => x.tbl_Ders.Ogretmen_id == Degerler.id &&
                x.Durum == true && x.tbl_Ders.Durum == true);
                dersler = dersler.OrderBy(x => x.Tarih_Saat);
                List<Ogretmen> liste = new List<Ogretmen>();
                foreach (var item in dersler.ToList())
                {
                    DateTime zaman = (DateTime)item.Tarih_Saat;
                    Ogretmen og = new Ogretmen()
                    {
                        Ad = item.tbl_Ders.Ders_Adı,
                        Tarih = zaman.ToLongDateString(),
                        Saat = zaman.ToShortTimeString(),
                        Id = (int)item.YapılacakDers_id,
                        Link = item.Host_Link,
                        BirleşikTarih = (DateTime)item.Tarih_Saat,
                    };
                    liste.Add(og);
                }

                return View(liste);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }
        public ActionResult Sil(int id)
        {
            var kontrol = entities.tbl_YapılacakDers.Find(id);
            kontrol.Durum = false;
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('" + kontrol.tbl_Ders.Ders_Adı + " Ders " + kontrol.Tarih_Saat + " Tarihli Randevu İptal Edildi');};</script>";
            return RedirectToAction("Index");
        }
    }
}