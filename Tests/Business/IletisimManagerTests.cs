using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Moq;
using Xunit;

namespace MASKER.Tests.Business
{
	public class IletisimManagerTests
	{
		private readonly Mock<IUnitOfWork> _mockUnitOfWork;
		private readonly Mock<IRepository<IletisimMesajlari>> _mockRepository;
		private readonly IletisimManager _manager;

		public IletisimManagerTests()
		{
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockRepository = new Mock<IRepository<IletisimMesajlari>>();
			_mockUnitOfWork.Setup(u => u.IletisimMesajlariRepository).Returns(_mockRepository.Object);
			_manager = new IletisimManager(_mockUnitOfWork.Object);
		}

		[Fact]
		public void InsertMesaj_GecerliModel_MesajEkler()
		{
			// Arrange
			var dto = new Application.DTOs.IletisimMesajlariDto
			{
				Ad = "Test",
				Eposta = "test@test.com",
				Konu = "Test Konu",
				Mesaj = "Test Mesaj"
			};

			var insertedEntity = new IletisimMesajlari
			{
				Id = 1,
				Ad = "Test",
				Eposta = "test@test.com",
				Konu = "Test Konu",
				Mesaj = "Test Mesaj",
				EklemeTarihi = DateTime.UtcNow,
				OkunduMu = false,
				CevaplandiMi = false
			};

			_mockRepository.Setup(r => r.Insert(It.IsAny<IletisimMesajlari>())).Returns(insertedEntity);

			// Act
			var result = _manager.InsertMesaj(dto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Id);
			Assert.Equal("Test", result.Ad);
			Assert.False(result.OkunduMu);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
		}

		[Fact]
		public void GetMesajlar_MesajlarVar_ListeDoner()
		{
			// Arrange
			var mesajlar = new List<IletisimMesajlari>
			{
				new IletisimMesajlari { Id = 1, Ad = "Test1", Eposta = "test1@test.com", Konu = "Konu1", Mesaj = "Mesaj1", EklemeTarihi = DateTime.UtcNow },
				new IletisimMesajlari { Id = 2, Ad = "Test2", Eposta = "test2@test.com", Konu = "Konu2", Mesaj = "Mesaj2", EklemeTarihi = DateTime.UtcNow.AddMinutes(-5) }
			}.AsQueryable();

			_mockRepository.Setup(r => r.GetAll()).Returns(mesajlar);

			// Act
			var result = _manager.GetMesajlar();

			// Assert
			Assert.Equal(2, result.Count);
			Assert.Equal("Test1", result[0].Ad); // En yeni ilk
		}

		[Fact]
		public void GetOkunmamisMesajlar_SadeceokunmamisDoner()
		{
			// Arrange
			var mesajlar = new List<IletisimMesajlari>
			{
				new IletisimMesajlari { Id = 1, Ad = "Test1", Eposta = "test1@test.com", Konu = "Konu1", Mesaj = "Mesaj1", OkunduMu = false, EklemeTarihi = DateTime.UtcNow },
				new IletisimMesajlari { Id = 2, Ad = "Test2", Eposta = "test2@test.com", Konu = "Konu2", Mesaj = "Mesaj2", OkunduMu = true, EklemeTarihi = DateTime.UtcNow },
				new IletisimMesajlari { Id = 3, Ad = "Test3", Eposta = "test3@test.com", Konu = "Konu3", Mesaj = "Mesaj3", OkunduMu = false, EklemeTarihi = DateTime.UtcNow }
			}.AsQueryable();

			_mockRepository.Setup(r => r.GetAll()).Returns(mesajlar);

			// Act
			var result = _manager.GetOkunmamisMesajlar();

			// Assert
			Assert.Equal(2, result.Count);
			Assert.All(result, m => Assert.False(m.OkunduMu));
		}

		[Fact]
		public void OkunduOlarakIsaretle_MesajVar_TrueDoner()
		{
			// Arrange
			var mesaj = new IletisimMesajlari { Id = 1, OkunduMu = false };
			_mockRepository.Setup(r => r.GetById(1)).Returns(mesaj);
			_mockRepository.Setup(r => r.Update(It.IsAny<IletisimMesajlari>())).Returns(mesaj);

			// Act
			var result = _manager.OkunduOlarakIsaretle(1);

			// Assert
			Assert.True(result);
			Assert.True(mesaj.OkunduMu);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
		}

		[Fact]
		public void OkunduOlarakIsaretle_MesajYok_FalseDoner()
		{
			// Arrange
			_mockRepository.Setup(r => r.GetById(999)).Returns((IletisimMesajlari)null!);

			// Act
			var result = _manager.OkunduOlarakIsaretle(999);

			// Assert
			Assert.False(result);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
		}

		[Fact]
		public void CevaplandiOlarakIsaretle_MesajVar_TrueDoner()
		{
			// Arrange
			var mesaj = new IletisimMesajlari { Id = 1, CevaplandiMi = false };
			_mockRepository.Setup(r => r.GetById(1)).Returns(mesaj);
			_mockRepository.Setup(r => r.Update(It.IsAny<IletisimMesajlari>())).Returns(mesaj);

			// Act
			var result = _manager.CevaplandiOlarakIsaretle(1);

			// Assert
			Assert.True(result);
			Assert.True(mesaj.CevaplandiMi);
			Assert.NotNull(mesaj.CevapTarihi);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
		}

		[Fact]
		public void DeleteMesaj_MesajVar_TrueDoner()
		{
			// Arrange
			_mockRepository.Setup(r => r.Delete(It.IsAny<IletisimMesajlari>())).Returns(true);

			// Act
			var result = _manager.DeleteMesaj(1);

			// Assert
			Assert.True(result);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
		}

		[Fact]
		public void DeleteMesaj_MesajYok_FalseDoner()
		{
			// Arrange
			_mockRepository.Setup(r => r.Delete(It.IsAny<IletisimMesajlari>())).Returns(false);

			// Act
			var result = _manager.DeleteMesaj(999);

			// Assert
			Assert.False(result);
			_mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
		}

		[Fact]
		public void GetOkunmamisMesajSayisi_DogruSayiDoner()
		{
			// Arrange
			var mesajlar = new List<IletisimMesajlari>
			{
				new IletisimMesajlari { Id = 1, OkunduMu = false },
				new IletisimMesajlari { Id = 2, OkunduMu = true },
				new IletisimMesajlari { Id = 3, OkunduMu = false },
				new IletisimMesajlari { Id = 4, OkunduMu = false }
			}.AsQueryable();

			_mockRepository.Setup(r => r.GetAll()).Returns(mesajlar);

			// Act
			var result = _manager.GetOkunmamisMesajSayisi();

			// Assert
			Assert.Equal(3, result);
		}
	}
}
