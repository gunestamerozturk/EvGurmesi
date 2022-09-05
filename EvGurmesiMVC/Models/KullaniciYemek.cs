using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class KullaniciYemek
    {
        [Key]
        public int KullaniciYemekID { get; set; }
        public string KullaniciYemekicerik { get; set; }
        virtual public Kullanici Kullanici { get; set; }
        virtual public SistemYemek SistemYemek { get; set; }
        virtual public ICollection<Siparisler> Siparislers { get; set; }
        virtual public Menu Menu { get; set; }
    }
}