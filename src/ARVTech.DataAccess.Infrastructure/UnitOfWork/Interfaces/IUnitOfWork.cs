namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create();
    }
}