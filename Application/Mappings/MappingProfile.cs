using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
	/// <summary>
	/// AutoMapper profile for mapping between Domain entities and DTOs
	/// </summary>
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Haberler mappings
			CreateMap<Domain.Entities.Haberler, HaberlerDto>()
				.ReverseMap();

			// Kategoriler mappings
			CreateMap<Kategoriler, KategorilerDto>()
				.ReverseMap();

			// Yazarlar mappings
			CreateMap<Yazarlar, YazarlarDto>()
				.ReverseMap();

			// Yorumlar mappings
			CreateMap<Yorumlar, YorumlarDto>()
				.ReverseMap();

			// Slaytlar mappings
			CreateMap<Slaytlar, SlaytlarDto>()
				.ReverseMap();

			// Kullanicilar mappings
			CreateMap<Kullanicilar, KullaniciDto>()
				.ForMember(dest => dest.Roller, opt => opt.Ignore())
				.ReverseMap();

			CreateMap<Kullanicilar, ProfilDto>()
				.ForMember(dest => dest.Roller, opt => opt.Ignore())
				.ReverseMap();

			// Roller mappings
			CreateMap<Roller, RolDto>()
				.ReverseMap();

			// KullaniciRol mappings
			CreateMap<KullaniciRol, KullaniciRolDto>()
				.ForMember(dest => dest.RolAdi, opt => opt.Ignore())
				.ReverseMap();
		}
	}
}
