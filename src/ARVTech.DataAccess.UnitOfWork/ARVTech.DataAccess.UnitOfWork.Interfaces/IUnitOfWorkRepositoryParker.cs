namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.Parker;

    public interface IUnitOfWorkRepositoryParker
    {
        IProdutoRepository ProdutoRepository { get; }
    }
}