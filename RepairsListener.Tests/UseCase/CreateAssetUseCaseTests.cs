using AutoFixture;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairsListener.Boundary;
using RepairsListener.Gateway;
using RepairsListener.Gateway.Interfaces;
using RepairsListener.UseCase;
using RepairsListener.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

      //  [Setup]
        public CreateAssetUseCaseTests()
        {
            _gatewayMock = new Mock<IRepairsStoredProcedureGateway>();

            _classUnderTest = new CreateAssetUseCase(_gatewayMock.Object);
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
        public async void ProcessMessageAsync_WhenValid_CallsStoredProcedure()
        {
            // Arrange
            var message = new EntityEventSns
            {
                EntityId = Guid.NewGuid()
            };

            // Act
            await _classUnderTest.ProcessMessageAsync(message);

            // Assert
            var expectedParameters = ("property_reference", message.EntityId.ToString());

            _gatewayMock
                .Verify(x => x.RunProcedure("assign_dlo_property_contracts_to_newbuild", expectedParameters), Times.Once);
        }
    }
}
