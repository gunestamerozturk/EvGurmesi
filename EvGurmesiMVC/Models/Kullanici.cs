using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciID { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string KullaniciAd { get; set; }
        public string KullaniciSoyad { get; set; }
        public string KullaniciResim { get; set; }
        public string KullaniciPazarAdi { get; set; }
        public string KullaniciMail { get; set; }
        public string KullaniciTelefon { get; set; }
        public string KullaniciSehir { get; set; }
        public string Kullaniciilce { get; set; }
        public string KullaniciAdres { get; set; }
        
        public double KullaniciPuan { get; set; }
        public string KullaniciTipi { get; set; }
        public string KullaniciAktif { get; set; }
        public string KullaniciDukkanResim { get; set; }
        public string KullaniciGlutensiz { get; set; }
        virtual public ICollection<KullaniciYemek> KullaniciYemeks { get; set; }
        virtual public ICollection<Siparisler> Siparislers { get; set; }

        virtual public ICollection<Menu> Menus { get; set; }
    }
}