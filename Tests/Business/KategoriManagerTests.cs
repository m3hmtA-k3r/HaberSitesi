using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;

namespace MASKER.Tests.Business;

public class KategoriManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IRepository<Kategoriler>> _mockKategoriRepo;
    private readonly KategoriManager _manager;

    public KategoriManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockKategoriRepo = new Mock<IRepository<Kategoriler>>();
        _mockUow.Setup(u => u.KategorilerRepository).Returns(_mockKategoriRepo.Object);
        _manager = new KategoriManager(_mockUow.Object);
    }

    [Fact]
    public void GetKategoriler_TumKategorileriDondurur()
    {
        // Arrange
        var kategoriler = new List<Kategoriler>
        {
            new() { Id = 1, Aciklama = "Teknoloji", Aktifmi = true },
            new() { Id = 2, Aciklama = "Spor", Aktifmi = true },
            new() { Id = 3, Aciklama = "Saglik", Aktifmi = false }
        };
        _mockKategoriRepo.Setup(r => r.GetAll()).Returns(kategoriler);

        // Act
        var result = _manager.GetKategoriler();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Teknoloji", result[0].Aciklama);
    }

    [Fact]
    public void GetKategoriById_MevcutId_KategoriDondurur()
    {
        // Arrange
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(new Kategoriler { Id = 1, Aciklama = "Ekonomi", Aktifmi = true });

        // Act
        var result = _manager.GetKategoriById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ekonomi", result.Aciklama);
        Assert.True(result.Aktifmi);
    }

    [Fact]
    public void InsertKategori_GecerliModel_EklerVeKaydeder()
    {
        // Arrange
        var dto = new KategorilerDto { Aciklama = "Yeni Kategori", Aktifmi = true };
        _mockKategoriRepo.Setup(r => r.Insert(It.IsAny<Kategoriler>())).Returns((Kategoriler k) => k);

        // Act
        var result = _manager.InsertKategori(dto);

        // Assert
        Assert.Equal("Yeni Kategori", result.Aciklama);
        _mockKategoriRepo.Verify(r => r.Insert(It.IsAny<Kategoriler>()), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateKategori_GecerliModel_GuncellerVeKaydeder()
    {
        // Arrange
        var existing = new Kategoriler { Id = 1, Aciklama = "Eski", Aktifmi = true };
        _mockKategoriRepo.Setup(r => r.GetById(1)).Returns(existing);
        _mockKategoriRepo.Setup(r => r.Update(It.IsAny<Kategoriler>())).Returns((Kategoriler k) => k);

        var dto = new KategorilerDto { Id = 1, Aciklama = "Guncellenmis", Aktifmi = false };

        // Act
        var result = _manager.UpdateKategori(dto);

        // Assert
        Assert.Equal("Guncellenmis", result.Aciklama);
        Assert.False(result.Aktifmi);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteKategori_MevcutId_SilerVeKaydeder()
    {
        // Arrange
        _mockKategoriRepo.Setup(r => r.Delete(It.IsAny<Kategoriler>())).Returns(true);

        // Act
        var result = _manager.DeleteKategori(1);

        // Assert
        Assert.True(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteKategori_OlmayanId_FalseDondurur()
    {
        // Arrange
        _mockKategoriRepo.Setup(r => r.Delete(It.IsAny<Kategoriler>())).Returns(false);

        // Act
        var result = _manager.DeleteKategori(999);

        // Assert
        Assert.False(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Never);
    }
}
