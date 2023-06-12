
using MusicShop.CartApi.Repositories;

namespace MusicShop.CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _Repository;

        public CartController(ICartRepository repository)
        {
            _Repository = repository;
        }
        [HttpGet("getcart/{id}")]
        public async Task<ActionResult> GetByUserId(string userid)
        {
            var cartDto = await _Repository.GetCartByUserIdAsync(userid);
            if (cartDto is null)
            {
                return NotFound();
            }
            return Ok(cartDto);
        }
        [HttpPost("addCart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDto)
        {
            var cart = await _Repository.UpdateCartAsync(cartDto);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);

        }
        [HttpPut("updateCart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDto)
        {
            var cart = await _Repository.UpdateCartAsync(cartDto);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpDelete("deletecart/{id}")]
        public async Task<ActionResult<bool>> DeleteCart(int id)
        {
            var status = await _Repository.DeleteItemCartAsync(id);
            if (!status) return BadRequest();
            return Ok(status);

        }





    }
}
