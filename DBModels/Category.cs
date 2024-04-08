using System.ComponentModel.DataAnnotations;

namespace MintCartWebApi.DBModels
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string categoryDes { get; set; }
    }
}