namespace MusicShop.IdentityServer.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {}
}
