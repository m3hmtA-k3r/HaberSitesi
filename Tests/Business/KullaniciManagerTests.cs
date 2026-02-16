using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Security;

namespace MASKER.Tests.Business;

public class KullaniciManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly Mock<IRepository<Kullanicilar>> _mockKullaniciRepo;
    private readonly Mock<IRepository<KullaniciRol>> _mockKullaniciRolRepo;
    private readonly Mock<IRepository<Roller>> _mockRolRepo;
    private readonly KullaniciManager _manager;

    public KullaniciManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockKullaniciRepo = new Mock<IRepository<Kullanicilar>>();
        _mockKullaniciRolRepo = new Mock<IRepository<KullaniciRol>>();
        _mockRolRepo = new Mock<IRepository<Roller>>();

        _mockUow.Setup(u => u.KullanicilarRepository).Returns(_mockKullaniciRepo.Object);
        _mockUow.Setup(u => u.KullaniciRollerRepository).Returns(_mockKullaniciRolRepo.Object);
        _mockUow.Setup(u => u.RollerRepository).Returns(_mockRolRepo.Object);

        _manager = new KullaniciManager(_mockUow.Object, _mockHasher.Object);
    }

    [Fact]
    public void GetKullanicilar_TumKullanicilariDondurur()
    {
        // Arrange
        var kullanicilar = new List<Kullanicilar>
        {
            new() { Id = 1, Ad = "Admin", Soyad = "User", Eposta = "admin@masker.com", SifreHash = "h1", AktifMi = true },
            new() { Id = 2, Ad = "Editor", Soyad = "User", Eposta = "editor@masker.com", SifreHash = "h2", AktifMi = true }
        };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(kullanicilar);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller>());

        // Act
        var result = _manager.GetKullanicilar();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void CreateKullanici_GecerliModel_OlusturulurVeKaydedilir()
    {
        // Arrange
        var model = new KullaniciCreateDto
        {
            Ad = "Yeni",
            Soyad = "Kullanici",
            Eposta = "yeni@masker.com",
            Sifre = "Sifre123",
            AktifMi = true,
            RolIdler = new List<int> { 1 }
        };
        _mockHasher.Setup(h => h.HashPassword("Sifre123")).Returns("hashed_sifre");
        _mockKullaniciRepo.Setup(r => r.Insert(It.IsAny<Kullanicilar>())).Returns((Kullanicilar k) => { k.Id = 1; return k; });
        _mockKullaniciRolRepo.Setup(r => r.Insert(It.IsAny<KullaniciRol>())).Returns((KullaniciRol kr) => kr);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller>());

        // Act
        var result = _manager.CreateKullanici(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yeni", result.Ad);
        _mockHasher.Verify(h => h.HashPassword("Sifre123"), Times.Once);
        _mockKullaniciRepo.Verify(r => r.Insert(It.Is<Kullanicilar>(k => k.SifreHash == "hashed_sifre")), Times.Once);
    }

    [Fact]
    public void UpdateKullanici_SifreVarsa_HashlerVeGunceller()
    {
        // Arrange
        var existing = new Kullanicilar { Id = 1, Ad = "Eski", Soyad = "Ad", Eposta = "e@m.com", SifreHash = "eski_hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockKullaniciRepo.Setup(r => r.Update(It.IsAny<Kullanicilar>())).Returns((Kullanicilar k) => k);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller>());
        _mockHasher.Setup(h => h.HashPassword("YeniSifre")).Returns("yeni_hash");

        var model = new KullaniciUpdateDto { Id = 1, Ad = "Yeni", Soyad = "Ad", Eposta = "e@m.com", Sifre = "YeniSifre", AktifMi = true };

        // Act
        var result = _manager.UpdateKullanici(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yeni", result.Ad);
        _mockHasher.Verify(h => h.HashPassword("YeniSifre"), Times.Once);
    }

    [Fact]
    public void UpdateKullanici_SifreYoksa_HashlemezGunceller()
    {
        // Arrange
        var existing = new Kullanicilar { Id = 1, Ad = "Eski", Soyad = "Ad", Eposta = "e@m.com", SifreHash = "mevcut_hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockKullaniciRepo.Setup(r => r.Update(It.IsAny<Kullanicilar>())).Returns((Kullanicilar k) => k);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller>());

        var model = new KullaniciUpdateDto { Id = 1, Ad = "Yeni", Soyad = "Ad", Eposta = "e@m.com", Sifre = null, AktifMi = true };

        // Act
        _manager.UpdateKullanici(model);

        // Assert
        _mockHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void DeleteKullanici_MevcutKullanici_RolleriVeKullaniciyiSiler()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "Silinecek", Soyad = "User", Eposta = "s@m.com", SifreHash = "h", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(kullanici);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol> { new() { Id = 1, KullaniciId = 1, RolId = 1 } });
        _mockKullaniciRolRepo.Setup(r => r.Delete(It.IsAny<KullaniciRol>())).Returns(true);
        _mockKullaniciRepo.Setup(r => r.Delete(It.IsAny<Kullanicilar>())).Returns(true);

        // Act
        var result = _manager.DeleteKullanici(1);

        // Assert
        Assert.True(result);
        _mockKullaniciRolRepo.Verify(r => r.Delete(It.IsAny<KullaniciRol>()), Times.Once);
        _mockKullaniciRepo.Verify(r => r.Delete(kullanici), Times.Once);
    }

    [Fact]
    public void ValidateCredentials_GecerliKullanici_TrueDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "A", Soyad = "B", Eposta = "a@b.com", SifreHash = "hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar> { kullanici });
        _mockHasher.Setup(h => h.VerifyPassword("dogru_sifre", "hash")).Returns(true);

        // Act
        var result = _manager.ValidateCredentials("a@b.com", "dogru_sifre");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateCredentials_YanlisSifre_FalseDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "A", Soyad = "B", Eposta = "a@b.com", SifreHash = "hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar> { kullanici });
        _mockHasher.Setup(h => h.VerifyPassword("yanlis", "hash")).Returns(false);

        // Act
        var result = _manager.ValidateCredentials("a@b.com", "yanlis");

        // Assert
        Assert.False(result);
    }
}
