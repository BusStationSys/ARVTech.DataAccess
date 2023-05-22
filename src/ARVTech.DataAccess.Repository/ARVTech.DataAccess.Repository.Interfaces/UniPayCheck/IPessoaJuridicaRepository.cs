namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaJuridicaRepository : ICreateRepository<PessoaJuridicaEntity>, IReadRepository<PessoaJuridicaEntity, Guid>, IUpdateRepository<PessoaJuridicaEntity>, IDeleteRepository<Guid>
    { }
}