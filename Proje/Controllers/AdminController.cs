using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proje.Models;
using Proje.Models.EntityFramework;
using Proje.Models.admin;


namespace Proje.Controllers
{
    public class AdminController : Controller
    {
        OnlineEğitimEntities entities =  new OnlineEğitimEntities();
        public ActionResult Index()
        {
            
            if (Degerler.kontrol == 1)
            {
                var Y_Ders = entities.tbl_YapılacakDers.Where(x => x.Durum == true && x.tbl_Ders.Durum == true
                && entities.tbl_OnlineDers.Where(y => y.YapılacakDers_id == x.YapılacakDers_id).ToList().Count() == 0).OrderBy(y => y.Tarih_Saat).ToList();
                var AktifDers = entities.tbl_OnlineDers.Where(x => x.tbl_YapılacakDers.tbl_Ders.Durum == true).OrderBy(y => y.tbl_YapılacakDers.Tarih_Saat).ToList();
                var Y_Ders_İptal = entities.tbl_YapılacakDers.Where(x => x.Durum == false).OrderBy(y => y.Tarih_Saat).ToList();
                AdminClass admin = new AdminClass(Y_Ders,Y_Ders_İptal,AktifDers);
                return View(admin);
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
            tbl_AdminLog adminLog = new tbl_AdminLog()
            {
                Admin_id = Degerler.AdminId,
                İşlem_id = 5,
                Tarih = DateTime.Now,
                tbl_Ders = kontrol.tbl_Ders,
            };
            entities.tbl_AdminLog.Add(adminLog);
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('" + kontrol.tbl_Ders.Ders_Adı + " Ders " + kontrol.Tarih_Saat + " Tarihli Randevu İptal Edildi');};</script>";
            return RedirectToAction("Index","Admin");
        }
        [HttpGet]
        public ActionResult Y_Ders()
        {
            if (Degerler.kontrol == 1)
            {
                var dersler = entities.tbl_YapılanDersLog.OrderBy(x => x.Açma).ToList();
                
                return View(dersler);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }

        //Öğretmen-------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Ogretmen_Listesi()
        {
            if (Degerler.kontrol == 1)
            {
                var öğretmenler = entities.tbl_Ogretmen.OrderBy(x => x.Ogretmen_Ad).ToList();

                //Bölüm Gönderelim;
                var bolum = entities.tbl_Bolum;
                List<SelectListItem> bol = (from x in bolum.ToList()
                                            select new SelectListItem
                                            {
                                                Text = x.Bolum_Adı,
                                                Value = x.Bolum_id.ToString()
                                            }).ToList();
                ViewBag.bolum = bol;

                //Öğretmen Gönderelim
                var List_Ogretmen = entities.tbl_Ogretmen;
                List<SelectListItem> ogr = (from x in List_Ogretmen.ToList()
                                            select new SelectListItem
                                            {
                                                Text = x.Ogretmen_Ad + " " + x.Ogretmen_Soyad,
                                                Value = x.Ogretmen_id.ToString()
                                            }).ToList();
                ViewBag.ogr = ogr;

                //Öğretim Gönderelim
                var List_Ogretim = entities.tbl_Ogretim;
                List<SelectListItem> ogretim = (from x in List_Ogretim.ToList()
                                            select new SelectListItem
                                            {
                                                Text = x.Ogretim_Ad,
                                                Value = x.Ogretim_id.ToString()
                                            }).ToList();
                ViewBag.ogretim = ogretim;

                OgretmenClass ogretmen = new OgretmenClass { List_Ogretmen = öğretmenler, List_ders = entities.tbl_Ders.Where(x => x.Durum == true).ToList()};

                return View(ogretmen);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }
        [HttpPost]
        public ActionResult Ogretmen_Listesi(tbl_Ders ders) //Yeni Ders Ekleniyor
        {
            if (ModelState.IsValid)
            {
                tbl_Ders y_Ders = new tbl_Ders()
                {
                    Ders_Adı = ders.Ders_Adı,
                    Ogretmen_id = ders.Ogretmen_id,
                    Ogretim_id = ders.Ogretim_id,
                    Durum = true,
                };
                entities.tbl_Ders.Add(y_Ders);
                tbl_AdminLog adminLog = new tbl_AdminLog()
                {
                    Admin_id = Degerler.AdminId,
                    İşlem_id = 8,
                    Tarih = DateTime.Now,
                    Ogretmen_id = ders.Ogretmen_id,
                    tbl_Ders = y_Ders,
                };
                entities.tbl_AdminLog.Add(adminLog);
                TempData["msg"] = "<script>window.onload = function(){ alert('Yeni Ders Eklendi');};</script>";
                entities.SaveChanges();
            }
            else
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Alanları Tam Doldurun Lütfen');};</script>";
            }
            return RedirectToAction("Ogretmen_Listesi", "Admin");
            
        }
        [HttpPost]
        public ActionResult Ogretmen_Ekleme(tbl_Ogretmen ogretmen) //Yeni Öğretmen Ekleme
        {
            if (ModelState.IsValid)
            {
                if (entities.tbl_Ogretmen.Where(x => x.Ogretmen_KullanıcıAdı == ogretmen.Ogretmen_KullanıcıAdı).ToList().Count == 0)
                {
                    tbl_Ogretmen y_ogretmen = new tbl_Ogretmen
                    {
                        Ogretmen_Ad = ogretmen.Ogretmen_Ad,
                        Ogretmen_Soyad = ogretmen.Ogretmen_Soyad,
                        Ogretmen_KullanıcıAdı = "K-" + ogretmen.Ogretmen_KullanıcıAdı,
                        Ogretmen_Sifre = ogretmen.Ogretmen_Sifre,
                        Ogretmen_Durum = true,
                        Bolum_id = ogretmen.Bolum_id,
                        Api_Key = ogretmen.Api_Key,
                        Api_secret = ogretmen.Api_secret,
                        Client = ogretmen.Client
                    };
                    entities.tbl_Ogretmen.Add(y_ogretmen);
                    tbl_AdminLog adminLog = new tbl_AdminLog()
                    {
                        Admin_id = Degerler.AdminId,
                        İşlem_id = 1,
                        Tarih = DateTime.Now,
                        tbl_Ogretmen = y_ogretmen,
                    };
                    entities.tbl_AdminLog.Add(adminLog);
                    entities.SaveChanges();
                    TempData["msg"] = "<script>window.onload = function(){ alert('Yeni Öğretmen Eklendi');};</script>";
                }
                else //kullanıcı Adı Var uyarısı versin.
                {
                    TempData["msg"] = "<script>window.onload = function(){ alert('Bu Kullanıcı Adına Sahip Öğretmen Var! lütfen Başka Deneyiniz');};</script>";
                }
            }
            else
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Alanları Tam Doldurun Lütfen');};</script>";
            }
            return RedirectToAction("Ogretmen_Listesi", "Admin");

        }
        [HttpPost]
        public ActionResult O_DersEkle(int? id) //alınmayan dersler gönderilsin
        {
            var AlınmayanDersler = entities.tbl_Ders.Where(x => x.Ogretmen_id == null || x.Durum == false).ToList();
            List<SelectListItem> BosDers = (from x in AlınmayanDersler
                                                      select new SelectListItem
                                                      {
                                                          Text = x.Ders_Adı,
                                                          Value = x.Ders_id.ToString()
                                                      }).ToList();
            ViewBag.BosDers = BosDers;

            OgretmenClass ogretmen = new OgretmenClass { ogretmen = entities.tbl_Ogretmen.Find(id) };
            return PartialView(ogretmen);
        }
        [HttpPost]
        public ActionResult O_DersSil(int? id)
        {
            var AldığıDers = entities.tbl_Ders.Where(x => x.Ogretmen_id == id && x.Durum == true).ToList();
            var randevu = entities.tbl_YapılacakDers.Where(x => x.tbl_Ders.Ogretmen_id == id && x.Durum == true).ToList();
            if (randevu.Count != 0)
            {
                foreach (var item in randevu)
                {
                    var RandevuluDers = entities.tbl_Ders.Find(item.Ders_id);
                    AldığıDers.Remove(RandevuluDers);
                }
            }
            
            List<SelectListItem> OgretmenAlınanDers = (from x in AldığıDers
                                                select new SelectListItem
                                                {
                                                    Text = x.Ders_Adı,
                                                    Value = x.Ders_id.ToString()
                                                }).ToList();
            ViewBag.OgretmenAlınanDers = OgretmenAlınanDers;

            OgretmenClass ogretmen = new OgretmenClass { ogretmen = entities.tbl_Ogretmen.Find(id) };
            return PartialView(ogretmen);
        }
        [HttpPost]
        public ActionResult O_Ekle(OgretmenClass id) //Öğretmene Ders Ekleme
        {
            var Y_Ders = entities.tbl_Ders.Find(id.ders.Ders_id);
            Y_Ders.Ogretmen_id = id.ogretmen.Ogretmen_id;
            Y_Ders.Durum = true;
            tbl_AdminLog adminLog = new tbl_AdminLog()
            {
                Admin_id = Degerler.AdminId,
                İşlem_id = 6,
                Tarih = DateTime.Now,
                Ogretmen_id = Y_Ders.Ogretmen_id,
                tbl_Ders = Y_Ders,
            };
            entities.tbl_AdminLog.Add(adminLog);
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('Öğretmene Ders Eklendi');};</script>";
            return RedirectToAction("Ogretmen_Listesi", "Admin");
        }
        [HttpPost]
        public ActionResult O_Sil(OgretmenClass id) //Öğretmen Dersini Silme
        {
            var y_Ders = entities.tbl_Ders.Find(id.ders.Ders_id);
            y_Ders.Durum = false;
            tbl_AdminLog adminLog = new tbl_AdminLog()
            {
                Admin_id = Degerler.AdminId,
                İşlem_id = 7,
                Tarih = DateTime.Now,
                tbl_Ogretmen = y_Ders.tbl_Ogretmen,
                tbl_Ders = y_Ders,
            };
            entities.tbl_AdminLog.Add(adminLog);
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('Öğretmenin Dersi Silindi');};</script>";
            return RedirectToAction("Ogretmen_Listesi", "Admin");
        }



        //Öğrenci----------------------------------------------------------------------

        [HttpGet]
        public ActionResult Ogrenci_Listesi(string p)
        {
            if (Degerler.kontrol == 1)
            {
                var öğrenciler = entities.tbl_Ogrenci.Where(x => x.Ogrenci_Durum == true).OrderBy(y => y.Ogrenci_No).ToList();

                //Öğrenci Gönderelim
                if (!string.IsNullOrEmpty(p)) 
                {
                    öğrenciler = entities.tbl_Ogrenci.Where(x => x.Ogrenci_No.Contains(p) && x.Ogrenci_Durum == true).ToList();
                }

                
                List<SelectListItem> Listogrenci = (from x in öğrenciler.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.Ogrenci_Ad + " " + x.Ogrenci_Soyad,
                                                    Value = x.Ogrenci_id.ToString()
                                                }).ToList();
                ViewBag.ogrenci = Listogrenci;

                OgrenciClass ogrenci = new OgrenciClass() { List_Ogrenci = öğrenciler.ToList()};
                return View(ogrenci);
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }
        [HttpPost]
        public ActionResult Ogrenci_Silme(OgrenciClass id) //Öğrenci Silme
        {
            if (id.No == null) //Boş direk kaydet butonuna basılır ise buraya girer
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Öğrenci Girilmedi');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin");
            }
            else
            {
                var y_Ogrenci = entities.tbl_Ogrenci.Where(x => x.Ogrenci_No == id.No).ToList();
                var y_Ogrenci_2 = entities.tbl_Ogrenci.Find(y_Ogrenci[0].Ogrenci_id);
                y_Ogrenci_2.Ogrenci_Durum = false;
                tbl_AdminLog adminLog = new tbl_AdminLog()
                {
                    Admin_id = Degerler.AdminId,
                    İşlem_id = 4,
                    Tarih = DateTime.Now,
                    tbl_Ogrenci = y_Ogrenci_2,
                };
                entities.tbl_AdminLog.Add(adminLog);
                entities.SaveChanges();
                TempData["msg"] = "<script>window.onload = function(){ alert('Öğrenci Kaldırıldı');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin");
            }
            
        }
        [HttpPost]
        public ActionResult DersEkle(int? id)
        {
            // Öğrencinin almadığı dersleri Gönderelim
            var AldığıDersler = entities.tbl_AlınanDersler.Where(x => x.Ogrenci_id == id && x.Durum == true).ToList();
            AldığıDersler = AldığıDersler.OrderBy(x => x.Ders_id).ToList();
            var BütünDersler = entities.tbl_Ders.ToList();
            List<tbl_Ders> AlmadığıDers = new List<tbl_Ders>();
            int i = 0;
            if (AldığıDersler.Count == 0)
            {
                foreach (var item in BütünDersler)
                {
                    AlmadığıDers.Add(item);
                }
            }
            else
            {
                foreach (var BDers in BütünDersler) 
                {
                    if (AldığıDersler[i].Ders_id != BDers.Ders_id)
                    {
                        AlmadığıDers.Add(BDers);
                    }
                    else
                    {
                        i++;
                        if (i == (AldığıDersler.Count))
                        {
                            i--;
                        }
                    }
                }
            }
            

            List<SelectListItem> Almadığı_dersList = (from x in AlmadığıDers
                                                select new SelectListItem
                                                {
                                                    Text = x.Ders_Adı,
                                                    Value = x.Ders_id.ToString()
                                                }).ToList();
            ViewBag.AlmadığıDersList = Almadığı_dersList;

            

            OgrenciClass ogrenci = new OgrenciClass { List_Ders = AldığıDersler, Ogrenci = entities.tbl_Ogrenci.Find(id)};
            return PartialView(ogrenci);
        }
        [HttpPost]
        public ActionResult Ekle(OgrenciClass id) //Öğrenciye Ders Ekleme
        {
            tbl_AlınanDersler y_AlınanDers = new tbl_AlınanDersler() { Ders_id = id.Ders.Ders_id, Ogrenci_id = id.Ogrenci.Ogrenci_id, Durum = true};
            entities.tbl_AlınanDersler.Add(y_AlınanDers);
            tbl_AdminLog adminLog = new tbl_AdminLog()
            {
                Admin_id = Degerler.AdminId,
                İşlem_id = 9,
                Tarih = DateTime.Now,
                Ders_id = id.Ders.Ders_id,
                Ogrenci_id = id.Ogrenci.Ogrenci_id
            };
            entities.tbl_AdminLog.Add(adminLog);
            entities.SaveChanges();
            TempData["msg"] = "<script>window.onload = function(){ alert('Öğrenciye Ders Eklendi');};</script>";
            return RedirectToAction("Ogrenci_Listesi", "Admin");
        }
        [HttpPost]
        public ActionResult DersSil(int? id)
        {
            // Öğrencinin aldığı dersleri gönderelim
            var List_Dersler = entities.tbl_AlınanDersler.Where(x => x.Ogrenci_id == id && x.Durum == true).ToList();
            List<SelectListItem> AlınanDers = (from x in List_Dersler
                                               select new SelectListItem
                                               {
                                                   Text = x.tbl_Ders.Ders_Adı,
                                                   Value = x.AlınanDers_id.ToString()
                                               }).ToList();
            ViewBag.AlınanDersler = AlınanDers;
            OgrenciClass ogrenci = new OgrenciClass { List_Ders = List_Dersler, Ogrenci = entities.tbl_Ogrenci.Find(id) };
            return PartialView(ogrenci);
        }
        [HttpPost]
        public ActionResult DersiniSil(OgrenciClass id) //Öğrencinin Dersini Silme
        {
            if (id.A_Ders == null) //Hata Mesajı Versin
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Öğrencinin Dersi Silinemedi');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin");
            }
            else 
            {
               var y_AlınanDers = entities.tbl_AlınanDersler.Find(id.A_Ders.AlınanDers_id);
                y_AlınanDers.Durum = false;
                tbl_AdminLog adminLog = new tbl_AdminLog()
                {
                    Admin_id = Degerler.AdminId,
                    İşlem_id = 10,
                    Tarih = DateTime.Now,
                    tbl_Ogrenci = y_AlınanDers.tbl_Ogrenci,
                    tbl_Ders = y_AlınanDers.tbl_Ders,
                };
                entities.tbl_AdminLog.Add(adminLog);
                entities.SaveChanges();
                TempData["msg"] = "<script>window.onload = function(){ alert('Öğrencinin Dersi Kaldırıldı');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin"); 
            }
            
        }
        
        [HttpPost]
        public ActionResult Ogrenci_Ekleme(tbl_Ogrenci ogrenci) //Yeni Öğrenci Eklendi
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Hatta');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin");
            }
            else //bunların hepsi doldurulmalı.
            {
                tbl_Ogrenci y_Ogrenci = new tbl_Ogrenci
                {
                    Ogrenci_Ad = ogrenci.Ogrenci_Ad,
                    Ogrenci_Soyad = ogrenci.Ogrenci_Soyad,
                    Ogrenci_No = ogrenci.Ogrenci_No,
                    Ogrenci_Durum = true,
                    Ogrenci_Tc = ogrenci.Ogrenci_Tc,
                };
                entities.tbl_Ogrenci.Add(y_Ogrenci);
                tbl_AdminLog adminLog = new tbl_AdminLog()
                {
                    Admin_id = Degerler.AdminId,
                    İşlem_id = 2,
                    Tarih = DateTime.Now,
                    tbl_Ogrenci = y_Ogrenci,
                };
                entities.tbl_AdminLog.Add(adminLog);
                entities.SaveChanges();
                TempData["msg"] = "<script>window.onload = function(){ alert('Yeni Öğrenci Eklendi');};</script>";
                return RedirectToAction("Ogrenci_Listesi", "Admin");
            }
        }
    }
}