using RepairsListener.Boundary;
using System.Threading.Tasks;

namespace RepairsListener.UseCase.Interfaces
{
    public interface IMessageProcessing
    {
        Task ProcessMessageAsync(EntityEventSns message);
    }
}
