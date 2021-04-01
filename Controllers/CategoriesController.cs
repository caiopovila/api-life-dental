using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DbApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new CategoriesQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new CategoriesQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Categories body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Categories body)
        {
            await Db.Connection.OpenAsync();
            var query = new CategoriesQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.name = body.name;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new CategoriesQuery(Db);
            var result = await query.FindOneAsync(id);
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
            var query = new CategoriesQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
*/
        public AppDb Db { get; }
    }
}