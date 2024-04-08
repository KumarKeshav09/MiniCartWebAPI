using System.ComponentModel.DataAnnotations;

namespace MintCartWebApi.ModelDto
{
    public class RegisterCategoryDto
    {
        [Required(ErrorMessage ="category Name is required")]
        public string categoryName { get; set; }

        [Required(ErrorMessage ="category desciption is required")]
        public string categoryDes { get; set; }
    }
}
