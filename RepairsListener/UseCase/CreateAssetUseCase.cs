using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase.Interfaces;
using Hackney.Core.Logging;
using System;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

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

            string jsonSnsMessage = JsonConvert.SerializeObject(message);
            _logger.LogInformation("RepairsListener received SNS message with body {JsonSnsMessage}", jsonSnsMessage);

            string newDataRawText = message.EventData.NewData.ToString();
            _logger.LogInformation("For troubleshooting purposes: NewData raw text from SNS message: {NewDataRawText}", newDataRawText);

            string newDataJson = JsonConvert.SerializeObject(message.EventData.NewData);
            _logger.LogInformation("For troubleshooting purposes: NewData JSON from SNS message: {NewDataJson}", newDataJson);

            var newData = (Asset) message.EventData.NewData;
            var propertyReference = newData.AssetId;

            if (string.IsNullOrEmpty(propertyReference))
            {
                throw new ArgumentNullException(nameof(newData.AssetId));
            }

            var parameters = ("property_reference", propertyReference);

            await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);
        }
    }
}
