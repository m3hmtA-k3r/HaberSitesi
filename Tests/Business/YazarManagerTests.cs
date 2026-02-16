using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Security;

namespace MASKER.Tests.Business;

public class YazarManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly Mock<IRepository<Yazarlar>> _mockYazarRepo;
    private readonly YazarManager _manager;

    public YazarManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockYazarRepo = new Mock<IRepository<Yazarlar>>();

        _mockUow.Setup(u => u.YazarlarRepository).Returns(_mockYazarRepo.Object);

        _manager = new YazarManager(_mockUow.Object, _mockHasher.Object);
    }

    [Fact]
    public void GetYazarlar_TumYazarlariDondurur()
    {
        // Arrange
        var yazarlar = new List<Yazarlar>
        {
            new() { Id = 1, Ad = "Ali", Soyad = "Veli", Eposta = "ali@test.com", Sifre = "$2a$hash1", Resim = "", Aktifmi = true },
            new() { Id = 2, Ad = "Ayse", Soyad = "Fatma", Eposta = "ayse@test.com", Sifre = "$2a$hash2", Resim = "", Aktifmi = true }
        };
        _mockYazarRepo.Setup(r => r.GetAll()).Returns(yazarlar);

        // Act
        var result = _manager.GetYazarlar();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Ali", result[0].Ad);
    }

    [Fact]
    public void GetYazarById_MevcutId_YazarDondurur()
    {
        // Arrange
        var yazar = new Yazarlar { Id = 1, Ad = "Test", Soyad = "Yazar", Eposta = "t@t.com", Sifre = "hash", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(yazar);

        // Act
        var result = _manager.GetYazarById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Ad);
    }

    [Fact]
    public void GetYazarByEmailPassword_HashliSifre_DogrulamaBasarili()
    {
        // Arrange
        var yazar = new Yazarlar { Id = 1, Ad = "Hash", Soyad = "User", Eposta = "hash@test.com", Sifre = "$2a$12$somehashedpassword", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetAll()).Returns(new List<Yazarlar> { yazar });
        _mockHasher.Setup(h => h.VerifyPassword("dogru_sifre", "$2a$12$somehashedpassword")).Returns(true);

        // Act
        var result = _manager.GetYazarByEmailPassword("hash@test.com", "dogru_sifre");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hash", result.Ad);
    }

    [Fact]
    public void GetYazarByEmailPassword_HashliSifre_YanlisSifre_NullDondurur()
    {
        // Arrange
        var yazar = new Yazarlar { Id = 1, Ad = "Hash", Soyad = "User", Eposta = "hash@test.com", Sifre = "$2a$12$somehashedpassword", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetAll()).Returns(new List<Yazarlar> { yazar });
        _mockHasher.Setup(h => h.VerifyPassword("yanlis_sifre", "$2a$12$somehashedpassword")).Returns(false);

        // Act
        var result = _manager.GetYazarByEmailPassword("hash@test.com", "yanlis_sifre");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetYazarByEmailPassword_LegacyDuzMetinSifre_OtomatikHashler()
    {
        // Arrange
        var yazar = new Yazarlar { Id = 1, Ad = "Legacy", Soyad = "User", Eposta = "legacy@test.com", Sifre = "duz_metin_sifre", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetAll()).Returns(new List<Yazarlar> { yazar });
        _mockYazarRepo.Setup(r => r.Update(It.IsAny<Yazarlar>())).Returns((Yazarlar y) => y);
        _mockHasher.Setup(h => h.HashPassword("duz_metin_sifre")).Returns("$2a$12$newhash");

        // Act
        var result = _manager.GetYazarByEmailPassword("legacy@test.com", "duz_metin_sifre");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Legacy", result.Ad);
        // Sifre otomatik olarak hashlenmeli
        _mockHasher.Verify(h => h.HashPassword("duz_metin_sifre"), Times.Once);
        _mockYazarRepo.Verify(r => r.Update(It.Is<Yazarlar>(y => y.Sifre == "$2a$12$newhash")), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetYazarByEmailPassword_OlmayanKullanici_NullDondurur()
    {
        // Arrange
        _mockYazarRepo.Setup(r => r.GetAll()).Returns(new List<Yazarlar>());

        // Act
        var result = _manager.GetYazarByEmailPassword("yok@test.com", "sifre");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void InsertYazar_SifreyiHashleyerekEkler()
    {
        // Arrange
        var dto = new YazarlarDto { Ad = "Yeni", Soyad = "Yazar", Eposta = "yeni@test.com", Sifre = "yeni_sifre", Resim = "", Aktifmi = true };
        _mockHasher.Setup(h => h.HashPassword("yeni_sifre")).Returns("$2a$12$hashednewyazar");
        _mockYazarRepo.Setup(r => r.Insert(It.IsAny<Yazarlar>())).Returns((Yazarlar y) => y);

        // Act
        var result = _manager.InsertYazar(dto);

        // Assert
        _mockHasher.Verify(h => h.HashPassword("yeni_sifre"), Times.Once);
        _mockYazarRepo.Verify(r => r.Insert(It.Is<Yazarlar>(y => y.Sifre == "$2a$12$hashednewyazar")), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateYazar_YeniSifreVerilirse_Hashler()
    {
        // Arrange
        var existing = new Yazarlar { Id = 1, Ad = "Eski", Soyad = "Ad", Eposta = "e@t.com", Sifre = "$2a$12$eskihash", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockYazarRepo.Setup(r => r.Update(It.IsAny<Yazarlar>())).Returns((Yazarlar y) => y);
        _mockHasher.Setup(h => h.HashPassword("yeni_sifre")).Returns("$2a$12$yenihash");

        var dto = new YazarlarDto { Id = 1, Ad = "Yeni", Soyad = "Ad", Eposta = "e@t.com", Sifre = "yeni_sifre", Resim = "", Aktifmi = true };

        // Act
        _manager.UpdateYazar(dto);

        // Assert
        _mockHasher.Verify(h => h.HashPassword("yeni_sifre"), Times.Once);
    }

    [Fact]
    public void UpdateYazar_SifreBosBirakilirsa_MevcutSifreKorunur()
    {
        // Arrange
        var existing = new Yazarlar { Id = 1, Ad = "Eski", Soyad = "Ad", Eposta = "e@t.com", Sifre = "$2a$12$eskihash", Resim = "", Aktifmi = true };
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockYazarRepo.Setup(r => r.Update(It.IsAny<Yazarlar>())).Returns((Yazarlar y) => y);

        var dto = new YazarlarDto { Id = 1, Ad = "Yeni", Soyad = "Ad", Eposta = "e@t.com", Sifre = "", Resim = "", Aktifmi = true };

        // Act
        _manager.UpdateYazar(dto);

        // Assert
        _mockHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void DeleteYazar_MevcutId_SilerVeKaydeder()
    {
        // Arrange
        _mockYazarRepo.Setup(r => r.Delete(It.IsAny<Yazarlar>())).Returns(true);

        // Act
        var result = _manager.DeleteYazar(1);

        // Assert
        Assert.True(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
