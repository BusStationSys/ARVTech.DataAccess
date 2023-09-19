﻿namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaJuridicaRepository : ICreateRepository<PessoaJuridicaEntity>, IReadRepository<PessoaJuridicaEntity, Guid>, IUpdateRepository<PessoaJuridicaEntity, Guid, PessoaJuridicaEntity>, IDeleteRepository<Guid>
    {
        PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);
    }
}