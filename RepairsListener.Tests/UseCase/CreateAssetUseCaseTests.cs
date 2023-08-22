using AutoFixture;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;
using Moq;
using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase;
using System;
using System.CodeDom;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RepairsListener.Tests.UseCase
{
    [Collection("LogCall collection")]
    public class CreateAssetUseCaseTests
    {
        private readonly CreateAssetUseCase _classUnderTest;
        private readonly Mock<IRepairsStoredProcedureGateway> _gatewayMock;

        private readonly Fixture _fixture = new Fixture();

        public CreateAssetUseCaseTests()
        {
            _gatewayMock = new Mock<IRepairsStoredProcedureGateway>();

            _classUnderTest = new CreateAssetUseCase(_gatewayMock.Object, new NullLogger<CreateAssetUseCase>());
        }

        [Fact]
        public async void ProcessMessageAsync_WhenMessageNull_ThrowsException()
        {
            // Arrange
            var message = (EntityEventSns) null;

            // Act
            Func<Task> func = async () => await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            await func.Should().ThrowAsync<ArgumentNullException>(nameof(EntityEventSns));
        }

        [Fact]
        public async void ProcessMessageAsync_WhenAssetIdNull_ThrowsException()
        {
            // Arrange
            JsonElement assetJsonElement = JsonSerializer.SerializeToElement(new Asset { });

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = assetJsonElement
                }
            };

            // Act
            Func<Task> func = async () => await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void ProcessMessageAsync_WhenValid_CallsStoredProcedure()
        {
            // Arrange
            string assetId = _fixture.Create<string>();

            JsonElement assetJsonElement = JsonSerializer.SerializeToElement(new Asset { AssetId = assetId });

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = assetJsonElement
                }
            };

            // Act
            await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            var expectedParameters = ("property_reference", assetId);

            _gatewayMock
                .Verify(x => x.RunProcedure("assign_dlo_property_contracts_to_newbuild", expectedParameters), Times.Once);
        }
    }
}
