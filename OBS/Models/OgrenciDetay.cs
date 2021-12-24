using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBS
{
    public class OgrenciDetay
    {
     /// <summary>
     /// iletişim
     /// </summary>
        public string Adres { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Email { get; set; }
        public string GSM { get; set; }

      /// <summary>
      /// kimlik
      /// </summary>
        public string TcNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string DogumYeri { get; set; }
        public Nullable<System.DateTime> DogumTarihi { get; set; }


   /// <summary>
   /// kullanicilar
   /// </summary>
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public bool Tur { get; set; }
      /// <summary>
      /// öğrenci
      /// </summary>
        public int MufredatID { get; set; }
        public string MufredatAdi { get; set; }
        public int? OgrenciID { get; set; }
        public string OgrenciNo { get; set; }
        public int OgrGirisNo { get; set; }
        public int BolumID { get; set; }
        public List<SelectListItem> DDLMufredat { get; set; }
        public List<SelectListItem> DDLBolumler { get; set; }

    }
}