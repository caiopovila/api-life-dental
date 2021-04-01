using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class Login
    {
        private int id { get; set; }
        public string user { get; set; }
        public string password { get; set; }

        internal AppDb Db { get; set; }

        public Login()
        {
        }

        internal Login(AppDb db)
        {
            Db = db;
        }

        public async Task<Login> auth()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL auth_user(@user, @password);";
            BindParams(cmd);
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `users` WHERE `id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }    

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@user",
                DbType = DbType.String,
                Value = user,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }
        
        public int IdUser {
            get { return id; }
        }

        public bool ValidLogin {
            get { return id != 0 ? true : false; }
        }

        private async Task<Login> ReadAllAsync(DbDataReader reader)
        {
            var posts = new Login();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Login(Db)
                    {
                        id = reader.GetInt32(0),
                    };
                    posts = post;
                }
            }
            return posts;
        }
    }
}