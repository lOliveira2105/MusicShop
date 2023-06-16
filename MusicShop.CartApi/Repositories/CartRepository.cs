namespace MusicShop.CartApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    private IMapper _mapper;
    public CartRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CartDTO> GetCartByUserIdAsync(string userId)

    {
        Cart cart = new Cart
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
        };
        //Obter os itents 
        cart.CartItems = _context.CartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id).Include(c => c.Product);
        return _mapper.Map<CartDTO>(cart);
    }
    public async Task<bool> DeleteItemCartAsync(int cartItemId)
    {
        try
        {
            CartItem cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);
            int total = _context.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();
            _context.CartItems.Remove(cartItem);

            if (total == 1)
            {
                var cartHeaderRemove = await _context.CartHeaders.FirstOrDefaultAsync(
                                                                    c => c.Id == cartItem.CartHeaderId);
                _context.CartHeaders.Remove(cartHeaderRemove);
            }
            await _context.SaveChangesAsync();
            return true;
        } 
        catch (Exception ex)
        {
            return false;
        }
     
    }
    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cartHeader is not  null)
        {
            _context.CartItems.RemoveRange(_context.CartItems.
                                            Where(c => c.CartHeaderId == cartHeader.Id));
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CartDTO> UpdateCartAsync(CartDTO cartDto)
    {
        Cart cart = _mapper.Map<Cart>(cartDto);
        
        await SaveProductInDataBase(cartDto, cart);
        //Verify if the CartHeader is null
        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);
        if (cartHeader is null)
        {
            await CreateHeaderAndItems(cart);

        }
        else
        {
            //update quantity of itens
            await UpdateQuantityAndItems(cartDto, cart, cartHeader);
        }
        return _mapper.Map<CartDTO>(cart);
    }
    private async Task UpdateQuantityAndItems(CartDTO cartDto, Cart cart, CartHeader? cartHeader)
    {
        //Se CartHeader não é null
        //verifica se CartItems possui o mesmo produto
        var cartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(
                               p => p.ProductId == cartDto.CartItems.FirstOrDefault()
                               .ProductId && p.CartHeaderId == cartHeader.Id);

        if (cartItem is null)
        {
            //Cria o CartItems
            cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;
            _context.CartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else
        {
            //Atualiza a quantidade e o CartItems
            cart.CartItems.FirstOrDefault().Product = null;
            cart.CartItems.FirstOrDefault().Quantity += cartItem.Quantity;
            cart.CartItems.FirstOrDefault().Id = cartItem.Id;
            cart.CartItems.FirstOrDefault().CartHeaderId = cartItem.CartHeaderId;
            _context.CartItems.Update(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }
    private async Task SaveProductInDataBase(CartDTO cartDto, Cart cart)
    {
        //Verifica se o produto ja foi salvo senão salva
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id ==
                            cartDto.CartItems.FirstOrDefault().ProductId);

        //salva o produto senão existe no bd
        if (product is null)
        {
            _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ApplyCouponsAsyn(string userID, string couponCode)
    {
        var cartHeaderApplyCoupon = await _context.CartHeaders
                               .FirstOrDefaultAsync(c => c.UserId == userID);

        if (cartHeaderApplyCoupon is not null)
        {
            cartHeaderApplyCoupon.CouponCode = couponCode;

            _context.CartHeaders.Update(cartHeaderApplyCoupon);

            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCouponAsyn(string userId)
    {
        var cartHeaderDeleteCoupon = await _context.CartHeaders
                               .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderDeleteCoupon is not null)
        {
            cartHeaderDeleteCoupon.CouponCode = "";

            _context.CartHeaders.Update(cartHeaderDeleteCoupon);

            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }
 
    private async Task CreateHeaderAndItems(Cart cart)
    {
        _context.CartHeaders.Add(cart.CartHeader);
        await _context.SaveChangesAsync();
        cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
        cart.CartItems.FirstOrDefault().Product = null;
        _context.CartItems.Add(cart.CartItems.FirstOrDefault());
        await _context.SaveChangesAsync();
    }

}
