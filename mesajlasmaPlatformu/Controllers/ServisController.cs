using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.DynamicData;
using System.Web.Http;
using mesajlasmaPlatformu.Models;
using mesajlasmaPlatformu.ViewModel;

namespace mesajlasmaPlatformu.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();



        #region Kayit

        [HttpGet]
        [Route("api/Kisilermesajliste/{kisiId}")]

        public List<KayitModel> KisilerMesajListe(string kisiId)
        {

            List<KayitModel> liste = db.Kayit.Where(s => s.kayitKisiId == kisiId).Select(x => new
              KayitModel()
            {
                kayitId = x.kayitId,
                kayitMesajId = x.kayitMesajId,
                kayitKisiId = x.kayitKisiId,
                kayitGrupId = x.kayitGrupId,

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.kisiBilgi = KisilerById(kayit.kayitKisiId);
                kayit.mesajBilgi = MesajlarById(kayit.kayitMesajId);
            }

            return liste;
        }


        [HttpGet]
        [Route("api/Mesajkisilerliste/{mesajId}")]

        public List<KayitModel> MesajKisilerListe(string mesajId)
        {

            List<KayitModel> liste = db.Kayit.Where(s => s.kayitMesajId == mesajId).Select(x => new
              KayitModel()
            {
                kayitId = x.kayitId,
                kayitMesajId = x.kayitMesajId,
                kayitKisiId = x.kayitKisiId,
                kayitGrupId = x.kayitGrupId,


            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.kisiBilgi = KisilerById(kayit.kayitKisiId);
                kayit.mesajBilgi = MesajlarById(kayit.kayitMesajId);
            }

            return liste;
        }


        [HttpPost]
        [Route("api/Kayitekle")]

        public SonucModel KayitEkle(KayitModel model)
        {
            if (db.Kayit.Count(s => s.kayitMesajId == model.kayitMesajId && s.kayitKisiId ==
            model.kayitKisiId) > 0)

            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu kişiye aynı mesaj daha önce gönderilmiştir";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitKisiId = model.kayitKisiId;
            yeni.kayitMesajId = model.kayitMesajId;
            yeni.kayitGrupId = model.kayitGrupId;

            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Mesaj Gönderildi";

            return sonuc;
        }


        [HttpDelete]
        [Route("api/Kayitsil/{kayitId}")]


        public SonucModel KayitSil(string kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Mesaj Silindi";
            return sonuc;

        }

        #endregion


        #region Kişiler

        [HttpGet]
        [Route("api/Kisilerliste")]
        public List<KisilerModel> KisilerListe()
        {
            List<KisilerModel> liste = db.Kisiler.Select(x => new
                KisilerModel()
            {
                kisiId = x.kisiId,
                kisiAdSoyad = x.kisiAdSoyad,
                kisiNumara = x.kisiNumara,
                kisiEposta = x.kisiEposta,
                kisiYas = x.kisiYas

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/Kisilerbyid/{Kisiid}")]

        public KisilerModel KisilerById(string kisiId)
        {
            KisilerModel kayit = db.Kisiler.Where(s => s.kisiId ==
              kisiId).Select(x => new KisilerModel()
              {

                  kisiId = x.kisiId,
                  kisiAdSoyad = x.kisiAdSoyad,
                  kisiNumara = x.kisiNumara,
                  kisiEposta = x.kisiEposta,
                  kisiYas = x.kisiYas

              }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/Kisilerekle")]
        public SonucModel KisilerEkle(KisilerModel model)
        {
            if (db.Kisiler.Count(c => c.kisiNumara == model.kisiNumara) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kişi Numarasi Kayitlidir!";
                return sonuc;
            }
            Kisiler yeni = new Kisiler();
            yeni.kisiId = Guid.NewGuid().ToString();
            yeni.kisiAdSoyad = model.kisiAdSoyad;
            yeni.kisiNumara = model.kisiNumara;
            yeni.kisiEposta = model.kisiEposta;
            yeni.kisiYas = model.kisiYas;

            db.Kisiler.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kişi Eklendi";


            return sonuc;
        }

        [HttpPut]
        [Route("api/Kisilerduzenle")]

        public SonucModel KisilerDuzenle(KisilerModel model)
        {
            Kisiler kayit = db.Kisiler.Where(s => s.kisiId == model.kisiId).FirstOrDefault();
            if (kayit == null)
            {

                sonuc.islem = false;
                sonuc.mesaj = "Kayit Bulunamadi!";
                return sonuc;

            }

            kayit.kisiAdSoyad = model.kisiAdSoyad;
            kayit.kisiNumara = model.kisiNumara;
            kayit.kisiEposta = model.kisiEposta;
            kayit.kisiYas = model.kisiYas;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kişiler Güncellendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/Kisilersil/{kisiId}")]

        public SonucModel KisilerSil(string kisiId)
        {
            Kisiler kayit = db.Kisiler.Where(s => s.kisiId == kisiId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            if (db.Kayit.Count(s => s.kayitKisiId == kisiId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kişi Üzerinde Grup Kaydı Olduğu İçin Kişi Silinemez!";
                return sonuc;
            }

            db.Kisiler.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kişi Silindi";

            return sonuc;
        }

        #endregion


        #region Gruplar

        [HttpGet]
        [Route("api/Grupliste")]
        public List<GruplarModel> GruplarListe()
        {
            List<GruplarModel> liste = db.Gruplar.Select(x => new GruplarModel()
            {
                grupId = x.grupId,
                grupAdi = x.grupAdi,

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/Gruplistesoneklenenler/{s}")]
        public List<GruplarModel> GruplarListeSonEklenenler(int s)
        {
            List<GruplarModel> liste = db.Gruplar.OrderByDescending(o => o.grupId).Take(s).Select(x => new GruplarModel()
            {
                grupId = x.grupId,
                grupAdi = x.grupAdi,

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/Gruplistebykatid/{katId}")]
        public List<GruplarModel> GruplarListeByMesajId(string mesajId)
        {
            List<GruplarModel> liste = db.Gruplar.Select(x => new GruplarModel()
            {
                grupId = x.grupId,
                grupAdi = x.grupAdi,
            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/Gruplistebykisiid/{kisiId}")]
        public List<GruplarModel> KisiListeByKisiId(string kisiId)
        {
            List<GruplarModel> liste = db.Gruplar.Select(x => new GruplarModel()
            {
                grupId = x.grupId,
                grupAdi = x.grupAdi,

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/Grupbyid/{grupId}")]
        public GruplarModel GruplarById(string grupId)
        {
            GruplarModel kayit = db.Gruplar.Where(s => s.grupId == grupId).Select(x => new GruplarModel()
            {
                grupId = x.grupId,
                grupAdi = x.grupAdi,
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/Grupekle")]
        public SonucModel GruplarEkle(GruplarModel model)
        {
            if (db.Gruplar.Count(s => s.grupAdi == model.grupAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Grup Başlığı Kayıtlıdır!";
                return sonuc;
            }

            Gruplar yeni = new Gruplar();
            yeni.grupId = model.grupId;
            yeni.grupAdi = model.grupAdi;
            db.Gruplar.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Grup Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/Grupduzenle")]
        public SonucModel GruplarDuzenle(GruplarModel model)
        {
            Gruplar kayit = db.Gruplar.Where(s => s.grupId == model.grupId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.grupId = model.grupId;
            kayit.grupAdi = model.grupAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Grup Düzenlendi";
            return sonuc;

        }

        [HttpDelete]
        [Route("api/Grupsil/{grupId}")]
        public SonucModel GruplarSil(string grupId)
        {
            Gruplar kayit = db.Gruplar.Where(s => s.grupId == grupId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            db.Gruplar.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Grup Silindi";
            return sonuc;
        }

        #endregion


        #region Mesajlar

        [HttpGet]
        [Route("api/Mesajliste")]
        public List<MesajlarModel> MesajlarListe()
        {
            List<MesajlarModel> liste = db.Mesajlar.Select(x => new MesajlarModel()
            {
                mesajId = x.mesajId,
                mesajText = x.mesajText,
                kimdenId = x.kimdenId,
                kimeId = x.kimeId,
                grupId = x.grupId,
                bulkMesaj = x.bulkMesaj,
                kisiAdSoyad = x.Kisiler.kisiAdSoyad,
                

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/Mesajlistesoneklenenler/{s}")]
        public List<MesajlarModel> MesajlarListeSonEklenenler(int s)
        {
            List<MesajlarModel> liste = db.Mesajlar.OrderByDescending(o => o.mesajId).Take(s).Select(x => new MesajlarModel()
            {
                mesajId = x.mesajId,
                mesajText = x.mesajText,
                kimdenId = x.kimdenId,
                kimeId = x.kimeId,
                grupId = x.grupId,
                bulkMesaj = x.bulkMesaj,
                kisiAdSoyad = x.Kisiler.kisiAdSoyad,

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/Mesajbyid/{mesajId}")]
        public MesajlarModel MesajlarById(string mesajId)
        {
            MesajlarModel kayit = db.Mesajlar.Where(s => s.mesajId == mesajId).Select(x => new MesajlarModel()
            {
                mesajId = x.mesajId,
                mesajText = x.mesajText,
                kimdenId = x.kimdenId,
                kimeId = x.kimeId,
                grupId = x.grupId,
                bulkMesaj = x.bulkMesaj,
                kisiAdSoyad = x.Kisiler.kisiAdSoyad,
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/Mesajekle")]
        public SonucModel MesajEkle(MesajlarModel model)
        {

            Mesajlar yeni = new Mesajlar();
            yeni.mesajId = model.mesajId;
            yeni.mesajText = model.mesajText;
            yeni.bulkMesaj = model.bulkMesaj;
            yeni.grupId = model.grupId;
            yeni.kimdenId = model.kimdenId;
            yeni.kimeId = model.kimeId;


            db.Mesajlar.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Mesaj Gönderildi";
            return sonuc;
        }


        [HttpDelete]
        [Route("api/Mesajsil/{mesajId}")]
        public SonucModel MesajSil(string mesajId)
        {
            Mesajlar kayit = db.Mesajlar.Where(s => s.mesajId == mesajId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            db.Mesajlar.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Mesaj Silindi";
            return sonuc;
        }

        #endregion

    }
}
