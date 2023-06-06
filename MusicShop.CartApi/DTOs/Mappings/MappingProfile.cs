namespace MusicShop.CartApi.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartDTO, Cart>().ReverseMap();
        CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
        CreateMap<CartItemDTO, Cart>().ReverseMap();
        CreateMap<ProductDTO, Product>().ReverseMap();


    }

}
