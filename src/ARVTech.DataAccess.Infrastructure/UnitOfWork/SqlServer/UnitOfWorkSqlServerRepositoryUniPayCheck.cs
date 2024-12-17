namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Repository.SqlServer.UniPayCheck;

    public class UnitOfWorkSqlServerRepositoryUniPayCheck : IUnitOfWorkRepositoryUniPayCheck
    {
        public IEventoRepository EventoRepository { get; }

        public IMatriculaRepository MatriculaRepository { get; }

        public IMatriculaDemonstrativoPagamentoRepository MatriculaDemonstrativoPagamentoRepository { get; }

        public IMatriculaDemonstrativoPagamentoEventoRepository MatriculaDemonstrativoPagamentoEventoRepository { get; }

        public IMatriculaDemonstrativoPagamentoTotalizadorRepository MatriculaDemonstrativoPagamentoTotalizadorRepository { get; }

        public IMatriculaEspelhoPontoRepository MatriculaEspelhoPontoRepository { get; }

        public IMatriculaEspelhoPontoMarcacaoRepository MatriculaEspelhoPontoMarcacaoRepository { get; }

        public IMatriculaEspelhoPontoCalculoRepository MatriculaEspelhoPontoCalculoRepository { get; }

        public IPessoaRepository PessoaRepository { get; }

        public IPessoaFisicaRepository PessoaFisicaRepository { get; }

        public IPessoaJuridicaRepository PessoaJuridicaRepository { get; }

        public IPublicacaoRepository PublicacaoRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }

        public UnitOfWorkSqlServerRepositoryUniPayCheck(SqlConnection connection, SqlTransaction? transaction = null)
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

            this.MatriculaEspelhoPontoMarcacaoRepository = new MatriculaEspelhoPontoMarcacaoRepository(
                connection,
                transaction);

            this.MatriculaEspelhoPontoCalculoRepository = new MatriculaEspelhoPontoCalculoRepository(
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

            this.PublicacaoRepository = new PublicacaoRepository(
                connection,
                transaction);

            this.UsuarioRepository = new UsuarioRepository(
                connection,
                transaction);
        }
    }
}