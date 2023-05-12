﻿using Duende.IdentityServer.Models;
using IdentityModel;
using System.Security.Claims;

namespace MusicShop.IdentityServer.SeedDataBase;

public class DatabaseIdentityServerInitializer : IDataBaseSeedInitialazer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseIdentityServerInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;

    }

    public void InitializeRoles()
    {       //Se o Perfil não extistir emtão cria o perfil
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
        {
            IdentityRole roleAdmin = new IdentityRole();
            roleAdmin.Name = IdentityConfiguration.Admin;
            roleAdmin.NormalizedName = IdentityConfiguration.Admin.ToUpper();
            _roleManager.CreateAsync(roleAdmin).Wait();
        }
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
        {
            IdentityRole roleClient = new IdentityRole();
            roleClient.Name = IdentityConfiguration.Client;
            roleClient.NormalizedName = IdentityConfiguration.Client.ToUpper();
            _roleManager.CreateAsync(roleClient).Wait();
        }
    }

    public void InitializeUsers()
    {
        if (_userManager.FindByEmailAsync("admin1@com.pt").Result == null)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "admin1",
                NormalizedUserName = "ADMIN1",
                Email = "admin1@com.pt",
                NormalizedEmail = "ADMIN1@COM.PT",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "AnyNumber",
                FirstName = "Usuario",
                LastName = "Admin1",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Admin#2023").Result;
            if(resultAdmin.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();
                var adminClains = _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName,admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                }).Result;
            }
         }
        if (_userManager.FindByEmailAsync("client1@com.pt").Result == null)
        {
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "client1",
                NormalizedUserName = "CLIENT1",
                Email = "client1@com.pt",
                NormalizedEmail = "CLIENT1!@COM.PT",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "AnyNumber",
                FirstName = "Usuario",
                LastName = "client1",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            IdentityResult resultClient = _userManager.CreateAsync(client, "Client#2023").Result;
            if (resultClient.Succeeded)
            {
                _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).Wait();
                var clientClains = _userManager.AddClaimsAsync(client, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                    new Claim(JwtClaimTypes.GivenName,client.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, client.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                }).Result;
            }
        }
    }
}
