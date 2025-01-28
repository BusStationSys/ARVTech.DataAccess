namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaFisicaRepository : ICreateRepository<PessoaFisicaEntity>, IReadRepository<PessoaFisicaEntity, Guid>, IUpdateRepository<PessoaFisicaEntity, Guid, PessoaFisicaEntity>, IDeleteRepository<Guid>
    {
        IEnumerable<PessoaFisicaEntity> GetAniversariantes(string periodoInicialString, string periodoFinalString);

        PessoaFisicaEntity GetByCpf(string cpf);

        PessoaFisicaEntity GetByNome(string nome);

        PessoaFisicaEntity GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps);
    }
}