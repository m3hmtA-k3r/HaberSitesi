using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;

namespace MASKER.Tests.Business;

public class RolManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IRepository<Roller>> _mockRolRepo;
    private readonly Mock<IRepository<KullaniciRol>> _mockKullaniciRolRepo;
    private readonly Mock<IRepository<Kullanicilar>> _mockKullaniciRepo;
    private readonly RolManager _manager;

    public RolManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockRolRepo = new Mock<IRepository<Roller>>();
        _mockKullaniciRolRepo = new Mock<IRepository<KullaniciRol>>();
        _mockKullaniciRepo = new Mock<IRepository<Kullanicilar>>();

        _mockUow.Setup(u => u.RollerRepository).Returns(_mockRolRepo.Object);
        _mockUow.Setup(u => u.KullaniciRollerRepository).Returns(_mockKullaniciRolRepo.Object);
        _mockUow.Setup(u => u.KullanicilarRepository).Returns(_mockKullaniciRepo.Object);

        _manager = new RolManager(_mockUow.Object);
    }

    [Fact]
    public void GetRoller_TumRolleriDondurur()
    {
        // Arrange
        var roller = new List<Roller>
        {
            new() { Id = 1, RolAdi = "Admin", Aciklama = "Yonetici", AktifMi = true },
            new() { Id = 2, RolAdi = "Editor", Aciklama = "Icerik editoru", AktifMi = true },
            new() { Id = 3, RolAdi = "Yazar", Aciklama = "Yazar", AktifMi = true }
        };
        _mockRolRepo.Setup(r => r.GetAll()).Returns(roller);

        // Act
        var result = _manager.GetRoller();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Admin", result[0].RolAdi);
    }

    [Fact]
    public void AtaRol_GecerliKullaniciVeRol_BasariliDondurur()
    {
        // Arrange
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(new Kullanicilar { Id = 1, Ad = "A", Soyad = "B", Eposta = "a@b.com", SifreHash = "h" });
        _mockRolRepo.Setup(r => r.GetById(1)).Returns(new Roller { Id = 1, RolAdi = "Admin" });
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());
        _mockKullaniciRolRepo.Setup(r => r.Insert(It.IsAny<KullaniciRol>())).Returns((KullaniciRol kr) => kr);

        // Act
        var result = _manager.AtaRol(1, 1);

        // Assert
        Assert.True(result);
        _mockKullaniciRolRepo.Verify(r => r.Insert(It.IsAny<KullaniciRol>()), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void AtaRol_ZatenAtanmisRol_InsertYapmadanTrueDondurur()
    {
        // Arrange
        _mockKullaniciRepo.Setup(r => r.GetById(1)).Returns(new Kullanicilar { Id = 1, Ad = "A", Soyad = "B", Eposta = "a@b.com", SifreHash = "h" });
        _mockRolRepo.Setup(r => r.GetById(1)).Returns(new Roller { Id = 1, RolAdi = "Admin" });
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol> { new() { KullaniciId = 1, RolId = 1 } });

        // Act
        var result = _manager.AtaRol(1, 1);

        // Assert
        Assert.True(result);
        _mockKullaniciRolRepo.Verify(r => r.Insert(It.IsAny<KullaniciRol>()), Times.Never);
    }

    [Fact]
    public void AtaRol_OlmayanKullanici_FalseDondurur()
    {
        // Arrange
        _mockKullaniciRepo.Setup(r => r.GetById(999)).Returns((Kullanicilar)null!);

        // Act
        var result = _manager.AtaRol(999, 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void KullaniciRoluVarMi_RolMevcut_TrueDondurur()
    {
        // Arrange
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller> { new() { Id = 1, RolAdi = "Admin" } });
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol> { new() { KullaniciId = 1, RolId = 1 } });

        // Act
        var result = _manager.KullaniciRoluVarMi(1, "Admin");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void KullaniciRoluVarMi_RolYok_FalseDondurur()
    {
        // Arrange
        _mockRolRepo.Setup(r => r.GetAll()).Returns(new List<Roller> { new() { Id = 1, RolAdi = "Admin" } });
        _mockKullaniciRolRepo.Setup(r => r.GetAll()).Returns(new List<KullaniciRol>());

        // Act
        var result = _manager.KullaniciRoluVarMi(1, "Admin");

        // Assert
        Assert.False(result);
    }
}
