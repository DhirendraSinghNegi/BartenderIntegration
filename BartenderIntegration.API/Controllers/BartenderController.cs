using BartenderIntegration.API.Helper;
using BartenderIntegration.API.Model;
using BartenderIntegration.Infrastructure.Identity;
using BartenderIntegration.Infrastructure.Models;
using BartenderIntegration.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BartenderIntegration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BartenderController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IBartenderHelper bartenderHelper;

        public BartenderController(UserManager<AppUser> userManager, ITokenService tokenService, IBartenderHelper bartenderHelper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            this.bartenderHelper = bartenderHelper;
        }

        [HttpPost, Route("Token")]
        public async Task<IActionResult> Token([FromBody] TokenRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = await _tokenService.GenerateTokenAsync(user);
                    return Ok(new ResultModel<AuthorizedModel>(true, token));
                }
                return Ok(new ResultModel<AuthorizedModel>(true, "Wrong password."));
            }
            return Ok(new ResultModel<bool>(false, "User not found."));
        }

        [Authorize, HttpPost, Route("PostData")]
        public IActionResult PostData([FromBody] CustomerModel data)
        {
            bartenderHelper.SendRequestAsync(data).ConfigureAwait(false);
            return Ok();
        }
    }
}
