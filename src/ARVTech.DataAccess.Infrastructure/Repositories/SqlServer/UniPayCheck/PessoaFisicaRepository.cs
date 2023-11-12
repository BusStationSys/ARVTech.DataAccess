namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class PessoaFisicaRepository : BaseRepository, IPessoaFisicaRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly PessoaQuery _pessoaQuery;
        private readonly PessoaFisicaQuery _pessoaFisicaQuery;

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

            this._pessoaQuery = new PessoaQuery(
                connection,
                transaction);

            this._pessoaFisicaQuery = new PessoaFisicaQuery(
                connection,
                transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaFisicaEntity Create(PessoaFisicaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                //  Primeiramente, insere o registro na tabela "PESSOAS".
                entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                    sql: this._pessoaQuery.CommandTextCreate(),
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Insere o registro na tabela "PESSOAS_FISICAS".
                this._connection.Execute(
                    sql: this._pessoaFisicaQuery.CommandTextCreate(),
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
        /// Deletes the "Pessoa Física" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Física" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                this._connection.Execute(
                    sql: this._pessoaFisicaQuery.CommandTextDelete(),
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
        /// Gets the "Pessoa Física" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Física" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaFisicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: this._pessoaFisicaQuery.CommandTextGetById(),
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
        /// Get all "Pessoas Físicas" records.
        /// </summary>
        /// <returns>If success, the list with all "Pessoas Físicas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PessoaFisicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoasFisicas = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    sql: this._pessoaFisicaQuery.CommandTextGetAll(),
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
        /// Gets the "Pessoa Física" record by "Nome".
        /// </summary>
        /// <param name="nome">"Nome" of "Pessoa Física" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
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
                    sql: this._pessoaFisicaQuery.CommandTextGetByNome(),
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
        /// Gets the "Pessoa Física" record by "Nome", "Número Ctps", "Série Ctps" and "Uf Ctps".
        /// </summary>
        /// <param name="nome">"Nome" of "Pessoa Física" record.</param>
        /// <param name="numeroCtps"></param>
        /// <param name="serieCtps"></param>
        /// <param name="ufCtps"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
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
                    sql: this._pessoaFisicaQuery.CommandTextGetByNomeNumeroCtpsSerieCtpsAndUfCtps(),
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
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
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

                //  Primeiramente, atualiza o registro na tabela "PESSOAS".
                this._connection.Execute(
                    sql: this._pessoaQuery.CommandTextUpdate(),
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_FISICAS".
                this._connection.Execute(
                    sql: this._pessoaFisicaQuery.CommandTextUpdate(),
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
                    this._pessoaFisicaQuery.Dispose();
                    this._pessoaQuery.Dispose();
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