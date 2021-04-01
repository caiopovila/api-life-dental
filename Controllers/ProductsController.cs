using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class ReqQuery {
    public string q { get; set; }
}

namespace DbApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            string param = Request.Query["limit"];
            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            var result = await query.LatestPostsAsync(int.Parse(User.Identity.Name), param is not null ? int.Parse(param) : 10);
            return new OkObjectResult(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetSearch([FromBody]ReqQuery body)
        {
            string off = Request.Query["offset"];
            string row = Request.Query["row"];

            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            var result = await query.SearchAsync(body.q, off is not null ? int.Parse(off) : 0, row is not null ? int.Parse(row) : 10);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            var result = await query.FindOneAsync(id, int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Products body)
        {
            body.id_user = int.Parse(User.Identity.Name);
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Products body)
        {
            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            var result = await query.FindOneAsync(id, int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            result.name = body.name;
            result.amount = body.amount;
            result.price = body.price;
            result.id_categories = body.id_categories;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            var result = await query.FindOneAsync(id, int.Parse(User.Identity.Name));
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }
/*
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ProductsQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
*/
        public AppDb Db { get; }
    }
}