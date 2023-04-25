namespace ARVTech.DataAccess.UnitOfWork.SqlServer
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Repository.Interfaces.Empresarius;
    using ARVTech.DataAccess.Repository.SqlServer.Empresarius;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryEmpresarius : IUnitOfWorkRepositoryEmpresarius
    {
        public IProdutoRepository ProdutoRepository { get; }

        public UnitOfWorkSqlServerRepositoryEmpresarius(SqlConnection connection, SqlTransaction transaction)
        {
            this.ProdutoRepository = new ProdutoRepository(connection, transaction);
        }
    }
}