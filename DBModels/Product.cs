using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCartWebApi.DBModels
{
    public class Product
    {
        [Key]
        public int productId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [Column(TypeName = "nvarchar(255)")]
        public string ProductName { get; set; }
        [Required]
        public string MainProductImageUrl { get; set; }
        [Required]
        public List<string> ProductImageUrl { get; set; }
        [Required]
        public double CostPrice { get; set; }
        [Required]
        public double SellingPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string LongDescription { get; set; }
        [Required]
        public List<string> HighlightsOfProduct { get; set; }
        [Required]
        public string Specifications { get; set; }
        [Required]
        public string BrandName { get; set; }

        [Required]
        public int subcategoryId { get; set; }

        [ForeignKey("subcategoryId")]
        public Subcategory Subcategory { get; set; }

    }
}