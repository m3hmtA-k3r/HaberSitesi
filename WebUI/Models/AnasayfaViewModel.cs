using Shared.Dtos;

namespace WebUI.Models
{
    public class AnasayfaViewModel
    {
        public List<SlaytlarDto> Slaytlar { get; set; } = new();
        public List<HaberlerDto> Haberler { get; set; } = new();
    }
}
