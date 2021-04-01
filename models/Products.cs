using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class Products
    {
        public int id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public decimal price { get; set; }
        public int id_categories { get; set; }
        public int id_user { get; set; }

        internal AppDb Db { get; set; }

        public Products()
        {
        }

        internal Products(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL register_product(@name, @amount, @price, @id_categories, @id_user);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL put_product(@name, @amount, @price, @id_categories, @id, @id_user);";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"CALL del_product(@id_user, @id);";
            BindId(cmd);
            BindParams(cmd);
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
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@amount",
                DbType = DbType.Int32,
                Value = amount,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@price",
                DbType = DbType.Decimal,
                Value = price,
            });           
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_user",
                DbType = DbType.Int32,
                Value = id_user,
            });  
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_categories",
                DbType = DbType.Int32,
                Value = id_categories,
            });   
        }
    }
}