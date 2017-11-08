using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class BaseService
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        protected T GetValue<T>(object readerValue, T defaultValue = default(T))
        {
            if (readerValue == null || readerValue == DBNull.Value)
                return defaultValue;
            else
                return (T)Convert.ChangeType(readerValue, typeof(T));
        }

        protected void ExecuteNonQuery(string storedProcedure, Action<SqlCommand> fillCommand)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<string> ExecuteScalar(string storedProcedure, Action<SqlCommand> fillCommand)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    await connection.OpenAsync();
                    return (await command.ExecuteScalarAsync()).ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void ExecuteReader(string storedProcedure, Action<SqlCommand> fillCommand, Action<SqlDataReader> fetchData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    connection.Open();
                    fetchData(command.ExecuteReader());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}