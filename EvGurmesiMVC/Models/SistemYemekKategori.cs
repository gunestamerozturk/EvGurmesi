using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class SistemYemekKategori
    {
        [Key]
        public int SistemYemekKategoriID { get; set; }
        public string SistemYemekKategoriAdi { get; set; }
        virtual public ICollection<SistemYemek> SistemYemeks { get; set; }
    }
}