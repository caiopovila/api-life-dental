using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DbApi.Controllers
{
    [Route("api/[controller]")]
    public class NaturalPersonController : ControllerBase
    {
        public NaturalPersonController(AppDb db)
        {
            Db = db;
        }
/*
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new NaturalPersonQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }
*/
        [HttpGet]
        public async Task<IActionResult> GetOne()
        {
            await Db.Connection.OpenAsync();
            var query = new NaturalPersonQuery(Db);
            var result = await query.FindOneAsync(int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
/*
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NaturalPerson body)
        {
            body.id_customers = int.Parse(User.Identity.Name);
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        [HttpPut]
        public async Task<IActionResult> PutOne([FromBody]NaturalPerson body)
        {
            await Db.Connection.OpenAsync();
            var query = new NaturalPersonQuery(Db);
            var result = await query.FindOneAsync(int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            result.cpf = body.cpf;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }
*/
        public AppDb Db { get; }
    }
}