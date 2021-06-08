using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proje.Models.EntityFramework;
using Proje.Models;

namespace Proje.Controllers
{
    public class Ogretmen_MesajController : Controller
    {
        // GET: Ogretmen_Mesaj
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        public ActionResult Index()
        {
            if (Degerler.kontrol == 2)
            {
                var gelen_mesaj = entities.tbl_Mesaj.Where(x => x.tbl_Danısman.Ogretmen_id == Degerler.id && x.Durum == false);
                var yanıtlananlar = entities.tbl_Mesaj.Where(x => x.tbl_Danısman.Ogretmen_id == Degerler.id && x.Durum == true);

                gelen_mesaj = gelen_mesaj.OrderBy(x => x.Tarih);
                yanıtlananlar = yanıtlananlar.OrderBy(x => x.Tarih);

                mesaj mesaj = new mesaj();
                mesaj.Gelenler = gelen_mesaj.ToList();
                mesaj.Yanıtlananlar = yanıtlananlar.ToList();

            

                return View(mesaj);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
            
        }
        [HttpPost]
        public ActionResult MesajGonder(int? id)
        {
            var kontrol = entities.tbl_Mesaj.Find(id);
            return PartialView(kontrol);
        }
        [HttpPost]
        public ActionResult Yanıt(tbl_Mesaj m)
        {
            /*en son eklenen değerin id sini alıyoruz
            int sonid = entities.tbl_Mesaj.Max(x => x.Mesaj_id);
            sonid++;
            */

            var ö_mesaj = entities.tbl_Mesaj.Find(m.Mesaj_id);
            ö_mesaj.Yanıt = m.Y_Mesaj;
            ö_mesaj.Durum = true;
            ö_mesaj.Yanıtlanma_Tarihi = DateTime.Now;
            
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('Mesaj Gönderildi');};</script>";
            return RedirectToAction("Index");
        }
    }
}