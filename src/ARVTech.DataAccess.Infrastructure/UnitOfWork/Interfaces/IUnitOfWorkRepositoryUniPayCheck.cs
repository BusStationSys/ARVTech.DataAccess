namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;

    public interface IUnitOfWorkRepositoryUniPayCheck
    {
        IEventoRepository EventoRepository { get; }

        IMatriculaRepository MatriculaRepository { get; }

        IMatriculaDemonstrativoPagamentoRepository MatriculaDemonstrativoPagamentoRepository { get; }

        IMatriculaDemonstrativoPagamentoEventoRepository MatriculaDemonstrativoPagamentoEventoRepository { get; }

        IMatriculaDemonstrativoPagamentoTotalizadorRepository MatriculaDemonstrativoPagamentoTotalizadorRepository { get; }

        IMatriculaEspelhoPontoRepository MatriculaEspelhoPontoRepository { get; }

        IMatriculaEspelhoPontoCalculoRepository MatriculaEspelhoPontoCalculoRepository { get; }

        IMatriculaEspelhoPontoMarcacaoRepository MatriculaEspelhoPontoMarcacaoRepository { get; }

        IPessoaRepository PessoaRepository { get; }

        IPessoaFisicaRepository PessoaFisicaRepository { get; }

        IPessoaJuridicaRepository PessoaJuridicaRepository { get; }

        IPublicacaoRepository PublicacaoRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }
    }
}