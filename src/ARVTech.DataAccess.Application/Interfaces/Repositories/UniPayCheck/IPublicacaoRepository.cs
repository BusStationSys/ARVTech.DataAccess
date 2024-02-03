namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Application.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPublicacaoRepository : ICreateRepository<PublicacaoEntity>, IReadRepository<PublicacaoEntity, int>, IUpdateRepository<PublicacaoEntity, int, EventoEntity>, IDeleteRepository<int>
    { }
}