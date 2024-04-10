using MintCartWebApi.DBModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCartWebApi.ModelDto
{
    public class ProductDto
    {

        public string ProductName { get; set; }

        public string MainProductImageUrl { get; set; }
        public IFormFile? MainProductImage { get; set; }


        public List<string> ProductImageUrl { get; set; }
        public List<IFormFile>? ProductImage { get; set; }


        public double CostPrice { get; set; }

        public double SellingPrice { get; set; }

        public int Quantity { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public List<string> HighlightsOfProduct { get; set; }

        public string Specifications { get; set; }

        public string BrandName { get; set; }

        public Subcategory Subcategory { get; set; }
    }
}
