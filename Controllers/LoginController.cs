using System.Threading.Tasks;
using DbApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public LoginController(AppDb db)
        {
            Db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<dynamic>> auth([FromBody]Login body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            var retAuth = await body.auth();
            if (!retAuth.ValidLogin)
                return NotFound(new { message = "Usuário ou senha incorreto(s)!" });
            var token = TokenService.GenerateToken(retAuth);
            return new { token = token, id = retAuth.IdUser };
        }
        public AppDb Db { get; }
    }
}