using RepairsListener.Gateway.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Logging;
using RepairsListener.Helpers;

namespace RepairsListener.Gateway
{
    public class RepairsStoredProcedureGateway : IRepairsStoredProcedureGateway
    {
        private readonly ILogger<IRepairsStoredProcedureGateway> _logger;

        public RepairsStoredProcedureGateway(ILogger<IRepairsStoredProcedureGateway> logger)
        {
            _logger = logger;
        }

        public async Task RunProcedure(string procName, params (string parameterName, string value)[] parameters)
        {
            var connectionString = EnvironmentHelper.GetEnvironmentVariable("REPAIRS_DB_CONNECTION_STRING");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(procName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var (parameterName, value) in parameters)
                    {
                        command.Parameters.AddWithValue(parameterName, value);
                    }

                    _logger.LogInformation("Calling stored procedure called {ProcName} with {Parameters}", procName, parameters);

                    try
                    {
                        var result = await command.ExecuteNonQueryAsync();

                        _logger.LogInformation("Successfully called stored procedure called {ProcName} with {Parameters}", procName, parameters);

                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation("Failed to call stored procedure called {ProcName} with {Parameters} with {Exception}", procName, parameters, e);
                        throw;
                    }
                }
            }
        }
    }
}
