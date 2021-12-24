using OBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBS.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        OBSDbEntities ent = new OBSDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
        public ActionResult KimlikInfo()
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
            
            //var adsoyad = ent.Kimlik.Where(x => x.ID == u.KimlikID).Select(x => x.Ad + x.Soyad).FirstOrDefault().ToString();
            //ViewBag.Name = adsoyad;
            ////var TempDateVeri = TempData["Veri"];
            var kmlk = ent.Kimlik.Where(x => x.ID == u.KimlikID).ToList();
            return View(kmlk);
        }
        public ActionResult IletisimInfo()
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
           
            //var adsoyad = ent.Kimlik.Where(x => x.ID == u.KimlikID).Select(x => x.Ad + x.Soyad).FirstOrDefault().ToString();
            //ViewBag.Name = adsoyad;
            var ilet = ent.Kimlik.Where(x => x.ID == u.KimlikID).ToList();
            return View(ilet);
        }
        public ActionResult UpdateIletisim(int IletisimID)
        {
            var iletValue = ent.Iletisim.Where(x => x.ID == IletisimID).FirstOrDefault();

            return View(iletValue);
        }
        [HttpPost]
        public ActionResult UpdateIletisim(Iletisim model)
        {
           
            var ilet = ent.Iletisim.Where(x => x.ID == model.ID).FirstOrDefault();
            ilet.Adres = model.Adres;
            ilet.Il = model.Il;
            ilet.Ilce = model.Ilce;
            ilet.Email = model.Email;
            ilet.GSM = model.GSM;
            ent.SaveChanges();
            return RedirectToAction("IletisimInfo");
        }
        public ActionResult DersKayit()
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
            var mufid = ent.Ogrenci.Where(x => x.KimlikID == u.KimlikID).Select(x=>x.MufredatID).FirstOrDefault();
            var mufders = ent.MufredatDersler.Where(x => x.MufredatID == mufid).Select(x => x.DersID).ToList();
            var dersler = ent.Dersler.Where(x => mufders.Contains(x.ID) && x.Durum==true).ToList();
            
            return View(dersler);
        }

        public string DersKaydiYap(int DersID)
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
            DersKayit dersKayit = new DersKayit();
            bool derskayitlimi = true;
            var ogrid = ent.Ogrenci.Where(x => x.KimlikID == u.KimlikID).Select(x => x.ID).FirstOrDefault();

            var dk = ent.DersKayit.Where(x => x.DersID == DersID && x.OgrenciID == ogrid).FirstOrDefault();
            ViewBag.DersKayitliMi = 0;
            if (dk == null) {
            derskayitlimi = false;
            dersKayit.OgrenciID = ogrid;
            dersKayit.DersID = DersID;
            dersKayit.CreateDate = DateTime.Now;
            ent.DersKayit.Add(dersKayit);
            ent.SaveChanges();
            return "0";
            }
            else
            {
                return  "1";
            }
        }
        public ActionResult OgrenciDers()
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
            var ogrid = ent.Ogrenci.Where(x => x.KimlikID == u.KimlikID).Select(x => x.ID).FirstOrDefault();
            var kayitlidersler = ent.DersKayit.Where(x=>x.OgrenciID == ogrid).Select(x=>x.DersID);
            var dersler = ent.Dersler.Where(x => kayitlidersler.Contains(x.ID)).ToList();
            return View(dersler);
        }
        public void DersKaydiSil(int DersId)
        {
            Kullanicilar u = Session["kullBilgi"] as Kullanicilar;
            var ogrid = ent.Ogrenci.Where(x => x.KimlikID == u.KimlikID).Select(x => x.ID).FirstOrDefault();
            var kayitliders = ent.DersKayit.Where(x => x.OgrenciID == ogrid && x.DersID==DersId).FirstOrDefault();
            ent.DersKayit.Remove(kayitliders);
            ent.SaveChanges();
        }
    }
}