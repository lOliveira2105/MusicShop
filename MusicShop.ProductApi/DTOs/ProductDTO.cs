namespace MusicShop.ProductApi.DTOs; 
public class ProductDTO {
    public int Id { get; set; }
    [Required(ErrorMessage = "The Name is Required")]
    [MaxLength(100)]
    public string? Name { get; set; }
    [Required(ErrorMessage = "The Name is Required")]
    [MinLength(1)]
    [MaxLength(100)]
    public string? Description { get; set; }
    [Required(ErrorMessage = "The Price is Required")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "The Name is Required")]
    [Range(1,9999)]
    public long Stock { get; set; }
    public string? Artist { get; set; }
    public string? ImageURL { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}
