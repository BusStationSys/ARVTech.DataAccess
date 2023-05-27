namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;

    public interface IUnitOfWorkRepositoryUniPayCheck
    {
        IPessoaRepository PessoaRepository { get; }

        IPessoaFisicaRepository PessoaFisicaRepository { get; }

        IPessoaJuridicaRepository PessoaJuridicaRepository { get; }
    }
}