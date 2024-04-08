using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCartWebApi.DBModels
{
    public class Subcategory
    {
        [Key]
        public int subcategoryId { get; set; }
        public string subcategoryName { get; set; }
        public string subcategoryDes { get; set; }
        [Required]
        public int categoryId { get; set; }

        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
}
