namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.Empresarius;

    public interface IUnitOfWorkRepositoryEmpresarius
    {
        IProdutoRepository ProdutoRepository { get; }
    }
}