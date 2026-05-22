namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryUniPayCheck : IUnitOfWorkRepositoryUniPayCheck
    {
        private bool _disposedValue;

        public IEventoRepository EventoRepository { get; private set; }

        public IMatriculaRepository MatriculaRepository { get; private set; }

        public IMatriculaDemonstrativoPagamentoRepository MatriculaDemonstrativoPagamentoRepository { get; private set; }

        public IMatriculaDemonstrativoPagamentoEventoRepository MatriculaDemonstrativoPagamentoEventoRepository { get; private set; }

        public IMatriculaDemonstrativoPagamentoTotalizadorRepository MatriculaDemonstrativoPagamentoTotalizadorRepository { get; private set; }

        public IMatriculaEspelhoPontoRepository MatriculaEspelhoPontoRepository { get; private set; }

        public IMatriculaEspelhoPontoMarcacaoRepository MatriculaEspelhoPontoMarcacaoRepository { get; private set; }

        public IMatriculaEspelhoPontoCalculoRepository MatriculaEspelhoPontoCalculoRepository { get; private set; }

        public IPessoaRepository PessoaRepository { get; private set; }

        public IPessoaFisicaRepository PessoaFisicaRepository { get; private set; }

        public IPessoaJuridicaRepository PessoaJuridicaRepository { get; private set; }

        public IPublicacaoRepository PublicacaoRepository { get; private set; }

        public IUsuarioRepository UsuarioRepository { get; private set; }

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

            //this.PublicacaoRepository = new PublicacaoRepository(
            //    connection,
            //    transaction);

            this.UsuarioRepository = new UsuarioRepository(
                connection,
                transaction);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    (this.EventoRepository as IDisposable)?.Dispose();
                    (this.MatriculaRepository as IDisposable)?.Dispose();
                    (this.MatriculaDemonstrativoPagamentoRepository as IDisposable)?.Dispose();
                    (this.MatriculaDemonstrativoPagamentoEventoRepository as IDisposable)?.Dispose();
                    (this.MatriculaDemonstrativoPagamentoTotalizadorRepository as IDisposable)?.Dispose();
                    (this.MatriculaEspelhoPontoRepository as IDisposable)?.Dispose();
                    (this.MatriculaEspelhoPontoMarcacaoRepository as IDisposable)?.Dispose();
                    (this.MatriculaEspelhoPontoCalculoRepository as IDisposable)?.Dispose();
                    (this.PessoaRepository as IDisposable)?.Dispose();
                    (this.PessoaFisicaRepository as IDisposable)?.Dispose();
                    (this.PessoaJuridicaRepository as IDisposable)?.Dispose();
                    (this.PublicacaoRepository as IDisposable)?.Dispose();
                    (this.UsuarioRepository as IDisposable)?.Dispose();

                    this.EventoRepository = null;
                    this.MatriculaRepository = null;
                    this.MatriculaDemonstrativoPagamentoRepository = null;
                    this.MatriculaDemonstrativoPagamentoEventoRepository = null;
                    this.MatriculaDemonstrativoPagamentoTotalizadorRepository = null;
                    this.MatriculaEspelhoPontoRepository = null;
                    this.MatriculaEspelhoPontoMarcacaoRepository = null;
                    this.MatriculaEspelhoPontoCalculoRepository = null;
                    this.PessoaRepository = null;
                    this.PessoaFisicaRepository = null;
                    this.PessoaJuridicaRepository = null;
                    this.PublicacaoRepository = null;
                    this.UsuarioRepository = null;
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}