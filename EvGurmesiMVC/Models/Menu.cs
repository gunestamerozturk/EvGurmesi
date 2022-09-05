using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string MenuAd { get; set; }
        public double MenuFiyat { get; set; }
        public int MenuAdet { get; set; }
        public string MenuAktif { get; set; }
        public string Glutensiz { get; set; }
        virtual public ICollection<KullaniciYemek> KullaniciYemeks { get; set; }
        virtual public Kullanici Kullanici { get; set; }
    }
}