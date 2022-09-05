namespace EvGurmesiMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MVCMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kullanicis",
                c => new
                    {
                        KullaniciID = c.Int(nullable: false, identity: true),
                        Password = c.String(),
                        Username = c.String(),
                        KullaniciAd = c.String(),
                        KullaniciSoyad = c.String(),
                        KullaniciResim = c.String(),
                        KullaniciPazarAdi = c.String(),
                        KullaniciMail = c.String(),
                        KullaniciTelefon = c.String(),
                        KullaniciSehir = c.String(),
                        Kullaniciilce = c.String(),
                        KullaniciAdres = c.String(),
                        KullaniciPuan = c.Double(nullable: false),
                        KullaniciTipi = c.String(),
                        KullaniciAktif = c.String(),
                        KullaniciDukkanResim = c.String(),
                        KullaniciGlutensiz = c.String(),
                        KullaniciMenuFiyat = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.KullaniciID);
            
            CreateTable(
                "dbo.KullaniciYemeks",
                c => new
                    {
                        KullaniciYemekID = c.Int(nullable: false, identity: true),
                        KullaniciYemekicerik = c.String(),
                        KullaniciYemekFiyat = c.Double(nullable: false),
                        KullaniciYemekAdet = c.Int(nullable: false),
                        Kullanici_KullaniciID = c.Int(),
                        Menu_MenuID = c.Int(),
                        SistemYemek_SistemYemekID = c.Int(),
                    })
                .PrimaryKey(t => t.KullaniciYemekID)
                .ForeignKey("dbo.Kullanicis", t => t.Kullanici_KullaniciID)
                .ForeignKey("dbo.Menus", t => t.Menu_MenuID)
                .ForeignKey("dbo.SistemYemeks", t => t.SistemYemek_SistemYemekID)
                .Index(t => t.Kullanici_KullaniciID)
                .Index(t => t.Menu_MenuID)
                .Index(t => t.SistemYemek_SistemYemekID);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        MenuID = c.Int(nullable: false, identity: true),
                        MenuAd = c.String(),
                        MenuFiyat = c.Double(nullable: false),
                        MenuAdet = c.Int(nullable: false),
                        Kullanici_KullaniciID = c.Int(),
                    })
                .PrimaryKey(t => t.MenuID)
                .ForeignKey("dbo.Kullanicis", t => t.Kullanici_KullaniciID)
                .Index(t => t.Kullanici_KullaniciID);
            
            CreateTable(
                "dbo.Siparislers",
                c => new
                    {
                        SiparisID = c.Int(nullable: false, identity: true),
                        SiparisYorum = c.String(),
                        SiparisPuan = c.Double(nullable: false),
                        Siparisicerik = c.String(),
                        SiparisTarih = c.DateTime(nullable: false),
                        SiparisAdet = c.Int(nullable: false),
                        SiparisToplam = c.Double(nullable: false),
                        AliciKullanici_KullaniciID = c.Int(),
                        SaticiKullanici_KullaniciID = c.Int(),
                        Kullanici_KullaniciID = c.Int(),
                    })
                .PrimaryKey(t => t.SiparisID)
                .ForeignKey("dbo.Kullanicis", t => t.AliciKullanici_KullaniciID)
                .ForeignKey("dbo.Kullanicis", t => t.SaticiKullanici_KullaniciID)
                .ForeignKey("dbo.Kullanicis", t => t.Kullanici_KullaniciID)
                .Index(t => t.AliciKullanici_KullaniciID)
                .Index(t => t.SaticiKullanici_KullaniciID)
                .Index(t => t.Kullanici_KullaniciID);
            
            CreateTable(
                "dbo.SistemYemeks",
                c => new
                    {
                        SistemYemekID = c.Int(nullable: false, identity: true),
                        SistemYemekAdi = c.String(),
                        SistemYemekicerik = c.String(),
                        SistemYemekResim = c.String(),
                        SistemYemekKategori_SistemYemekKategoriID = c.Int(),
                    })
                .PrimaryKey(t => t.SistemYemekID)
                .ForeignKey("dbo.SistemYemekKategoris", t => t.SistemYemekKategori_SistemYemekKategoriID)
                .Index(t => t.SistemYemekKategori_SistemYemekKategoriID);
            
            CreateTable(
                "dbo.SistemYemekKategoris",
                c => new
                    {
                        SistemYemekKategoriID = c.Int(nullable: false, identity: true),
                        SistemYemekKategoriAdi = c.String(),
                    })
                .PrimaryKey(t => t.SistemYemekKategoriID);
            
            CreateTable(
                "dbo.SiparislerKullaniciYemeks",
                c => new
                    {
                        Siparisler_SiparisID = c.Int(nullable: false),
                        KullaniciYemek_KullaniciYemekID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Siparisler_SiparisID, t.KullaniciYemek_KullaniciYemekID })
                .ForeignKey("dbo.Siparislers", t => t.Siparisler_SiparisID, cascadeDelete: true)
                .ForeignKey("dbo.KullaniciYemeks", t => t.KullaniciYemek_KullaniciYemekID, cascadeDelete: true)
                .Index(t => t.Siparisler_SiparisID)
                .Index(t => t.KullaniciYemek_KullaniciYemekID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Siparislers", "Kullanici_KullaniciID", "dbo.Kullanicis");
            DropForeignKey("dbo.SistemYemeks", "SistemYemekKategori_SistemYemekKategoriID", "dbo.SistemYemekKategoris");
            DropForeignKey("dbo.KullaniciYemeks", "SistemYemek_SistemYemekID", "dbo.SistemYemeks");
            DropForeignKey("dbo.Siparislers", "SaticiKullanici_KullaniciID", "dbo.Kullanicis");
            DropForeignKey("dbo.SiparislerKullaniciYemeks", "KullaniciYemek_KullaniciYemekID", "dbo.KullaniciYemeks");
            DropForeignKey("dbo.SiparislerKullaniciYemeks", "Siparisler_SiparisID", "dbo.Siparislers");
            DropForeignKey("dbo.Siparislers", "AliciKullanici_KullaniciID", "dbo.Kullanicis");
            DropForeignKey("dbo.KullaniciYemeks", "Menu_MenuID", "dbo.Menus");
            DropForeignKey("dbo.Menus", "Kullanici_KullaniciID", "dbo.Kullanicis");
            DropForeignKey("dbo.KullaniciYemeks", "Kullanici_KullaniciID", "dbo.Kullanicis");
            DropIndex("dbo.SiparislerKullaniciYemeks", new[] { "KullaniciYemek_KullaniciYemekID" });
            DropIndex("dbo.SiparislerKullaniciYemeks", new[] { "Siparisler_SiparisID" });
            DropIndex("dbo.SistemYemeks", new[] { "SistemYemekKategori_SistemYemekKategoriID" });
            DropIndex("dbo.Siparislers", new[] { "Kullanici_KullaniciID" });
            DropIndex("dbo.Siparislers", new[] { "SaticiKullanici_KullaniciID" });
            DropIndex("dbo.Siparislers", new[] { "AliciKullanici_KullaniciID" });
            DropIndex("dbo.Menus", new[] { "Kullanici_KullaniciID" });
            DropIndex("dbo.KullaniciYemeks", new[] { "SistemYemek_SistemYemekID" });
            DropIndex("dbo.KullaniciYemeks", new[] { "Menu_MenuID" });
            DropIndex("dbo.KullaniciYemeks", new[] { "Kullanici_KullaniciID" });
            DropTable("dbo.SiparislerKullaniciYemeks");
            DropTable("dbo.SistemYemekKategoris");
            DropTable("dbo.SistemYemeks");
            DropTable("dbo.Siparislers");
            DropTable("dbo.Menus");
            DropTable("dbo.KullaniciYemeks");
            DropTable("dbo.Kullanicis");
        }
    }
}
