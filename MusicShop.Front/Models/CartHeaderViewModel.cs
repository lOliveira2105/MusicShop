﻿namespace MusicShop.Front.Models;

public class CartHeaderViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; } = 0.00m;

    public decimal Discount { get; set; }

}
