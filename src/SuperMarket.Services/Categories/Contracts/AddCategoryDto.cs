using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories.Contracts
{
    public class AddCategoryDto
    {
        [Required]
        public string Title { get; set; }
    }
}
