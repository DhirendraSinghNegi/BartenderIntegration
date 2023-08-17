using BartenderIntegration.Infrastructure.Identity;
using BartenderIntegration.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BartenderIntegration.Infrastructure.Persistence
{
    public class AppDBInitializer : IAppDBInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppDBInitializer> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DefaultUser defaultUser;

        public AppDBInitializer(AppDbContext context, ILogger<AppDBInitializer> logger, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<DefaultUser> defaultUser)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            this.defaultUser = defaultUser.Value;
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while seeding data.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            try
            {
                var role = new IdentityRole("admin");
                var admin = new AppUser
                {
                    UserName = defaultUser.UserName,
                    Email = defaultUser.Email,
                    EmailConfirmed = true
                };

                if (_roleManager.Roles.All(x => x.Name != role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }

                if (_userManager.Users.All(x => x.UserName != admin.UserName))
                {
                    await _userManager.CreateAsync(admin, defaultUser.Password);
                    if (!string.IsNullOrWhiteSpace(role.Name))
                    {
                        await _userManager.AddToRoleAsync(admin, role.Name);
                    }
                    var user = await _userManager.FindByEmailAsync(admin.Email);
                    _context.UserProfiles.Add(new UserProfile
                    {
                        FirstName = defaultUser.FirstName,
                        UserId = user!.Id
                    });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while seeding data into db.");
                throw;
            }
        }
    }

    public interface IAppDBInitializer
    {
        Task SeedAsync();
    }
}
