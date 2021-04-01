using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class CustomersQuery
    {
        public AppDb Db { get; }

        public CustomersQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Customers> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL get_user_info(@id)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
/*
        public async Task<List<Customers>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `name`, `street`, `city`, `state`, `credit_limit` FROM `customers` ORDER BY `id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `customers`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }
*/
        private async Task<List<Customers>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Customers>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Customers(Db)
                    {
                        id_user = reader.GetInt32(0),
                        user = reader.GetString(1),
                        cpf = reader.GetString(2),
                        name = reader.GetString(3),
                        street = reader.GetString(4),
                        city = reader.GetString(5),
                        state = reader.GetString(6),
                        credit_limit = reader.GetDecimal(7),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}