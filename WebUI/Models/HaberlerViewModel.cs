using Shared.Dtos;

namespace WebUI.Models
{
	public class HaberlerViewModel
    {
        public List<KategorilerDto> Kategoriler { get; set; } = new List<KategorilerDto>();
		public List<HaberlerDto> Haberler { get; set; } = new List<HaberlerDto>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
        public int? SeciliKategoriId { get; set; }
        public string SiralamaSecimi { get; set; } = "yeni";
    }
}
