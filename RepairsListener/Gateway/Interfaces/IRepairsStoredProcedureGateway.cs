using System.Threading.Tasks;

namespace RepairsListener.Gateway.Interfaces
{
    public interface IRepairsStoredProcedureGateway
    {
        Task RunProcedure(string procName, params (string parameterName, string value)[] parameters);
    }
}
