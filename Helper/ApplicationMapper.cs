using AutoMapper;
using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory.subcategoryName));
            CreateMap<ProductDto, Product>();
        }
    }
}
