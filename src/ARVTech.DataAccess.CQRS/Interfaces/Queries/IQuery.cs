namespace ARVTech.DataAccess.CQRS.Interfaces.Queries
{
    public interface IQuery
    {
        string CommandTextGetAll();

        string CommandTextGetById();

        string CommandTextGetCustom(string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null);
    }
}