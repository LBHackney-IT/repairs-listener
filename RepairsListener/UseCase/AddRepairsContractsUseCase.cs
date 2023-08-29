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


        public AddRepairsContractsUseCase(IRepairsStoredProcedureGateway gateway, ILogger<IAddRepairsContractsUseCase> logger) : base()
        {
            _gateway = gateway;
            _logger = logger;
        }

        [LogCall]
        public async Task ProcessMessageAsync(EntityEventSns message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            string jsonSnsMessage = JsonSerializer.Serialize(message);
            _logger.LogInformation("RepairsListener received AssetCreatedEvent SNS message with body {JsonSnsMessage}", jsonSnsMessage);

            // Get asset data from SNS message
            string addRepairsContractsMessage = message.EventData.NewData.ToString();

            // Required for deserialization
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the JSON content directly into an AddRepairsContractsToNewAssetObject object
            AddRepairsContractsToNewAssetObject addRepairsContractsMessageObject = JsonSerializer.Deserialize<AddRepairsContractsToNewAssetObject>(addRepairsContractsMessage, options);
            _logger.LogInformation("AddRepairsContractsMessageObject is being processed. Object: {AddRepairsContractsMessageObject}", JsonSerializer.Serialize(addRepairsContractsMessageObject));

            string propertyReference = addRepairsContractsMessageObject.PropRef;
            bool addRepairsContractsToAsset = addRepairsContractsMessageObject.AddRepairsContracts;

            if (string.IsNullOrEmpty(propertyReference))
            {
                throw new ArgumentNullException(nameof(addRepairsContractsMessageObject.PropRef));
            }

            if (addRepairsContractsToAsset)
            {
                var parameters = ("property_reference", propertyReference);

                await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);
            }
        }
    }
}
