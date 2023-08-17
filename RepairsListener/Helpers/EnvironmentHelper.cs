using System;

namespace RepairsListener.Helpers
{
    public static class EnvironmentHelper
    {
        public static string GetEnvironmentVariable(string name)
        {
            var connectionString = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"The Environment Variable \"{name}\" is empty");
            }

            return connectionString;
        }
    }
}
