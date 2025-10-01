using Shared.Dtos;

namespace WebUI.Models
{
    public class HaberDetayViewModel
    {
        public HaberlerDto Haber {  get; set; }
        public List<KategorilerDto> Kategoriler { get; set;}
        public List<YorumlarDto> Yorumlar { get; set;} = new List<YorumlarDto>();
    }
}
