namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using Dapper;

    public class PessoaFisicaRepository : BaseRepository, IPessoaFisicaRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaFisicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaFisicaRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));
        }

        /// <summary>
        /// Creates a new "Pessoa Física" (Individual Person) in the database, including the insertion of associated "Pessoa" (Person) data.
        /// </summary>
        /// <param name="entity">The <see cref="PessoaFisicaEntity"/> object containing all necessary data to create both the "Pessoa" and the "Pessoa Física".</param>
        /// <returns>The newly created <see cref="PessoaFisicaEntity"/> retrieved from the database, including all persisted values.</returns>
        /// <exception cref="Exception">Propagates any exception that occurs during the insertion process.</exception>
        public PessoaFisicaEntity Create(PessoaFisicaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                //  Primeiramente, insere o registro na tabela "PESSOAS".
                entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                    sql: "UspInserirPessoa",
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Insere o registro na tabela "PESSOAS_FISICAS".
                this._connection.Execute(
                    sql: "UspInserirPessoaFisica",
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a "Pessoa Física" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Pessoa Física" record to delete.</param>
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
                    sql: "UspExcluirPessoaFisicaPorId",
                    new
                    {
                        Guid = guid,
                    },
                    transaction: this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Física" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Pessoa Física" record.</param>
        /// <returns>The matching <see cref="PessoaFisicaEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaFisicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(
                            guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterPessoaFisicaPorId",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Pessoas Físicas" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PessoaFisicaEntity}"/> containing all "Pessoas Físicas" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<PessoaFisicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoasFisicas = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterPessoasFisicas",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoasFisicas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Pessoa Física" records whose birthdays fall within the specified period, including related "Pessoa" data.
        /// </summary>
        /// <param name="periodoInicialString">Start of the period, in MMdd format (e.g., "0310" 10/03).</param>
        /// <param name="periodoFinalString">End of the period, in MMdd format (e.g., "0415" for 15/04).</param>
        /// <returns>An <see cref="IEnumerable{PessoaFisicaEntity}"/> containing matched records with related entities.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<PessoaFisicaEntity> GetAniversariantes(string periodoInicialString, string periodoFinalString)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoasFisicas = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterAniversariantes",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        PeriodoInicial = periodoInicialString,
                        PeriodoFinal = periodoFinalString,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoasFisicas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Física" record from the database using the specified CPF, including related "Pessoa" information.
        /// </summary>
        /// <param name="cpf">The CPF (Cadastro de Pessoas Físicas) used to identify the record.</param>
        /// <returns>The matching <see cref="PessoaFisicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="cpf"/> parameter is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaFisicaEntity GetByCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrEmpty(
                    cpf))
                    throw new ArgumentNullException(
                        nameof(
                            cpf));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterPessoaFisicaPorCpf",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Cpf = cpf,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Física" record from the database using the specified "Nome", including related "Pessoa" information.
        /// </summary>
        /// <param name="nome">The "Nome" used to identify the record.</param>
        /// <returns>The matching <see cref="PessoaFisicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="nome"/> parameter is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaFisicaEntity GetByNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterPessoaFisicaPorNome",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Nome = nome,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a "Pessoa Física" record by name, CTPS number, CTPS series, and CTPS state (UF), including related "Pessoa" information.
        /// </summary>
        /// <param name="nome">The full name of the person.</param>
        /// <param name="numeroCtps">The CTPS (work permit) number.</param>
        /// <param name="serieCtps">The CTPS series.</param>
        /// <param name="ufCtps">The state abbreviation (UF) where the CTPS was issued.</param>
        /// <returns>The matching <see cref="PessoaFisicaEntity"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the required parameters is null or empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public PessoaFisicaEntity GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));
                else if (string.IsNullOrEmpty(numeroCtps))
                    throw new ArgumentNullException(
                        nameof(
                            numeroCtps));
                else if (string.IsNullOrEmpty(serieCtps))
                    throw new ArgumentNullException(
                        nameof(
                            serieCtps));
                else if (string.IsNullOrEmpty(ufCtps))
                    throw new ArgumentNullException(
                        nameof(
                            ufCtps));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: "UspObterPessoaFisicaPorNomeNumeroCtpsSerieCtpsEUfCtps",
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Nome = nome,
                        NumeroCtps = numeroCtps,
                        SerieCtps = serieCtps,
                        UfCtps = ufCtps,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing "Pessoa Física" (Individual Person) record, along with its associated "Pessoa" (Person) data.
        /// </summary>
        /// <param name="guid">The GUID that identifies the "Pessoa Física" record to be updated.
        /// </param>
        /// <param name="entity">The entity containing the new data for the "Pessoa Física", including the linked "Pessoa".
        /// </param>
        /// <returns>
        /// The updated <see cref="PessoaFisicaEntity"/> retrieved from the database after the update.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="guid"/> is empty.
        /// </exception>
        /// <exception cref="NullReferenceException">Thrown when the <paramref name="entity.GuidPessoa"/> is empty.
        /// </exception>
        public PessoaFisicaEntity Update(Guid guid, PessoaFisicaEntity entity)
        {
            try
            {
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
                    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_FISICAS".
                this._connection.Execute(
                    sql: "UspAtualizarPessoaFisica",
                    param: entity,
                    transaction: this._transaction);

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

        public IEnumerable<PessoaFisicaEntity> GetMany(Expression<Func<PessoaFisicaEntity, bool>> filter = null, Func<IQueryable<PessoaFisicaEntity>, IOrderedQueryable<PessoaFisicaEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}