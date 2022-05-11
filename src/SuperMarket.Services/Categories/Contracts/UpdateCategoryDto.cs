using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Services.Categories.Contracts
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Title { get; set; }
    }
}
