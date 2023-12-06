namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaFisicaRepository : ICreateRepository<PessoaFisicaEntity>, IReadRepository<PessoaFisicaEntity, Guid>, IUpdateRepository<PessoaFisicaEntity, Guid, PessoaFisicaEntity>, IDeleteRepository<Guid>
    {
        PessoaFisicaEntity GetByCpf(string cpf);

        PessoaFisicaEntity GetByNome(string nome);

        PessoaFisicaEntity GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps);
    }
}