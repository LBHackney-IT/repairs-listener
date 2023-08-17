using RepairsListener.Domain;
using RepairsListener.Gateway.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairsListener.Gateway
{
    public class RepairsStoredProcedureGateway : IRepairsStoredProcedureGateway
    {
        public Task RunProcedure(string procName, params (string, string)[] parameters)
        {
            string connectionString = Environment.GetEnvironmentVariable("REPAIRS_DB_CONNECTION_STRING");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }

                    var result = command.ExecuteNonQuery();

                }
            }
        }
    }
}
