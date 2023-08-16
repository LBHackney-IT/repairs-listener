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
        //private readonly IDbEntityGateway _gateway;

        public CreateAssetUseCase()
        {
            //_gateway = gateway;
        }

        public IConfiguration Configuration { get; }

        [LogCall]
        public async Task ProcessMessageAsync(EntityEventSns message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            LambdaLogger.Log($"This is working and Dave is a legend. Environment: {Environment.GetEnvironmentVariable("ENVIRONMENT")} ASPNETCORE Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            LambdaLogger.Log($"Database: {Environment.GetEnvironmentVariable("REPAIRS_DB_CONNECTION_STRING")}");


            // TODO - Implement use case logic
            //DomainEntity entity = await _gateway.GetEntityAsync(message.EntityId).ConfigureAwait(false);
            //if (entity is null) throw new EntityNotFoundException<DomainEntity>(message.EntityId);

            //entity.Description = "Updated";

            // Save updated entity
            //await _gateway.SaveEntityAsync(entity).ConfigureAwait(false);
            await Task.CompletedTask;
        }
    }
}
