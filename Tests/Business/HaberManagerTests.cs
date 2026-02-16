using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;

namespace MASKER.Tests.Business;

public class HaberManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IRepository<Haberler>> _mockHaberRepo;
    private readonly Mock<IRepository<Yazarlar>> _mockYazarRepo;
    private readonly Mock<IRepository<Kategoriler>> _mockKategoriRepo;
    private readonly HaberManager _manager;

    public HaberManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHaberRepo = new Mock<IRepository<Haberler>>();
        _mockYazarRepo = new Mock<IRepository<Yazarlar>>();
        _mockKategoriRepo = new Mock<IRepository<Kategoriler>>();

        _mockUow.Setup(u => u.HaberlerRepository).Returns(_mockHaberRepo.Object);
        _mockUow.Setup(u => u.YazarlarRepository).Returns(_mockYazarRepo.Object);
        _mockUow.Setup(u => u.KategorilerRepository).Returns(_mockKategoriRepo.Object);

        _manager = new HaberManager(_mockUow.Object);
    }

    [Fact]
    public void GetHaberler_ReturnsAllHaberler()
    {
        // Arrange
        var haberler = new List<Haberler>
        {
            new() { Id = 1, Baslik = "Haber 1", Icerik = "Icerik 1", YazarId = 1, KategoriId = 1, Resim = "img1.jpg", Video = "", EklenmeTarihi = DateTime.UtcNow, Aktifmi = true },
            new() { Id = 2, Baslik = "Haber 2", Icerik = "Icerik 2", YazarId = 1, KategoriId = 2, Resim = "img2.jpg", Video = "", EklenmeTarihi = DateTime.UtcNow, Aktifmi = true }
        };
        _mockHaberRepo.Setup(r => r.GetAll()).Returns(haberler);
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(new Yazarlar { Id = 1, Ad = "Test", Soyad = "Yazar", Eposta = "t@t.com", Sifre = "x", Resim = "" });
        _mockKategoriRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Kategoriler { Id = 1, Aciklama = "Teknoloji" });

        // Act
        var result = _manager.GetHaberler();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Haber 1", result[0].Baslik);
        Assert.Equal("Haber 2", result[1].Baslik);
    }

    [Fact]
    public void GetHaberById_ExistingId_ReturnsHaber()
    {
        // Arrange
        var haber = new Haberler { Id = 1, Baslik = "Test Haber", Icerik = "Test Icerik", YazarId = 1, KategoriId = 1, Resim = "img.jpg", Video = "", EklenmeTarihi = DateTime.UtcNow, Aktifmi = true };
        _mockHaberRepo.Setup(r => r.GetById(1)).Returns(haber);
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(new Yazarlar { Id = 1, Ad = "Ali", Soyad = "Veli", Eposta = "a@b.com", Sifre = "x", Resim = "" });
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(new Kategoriler { Id = 1, Aciklama = "Spor" });

        // Act
        var result = _manager.GetHaberById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Haber", result.Baslik);
        Assert.Equal("Ali Veli", result.Yazar);
        Assert.Equal("Spor", result.Kategori);
    }

    [Fact]
    public void GetHaberById_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockHaberRepo.Setup(r => r.GetById(999)).Returns((Haberler)null!);

        // Act
        var result = _manager.GetHaberById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void InsertHaber_ValidModel_InsertsAndSaves()
    {
        // Arrange
        var dto = new HaberlerDto { Baslik = "Yeni Haber", Icerik = "Icerik", YazarId = 1, KategoriId = 1, Resim = "img.jpg", Video = "", Aktifmi = true };
        _mockHaberRepo.Setup(r => r.Insert(It.IsAny<Haberler>())).Returns((Haberler h) => h);
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(new Yazarlar { Id = 1, Ad = "Yazar", Soyad = "Test", Eposta = "y@t.com", Sifre = "x", Resim = "" });
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(new Kategoriler { Id = 1, Aciklama = "Genel" });

        // Act
        var result = _manager.InsertHaber(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yeni Haber", result.Baslik);
        _mockHaberRepo.Verify(r => r.Insert(It.IsAny<Haberler>()), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateHaber_ValidModel_UpdatesAndSaves()
    {
        // Arrange
        var existing = new Haberler { Id = 1, Baslik = "Eski Baslik", Icerik = "Eski Icerik", YazarId = 1, KategoriId = 1, Resim = "old.jpg", Video = "", EklenmeTarihi = DateTime.UtcNow, Aktifmi = true };
        _mockHaberRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockHaberRepo.Setup(r => r.Update(It.IsAny<Haberler>())).Returns((Haberler h) => h);
        _mockYazarRepo.Setup(r => r.GetById(1)).Returns(new Yazarlar { Id = 1, Ad = "A", Soyad = "B", Eposta = "a@b.com", Sifre = "x", Resim = "" });
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(new Kategoriler { Id = 1, Aciklama = "Kat" });

        var dto = new HaberlerDto { Id = 1, Baslik = "Yeni Baslik", Icerik = "Yeni Icerik", YazarId = 1, KategoriId = 1, Resim = "new.jpg", Video = "vid.mp4", Aktifmi = true };

        // Act
        var result = _manager.UpdateHaber(dto);

        // Assert
        Assert.Equal("Yeni Baslik", result.Baslik);
        _mockHaberRepo.Verify(r => r.Update(It.IsAny<Haberler>()), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteHaber_ExistingId_DeletesAndSaves()
    {
        // Arrange
        _mockHaberRepo.Setup(r => r.Delete(It.IsAny<Haberler>())).Returns(true);

        // Act
        var result = _manager.DeleteHaber(1);

        // Assert
        Assert.True(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteHaber_NonExistingId_ReturnsFalse()
    {
        // Arrange
        _mockHaberRepo.Setup(r => r.Delete(It.IsAny<Haberler>())).Returns(false);

        // Act
        var result = _manager.DeleteHaber(999);

        // Assert
        Assert.False(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Never);
    }

    [Fact]
    public void GetHaberById_YazarNotFound_ReturnsBilinmeyenYazar()
    {
        // Arrange
        var haber = new Haberler { Id = 1, Baslik = "Haber", Icerik = "Icerik", YazarId = 999, KategoriId = 1, Resim = "img.jpg", Video = "", EklenmeTarihi = DateTime.UtcNow, Aktifmi = true };
        _mockHaberRepo.Setup(r => r.GetById(1)).Returns(haber);
        _mockYazarRepo.Setup(r => r.GetById(999)).Returns((Yazarlar)null!);
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(new Kategoriler { Id = 1, Aciklama = "Kat" });

        // Act
        var result = _manager.GetHaberById(1);

        // Assert
        Assert.Equal("Bilinmeyen Yazar", result.Yazar);
    }
}
