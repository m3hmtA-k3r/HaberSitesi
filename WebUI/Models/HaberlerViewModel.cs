using Shared.Dtos;

namespace WebUI.Models
{
	public class HaberlerViewModel
    {
        public List<KategorilerDto> Kategoriler {  get; set; }
		public List<HaberlerDto> Haberler { get; set; }
    }
}
