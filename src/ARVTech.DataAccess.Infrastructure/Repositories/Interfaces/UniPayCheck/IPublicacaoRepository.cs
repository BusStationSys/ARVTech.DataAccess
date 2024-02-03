﻿namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPublicacaoRepository : ICreateRepository<PublicacaoEntity>, IReadRepository<PublicacaoEntity, int>, IUpdateRepository<PublicacaoEntity, int, PublicacaoEntity>, IDeleteRepository<int>
    { }
}