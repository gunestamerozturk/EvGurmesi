using EvGurmesiMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvGurmesiMVC.Controllers
{
    //Authorize'ı sınıfın üzerine yazdığımız için içerideki tüm actionlara erişmek için kullanıcı oturum açması gerekir.
    public class HomeController : Controller
    {
        Context db = new Context();
        [Authorize]
        public ActionResult Index()
        {
            var saticilar = db.Kullanicis.Where(x => x.KullaniciAktif == "True"&&x.KullaniciYemeks.Count!=0&&x.Menus.Count!=0).ToList();          //saticilar değişkenine veritabanından Where komutu ile Aktif olan saticilari listeledik.
            return View(saticilar);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public ActionResult Profil()
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var bilgiler = db.Kullanicis.Where(x => x.KullaniciID == no).ToList();              //Giriş yapan kullanıcı bilgilerini profil sayfasında listeledik
            return View(bilgiler);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Profil(Kullanici a)
        {
            int no = int.Parse((string)Session["KullaniciID"]);                                 //Profil sayfasında yapılan değişiklikleri a parametresi ile aldık ve profil verilerine atadık.
            Kullanici profil = db.Kullanicis.Where(x => x.KullaniciID == no).FirstOrDefault();
            profil.KullaniciAd = a.KullaniciAd;
            profil.KullaniciAdres = a.KullaniciAdres;
            profil.Kullaniciilce = a.Kullaniciilce;
            profil.KullaniciMail = a.KullaniciMail;
            profil.KullaniciSehir = a.KullaniciSehir;
            profil.KullaniciSoyad = a.KullaniciSoyad;
            profil.KullaniciTelefon = a.KullaniciTelefon;

            var resim = Path.GetFileName(Request.Files[0].FileName);                            //Dosya girişi ile profil resmimizin adını, uzantısını ve yolunu belirleyerek veritabanına uzantı olarak kaydettik.
            var resimUzanti = Path.GetExtension(Request.Files[0].FileName);
            var resimYolu = "~/Resimler/Kisi" + resim + resimUzanti;
            Request.Files[0].SaveAs(Server.MapPath(resimYolu));
            a.KullaniciResim = "/Resimler/Kisi/" + resim;
            profil.KullaniciResim = a.KullaniciResim;
            Session["KullaniciResim"] = a.KullaniciResim;
            db.SaveChanges();                                                                   //Yapılan değişiklikleri kaydettik.
            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public ActionResult Dukkan()
        {

            int no = int.Parse((string)Session["KullaniciID"]);
            var bilgiler = db.Kullanicis.Where(x => x.KullaniciID == no).ToList();              //Giriş yapan kullanıcının dükkan bilgilerini dükkan sayfasında listeledik.
            return View(bilgiler);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Dukkan(Kullanici kullanici)
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            Kullanici profil = db.Kullanicis.Where(x => x.KullaniciID == no).FirstOrDefault();              //Giriş yapan kullanıcının dükkan bilgilerini dükkan sayfasında listeledik.
            profil.KullaniciGlutensiz = kullanici.KullaniciGlutensiz;                                       //kullanici parametresi ile View'den aldığımız değerleri atadık.
            profil.KullaniciPazarAdi = kullanici.KullaniciPazarAdi;                                         //Yapılan değişiklikleri veritabanına kaydettik.
            profil.KullaniciTipi = kullanici.KullaniciTipi;
            profil.KullaniciAktif = kullanici.KullaniciAktif;

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public ActionResult Menu()
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var bilgiler = db.Menus.Where(x => x.Kullanici.KullaniciID == no).ToList();                    //Giriş yapan kullanıcının eklemiş olduğu menüleri listeledik.
            return View(bilgiler);
        }


        [Authorize]
        public ActionResult MenuEkle()
        {


            return View();
        }
        [HttpPost]
        public ActionResult MenuEkle(string MenuAd)
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            Menu menu = new Menu();
            menu.MenuAd = MenuAd;
            menu.Kullanici = db.Kullanicis.Where(x => x.KullaniciID == no).FirstOrDefault();
            menu.MenuAdet = 0;
            menu.MenuFiyat = 0;
            db.Menus.Add(menu);
            db.SaveChanges();
            int eklenenID = db.Menus.Max(x => x.MenuID);
            
            return RedirectToAction("MenuDuzenle/" + eklenenID, "Home");
        }
        
        [Authorize]
        public ActionResult MenuSil(int id)                                                                 //Satıcının menüsündeki Menüyü silmesi için sileceği elemanın id değerini aldık.
        {
            var menu = db.Menus.Find(id);                                                        //id değeri eşit olan Menü değişkene atadık
            db.Menus.Remove(menu);                                                               //değişkeni tanımlayan Menüyü sildik.  
            db.SaveChanges();
            return RedirectToAction("Menu", "Home");
        }

        public ActionResult MenuDuzenle(int id)
        {
            Session["MenuNo"] = id.ToString();
            Session["MenuFiyat"] =db.Menus.Where(y=>y.MenuID==id).Select(z=>z.MenuFiyat).First().ToString();
            Session["MenuAdet"] = db.Menus.Where(j => j.MenuID == id).Select(p => p.MenuFiyat).First().ToString();
            Session["MenuAd"] = db.Menus.Where(k => k.MenuID == id).Select(h => h.MenuAd).First().ToString();
            int menuNo = int.Parse((string)Session["MenuNo"]);
            var yemekler = db.KullaniciYemeks.Where(x => x.Menu.MenuID == menuNo);
            return View(yemekler);
        }
        [HttpPost]
        public ActionResult MenuDuzenle(Menu menu)
        {
            int menuNo = int.Parse((string)Session["MenuNo"]);
            Menu duzenlenen = db.Menus.Where(x => x.MenuID == menuNo).FirstOrDefault();
            duzenlenen.MenuAdet = menu.MenuAdet;
            duzenlenen.MenuFiyat = menu.MenuFiyat;
            duzenlenen.MenuAd = menu.MenuAd;
            if (menu.MenuAktif == "true")
            {
                duzenlenen.MenuAktif = "Aktif";
            }
            else
            {
                duzenlenen.MenuAktif = "Deaktif";
            }
            db.SaveChanges();
            return RedirectToAction("Menu", "Home");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult MenuYemekSec()
        {
            var yemekler = db.SistemYemeks.ToList();
            return View(yemekler);
        }

        [Authorize]
        public ActionResult YemekEkle(int id)                                                                   //MenuEkle sayfasından aldığımız id parametresini kullanarak eklenecek yemeği getirdik.
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var eklenecek = db.SistemYemeks.Where(x=>x.SistemYemekID==id).ToList();
            
            return View(eklenecek);
        }

        [HttpPost]
        [Authorize]
        public ActionResult YemekEkle(SistemYemek kullaniciYemek,string KullaniciYemekicerik, int MenuNo, string Glutensiz)               //Eklenecek yemek ve içeriği için iki parametre aldık.
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            KullaniciYemek yeniYemek = new KullaniciYemek();                                                //Satıcı adına eklenecek yemek için yeni nesne oluşturduk.
            yeniYemek.Kullanici = db.Kullanicis.Where(x => x.KullaniciID == no).FirstOrDefault();           //eklenecek yemek için kullanıcı ve yemek bilgilerini ekledik.
            yeniYemek.SistemYemek = db.SistemYemeks.Where(b => b.SistemYemekID == kullaniciYemek.SistemYemekID).FirstOrDefault();   
            yeniYemek.KullaniciYemekicerik = KullaniciYemekicerik;
            int menuNo = int.Parse((string)Session["MenuNo"]);
            yeniYemek.Menu = db.Menus.Where(y=>y.MenuID==MenuNo).FirstOrDefault();
            if(Glutensiz=="true")
            {
                yeniYemek.Kullanici.KullaniciGlutensiz = "true";
                yeniYemek.Menu.Glutensiz = Glutensiz;
            }
            else
            {
                yeniYemek.Kullanici.KullaniciGlutensiz = "false";
                yeniYemek.Menu.Glutensiz = Glutensiz;
            }

            
            yeniYemek.Menu.MenuID = menuNo;                                                                 //eklenecek yemeğin içeriğini aldığımız parametreden atadık.
            db.KullaniciYemeks.Add(yeniYemek);                                                              //bilgileri girilen yemeği veritabanı kaydını yaptık.
            
            db.SaveChanges();
            var yemekno = db.KullaniciYemeks.Max(x => x.KullaniciYemekID);
            Session["MenuNo"] = menuNo;

            return RedirectToAction("Menu/"+menuNo,"Home");
        }

        [Authorize]
        public ActionResult YemekSil(int id)
        {
            var yemek = db.KullaniciYemeks.Where(x => x.KullaniciYemekID == id).First();
            db.KullaniciYemeks.Remove(yemek);
            db.SaveChanges();
            var MenuNo = Session["MenuNo"];
            return RedirectToAction("MenuDuzenle/" + MenuNo, "Home");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Siparisler()
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var siparisler = db.Siparislers.Where(x => x.AliciKullanici.KullaniciID == no).OrderByDescending(y=>y.SiparisID);                 //Kullanıcının vermiş olduğu siparişleri listeledik.
            return View(siparisler);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Satislar()
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var satislar = db.Siparislers.Where(x => x.SaticiKullanici.KullaniciID == no).OrderByDescending(y => y.SiparisID);                  //Kullanıcının yapmış olduğu satışları listeledik.
            return View(satislar);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Satislar(Siparisler siparis)
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            var satislar = db.Siparislers.Where(x => x.SaticiKullanici.KullaniciID == no);                  //Kullanıcının yapmış olduğu satışları listeledik.
            Siparisler satis = db.Siparislers.Where(x => x.SiparisID == siparis.SiparisID).First();
            satis.SiparisDurum = siparis.SiparisDurum;
            db.SaveChanges();
            return View(satislar);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Satici(int id)
        {
            //var satici = db.KullaniciYemeks.Where(x=>x.Kullanici.KullaniciID==id);                          //Anasayfada ismine tıklanan satıcının menüsüne erişmek için id bilgisi uyan satıcı menüsünü getirdik.
            //var satici = db.Menus.Where(x => x.Kullanici.KullaniciID == id&&x.MenuAktif=="Evet");
            
            Session["SaticiID"] = id.ToString();                
            MenuViewModel models = new MenuViewModel();
            var menu = db.Menus.Where(x => x.Kullanici.KullaniciID == id && x.MenuAktif == "Aktif"&&x.KullaniciYemeks.Count!=0);
            var yemek = db.KullaniciYemeks.Where(y=>y.Kullanici.KullaniciID==id);

            models.KullaniciYemek = yemek;
            models.Menu = menu;
            return View(models);
        }
        [HttpPost]
        public ActionResult Satici(int id,int Adet) 
        {
            Menu yeni = new Menu();
            
            Session["SiparisAdeti"]= Adet;
            Session["Siparisicerik"] = db.KullaniciYemeks.Where(z => z.Menu.MenuID == id).Select(t=>t.SistemYemek.SistemYemekAdi).ToList();
            Session["MenuFiyat"] = db.Menus.Where(x => x.MenuID == id).Select(y => y.MenuFiyat).First();
            return RedirectToAction("Siparis/"+ id, "Home");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Siparis(int id)
        {
            int no = int.Parse((string)Session["KullaniciID"]);
            
            
            var siparis = db.Menus.Where(x => x.MenuID == id).ToList();
            
            int adet = (int)Session["SiparisAdeti"];
            double menuFiyat = db.Menus.Where(x => x.MenuID == id).Select(y => y.MenuFiyat).First();
            Session["ToplamFiyat"] = (adet * menuFiyat).ToString();
            return View(siparis);
        }
        [HttpPost]
        public ActionResult Siparis()
        {
            int alicino = int.Parse((string)Session["KullaniciID"]);
            int saticino = int.Parse((string)Session["SaticiID"]);
            int adet = (int)Session["SiparisAdeti"];
            List<string> icerik=new List<string>((IEnumerable<string>)Session["Siparisicerik"]);
            double toplamFiyat = double.Parse((string)Session["ToplamFiyat"]);
            Siparisler siparis = new Siparisler();
            siparis.Siparisicerik = string.Join(Environment.NewLine, icerik.ToArray());
            siparis.SaticiKullanici = db.Kullanicis.Where(x => x.KullaniciID == saticino).First();
            siparis.AliciKullanici = db.Kullanicis.Where(x => x.KullaniciID == alicino).First();
            siparis.SiparisAdet = adet;
            siparis.SiparisToplam = toplamFiyat;
            siparis.SiparisTarih = DateTime.Now;
            siparis.SiparisDurum = "Hazırlanıyor";
            db.Siparislers.Add(siparis);
            db.SaveChanges();
            return RedirectToAction("Siparisler","Home");
        }
        public ActionResult SiparisDetay(int id)
        {
            var siparis = db.Siparislers.Where(x => x.SiparisID == id);
            return View(siparis);
        }
        [HttpPost]
        public ActionResult SiparisDetay(Siparisler siparis,int id)
        {
            var bilgi = db.Siparislers.Where(x => x.SiparisID == id).First();
            bilgi.SiparisYorum = siparis.SiparisYorum;
            var puan = siparis.SiparisPuan;
            bilgi.SiparisPuan = Math.Round(siparis.SiparisPuan,2);
            int saticiid = db.Siparislers.Where(y=>y.SiparisID==id).Select(k=>k.SaticiKullanici.KullaniciID).First();
            db.SaveChanges();
            var puanlar = db.Siparislers.Where(z => z.SaticiKullanici.KullaniciID == saticiid).Select(t=>t.SiparisPuan).Average();
            
            bilgi.SaticiKullanici.KullaniciPuan = puanlar;
            db.SaveChanges();
            return RedirectToAction("Siparisler", "Home");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
    }
}