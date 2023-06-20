namespace ARVTech.DataAccess.UnitOfWork.SqlServer.UniPayCheck
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using ARVTech.DataAccess.Repository.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryUniPayCheck : IUnitOfWorkRepositoryUniPayCheck
    {
        public IEventoRepository EventoRepository { get; }

        public IMatriculaRepository MatriculaRepository { get; }

        public IMatriculaDemonstrativoPagamentoRepository MatriculaDemonstrativoPagamentoRepository { get; }

        public IMatriculaDemonstrativoPagamentoEventoRepository MatriculaDemonstrativoPagamentoEventoRepository { get; }

        public IMatriculaDemonstrativoPagamentoTotalizadorRepository MatriculaDemonstrativoPagamentoTotalizadorRepository { get; }

        public IMatriculaEspelhoPontoRepository MatriculaEspelhoPontoRepository { get; }

        public IPessoaRepository PessoaRepository { get; }

        public IPessoaFisicaRepository PessoaFisicaRepository { get; }

        public IPessoaJuridicaRepository PessoaJuridicaRepository { get; }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection)
        {
            this.EventoRepository = new EventoRepository(
                connection);

            this.MatriculaRepository = new MatriculaRepository(
                connection);

            this.MatriculaDemonstrativoPagamentoRepository = new MatriculaDemonstrativoPagamentoRepository(
                connection);

            this.MatriculaDemonstrativoPagamentoEventoRepository = new MatriculaDemonstrativoPagamentoEventoRepository(
                connection);

            this.MatriculaDemonstrativoPagamentoTotalizadorRepository = new MatriculaDemonstrativoPagamentoTotalizadorRepository(
                connection);

            this.MatriculaEspelhoPontoRepository = new MatriculaEspelhoPontoRepository(
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
            this.EventoRepository = new EventoRepository(
                connection,
                transaction);

            this.MatriculaRepository = new MatriculaRepository(
                connection,
                transaction);

            this.MatriculaDemonstrativoPagamentoRepository = new MatriculaDemonstrativoPagamentoRepository(
                connection,
                transaction);

            this.MatriculaDemonstrativoPagamentoEventoRepository = new MatriculaDemonstrativoPagamentoEventoRepository(
                connection,
                transaction);

            this.MatriculaDemonstrativoPagamentoTotalizadorRepository = new MatriculaDemonstrativoPagamentoTotalizadorRepository(
                connection,
                transaction);

            this.MatriculaEspelhoPontoRepository = new MatriculaEspelhoPontoRepository(
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