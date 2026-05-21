namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create(int? connectionTimeout = null, string applicationName = "");
    }
}