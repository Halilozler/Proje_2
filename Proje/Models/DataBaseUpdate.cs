using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proje.Models.EntityFramework;

namespace Proje.Models
{
    public class DataBaseUpdate
    {
        OnlineEğitimEntities entities = new OnlineEğitimEntities();
        private DateTime ş_zaman = DateTime.Now;    //VERİTABANINI SAATİNE GÖRE OLMALI
        private DateTime zaman = DateTime.Now.AddMinutes(+10);

        tbl_OnlineDers ders;
        tbl_YapılanDersLog log;

        public DataBaseUpdate()
        {
            var dersZamanlar = entities.tbl_YapılacakDers.Where(x => x.Durum == true).ToList();
            var İptalDers = entities.tbl_YapılacakDers.Where(x => x.Durum == false).ToList();

            foreach (var item in İptalDers)
            {
                DateTime a = (DateTime)item.Tarih_Saat;
                a = a.AddDays(+7);
                if (DateTime.Now > a)
                {
                    entities.tbl_YapılacakDers.Remove(item);
                    entities.SaveChanges();
                }
            }

            foreach (var item in dersZamanlar)
            {
                if (zaman >= item.Tarih_Saat && entities.tbl_OnlineDers.Where(x => x.YapılacakDers_id == item.YapılacakDers_id).ToList().Count() == 0)
                {
                    ders = new tbl_OnlineDers();
                    ders.YapılacakDers_id = item.YapılacakDers_id;
                    entities.tbl_OnlineDers.Add(ders);
                    entities.SaveChanges();
                }
                if (ş_zaman >= item.Kapama)
                {
                    log = new tbl_YapılanDersLog();
                    log.Ders_id = item.Ders_id;
                    log.Açma = item.Tarih_Saat;
                    log.Kapama = item.Kapama;
                    entities.tbl_YapılanDersLog.Add(log);

                    var onlineDersSilinecek = entities.tbl_OnlineDers.Where(x => x.YapılacakDers_id == item.YapılacakDers_id).ToList();

                    entities.tbl_OnlineDers.Remove(entities.tbl_OnlineDers.Find(onlineDersSilinecek[0].OnlineDers_id));
                    entities.tbl_YapılacakDers.Remove(item);
                    entities.SaveChanges();

                }

            }
        }
    }
}