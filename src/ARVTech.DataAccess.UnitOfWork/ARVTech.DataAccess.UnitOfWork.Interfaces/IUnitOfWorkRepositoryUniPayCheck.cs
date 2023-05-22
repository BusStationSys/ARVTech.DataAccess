namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;

    public interface IUnitOfWorkRepositoryUniPayCheck
    {
        IPessoaFisicaRepository PessoaFisicaRepository { get; }

        IPessoaJuridicaRepository PessoaJuridicaRepository { get; }
    }
}