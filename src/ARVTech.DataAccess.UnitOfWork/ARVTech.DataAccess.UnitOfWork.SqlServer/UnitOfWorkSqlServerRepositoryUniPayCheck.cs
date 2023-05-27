namespace ARVTech.DataAccess.UnitOfWork.SqlServer.UniPayCheck
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using ARVTech.DataAccess.Repository.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryUniPayCheck : IUnitOfWorkRepositoryUniPayCheck
    {
        public IMatriculaRepository MatriculaRepository { get; }

        public IPessoaRepository PessoaRepository { get; }

        public IPessoaFisicaRepository PessoaFisicaRepository { get; }

        public IPessoaJuridicaRepository PessoaJuridicaRepository { get; }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection)
        {
            this.MatriculaRepository = new MatriculaRepository(
                connection);

            this.PessoaRepository = new PessoaRepository(
                connection);

            this.PessoaFisicaRepository = new PessoaFisicaRepository(
                connection);

            this.PessoaJuridicaRepository = new PessoaJuridicaRepository(
                connection);
        }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection, SqlTransaction transaction)
        {
            this.MatriculaRepository = new MatriculaRepository(
                connection,
                transaction);

            this.PessoaRepository = new PessoaRepository(
                connection,
                transaction);

            this.PessoaFisicaRepository = new PessoaFisicaRepository(
                connection,
                transaction);

            this.PessoaJuridicaRepository = new PessoaJuridicaRepository(
                connection,
                transaction);
        }
    }
}