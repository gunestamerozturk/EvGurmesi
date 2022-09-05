using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class Siparisler
    {
        [Key]
        public int SiparisID { get; set; }
        public string SiparisYorum { get; set; }
        public double SiparisPuan { get; set; }
        public string Siparisicerik { get; set; }
        public string SiparisDurum { get; set; }
        public DateTime SiparisTarih { get; set; }
        public int SiparisAdet { get; set; }
        public double SiparisToplam { get; set; }
        virtual public Kullanici SaticiKullanici { get; set; }
        virtual public Kullanici AliciKullanici { get; set; }
        virtual public ICollection<KullaniciYemek> KullaniciYemeks { get; set; }
    }
}