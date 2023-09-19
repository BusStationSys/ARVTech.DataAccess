namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaFisicaRepository : ICreateRepository<PessoaFisicaEntity>, IReadRepository<PessoaFisicaEntity, Guid>, IUpdateRepository<PessoaFisicaEntity, Guid, PessoaFisicaEntity>, IDeleteRepository<Guid>
    {
        PessoaFisicaEntity GetByNome(string nome);

        PessoaFisicaEntity GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps);
    }
}