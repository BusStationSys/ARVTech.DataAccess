namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    using System;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;

    public interface IUnitOfWorkRepositoryUniPayCheck : IDisposable
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

        IUsuarioRepository UsuarioRepository { get; }
    }
}