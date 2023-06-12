
namespace MusicShop.Front.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    [Range(1,9999)]

    public string? Description { get; set; }
    [Required]
    [Display(Name = "Image URL")]
    public decimal Price { get; set; }
    [Required]
    [Range(1, 9999)]
    public long Stock { get; set; }
    [Required]
    public string? Artist { get; set; }
    public string? ImageURL { get; set; }
    [Display(Name = "Category Name")]
    public string? CategoryName { get; set; }
    [Range(1,100)]
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
}
