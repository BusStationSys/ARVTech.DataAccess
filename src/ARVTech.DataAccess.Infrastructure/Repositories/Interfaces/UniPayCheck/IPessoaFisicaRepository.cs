namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

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