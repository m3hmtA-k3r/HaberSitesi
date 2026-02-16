using Moq;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Application.DTOs;

namespace MASKER.Tests.Business;

public class BlogManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IRepository<Bloglar>> _mockBlogRepo;
    private readonly Mock<IRepository<BlogKategoriler>> _mockBlogKatRepo;
    private readonly Mock<IRepository<Kullanicilar>> _mockKullaniciRepo;
    private readonly BlogManager _manager;

    public BlogManagerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockBlogRepo = new Mock<IRepository<Bloglar>>();
        _mockBlogKatRepo = new Mock<IRepository<BlogKategoriler>>();
        _mockKullaniciRepo = new Mock<IRepository<Kullanicilar>>();

        _mockUow.Setup(u => u.BloglarRepository).Returns(_mockBlogRepo.Object);
        _mockUow.Setup(u => u.BlogKategorilerRepository).Returns(_mockBlogKatRepo.Object);
        _mockUow.Setup(u => u.KullanicilarRepository).Returns(_mockKullaniciRepo.Object);

        _manager = new BlogManager(_mockUow.Object);
    }

    [Fact]
    public void GetBloglar_TumBloglariDondurur()
    {
        // Arrange
        var bloglar = new List<Bloglar>
        {
            new() { Id = 1, Baslik = "Blog 1", Icerik = "Icerik 1", AktifMi = true, YayinTarihi = DateTime.Now, OlusturmaTarihi = DateTime.Now },
            new() { Id = 2, Baslik = "Blog 2", Icerik = "Icerik 2", AktifMi = true, YayinTarihi = DateTime.Now, OlusturmaTarihi = DateTime.Now }
        };
        _mockBlogRepo.Setup(r => r.GetAll()).Returns(bloglar);

        // Act
        var result = _manager.GetBloglar();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetAktifBloglar_SadeceAktifVeYayinlanmisBloglariDondurur()
    {
        // Arrange
        var bloglar = new List<Bloglar>
        {
            new() { Id = 1, Baslik = "Aktif", Icerik = "I", AktifMi = true, YayinTarihi = DateTime.Now.AddDays(-1), OlusturmaTarihi = DateTime.Now },
            new() { Id = 2, Baslik = "Pasif", Icerik = "I", AktifMi = false, YayinTarihi = DateTime.Now.AddDays(-1), OlusturmaTarihi = DateTime.Now },
            new() { Id = 3, Baslik = "Gelecek", Icerik = "I", AktifMi = true, YayinTarihi = DateTime.Now.AddDays(10), OlusturmaTarihi = DateTime.Now }
        };
        _mockBlogRepo.Setup(r => r.GetAll()).Returns(bloglar.AsQueryable());

        // Act
        var result = _manager.GetAktifBloglar();

        // Assert
        Assert.Single(result);
        Assert.Equal("Aktif", result[0].Baslik);
    }

    [Fact]
    public void GetBloglarByKategori_KategoriyeGoreFiltreler()
    {
        // Arrange
        var bloglar = new List<Bloglar>
        {
            new() { Id = 1, Baslik = "Teknoloji Blog", Icerik = "I", KategoriId = 1, AktifMi = true, YayinTarihi = DateTime.Now, OlusturmaTarihi = DateTime.Now },
            new() { Id = 2, Baslik = "Spor Blog", Icerik = "I", KategoriId = 2, AktifMi = true, YayinTarihi = DateTime.Now, OlusturmaTarihi = DateTime.Now }
        };
        _mockBlogRepo.Setup(r => r.GetAll()).Returns(bloglar.AsQueryable());

        // Act
        var result = _manager.GetBloglarByKategori(1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Teknoloji Blog", result[0].Baslik);
    }

    [Fact]
    public void InsertBlog_GecerliModel_EklerVeKaydeder()
    {
        // Arrange
        var dto = new BloglarDto { Baslik = "Yeni Blog", Icerik = "Icerik", AktifMi = true };
        _mockBlogRepo.Setup(r => r.Insert(It.IsAny<Bloglar>())).Returns((Bloglar b) => b);

        // Act
        var result = _manager.InsertBlog(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yeni Blog", result.Baslik);
        _mockBlogRepo.Verify(r => r.Insert(It.IsAny<Bloglar>()), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteBlog_MevcutId_SilerVeKaydeder()
    {
        // Arrange
        _mockBlogRepo.Setup(r => r.Delete(It.IsAny<Bloglar>())).Returns(true);

        // Act
        var result = _manager.DeleteBlog(1);

        // Assert
        Assert.True(result);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void IncrementGoruntulenmeSayisi_MevcutBlog_SayiyiArttirir()
    {
        // Arrange
        var blog = new Bloglar { Id = 1, Baslik = "B", Icerik = "I", GoruntulenmeSayisi = 5, AktifMi = true, YayinTarihi = DateTime.Now, OlusturmaTarihi = DateTime.Now };
        _mockBlogRepo.Setup(r => r.GetById(1)).Returns(blog);
        _mockBlogRepo.Setup(r => r.Update(It.IsAny<Bloglar>())).Returns((Bloglar b) => b);

        // Act
        _manager.IncrementGoruntulenmeSayisi(1);

        // Assert
        Assert.Equal(6, blog.GoruntulenmeSayisi);
        _mockBlogRepo.Verify(r => r.Update(blog), Times.Once);
        _mockUow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void IncrementGoruntulenmeSayisi_OlmayanBlog_HicbirSeyYapmaz()
    {
        // Arrange
        _mockBlogRepo.Setup(r => r.GetById(999)).Returns((Bloglar)null!);

        // Act
        _manager.IncrementGoruntulenmeSayisi(999);

        // Assert
        _mockBlogRepo.Verify(r => r.Update(It.IsAny<Bloglar>()), Times.Never);
        _mockUow.Verify(u => u.SaveChanges(), Times.Never);
    }
}
