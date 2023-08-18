using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase.Interfaces;
using Hackney.Core.Logging;
using System;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;

namespace RepairsListener.UseCase
{
    public class CreateAssetUseCase : ICreateAssetUseCase
    {
        private readonly IRepairsStoredProcedureGateway _gateway;

        public CreateAssetUseCase(IRepairsStoredProcedureGateway gateway) : base()
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task ProcessMessageAsync(EntityEventSns message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            var newData = (Asset) message.EventData.NewData;
            var propertyReference = newData.AssetId;

            if (string.IsNullOrEmpty(propertyReference)) {
                throw new ArgumentNullException(nameof(newData.AssetId));
            }

            var parameters = ("property_reference", propertyReference);

            await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);
        }
    }
}
