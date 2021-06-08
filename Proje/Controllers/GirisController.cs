using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proje.Models.EntityFramework;
using System.Web.Security;
using Proje.Models;

namespace Proje.Controllers
{
    
    [AllowAnonymous]
    public class GirisController : Controller 
    {
        //public static bool _p;
        //public static int deger;
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        DataBaseUpdate dataBase;
        // GET: Giris
        public ActionResult Gırıs()
        {
            FormsAuthentication.SignOut();
            

            return View();
        }
        [HttpPost]
        public ActionResult Gırıs(Giris p)
        {
            if (p.KullanıcıAdı.Substring(0,1) == "K")
            {
                //öğretmen yeri.
                var bilgiler = entities.tbl_Ogretmen.FirstOrDefault(x => x.Ogretmen_KullanıcıAdı == p.KullanıcıAdı
                && x.Ogretmen_Sifre == p.Sifre );
                if (bilgiler != null)
                {
                    FormsAuthentication.SetAuthCookie(bilgiler.Ogretmen_KullanıcıAdı, false);
                    Degerler.kontrol = 2;
                    Degerler.id = bilgiler.Ogretmen_id;
                    dataBase = new DataBaseUpdate();
                    return RedirectToAction("Index", "Ogretmen_Randevu");
                }
                else
                {
                    return View();
                }
            }
            else if (p.KullanıcıAdı.Substring(0, 1) == "A")
            {
                //Admin yeri.
                var bilgiler = entities.tbl_Admin.FirstOrDefault(x => x.Admin_Kullanıcı_Adı == p.KullanıcıAdı
                && x.Admin_Sifre == p.Sifre);
                if (bilgiler != null)
                {
                    Degerler.AdminId = bilgiler.Admin_id;
                    FormsAuthentication.SetAuthCookie(bilgiler.Admin_Kullanıcı_Adı, false);
                    Degerler.kontrol = 1;
                    Degerler.id = bilgiler.Admin_id;
                    dataBase = new DataBaseUpdate();
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
            }   
            else
            {
                //öğrenci yeri.
                var bilgiler = entities.tbl_Ogrenci.FirstOrDefault(x => x.Ogrenci_Tc == p.KullanıcıAdı
                && x.Ogrenci_Tc == p.Sifre && x.Ogrenci_Durum == true);
                if (bilgiler != null)
                {
                    FormsAuthentication.SetAuthCookie(bilgiler.Ogrenci_Tc, false);
                    Degerler.kontrol = 3;
                    Degerler.id = bilgiler.Ogrenci_id;
                    dataBase = new DataBaseUpdate();
                    return RedirectToAction("Index", "Ogrenci_Randevu");
                }
                else
                    return View();
            }
        }
    }
}