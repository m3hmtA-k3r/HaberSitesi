using Shared.Dtos;

namespace WebUI.Models
{
    public class SearchViewModel
    {
        public string Keyword { get; set; } = "";
        public List<HaberlerDto> Haberler { get; set; } = new List<HaberlerDto>();
        public List<KategorilerDto> Kategoriler { get; set; } = new List<KategorilerDto>();
    }
}
