namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    using System;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin;

    public interface IUnitOfWorkRepositoryDestin : IDisposable
    {
        IConcursoRepository ConcursoRepository { get; }

        IModalidadeRepository ModalidadeRepository { get; }
    }
}