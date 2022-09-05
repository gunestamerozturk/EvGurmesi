using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EvGurmesiMVC.Models;
namespace EvGurmesiMVC.Controllers
{
    public class AccountController : Controller
    {
        //Model içerisinde veritabanı bilgilerimizi tutan Context nesnemizden 'db' adında bir nesne oluşturduk ve veritabanı bilgilerimizi db'ye attık.
        Context db = new Context();
        
        //HttpGet ile login sayfamızın görüntüsünü getirdik.
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //HttpPost kullanarak Login sayfamız içerisindeki bilgileri ilgili alana post etmiş olduk.
        [HttpPost]
        public ActionResult Login(Kullanici kullanici)                              //Login fonksiyonu içerisine kullanici adında bir Kullanici parametresi tanımladık.
        {
            var bilgiler = db.Kullanicis.FirstOrDefault(x => x.Username == kullanici.Username && x.Password == kullanici.Password);         //bilgiler değişkenine veritabanındaki Kullanicis tablosu içerisinde Loginden gelen Username ve Password ile kullanici parametresinden gelen Username ve Password eşitliğini sorgulayarak atadık.
            if(bilgiler!=null)                                                      //bilgiler değişkenimize bir değer gelmesi halinde authentication işlemimizi yaptık.
            {
                FormsAuthentication.SetAuthCookie(bilgiler.Username, false);
                Session["KullaniciID"] = bilgiler.KullaniciID.ToString();           //İhtiyaç halinde kullanılabilmesi için giriş yapan kullanıcının ID bilgisini Session ile hafızada tuttuk.
                Session["KullaniciResim"] = bilgiler.KullaniciResim;
                return RedirectToAction("Index", "Home");                           //Authentication işlemi sonucunda Home/Index sayfamıza yönlendirme yaptık. 
            }
            else
            {
                ViewBag.hata = "Kullanıcı adı veya şifre hatalı.";                  //Bilgilerin yanlış olması halinde hata mesajımızı oluşturduk.
            }
            return View();
        }

        //Herhangi bir attribute olmadığı zaman sistem otomatik olarak [HttpGet] olarak çalışır.
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Kullanici kullanici)
        {
            db.Kullanicis.Add(kullanici);                                           //veritabanındaki Kullanicis tablosuna kullanici nesnesinden gelen bilgileri ekledik.
            db.SaveChanges();                                                       //veritabanındaki değişiklikleri kaydettik.
            return RedirectToAction("Login","Account");                             //kayıt işlemi sonrasında kullanıcıyı giriş sayfasına yönlendirdik.
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();                                          //SignOut() komutu ile kullanıcının oturumunu sonlandırmış olduk.
            return RedirectToAction("Login", "Account");                            //Çıkış işlemi sonrası tekrar giriş sayfasına yönlendirdik.
        }
    }
}