using RepairsListener.Boundary;
using RepairsListener.Domain;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.Infrastructure.Exceptions;
using RepairsListener.UseCase.Interfaces;
using Hackney.Core.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.Lambda.Core;

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

            var assetId = message?.EntityId.ToString();
            var parameters = ("property_reference", assetId);

            await _gateway.RunProcedure("assign_dlo_property_contracts_to_newbuild", parameters);

            await Task.CompletedTask;
        }
    }
}
