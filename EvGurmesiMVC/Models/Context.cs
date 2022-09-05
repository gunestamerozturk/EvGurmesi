using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class Context:DbContext
    {
        public DbSet<Kullanici> Kullanicis { get; set; }
        public DbSet<KullaniciYemek> KullaniciYemeks { get; set; }
        public DbSet<Siparisler> Siparislers { get; set; }
        public DbSet<SistemYemek> SistemYemeks { get; set; }
        public DbSet<SistemYemekKategori> SistemYemekKategoris { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}