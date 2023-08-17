using RepairsListener.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairsListener.Gateway.Interfaces
{
    public interface IRepairsStoredProcedureGateway
    {
        Task RunProcedure(string procName, params (string parameterName, string value)[] parameters);
    }
}
