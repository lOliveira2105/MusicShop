﻿
using MusicShop.CartApi.Repositories;

namespace MusicShop.CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("applycoupon")]
        public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDto)
        {
            var result = await _repository.ApplyCouponsAsyn(cartDto.CartHeader.UserId,
                                                            cartDto.CartHeader.CouponCode);

            if (!result)
            {
                return NotFound($"CartHeader not found for userId = {cartDto.CartHeader.UserId}");
            }
            return Ok(result);
        }

        [HttpDelete("deletecoupon/{userId}")]
        public async Task<ActionResult<CartDTO>> DeleteCoupon(string userId)
        {
            var result = await _repository.DeleteCouponAsyn(userId);

            if (!result)
            {
                return NotFound($"Discount Coupon not found for userId = {userId}");
            }

            return Ok(result);
        }
        [HttpGet("getcart/{id}")]
        public async Task<ActionResult> GetByUserId(string userid)
        {
            var cartDto = await _repository.GetCartByUserIdAsync(userid);
            if (cartDto is null)
            {
                return NotFound();
            }
            return Ok(cartDto);
        }
        [HttpPost("addCart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDto)
        {
            var cart = await _repository.UpdateCartAsync(cartDto);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);

        }
        [HttpPut("updateCart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDto)
        {
            var cart = await _repository.UpdateCartAsync(cartDto);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpDelete("deletecart/{id}")]
        public async Task<ActionResult<bool>> DeleteCart(int id)
        {
            var status = await _repository.DeleteItemCartAsync(id);
            if (!status) return BadRequest();
            return Ok(status);

        }





    }
}
