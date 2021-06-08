using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proje.Models.EntityFramework;
using Proje.Models;

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text;

namespace Proje.Controllers
{
    public class RandevuAlController : Controller
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        // GET: RandevuAl
        [HttpGet]
        public ActionResult Index()
        {
            if (Degerler.kontrol == 2)
            {
                //hoca id sine göre dersleri getirme:
                var drs = entities.tbl_Ders.Where(p => p.Ogretmen_id == Degerler.id && p.Durum == true);
                List<SelectListItem> kat = (from x in drs.ToList()
                                            select new SelectListItem
                                            {
                                                Text = x.Ders_Adı,
                                                Value = x.Ders_id.ToString()
                                            }).ToList();
                ViewBag.ders = kat;

                DateTime Zaman = DateTime.Now;
                string yıl = Zaman.Year.ToString();
                string ay = tarih(Zaman.Month);
                string gün = tarih(Zaman.Day);
                string saat = tarih(Zaman.Hour);
                string dakika = tarih(Zaman.Minute);
                ViewBag.YazılacakZaman = yıl + "-" + ay + "-" + gün + "T" + saat + ":" + dakika;
                

                return View();
            }
            else
            {
                return RedirectToAction("Gırıs", "Giris");
            }
        }
        private string tarih(int num)
        {
            if (num < 10)
            {
                return "0" + num;
            }
            else
            {
                return num.ToString();
            }
        }
        [HttpPost]
        public ActionResult Index(tbl_YapılacakDers ders, DateTime Tarih_Saat)
        {
            if (DateTime.Now >= Tarih_Saat)
            {
                TempData["msg"] = "<script>window.onload = function(){ alert('Önceki Tarihlere Randevu Alınamaz');};</script>";
                return RedirectToAction("Index", "RandevuAl");
                //return JavaScript(alert("Önceki Tarihlere Meet alınamaz"));
                //return Content("<script language='javascript' type='text/javascript'>alert('Önceki Tarihlere Meet Alınamaz');</script>");
                //return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>" };
            }
            else 
            {
                var YapılacakDersler = entities.tbl_YapılacakDers.Where(x => x.tbl_Ders.Ogretmen_id == Degerler.id && x.tbl_Ders.Durum == true
                && x.Durum == true);
                bool olmaz = true;
                foreach (var item in YapılacakDersler)
                {
                    if (item.Tarih_Saat < Tarih_Saat && item.Kapama > Tarih_Saat) //Aldığı saate Almak istiyor burada
                    {
                        olmaz = false;
                        break;
                    }
                }

                if (olmaz == true)
                {
                    var drs = entities.tbl_Ders.Where(x => x.Ders_id == ders.tbl_Ders.Ders_id && x.Durum == true).FirstOrDefault();
                    ders.tbl_Ders = drs;
                    int kontrol = ZoomAl(ders);
                    if (kontrol == 201) //Onaylandı ve meet alındı
                    {
                        var ogr = entities.tbl_Ogretmen.Where(x => x.Ogretmen_id == Degerler.id).FirstOrDefault();

                        ders.Kapama = Tarih_Saat.AddMinutes(30);
                        ders.Durum = true;

                        entities.tbl_YapılacakDers.Add(ders);
                        entities.SaveChanges();
                        if (Tarih_Saat < DateTime.Now.AddMinutes(10)) //direk 10 dakika içinde alırsa buraya geliyor
                        {
                            tbl_OnlineDers O_ders = new tbl_OnlineDers { YapılacakDers_id = ders.YapılacakDers_id };
                            entities.tbl_OnlineDers.Add(O_ders);
                            entities.SaveChanges();
                        }
                        
                        TempData["msg"] = "<script>window.onload = function(){ alert('Randevunuz Alındı');};</script>";
                        return RedirectToAction("Index", "RandevuAl");
                    }
                    else //meet alınamadı.
                    {
                        Console.WriteLine();
                        TempData["msg"] = "<script>window.onload = function(){ alert('Meet Alma Hatası Oluştu: "+ kontrol +"');};</script>";
                        
                        return RedirectToAction("Index", "RandevuAl");
                    }
                    
                }
                else //O tarih ve saate çakışan meeti var.
                {
                    TempData["msg"] = "<script>window.onload = function(){ alert('Belirtiğiniz Saate Randevunuz Var');};</script>";
                    return RedirectToAction("Index", "RandevuAl");
                }
            }
        }
        private int ZoomAl(tbl_YapılacakDers ders)
        {
            var ogretmen = entities.tbl_Ogretmen.Where(x => x.Ogretmen_id == Degerler.id).ToList();

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = ogretmen[0].Api_secret; //apisecret
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = ogretmen[0].Api_Key, //Api key 
                //Expires = now.AddSeconds(300),
                Expires = ders.Tarih_Saat,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var client = new RestClient(ogretmen[0].Client);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            string Adı = ders.tbl_Ders.Ders_Adı + "--" + ders.Tarih_Saat;

            request.AddJsonBody(new { topic = Adı, duration = "30", start_time = ders.Tarih_Saat, type = "2", host_video = "true" });
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObject = JObject.Parse(restResponse.Content);
            ders.Host_Link = (string)jObject["start_url"];
            ders.Join_Link = (string)jObject["join_url"];
            int Code = numericStatusCode;
            return Code;
        }
    }
}