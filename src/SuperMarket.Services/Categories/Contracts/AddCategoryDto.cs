using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Services.Categories.Contracts
{
    public class AddCategoryDto
    {
        [Required]
        public string Title { get; set; }
    }
}
