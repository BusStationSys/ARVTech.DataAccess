namespace ARV.DataAccess.UnitOfWork.Access
{
    using System.Data.OleDb;
    using ARV.DataAccess.Repository.Access.Parker;
    using ARV.DataAccess.Repository.Interfaces.Parker;
    using ARV.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkAccessRepositoryParker : IUnitOfWorkRepositoryParker
    {
        public IProdutoRepository ProdutoRepository { get; }

        public UnitOfWorkAccessRepositoryParker(OleDbConnection connection, OleDbTransaction transaction)
        {
            this.ProdutoRepository = new ProdutoRepository(connection, transaction);
        }
    }
}