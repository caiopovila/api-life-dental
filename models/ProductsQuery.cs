using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class ProductsQuery
    {
        public AppDb Db { get; }

        public ProductsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Products> FindOneAsync(int id, int id_user)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL get_product(@id_user, @id)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });            
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Products>> LatestPostsAsync(int id, int limit)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL list_product(@id, @limit)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });        
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@limit",
                DbType = DbType.Int32,
                Value = limit,
            });                 
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Products>> SearchAsync(string q, int off, int row)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL search_products(@q, @off, @row)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@off",
                DbType = DbType.Int32,
                Value = off,
            });        
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@row",
                DbType = DbType.Int32,
                Value = row,
            });          
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@q",
                DbType = DbType.String,
                Value = q,
            });             
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
/*
        public async Task DeleteAllAsync(int id_user)
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `products` WHERE id_user = @id_user";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });            
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }
*/
        private async Task<List<Products>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Products>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Products(Db)
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetDecimal(3),
                        id_categories = reader.GetInt32(4),
                        id_user = reader.GetInt32(5),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}