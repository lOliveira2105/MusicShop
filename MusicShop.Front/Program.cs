
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ProductApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServicesUri:ProductApi"]);
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Coockies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Coockies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["ServicesUri:IdentityServer"];
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "musicshop";
        options.ClientSecret = builder.Configuration["Client:Secret"];
        options.ResponseType = "code";
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("MusicShop");
        options.SaveTokens = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
