namespace ARVTech.DataAccess.Application.Interfaces.Actions
{
    public interface IDeleteRepository<Y>
    {
        void Delete(Y id);
    }
}