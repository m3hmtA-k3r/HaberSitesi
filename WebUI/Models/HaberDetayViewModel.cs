using Shared.Dtos;

namespace WebUI.Models
{
    public class HaberDetayViewModel
    {
        public HaberlerDto Haber { get; set; } = new HaberlerDto();
        public List<KategorilerDto> Kategoriler { get; set; } = new List<KategorilerDto>();
        public List<YorumlarDto> Yorumlar { get; set; } = new List<YorumlarDto>();
        public List<HaberlerDto> IlgiliHaberler { get; set; } = new List<HaberlerDto>();
    }
}
