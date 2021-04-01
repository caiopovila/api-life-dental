using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class CategoriesQuery
    {
        public AppDb Db { get; }

        public CategoriesQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Categories> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `name` FROM `Categories` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Categories>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `name` FROM `Categories` ORDER BY `id` DESC;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
/*
        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Categories`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }
*/
        private async Task<List<Categories>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Categories>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Categories(Db)
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}