namespace ARVTech.DataAccess.Infrastructure.IntegrationTests
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures;
    using FluentAssertions;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class PessoaJuridicaRepositoryIntegrationTests : IClassFixture<UnitOfWorkFixture>
    {
        private readonly UnitOfWorkFixture _unitOfWorkFixture;

        public PessoaJuridicaRepositoryIntegrationTests(UnitOfWorkFixture unitOfWorkFixture)
        {
            this._unitOfWorkFixture = unitOfWorkFixture;
        }

        [Fact]
        public void GetAll_ShouldReturnRecords()
        {
            var result = this._unitOfWorkFixture.UnitOfWorkAdapter.RepositoriesUniPayCheck.PessoaJuridicaRepository.GetAll();

            result.Should().NotBeNull();
            result.Should().AllSatisfy(pj =>
            {
                pj.Guid.Should().NotBeEmpty();
                pj.Pessoa.Should().NotBeNull();
                pj.UnidadeNegocio.Should().NotBeNull();
            });
        }
    }
}