using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class Customers
    {
        public int id_user { get; set; }
        public string name { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public decimal credit_limit { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string cpf { get; set; }

        internal AppDb Db { get; set; }

        public Customers()
        {
        }

        internal Customers(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL register_user(@user, @password, @name, @street, @city, @state, @cpf);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_user = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL put_user(@id_user, @user, @password, @name, @street, @city, @state, @cpf, @credit_limit)";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
/*
        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `customers` WHERE `id_user` = @id_user;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
*/
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@street",
                DbType = DbType.String,
                Value = street,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@city",
                DbType = DbType.String,
                Value = city,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@state",
                DbType = DbType.String,
                Value = state,
            });
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
                ParameterName = "@credit_limit",
                DbType = DbType.Decimal,
                Value = credit_limit,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cpf",
                DbType = DbType.String,
                Value = cpf,
            });
        }
    }
}