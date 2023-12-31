using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase.Interfaces;
using Hackney.Core.Logging;
using System;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RepairsListener.Infrastructure;

namespace RepairsListener.UseCase
{
    public class AddRepairsContractsUseCase : IAddRepairsContractsUseCase
    {
        private readonly IRepairsStoredProcedureGateway _gateway;
        private readonly ILogger<IAddRepairsContractsUseCase> _logger;


        public AddRepairsContractsUseCase(IRepairsStoredProcedureGateway gateway, ILogger<IAddRepairsContractsUseCase> logger)
        {
            _gateway = gateway;
            _logger = logger;
        }

        [LogCall]
        public async Task ProcessMessageAsync(EntityEventSns message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            if (message.EventData.NewData is null)
            {
                _logger.LogInformation("The data within the AddRepairsContractsToAssetEvent message for asset with ID {EntityId} is invalid (null).", message.EntityId);
                throw new ArgumentNullException(nameof(message.EventData.NewData));
            }

            // Get data from SNS message in string format
            string addRepairsContractsMessage = message.EventData.NewData.ToString();

            // Required for deserialization
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Deserialize the JSON content directly into an AddRepairsContractsToNewAssetObject object
            AddRepairsContractsToNewAssetObject addRepairsContractsMessageObject = JsonSerializer.Deserialize<AddRepairsContractsToNewAssetObject>(addRepairsContractsMessage, options);

            string propertyReference = addRepairsContractsMessageObject.PropRef;

            _logger.LogInformation("RepairsListener received AddRepairsContractsToAssetEvent for asset ID: {PropRef}", propertyReference);

            if (string.IsNullOrEmpty(propertyReference))
            {
                throw new ArgumentNullException(nameof(addRepairsContractsMessageObject.PropRef));
            }

            var parameters = ("property_reference", propertyReference);

            await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);
        }
    }
}
