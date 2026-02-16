using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Security;
using Infrastructure.Identity;

namespace MASKER.Tests.Business;

public class AuthManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly Mock<IJwtTokenService> _mockJwt;
    private readonly Mock<IRepository<Kullanicilar>> _mockKullaniciRepo;
    private readonly Mock<IRepository<KullaniciRol>> _mockKullaniciRolRepo;
    private readonly Mock<IRepository<Roller>> _mockRolRepo;
    private readonly AuthManager _manager;

    public AuthManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockJwt = new Mock<IJwtTokenService>();
        _mockKullaniciRepo = new Mock<IRepository<Kullanicilar>>();
        _mockKullaniciRolRepo = new Mock<IRepository<KullaniciRol>>();
        _mockRolRepo = new Mock<IRepository<Roller>>();

        _mockUow.Setup(u => u.KullanicilarRepository).Returns(_mockKullaniciRepo.Object);
        _mockUow.Setup(u => u.KullaniciRollerRepository).Returns(_mockKullaniciRolRepo.Object);
        _mockUow.Setup(u => u.RollerRepository).Returns(_mockRolRepo.Object);

        _manager = new AuthManager(_mockUow.Object, _mockHasher.Object, _mockJwt.Object);
    }

    [Fact]
    public void Login_GecerliKullanici_TokenDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "Admin", Soyad = "User", Eposta = "admin@masker.com", SifreHash = "hash123", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar> { kullanici });
        _mockHasher.Setup(h => h.VerifyPassword("Admin123", "hash123")).Returns(true);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol> { new() { KullaniciId = 1, RolId = 1 } });
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller> { new() { Id = 1, RolAdi = "Admin" } });
        _mockJwt.Setup(j => j.GenerateToken(1, "admin@masker.com", "Admin User", It.IsAny<IEnumerable<string>>())).Returns("jwt-token-123");
        _mockKullaniciRepo.Setup(r => r.Update(It.IsAny<Kullanicilar>())).Returns(kullanici);

        // Act
        var (dto, token, hata) = _manager.Login("admin@masker.com", "Admin123");

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("jwt-token-123", token);
        Assert.Null(hata);
        Assert.Equal("Admin", dto.Ad);
    }

    [Fact]
    public void Login_KullaniciBulunamadi_HataDondurur()
    {
        // Arrange
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar>());

        // Act
        var (dto, token, hata) = _manager.Login("yok@masker.com", "sifre");

        // Assert
        Assert.Null(dto);
        Assert.Null(token);
        Assert.Equal("Kullanici bulunamadi", hata);
    }

    [Fact]
    public void Login_PasifKullanici_HataDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "Pasif", Soyad = "User", Eposta = "pasif@masker.com", SifreHash = "hash", AktifMi = false };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar> { kullanici });

        // Act
        var (dto, token, hata) = _manager.Login("pasif@masker.com", "sifre");

        // Assert
        Assert.Null(dto);
        Assert.Equal("Kullanici hesabi aktif degil", hata);
    }

    [Fact]
    public void Login_YanlisSifre_HataDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "User", Soyad = "Test", Eposta = "user@masker.com", SifreHash = "hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetAll()).Returns(new List<Kullanicilar> { kullanici });
        _mockHasher.Setup(h => h.VerifyPassword("yanlis", "hash")).Returns(false);

        // Act
        var (dto, token, hata) = _manager.Login("user@masker.com", "yanlis");

        // Assert
        Assert.Null(dto);
        Assert.Equal("Sifre hatali", hata);
    }

    [Fact]
    public void GetProfil_MevcutKullanici_ProfilDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "Ali", Soyad = "Veli", Eposta = "ali@masker.com", Resim = "profil.jpg", OlusturmaTarihi = DateTime.UtcNow, AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(kullanici);
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol> { new() { KullaniciId = 1, RolId = 1 } });
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller> { new() { Id = 1, RolAdi = "Admin" } });

        // Act
        var result = _manager.GetProfil(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ali", result.Ad);
        Assert.Contains("Admin", result.Roller);
    }

    [Fact]
    public void GetProfil_OlmayanKullanici_NullDondurur()
    {
        // Arrange
        _mockKullaniciRepo.Setup(r => r.GetById(999)).Returns((Kullanicilar)null!);

        // Act
        var result = _manager.GetProfil(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SifreDegistir_GecerliEskiSifre_BasariliDondurur()
    {
        // Arrange
        var kullanici = new Kullanicilar { Id = 1, Ad = "User", Soyad = "Test", Eposta = "u@t.com", SifreHash = "eski_hash", AktifMi = true };
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(kullanici);
        _mockHasher.Setup(h => h.VerifyPassword("eskisifre", "eski_hash")).Returns(true);
        _mockHasher.Setup(h => h.HashPassword("yenisifre")).Returns("yeni_hash");
        _mockKullaniciRepo.Setup(r => r.Update(It.IsAny<Kullanicilar>())).Returns(kullanici);

        var model = new SifreDegistirDto { EskiSifre = "eskisifre", YeniSifre = "yenisifre", YeniSifreTekrar = "yenisifre" };

        // Act
        var result = _manager.SifreDegistir(1, model);

        // Assert
        Assert.True(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void SifreDegistir_SifrelerUyusmuyor_FalseDondurur()
    {
        // Arrange
        var model = new SifreDegistirDto { EskiSifre = "eski", YeniSifre = "yeni1", YeniSifreTekrar = "yeni2" };

        // Act
        var result = _manager.SifreDegistir(1, model);

        // Assert
        Assert.False(result);
    }
}
