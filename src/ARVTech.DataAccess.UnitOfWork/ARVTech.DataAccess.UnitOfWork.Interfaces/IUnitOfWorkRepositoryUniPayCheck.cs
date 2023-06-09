namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;

    public interface IUnitOfWorkRepositoryUniPayCheck
    {
        IEventoRepository EventoRepository { get; }

        IMatriculaRepository MatriculaRepository { get; }

        IMatriculaDemonstrativoPagamentoRepository MatriculaDemonstrativoPagamentoRepository { get; }

        IMatriculaDemonstrativoPagamentoEventoRepository MatriculaDemonstrativoPagamentoEventoRepository { get; }

        IPessoaRepository PessoaRepository { get; }

        IPessoaFisicaRepository PessoaFisicaRepository { get; }

        IPessoaJuridicaRepository PessoaJuridicaRepository { get; }
    }
}