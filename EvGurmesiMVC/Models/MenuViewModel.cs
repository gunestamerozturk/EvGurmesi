using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvGurmesiMVC.Models
{
    public class MenuViewModel
    {
        public IEnumerable<Menu> Menu { get; set; }
        public IEnumerable<KullaniciYemek> KullaniciYemek { get; set; }
    }
}