using OBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBS.Controllers
{
    public class HomeController : Controller
    {
        OBSDbEntities ent = new OBSDbEntities();
        public ActionResult Index()
        {
            ent.Ogrenci.ToList();
            return View();
        }

     

        public ActionResult Login()
        {
            return View();
        }
        //Admin girişi ve öğrenci girişi burdan olup kullanıcıların  Kullanıcı Adi = ad.soyad // Şifre = tcno olarak ayarlandı.
        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            if (username != "" && pass != "")
            {
                string Sifre=PasswordOperations.EncryptString("", pass); //Hashlenen şifre çözülerek giriş yapılıyor.
                Kullanicilar u = ent.Kullanicilar.Where(x => x.KullaniciAdi == username && x.Sifre == Sifre).FirstOrDefault();
       
                if (u != null)
                {
                
                    if (u.Tur==false)//admin ise false öğrenci ise true
                    {
                        Session["kullBilgi"] = u;
                        return RedirectToAction("Index", "Admin");
                    }
                    else { 

                    Session["kullBilgi"] = u;
                     
                        return RedirectToAction("KimlikInfo", "Student");
                }
                }
                else
                {
                    //kullancı bilgileri yanlış hatası verilebilir .
                }
            }
            return View();
        }
    }
}