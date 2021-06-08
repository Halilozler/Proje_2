using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proje.Models.EntityFramework;
using Proje.Models;

namespace Proje.Controllers
{
    public class Ogrenci_MesajController : Controller
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        // GET: Ogrenci_Mesaj
        public ActionResult Index()
        {
            if (Degerler.kontrol == 3)
            {
                var yanıtlanmamış = entities.tbl_Mesaj.Where(x => x.Ogrenci_id == Degerler.id && x.Durum == false);
                var yanıtlanmış = entities.tbl_Mesaj.Where(x => x.Ogrenci_id == Degerler.id && x.Durum == true);

                yanıtlanmamış = yanıtlanmamış.OrderBy(x => x.Tarih);
                yanıtlanmış = yanıtlanmış.OrderBy(x => x.Tarih);

                mesaj mesaj = new mesaj();
                mesaj.Gelenler = yanıtlanmamış.ToList();
                mesaj.Yanıtlananlar = yanıtlanmış.ToList();

                return View(mesaj);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
            
        }

        [HttpPost]
        public ActionResult Mesaj(mesaj G_mesaj)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Hatta Oluştu');};</script>";
                return View("Index");
            }
            else
            {
                if (G_mesaj.m == null) //boş Göndere tıklaması demek
                {
                    TempData["msg"] = "<script>window.onload = function(){ alert('Mesaj Kısmı Boş Gönderilemez');};</script>";
                    return RedirectToAction("Index");
                }
                else if (G_mesaj.m.Length <= 500)
                {
                    tbl_Mesaj mesaj = new tbl_Mesaj();
                    var danışman = entities.tbl_Danısman.Where(x => x.Ogrenci_id == Degerler.id).ToList();
                    mesaj.Danısman_id = danışman[0].Danısman_id;
                    mesaj.Ogrenci_id = Degerler.id;
                    mesaj.Mesaj = G_mesaj.m;
                    mesaj.Tarih = DateTime.Now;
                    mesaj.Durum = false;
                    entities.tbl_Mesaj.Add(mesaj);
                    entities.SaveChanges();
                    TempData["msg"] = "<script>window.onload = function(){ alert('Mesajınız Danışmanınıza Gönderildi');};</script>";
                    return RedirectToAction("Index");
                }
                else //mesajı 500 karakterden fazla olamaz olursa buraya geliyor.
                {
                    TempData["msg"] = "<script>window.onload = function(){ alert('Mesajınız 500 Karakteri Geçmektedir');};</script>";
                    return RedirectToAction("Index");
                }
                
            }
        }
    }
}