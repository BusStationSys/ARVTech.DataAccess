namespace ARVTech.DataAccess.UnitOfWork.SqlServer.UniPayCheck
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using ARVTech.DataAccess.Repository.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryUniPayCheck : IUnitOfWorkRepositoryUniPayCheck
    {
        public IPessoaFisicaRepository PessoaFisicaRepository { get; }

        public IPessoaJuridicaRepository PessoaJuridicaRepository { get; }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection)
        {
            this.PessoaFisicaRepository = new PessoaFisicaRepository(
                connection);

            this.PessoaJuridicaRepository = new PessoaJuridicaRepository(
                connection);
        }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection, SqlTransaction transaction)
        {
            this.PessoaFisicaRepository = new PessoaFisicaRepository(
                connection, 
                transaction);

            this.PessoaJuridicaRepository = new PessoaJuridicaRepository(
                connection, 
                transaction);
        }
    }
}