
namespace MusicShop.IdentityServer.Services;

public class ProfileAppService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileAppService(UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, 
                            RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {   //id do usuário no IS
        string id = context.Subject.GetSubjectId();
        //localiza o usuário pelo Id
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        //Cria Claims para o usuario
        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        //Define uma coleção de claims para o usuario
        //e inclui o sobrenome e o nome do usuário
        List<Claim> claims = userClaims.Claims.ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

        //Se o userManager do identity sporta role
        if (_userManager.SupportsUserRole)
        {
            //Obtem a kusta dos nomes das roles para o usuário
            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                //adiciona a role na claim
                claims.Add(new Claim(JwtClaimTypes.Role, role));
                if (_roleManager.SupportsRoleClaims)
                {       //localiza o perfil
                    IdentityRole identityRole = await _roleManager.FindByNameAsync(role);
                        //nlcui o perfil
                        if (identityRole != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                    }
                }
            }

        }
        context.IssuedClaims = claims;

    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        //id do usuário no IS
        string id = context.Subject.GetSubjectId();
        //localiza o usuário pelo Id
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        //verifica se esta ativo
        context.IsActive = user is not null;
    }
}
