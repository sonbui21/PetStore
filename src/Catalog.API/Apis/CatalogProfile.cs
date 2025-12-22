namespace Catalog.API.Apis;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<CatalogItem, CatalogItemDto>();
        CreateMap<CatalogCategory, CatalogCategoryDto>();
        CreateMap<CreateCatalogCategoryRequest, CatalogCategory>();
        CreateMap<UpdateCatalogCategoryRequest, CatalogCategory>();

        CreateMap<CatalogCategory, CategoryOutputDto>()
            .ForMember(
                dest => dest.TotalCount,
                opt => opt.MapFrom(src => src.CatalogItems.Count)
            );
        CreateMap<CatalogItem, ItemCardDto>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.ImagesUrl.FirstOrDefault())
            )
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title.TruncateAtWord())
            )
            .ForMember(
                dest => dest.OldPrice,
                opt => opt.MapFrom(src => src.Price + 10)
            );
        CreateMap<CatalogItem, ItemOutputDto>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.CatalogCategories.OrderBy(c => c.Name))
            )
            .ForMember(
                dest => dest.Options,
                opt => opt.MapFrom(src => src.CatalogItemOptions.OrderBy(io => io.Name))
            )
            .ForMember(
                dest => dest.Images,
                opt => opt.MapFrom(src => src.ImagesUrl
                .Select(img => new ImageDto
                {
                    Src = img,
                    Alt = src.Title 
                }))
            )
            .ForMember(
                dest => dest.Variants,
                opt => opt.MapFrom(src => src.CatalogItemVariants.OrderBy(c => c.Title))
            )
            .ForMember(
                dest => dest.OldPrice,
                opt => opt.MapFrom(src => src.Price + 10)
            );
        CreateMap<CatalogItemOption, ItemOptionDto>();
        CreateMap<CatalogItemVariant, VariantOutputDto>();
        CreateMap<CatalogItemVariantOption, VariantSelectedOptionDto>();
    }
}
