using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class NaturalPersonQuery
    {
        public AppDb Db { get; }

        public NaturalPersonQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<NaturalPerson> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL get_cpf(@id)";
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
        public async Task<List<NaturalPerson>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_customers`, `cpf` FROM `natural_person` ORDER BY `id_customers` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
*/
        private async Task<List<NaturalPerson>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<NaturalPerson>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new NaturalPerson(Db)
                    {
                        id_user = reader.GetInt32(0),
                        cpf = reader.GetString(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}