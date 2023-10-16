namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create(int connectionTimeout = 0, string applicationName = "");
    }
}