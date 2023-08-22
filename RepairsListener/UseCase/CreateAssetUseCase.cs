using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase.Interfaces;
using Hackney.Core.Logging;
using System;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace RepairsListener.UseCase
{
    public class CreateAssetUseCase : ICreateAssetUseCase
    {
        private readonly IRepairsStoredProcedureGateway _gateway;
        private readonly ILogger<ICreateAssetUseCase> _logger;


        public CreateAssetUseCase(IRepairsStoredProcedureGateway gateway, ILogger<ICreateAssetUseCase> logger) : base()
        {
            _gateway = gateway;
            _logger = logger;
        }

        [LogCall]
        public async Task ProcessMessageAsync(EntityEventSns message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            string jsonSnsMessage = JsonSerializer.Serialize(message);
            _logger.LogInformation("RepairsListener received SNS message with body {JsonSnsMessage}", jsonSnsMessage);

            // Get asset data from SNS message
            string newAssetStringified = message.EventData.NewData.ToString();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the JSON content directly into an Asset object
            Asset newAsset = JsonSerializer.Deserialize<Asset>(newAssetStringified, options);
            _logger.LogInformation("New Asset received by RepairsListener. Asset: {NewAsset}", JsonSerializer.Serialize(newAsset));

            var propertyReference = newAsset.AssetId;

            if (string.IsNullOrEmpty(propertyReference))
            {
                throw new ArgumentNullException(nameof(newAsset.AssetId));
            }

            var parameters = ("property_reference", propertyReference);

            await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);
        }
    }
}
