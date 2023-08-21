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
            var asset = new Asset
            {

            };

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = asset
                }
            };

            // Act
            Func<Task> func = async () => await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            await func.Should().ThrowAsync<ArgumentNullException>(nameof(asset.Id));
        }

        [Fact]
        public async void ProcessMessageAsync_WhenValid_CallsStoredProcedure()
        {
            // Arrange
            var asset = new Asset
            {
                AssetId = _fixture.Create<string>()
            };

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = asset
                }
            };

            // Act
            await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            var expectedParameters = ("property_reference", asset.AssetId);

            _gatewayMock
                .Verify(x => x.RunProcedure("assign_dlo_property_contracts_to_newbuild", expectedParameters), Times.Once);
        }
    }
}
