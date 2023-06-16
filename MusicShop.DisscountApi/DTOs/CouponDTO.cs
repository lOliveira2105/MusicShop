namespace MusicShop.DiscountApi.DTOs;

public class CouponDTO
{
    public int CouponId { get; set; }

    [StringLength(30)]
    public string? CouponCode { get; set; }

    [Required]
    public decimal Discount { get; set; }
}
