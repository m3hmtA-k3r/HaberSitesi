using Shared.Dtos;

namespace WebUI.Models
{
    public class YazarProfilViewModel
    {
        public YazarlarDto Yazar { get; set; } = new YazarlarDto();
        public List<HaberlerDto> Haberler { get; set; } = new List<HaberlerDto>();
        public int ToplamHaberSayisi { get; set; }
        public int ToplamGoruntulenme { get; set; }
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    }
}
