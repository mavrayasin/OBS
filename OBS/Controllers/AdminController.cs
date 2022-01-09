using OBS.Models;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        OBSDbEntities ent = new OBSDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
        #region Ders İşlemleri
        public ActionResult Dersler()
        {
            var ders = ent.Dersler.ToList();
            return View(ders);
        }
        public ActionResult EkleDers()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EkleDers(Dersler ders)
        {
            if (ModelState.IsValid)
            {
                ent.Dersler.Add(ders);
                ent.SaveChanges();
                return RedirectToAction("Dersler");
            }

            return View();
        }
        public ActionResult DuzenleDers(int? id)
        {
            var dersValue = ent.Dersler.Where(x => x.ID == id).FirstOrDefault();
            return View(dersValue);
        }
        [HttpPost]
        public ActionResult DuzenleDers(Dersler model)
        {
            var Dersler = ent.Dersler.Where(x => x.ID == model.ID).FirstOrDefault();
            Dersler.DersAdi = model.DersAdi;
            Dersler.DersKodu = model.DersKodu;
            Dersler.Durum = model.Durum;
            Dersler.Kredi = model.Kredi;
            ent.SaveChanges();
            return RedirectToAction("Dersler");
        }
        #endregion

        #region Müfredat İşlemleri

        public ActionResult Mufredat()
        {
            var muf = ent.Mufredat.ToList();
            return View(muf);
        }
        public ActionResult EkleMufredat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EkleMufredat(Mufredat mufredat)
        {
            if (ModelState.IsValid)
            {
                ent.Mufredat.Add(mufredat);
                ent.SaveChanges();
                return RedirectToAction("Mufredat");
            }

            return View();
        }

        public ActionResult SilMufredat(int id)
        {
            var mufids = ent.MufredatDersler.Where(x => x.MufredatID == id).ToList();
            foreach (var item in mufids)
            {
                ent.MufredatDersler.Remove(item);
                ent.SaveChanges();
            }
            var muf = ent.Mufredat.Where(x => x.ID == id).FirstOrDefault();
            ent.Mufredat.Remove(muf);
            ent.SaveChanges();

           return RedirectToAction("Mufredat");
        }
        public ActionResult DuzenleMufredat(int? id)
        {
            var mufredatValue = ent.Mufredat.Where(x => x.ID == id).FirstOrDefault();
            return View(mufredatValue);
        }
        [HttpPost]
        public ActionResult DuzenleMufredat(Mufredat model)
        {
            var Mufredat = ent.Mufredat.Where(x => x.ID == model.ID).FirstOrDefault();
            Mufredat.MufredatAdi = model.MufredatAdi;
            ent.SaveChanges();
            return RedirectToAction("Mufredat");
        }
        
        public ActionResult MufredatClone(int id)
        {
            Mufredat mufredat = new Mufredat();
            mufredat.ID = id;
            return View(mufredat);
        }
        [HttpPost]
        public ActionResult MufredatClone(Mufredat model)
        {
            var dersids = ent.MufredatDersler.Where(x => x.MufredatID == model.ID).Select(x=>x.DersID).ToList();
            Mufredat mufredat = new Mufredat()
            {
                MufredatAdi = model.MufredatAdi
            };
            ent.Mufredat.Add(model);
            ent.SaveChanges();
            int mufredatID = model.ID;

            List<MufredatDersler> lst = new List<MufredatDersler>();

            foreach(var item in dersids)
            {
                MufredatDersler temp = new MufredatDersler();
                temp.MufredatID = mufredatID;
                temp.DersID = item;
                ent.MufredatDersler.Add(temp);
                ent.SaveChanges();
            }
            //ent.MufredatDersler.AddRange(lst);

            return RedirectToAction("MufredatDersList");
        }
        #endregion

        #region Müfredat Ders İlişkilendirme
        public ActionResult MufredatDers()
        {
            MufredatDersDetay mufredatDersDetay = new MufredatDersDetay();
            var mufDet = ent.Mufredat.ToList();
            var dersDet = ent.Dersler.ToList();
            mufredatDersDetay.DDLMufredat = mufDet.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.MufredatAdi,
                    Value = x.ID.ToString()
                };
            }).OrderBy(x => x.Text).ToList();
            mufredatDersDetay.DDLDers = dersDet.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.DersAdi,
                    Value = x.ID.ToString()
                };
            }).OrderBy(x => x.Text).ToList();
            return View(mufredatDersDetay);
        }
        [HttpPost]
        public ActionResult MufredatDers(MufredatDersDetay mufredatDersDetay)
        {
            MufredatDersler mufredatDersler = new MufredatDersler();
            mufredatDersler.DersID = mufredatDersDetay.DersID;
            mufredatDersler.MufredatID = mufredatDersDetay.MufredatID;
            ent.MufredatDersler.Add(mufredatDersler);
            try
            {
                ent.SaveChanges();
            }
            catch (Exception )
            {

                throw;
            }
            return RedirectToAction("MufredatDersList");
        }

        public ActionResult MufredatDersList()
        {
            var mufderss = ent.MufredatDersler.ToList();

            return View(mufderss);
        }

        #endregion

        #region Öğrenci Ekleme, Listeleme ve Güncelleme İşlemleri
        public ActionResult AddStudent()
        {
            OgrenciDetay ogrenciDetay = new OgrenciDetay();
            var mufredat = ent.Mufredat.ToList();
            ogrenciDetay.DDLMufredat = mufredat.ConvertAll(a =>
                 {
                     return new SelectListItem()
                     {
                         Text = a.MufredatAdi,
                         Value = a.ID.ToString()
                     };
                 }).OrderBy(x => x.Text).ToList();
            var bolumler = ent.Bolumler.Where(x => x.AktifPasif == 1).ToList();
            ogrenciDetay.DDLBolumler = bolumler.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.BolumAd,
                    Value = a.BolumNo.ToString()
                };
            }).OrderBy(x => x.Text).ToList();
            return View(ogrenciDetay);
        }

        [HttpPost]
        public ActionResult AddStudent(OgrenciDetay model)
        {

            Iletisim iletisim = new Iletisim();
            Kimlik kimlik = new Kimlik();
            Kullanicilar kullanici = new Kullanicilar();
            Ogrenci ogrenci = new Ogrenci();
            iletisim.Adres = model.Adres;
            iletisim.Il = model.Il;
            iletisim.Ilce = model.Ilce;
            iletisim.Email = model.Email;
            iletisim.GSM = model.GSM;
            kimlik.TcNo = model.TcNo;
            kimlik.Ad = model.Ad;
            kimlik.Soyad = model.Soyad;
            kimlik.DogumYeri = model.DogumYeri;
            kimlik.DogumTarihi = model.DogumTarihi;
            ent.Iletisim.Add(iletisim);

            try
            {
                ent.SaveChanges();
                var sonIletisimID = iletisim.ID;

                try
                {
                    kimlik.IletisimID = sonIletisimID;
                    ent.Kimlik.Add(kimlik);
                    try
                    {
                        ent.SaveChanges();
                        var sonKimlikID = kimlik.ID;
                        ogrenci.OgrenciNo = DateTime.Now.Year.ToString() + model.BolumID.ToString() + model.OgrGirisNo.ToString(); // öğrenci no alanı bolumID ve giriş sırası birleştirilerek yapıldı.
                        ogrenci.MufredatID = model.MufredatID;
                        ogrenci.KimlikID = sonKimlikID;
                        ent.Ogrenci.Add(ogrenci);
                        try
                        {
                            ent.SaveChanges();
                            kullanici.KullaniciAdi = kimlik.Ad + "." + kimlik.Soyad; //kullanıcı Adı ad.soyad şeklinde otomatik kayıt atılıyor.
                            kullanici.Sifre = PasswordOperations.EncryptString("",kimlik.TcNo); //şifre tcno olarak atandı.
                            kullanici.KimlikID = sonKimlikID;
                            kullanici.Tur = true;
                            try
                            {
                                ent.Kullanicilar.Add(kullanici);
                                ent.SaveChanges();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        catch (Exception )
                        {

                            throw;
                        }
                    }
                    catch (Exception )
                    {

                        throw;
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception )
            {

            }
            return RedirectToAction("StudentList", "Admin");
        }
        public ActionResult UpdateStudent(int OgrenciId)
        {
            OgrenciDetay ogrenciDetay = new OgrenciDetay();

            var mufredat = ent.Mufredat.ToList();
            ogrenciDetay.DDLMufredat = mufredat.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.MufredatAdi,
                    Value = a.ID.ToString()
                };
            }).OrderBy(x => x.Text).ToList();

            var sorgu = (from ogr in ent.Ogrenci
                         join kmlk in ent.Kimlik on ogr.KimlikID equals kmlk.ID
                         join ilet in ent.Iletisim on kmlk.IletisimID equals ilet.ID
                         join muf in ent.Mufredat on ogr.MufredatID equals muf.ID
                         select new
                         {
                             kmlk.TcNo,
                             kmlk.Ad,
                             kmlk.Soyad,
                             kmlk.DogumYeri,
                             kmlk.DogumTarihi,
                             ilet.Adres,
                             ilet.Il,
                             ilet.Ilce,
                             ilet.Email,
                             ilet.GSM,
                             ogr.OgrenciNo,
                             ogr.ID,
                             muf.MufredatAdi,
                         }).Where(x => x.ID == OgrenciId).FirstOrDefault();
            ogrenciDetay.TcNo = sorgu.TcNo;
            ogrenciDetay.Ad = sorgu.Ad;
            ogrenciDetay.Soyad = sorgu.Soyad;
            ogrenciDetay.DogumYeri = sorgu.DogumYeri;
            ogrenciDetay.DogumTarihi = sorgu.DogumTarihi;
            ogrenciDetay.Adres = sorgu.Adres;
            ogrenciDetay.Il = sorgu.Il;
            ogrenciDetay.Ilce = sorgu.Ilce;
            ogrenciDetay.Email = sorgu.Email;
            ogrenciDetay.GSM = sorgu.GSM;
            ogrenciDetay.OgrenciNo = sorgu.OgrenciNo;
            ogrenciDetay.OgrenciID = sorgu.ID;
            ogrenciDetay.MufredatAdi = sorgu.MufredatAdi;
            return View(ogrenciDetay);
        }
        [HttpPost]
        public ActionResult UpdateStudent(OgrenciDetay model)
        {
            var ogrenci = ent.Ogrenci.Where(x => x.ID == model.OgrenciID).FirstOrDefault();
            var kimlik = ent.Kimlik.Where(x => x.ID == ogrenci.KimlikID).FirstOrDefault();
            var iletisim = ent.Iletisim.Where(x => x.ID == kimlik.IletisimID).FirstOrDefault();
            kimlik.TcNo = model.TcNo;
            kimlik.Ad = model.Ad;
            kimlik.Soyad = model.Soyad;
            kimlik.DogumYeri = model.DogumYeri;
            kimlik.DogumTarihi = model.DogumTarihi;
            iletisim.Adres = model.Adres;
            iletisim.Il = model.Il;
            iletisim.Ilce = model.Ilce;
            iletisim.Email = model.Email;
            iletisim.GSM = model.GSM;
            ogrenci.MufredatID = model.MufredatID;
            try
            {
                ent.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("StudentList");
        }
        public ActionResult StudentList()
        {
            var sorgu = from ogr in ent.Ogrenci
                        join kmlk in ent.Kimlik on ogr.KimlikID equals kmlk.ID
                        join ilet in ent.Iletisim on kmlk.IletisimID equals ilet.ID
                        join muf in ent.Mufredat on ogr.MufredatID equals muf.ID
                        select new
                        {
                            kmlk.TcNo,
                            kmlk.Ad,
                            kmlk.Soyad,
                            kmlk.DogumYeri,
                            kmlk.DogumTarihi,
                            ilet.Adres,
                            ilet.Il,
                            ilet.Ilce,
                            ilet.Email,
                            ilet.GSM,
                            ogr.OgrenciNo,
                            ogr.ID,
                            muf.MufredatAdi,
                        };
            List<OgrenciDetay> lst = new List<OgrenciDetay>();
            foreach (var item in sorgu)
            {
                OgrenciDetay model = new OgrenciDetay();
                model.TcNo = item.TcNo;
                model.Ad = item.Ad;
                model.Soyad = item.Soyad;
                model.DogumYeri = item.DogumYeri;
                model.DogumTarihi = item.DogumTarihi;
                model.Adres = item.Adres;
                model.Il = item.Il;
                model.Ilce = item.Ilce;
                model.Email = item.Email;
                model.GSM = item.GSM;
                model.OgrenciNo = item.OgrenciNo;
                model.OgrenciID = item.ID;
                model.MufredatAdi = item.MufredatAdi;
                lst.Add(model);
            }
            return View(lst);
        }

        #endregion
        public ActionResult DersDetay(int ogrid)
        {
            var kayitlidersler = ent.DersKayit.Where(x => x.OgrenciID == ogrid).Select(x => x.DersID);
            var dersler = ent.Dersler.Where(x => kayitlidersler.Contains(x.ID)).ToList();
            return View(dersler);
        }

    }
}

