namespace MusicShop.CartApi.Models;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
    public Product Product { get; set; } = new Product();
    public CartHeader CartHEader { get; set; } = new CartHeader();
}
