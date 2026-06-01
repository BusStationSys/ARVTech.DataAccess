namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UnitOfWorkFixtureTest
    {
        [Fact]
        public void Dispose_WhenCalledTwice_ShouldNotThrowException()
        {
            var fixture = new UnitOfWorkFixture();
            fixture.Dispose();

            var exception = Record.Exception(() => fixture.Dispose());
            Assert.Null(exception);
        }

        [Fact]
        public void Dispose_WhenRollbackFails_ShouldNotThrowException()
        {
            //  Arrange — adapter já descartado força falha no Rollback
            var fixture = new BrokenRollbackUnitOfWorkFixture();

            //  Act & Assert — catch deve engolir a exceção silenciosamente
            var exception = Record.Exception(() => fixture.Dispose());
            Assert.Null(exception);
        }
    }
}