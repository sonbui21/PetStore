namespace Catalog.API.Apis;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryForSearchDto>()
            .ForMember(
                dest => dest.TotalCount,
                opt => opt.MapFrom(src => src.CatalogItems.Count)
            );

        CreateMap<CatalogItem, ItemCardDto>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Images.FirstOrDefault())
            )
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title.TruncateAtWord())
            )
            .ForMember(
                dest => dest.OldPrice,
                opt => opt.MapFrom(src => src.Price + 10) // TODO: mock
            );

        CreateMap<CatalogItem, CatalogItemDto>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.OrderBy(c => c.Name))
            )
            .ForMember(
                dest => dest.Options,
                opt => opt.MapFrom(src => src.ItemOptions.OrderBy(io => io.Name))
            )
            .ForMember(
                dest => dest.Images,
                opt => opt.MapFrom(src => src.Images
                .Select(img => new ImageDto
                {
                    Src = img,
                    Alt = src.Title
                }))
            )
            .ForMember(
                dest => dest.Variants,
                opt => opt.MapFrom(src => src.ItemVariants.OrderBy(c => c.Title))
            )
            .ForMember(
                dest => dest.OldPrice,
                opt => opt.MapFrom(src => src.Price + 10) // TODO: mock
            );

        CreateMap<ItemOption, ItemOptionDto>();
        CreateMap<ItemVariant, VariantDto>();
        CreateMap<ItemVariantOption, VariantOptionDto>();


        CreateMap<CatalogItem, CatalogItemAdminDto>();
        CreateMap<Category, CatalogCategoryDto>();
        CreateMap<CreateCatalogCategoryRequest, Category>();
        CreateMap<UpdateCatalogCategoryRequest, Category>();
    }
}
