
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






    }
}
