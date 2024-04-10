using MintCartWebApi.DBModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCartWebApi.ModelDto
{
    public class SubCategoryDto
    {
        [Required(ErrorMessage = "Sub Category Name is required")]
        public string subcategoryName { get; set; }

        [Required(ErrorMessage = "Sub Category desciption is required")]
        public string subcategoryDes { get; set; }
        public Category Category { get; set; }
    }
}
