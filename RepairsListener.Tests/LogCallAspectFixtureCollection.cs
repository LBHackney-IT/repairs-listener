using Hackney.Core.Testing.Shared;
using Xunit;

namespace RepairsListener.Tests
{
    [CollectionDefinition("LogCall collection")]
    public class LogCallAspectFixtureCollection : ICollectionFixture<LogCallAspectFixture>
    { }
}
