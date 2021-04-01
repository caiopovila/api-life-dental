using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace DbApi
{
    public class NaturalPerson
    {
        public int id_user { get; set; }
        public string cpf { get; set; }

        internal AppDb Db { get; set; }

        public NaturalPerson()
        {
        }

        internal NaturalPerson(AppDb db)
        {
            Db = db;
        }
/*
        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `natural_person` (`id_customers`, `cpf`) VALUES (@id_customers, @cpf);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_customers = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `natural_person` SET `cpf` = @cpf WHERE `id_user` = @id_user;";
            BindParams(cmd);
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
                ParameterName = "@cpf",
                DbType = DbType.String,
                Value = cpf,
            });
        }
    }
}