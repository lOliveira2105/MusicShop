namespace MusicShop.DiscountApi.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
    CreateMap<CouponDTO, Coupon>().ReverseMap();

    }
}
