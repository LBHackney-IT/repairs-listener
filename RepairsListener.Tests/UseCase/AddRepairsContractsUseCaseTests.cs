using AutoFixture;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;
using Moq;
using RepairsListener.Boundary;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.Infrastructure;
using RepairsListener.UseCase;
using System;
using System.CodeDom;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RepairsListener.Tests.UseCase
{
    [Collection("LogCall collection")]
    public class AddRepairsContractsUseCaseTests
    {
        private readonly AddRepairsContractsUseCase _classUnderTest;
        private readonly Mock<IRepairsStoredProcedureGateway> _gatewayMock;

        private readonly Fixture _fixture = new Fixture();

        public AddRepairsContractsUseCaseTests()
        {
            _gatewayMock = new Mock<IRepairsStoredProcedureGateway>();

            _classUnderTest = new AddRepairsContractsUseCase(_gatewayMock.Object, new NullLogger<AddRepairsContractsUseCase>());    
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
        public async void ProcessMessageAsync_WhenPropRefValid_CallsStoredProcedure()
        {
            // Arrange
            Guid entityId = _fixture.Create<Guid>();
            string propRef = _fixture.Create<string>();

            JsonElement addRepairsContractsMessageObject = JsonSerializer.SerializeToElement(new AddRepairsContractsToNewAssetObject {
                EntityId = entityId,
                PropRef = propRef,
            });

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = addRepairsContractsMessageObject
                }
            };

            // Act
            await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            var expectedParameters = ("property_reference", propRef);

            _gatewayMock
                .Verify(x => x.RunProcedure("assign_dlo_property_contracts_to_newbuild", expectedParameters), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void ProcessMessageAsync_WhenPropRefInvalid_ThrowsException(string inputPropRef)
        {
            // Arrange
            Guid entityId = new Guid();
            string propRef = inputPropRef;

            JsonElement addRepairsContractsMessageObject = JsonSerializer.SerializeToElement(new AddRepairsContractsToNewAssetObject
            {
                EntityId = entityId,
                PropRef = propRef,
            });

            var message = new EntityEventSns
            {
                EventData = new EventData
                {
                    NewData = addRepairsContractsMessageObject
                }
            };

            // Act
            Func<Task> func = async () => await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
