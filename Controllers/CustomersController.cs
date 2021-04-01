using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        public CustomersController(AppDb db)
        {
            Db = db;
        }
/*
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new CustomersQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }
*/
        [HttpGet]
        public async Task<IActionResult> GetOne()
        {
            await Db.Connection.OpenAsync();
            var query = new CustomersQuery(Db);
            var result = await query.FindOneAsync(int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Customers body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        [HttpPut]
        public async Task<IActionResult> PutOne([FromBody]Customers body)
        {
            await Db.Connection.OpenAsync();
            var query = new CustomersQuery(Db);
            var result = await query.FindOneAsync(int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            result.name = body.name;
            result.street = body.street;
            result.city = body.city;
            result.state = body.state;
            result.cpf = body.cpf;
            if (body.password != "")
                result.password = body.password;
            result.user = body.user;
            result.credit_limit = body.credit_limit;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }
/*
        [HttpDelete]
        public async Task<IActionResult> DeleteOne()
        {
            await Db.Connection.OpenAsync();
            var query = new CustomersQuery(Db);
            var result = await query.FindOneAsync(int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }
*/
        public AppDb Db { get; }
    }
}