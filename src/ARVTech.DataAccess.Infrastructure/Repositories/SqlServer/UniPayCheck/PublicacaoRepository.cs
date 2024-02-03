namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class PublicacaoRepository : BaseRepository, IPublicacaoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly PublicacaoQuery _publicacaoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicacaoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PublicacaoRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PublicacaoEntity));

            this._publicacaoQuery = new PublicacaoQuery(
                connection,
                transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PublicacaoEntity Create(PublicacaoEntity entity)
        {
            try
            {
                //  Insere o registro na tabela "PESSOAS_FISICAS".
                int id = this._connection.Execute(
                    sql: this._publicacaoQuery.CommandTextCreate(),
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the "Publicação" record.
        /// </summary>
        /// <param name="id">Id of "Publicação" record.</param>
        public void Delete(int id)
        {
            try
            {
                this._connection.Execute(
                    sql: this._publicacaoQuery.CommandTextDelete(),
                    new
                    {
                        Id = id,
                    },
                    transaction: this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Publicação" record.
        /// </summary>
        /// <param name="id">Id of "Publicação" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PublicacaoEntity Get(int id)
        {
            try
            {
                var publicacao = this._connection.Query<PublicacaoEntity>(
                    this._publicacaoQuery.CommandTextGetById(),
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction);

                return publicacao.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Publicações" records.
        /// </summary>
        /// <returns>If success, the list with all "Publicações" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PublicacaoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var publicacoes = this._connection.Query<PublicacaoEntity>(
                    this._publicacaoQuery.CommandTextGetAll(),
                    transaction: this._transaction);

                return publicacoes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PublicacaoEntity Update(int id, PublicacaoEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    sql: this._publicacaoQuery.CommandTextUpdate(),
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    entity.Id);
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
                    this._publicacaoQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public IEnumerable<PublicacaoEntity> GetMany(Expression<Func<PublicacaoEntity, bool>> filter = null, Func<IQueryable<PublicacaoEntity>, IOrderedQueryable<PublicacaoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<int, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}