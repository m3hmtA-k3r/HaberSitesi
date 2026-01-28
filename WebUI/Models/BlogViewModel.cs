using Shared.Dtos;

namespace WebUI.Models
{
    public class BlogListViewModel
    {
        public List<BloglarDto> Bloglar { get; set; } = new List<BloglarDto>();
        public List<BlogKategorilerDto> Kategoriler { get; set; } = new List<BlogKategorilerDto>();
        public int? SeciliKategoriId { get; set; }
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    }

    public class BlogDetayViewModel
    {
        public BloglarDto Blog { get; set; } = new BloglarDto();
        public List<BloglarDto> IlgiliBloglar { get; set; } = new List<BloglarDto>();
        public List<BlogKategorilerDto> Kategoriler { get; set; } = new List<BlogKategorilerDto>();
        public List<BlogYorumlarDto> Yorumlar { get; set; } = new List<BlogYorumlarDto>();
    }
}
