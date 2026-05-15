namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using System.Threading.Tasks;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// Defines the data access contract for the Pessoa Jurídica (Legal Entity) repository,
    /// including create, read, update, delete operations and domain-specific queries.
    /// </summary>
    public interface IPessoaJuridicaRepository :
        ICreateRepository<PessoaJuridicaEntity>,
        IReadRepository<PessoaJuridicaEntity, Guid>,
        IUpdateRepository<PessoaJuridicaEntity, Guid, PessoaJuridicaEntity>,
        IDeleteRepository<Guid>
    {
        /// <summary>
        /// Retrieves a Pessoa Jurídica record by its CNPJ.
        /// </summary>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        PessoaJuridicaEntity GetByCnpj(string cnpj);

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by its CNPJ.
        /// </summary>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        Task<PessoaJuridicaEntity> GetByCnpjAsync(string cnpj);

        /// <summary>
        /// Retrieves a Pessoa Jurídica record by its Razão Social (company name).
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial);

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by its Razão Social (company name).
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        Task<PessoaJuridicaEntity> GetByRazaoSocialAsync(string razaoSocial);

        /// <summary>
        /// Retrieves a Pessoa Jurídica record by both its Razão Social and CNPJ.
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by both its Razão Social and CNPJ.
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/>, or <c>null</c> if not found.</returns>
        Task<PessoaJuridicaEntity> GetByRazaoSocialAndCnpjAsync(string razaoSocial, string cnpj);

        /// <summary>
        /// Processes and imports employer data from the provided file content.
        /// </summary>
        /// <param name="content">The textual content of the employer file to be processed.</param>
        /// <returns>An <see cref="ImportFileEmpregadoresResult"/> containing processing statistics.</returns>
        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) ImportFileEmpregadores(string content);

        /// <summary>
        /// Asynchronously processes and imports employer data from the provided file content.
        /// </summary>
        /// <param name="content">The textual content of the employer file to be processed.</param>
        /// <returns>A task representing an <see cref="ImportFileEmpregadoresResult"/> containing processing statistics.</returns>
        Task<(DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos)> ImportFileEmpregadoresAsync(string content);
    }
}