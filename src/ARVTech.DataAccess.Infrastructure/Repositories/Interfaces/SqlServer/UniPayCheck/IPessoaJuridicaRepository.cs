﻿namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaJuridicaRepository : ICreateRepository<PessoaJuridicaEntity>, IReadRepository<PessoaJuridicaEntity, Guid>, IUpdateRepository<PessoaJuridicaEntity, Guid, PessoaJuridicaEntity>, IDeleteRepository<Guid>
    {
        PessoaJuridicaEntity GetByCnpj(string cnpj);

        PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) ImportFileEmpregadores(string content);
    }
}