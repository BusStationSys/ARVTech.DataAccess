namespace ARVTech.DataAccess.Infrastructure.IntegrationTests
{
    using ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures;
    using FluentAssertions;
    using Xunit;

    public class PessoaFisicaRepositoryIntegrationTests : IClassFixture<UnitOfWorkFixture>
    {
        private readonly UnitOfWorkFixture _unitOfWorkFixture;

        public PessoaFisicaRepositoryIntegrationTests(UnitOfWorkFixture unitOfWorkFixture)
        {
            this._unitOfWorkFixture = unitOfWorkFixture;
        }

        [Fact]
        public void GetAll_ShouldReturnRecords()
        {
            var result = this._unitOfWorkFixture.UnitOfWorkAdapter.RepositoriesUniPayCheck.PessoaFisicaRepository.GetAll();

            result.Should().NotBeNull();
            result.Should().AllSatisfy(pf =>
            {
                pf.Guid.Should().NotBeEmpty();
                pf.Pessoa.Should().NotBeNull();
            });
        }
    }
}