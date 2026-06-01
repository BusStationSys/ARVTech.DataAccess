namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class BrokenRollbackUnitOfWorkFixture : UnitOfWorkFixture
    {
        public BrokenRollbackUnitOfWorkFixture()
        {
            //  Descarta o adapter antecipadamente.
            //  Quando Dispose() chamar Rollback(), lançará exceção
            //  pois a conexão já foi fechada/descartada.
            this.UnitOfWorkAdapter.Dispose();
        }
    }
}