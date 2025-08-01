namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPublicacaoRepository : ICreateRepository<PublicacaoEntity>, IReadRepository<PublicacaoEntity, int>, IUpdateRepository<PublicacaoEntity, int, PublicacaoEntity>, IDeleteRepository<int>
    {
        IEnumerable<PublicacaoEntity> GetSobreNos(string dataAtualString);

        PublicacaoEntity GetImage(int id);
    }
}