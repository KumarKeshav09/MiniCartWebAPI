using AutoMapper;
using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.MainProductImageUrl, opt => opt.MapFrom(src => src.MainProductImageUrl))
                .ForMember(dest => dest.CostPrice, opt => opt.MapFrom(src => src.CostPrice))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.LongDescription, opt => opt.MapFrom(src => src.LongDescription))
                .ForMember(dest => dest.HighlightsOfProduct, opt => opt.MapFrom(src => src.HighlightsOfProduct))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ForMember(dest => dest.Subcategory, opt => opt.MapFrom(src => src.Subcategory))
                .ForPath(dest => dest.Subcategory.categoryId, opt => opt.MapFrom(src => src.Subcategory.categoryId));


            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.MainProductImageUrl, opt => opt.MapFrom(src => src.MainProductImageUrl))
                .ForMember(dest => dest.CostPrice, opt => opt.MapFrom(src => src.CostPrice))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.LongDescription, opt => opt.MapFrom(src => src.LongDescription))
                .ForMember(dest => dest.HighlightsOfProduct, opt => opt.MapFrom(src => src.HighlightsOfProduct))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ForMember(dest => dest.Subcategory, opt => opt.MapFrom(src => src.Subcategory))
                .ForPath(dest => dest.Subcategory.categoryId, opt => opt.MapFrom(src => src.Subcategory.categoryId));



            CreateMap<Subcategory, SubCategoryDto>()
                 .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<SubCategoryDto, Subcategory>();
        }
    }
}
