namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using ARVTech.DataAccess.Domain.Common;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using Dapper;

    public class PessoaJuridicaRepository : BaseRepository, IPessoaJuridicaRepository
    {
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaJuridicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaJuridicaRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PessoaJuridicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this.MapAttributeToField(
                typeof(
                    UnidadeNegocioEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaJuridicaEntity Create(PessoaJuridicaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                //  Primeiramente, insere o registro na tabela "PESSOAS".
                entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                    sql: "UspInserirPessoa",
                    param: entity.Pessoa,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                //  Insere o registro na tabela "PESSOAS_JURIDICAS".
                this._connection.Execute(
                    sql: "UspInserirPessoaJuridica",
                    param: entity,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return this.Get(
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a "Pessoa Jurídica" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Pessoa Jurídica" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(
                            guid));

                this._connection.Execute(
                    sql: "UspExcluirPessoaJuridicaPorId",
                    new
                    {
                        Guid = guid,
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Jurídica" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Pessoa Jurídica" record.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaJuridicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(
                            guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorId",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the record.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public async Task<PessoaJuridicaEntity> GetAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentNullException(nameof(id));

                var pessoaJuridicaEntity = await this._connection.QueryAsync<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorId",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Guid = id,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Pessoas Jurídicas" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PessoaJuridicaEntity}"/> containing all "Pessoas Jurídicas" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<PessoaJuridicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                return this._connection.Query<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoasJuridicas",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves all Pessoa Jurídica records from the database.
        /// </summary>
        /// <returns>A task representing an <see cref="IEnumerable{PessoaJuridicaEntity}"/> containing all records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public async Task<IEnumerable<PessoaJuridicaEntity>> GetAllAsync()
        {
            try
            {
                return await this._connection.QueryAsync<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoasJuridicas",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Jurídica" record from the database using the specified CNPJ, including related "Pessoa" information.
        /// </summary>
        /// <param name="cnpj">The CNPJ (Cadastro Nacional de Pessoas Jurídicas) used to identify the record.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="cnpj"/> parameter is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaJuridicaEntity GetByCnpj(string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(
                        nameof(
                            cnpj));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorCnpj",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Cnpj = cnpj,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by its CNPJ.
        /// </summary>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cnpj"/> is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public async Task<PessoaJuridicaEntity> GetByCnpjAsync(string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(nameof(cnpj));

                var pessoaJuridicaEntity = await this._connection.QueryAsync<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorCnpj",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Cnpj = cnpj,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Jurídica" record from the database using the specified "Razão Social", including related "Pessoa" information.
        /// </summary>
        /// <param name="razaoSocial">The "Razão Social" used to identify the record.</param>
        /// <returns>The matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="razaoSocial"/> parameter is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(
                        nameof(
                            razaoSocial));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorRazaoSocial",
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by its Razão Social.
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="razaoSocial"/> is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public async Task<PessoaJuridicaEntity> GetByRazaoSocialAsync(string razaoSocial)
        {
            try
            {
                if (string.IsNullOrEmpty(
                    razaoSocial))
                    throw new ArgumentNullException(
                        nameof(razaoSocial));

                var pessoasJuridicasEntities = await this._connection.QueryAsync<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorRazaoSocial",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoasJuridicasEntities.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record by "Razão Social" And "Cnpj".
        /// </summary>
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(
                        nameof(
                            razaoSocial));
                else if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(
                        nameof(
                            cnpj));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorRazaoSocialECnpj",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                        Cnpj = cnpj,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Pessoa Jurídica record by both its Razão Social and CNPJ.
        /// </summary>
        /// <param name="razaoSocial">The Razão Social of the legal entity.</param>
        /// <param name="cnpj">The CNPJ of the legal entity.</param>
        /// <returns>A task representing the matching <see cref="PessoaJuridicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="razaoSocial"/> or <paramref name="cnpj"/> is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public async Task<PessoaJuridicaEntity> GetByRazaoSocialAndCnpjAsync(string razaoSocial, string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(nameof(razaoSocial));
                else if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(nameof(cnpj));

                var pessoasJuridicasEntities = await this._connection.QueryAsync<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: "UspObterPessoaJuridicaPorRazaoSocialECnpj",
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    param: new { RazaoSocial = razaoSocial, Cnpj = cnpj },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return pessoasJuridicasEntities.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Imports employer data from the provided file content by executing the stored procedure 'UspImportarArquivoEmpregadores'.
        /// </summary>
        /// <param name="content">The textual content of the employer file to be processed.</param>
        /// <returns>
        /// A tuple containing:
        /// - dataInicio: The start date and time of the processing;
        /// - dataFim: The end date and time of the processing;
        /// - quantidadeRegistrosAtualizados: The number of records that were updated;
        /// - quantidadeRegistrosInalterados: The number of records that were already up to date;
        /// - quantidadeRegistrosInseridos: The number of records that were newly inserted.
        /// </returns>
        public (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) ImportFileEmpregadores(string content)
        {
            try
            {
                string sql = "UspImportarArquivoEmpregadores";

                return this._connection.QueryFirstOrDefault<(DateTime, DateTime, int, int, int)>(
                    sql,
                    param: new
                    {
                        Conteudo = content,
                    },
                    commandType: CommandType.StoredProcedure);

                //var result = this._connection.Execute(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);
                //this._connection.Execute(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //var resultado = this._connection.QueryFirstOrDefault<(DateTime, DateTime, int, int, int)>(
                //    sql,
                //    parameters,
                //    commandType: CommandType.StoredProcedure);

                //return resultado;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaJuridicaEntity Update(Guid guid, PessoaJuridicaEntity entity)
        {
            try
            {
                //if (guid == Guid.Empty)
                //    throw new ArgumentNullException(
                //        nameof(
                //            guid));
                //else if (entity.GuidPessoa == Guid.Empty)
                //    throw new NullReferenceException(
                //        nameof(
                //            entity.GuidPessoa));

                //entity.Guid = guid;

                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(
                            guid));
                else if (entity.GuidPessoa == Guid.Empty)
                    throw new NullReferenceException(
                        nameof(
                            entity.GuidPessoa));

                entity.Guid = guid;
                entity.Pessoa.Guid = entity.GuidPessoa;

                //  Primeiramente, atualiza o registro na tabela "PESSOAS".
                this._connection.Execute(
                    sql: "UspAtualizarPessoa",
                    param: entity.Pessoa,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                //  Por último, atualiza o registro na tabela "PESSOAS_JURIDICAS".
                this._connection.Execute(
                    sql: "UspAtualizarPessoaJuridica",
                    param: entity,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return this.Get(
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public PagedResult<PessoaJuridicaEntity> GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<PessoaJuridicaEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<(DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos)> ImportFileEmpregadoresAsync(string content)
        {
            throw new NotImplementedException();
        }
    }
}