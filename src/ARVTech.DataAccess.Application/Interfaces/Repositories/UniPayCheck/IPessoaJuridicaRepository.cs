namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaJuridicaRepository : ICreateRepository<PessoaJuridicaEntity>, IReadRepository<PessoaJuridicaEntity, Guid>, IUpdateRepository<PessoaJuridicaEntity, Guid, PessoaJuridicaEntity>, IDeleteRepository<Guid>
    {
        PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);
    }
}