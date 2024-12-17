namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class PessoaJuridicaRepository : BaseRepository, IPessoaJuridicaRepository
    {
        private bool _disposedValue = false;

        private readonly PessoaCommand _pessoaCommand;
        private readonly PessoaQuery _pessoaQuery;

        private readonly PessoaJuridicaCommand _pessoaJuridicaCommand;
        private readonly PessoaJuridicaQuery _pessoaJuridicaQuery;

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

            this._pessoaCommand = new PessoaCommand();

            this._pessoaQuery = new PessoaQuery(
                connection,
                transaction);

            this._pessoaJuridicaCommand = new PessoaJuridicaCommand();

            this._pessoaJuridicaQuery = new PessoaJuridicaQuery(
                connection,
                transaction);
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
                    sql: this._pessoaCommand.CommandTextCreate(),
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Insere o registro na tabela "PESSOAS_JURIDICAS".
                this._connection.Execute(
                    sql: this._pessoaJuridicaCommand.CommandTextCreate(),
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
        /// Deletes the "Pessoa Jurídica" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Jurídica" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                this._connection.Execute(
                    sql: this._pessoaJuridicaCommand.CommandTextDelete(),
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
        /// Gets the "Pessoa Jurídica" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Jurídica" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaJuridicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    sql: this._pessoaJuridicaQuery.CommandTextGetById(),
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Pessoas Jurídicas" records.
        /// </summary>
        /// <returns>If success, the list with all "Pessoas Jurídicas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PessoaJuridicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var pessoasJuridicasEntities = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, UnidadeNegocioEntity, PessoaJuridicaEntity>(
                    sql: this._pessoaJuridicaQuery.CommandTextGetAll(),
                    map: (mapPessoaJuridica, mapPessoa, mapUnidadeNegocio) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;
                        mapPessoaJuridica.UnidadeNegocio = mapUnidadeNegocio;

                        return mapPessoaJuridica;
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return pessoasJuridicasEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record by "Cnpj".
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
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
                    sql: this._pessoaJuridicaQuery.CommandTextGetByCnpj(),
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
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record by "Razão Social".
        /// </summary>
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
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
                    sql: this._pessoaJuridicaQuery.CommandTextGetByRazaoSocial(),
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
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
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
                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    sql: this._pessoaJuridicaQuery.CommandTextGetByRazaoSocialAndCnpj(),
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                        Cnpj = cnpj,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
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
                    sql: this._pessoaCommand.CommandTextUpdate(),
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, atualiza o registro na tabela "PESSOAS_JURIDICAS".
                this._connection.Execute(
                    sql: this._pessoaJuridicaCommand.CommandTextUpdate(),
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
                    this._pessoaJuridicaQuery.Dispose();
                    this._pessoaQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public IEnumerable<PessoaJuridicaEntity> GetMany(Expression<Func<PessoaJuridicaEntity, bool>> filter = null, Func<IQueryable<PessoaJuridicaEntity>, IOrderedQueryable<PessoaJuridicaEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}