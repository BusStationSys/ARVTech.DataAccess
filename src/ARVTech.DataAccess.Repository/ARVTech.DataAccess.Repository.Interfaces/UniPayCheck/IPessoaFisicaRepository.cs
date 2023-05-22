namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaFisicaRepository : ICreateRepository<PessoaFisicaEntity>, IReadRepository<PessoaFisicaEntity, Guid>, IUpdateRepository<PessoaFisicaEntity>, IDeleteRepository<Guid>
    { }
}