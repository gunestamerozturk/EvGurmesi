using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class SistemYemek
    {
        [Key]
        public int SistemYemekID { get; set; }
        public string SistemYemekAdi { get; set; }
        public string SistemYemekicerik { get; set; }
        public string SistemYemekResim { get; set; }
        virtual public SistemYemekKategori SistemYemekKategori { get; set; }
        virtual public ICollection<KullaniciYemek> KullaniciYemeks { get; set; }
    }
}