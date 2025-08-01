namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class TotalizadorRepository : BaseRepository, ITotalizadorRepository
    {
        //  To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TotalizadorRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public TotalizadorRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));
        }

        /// <summary>
        /// Inserts a new "Totalizador" record into the database.
        /// </summary>
        /// <param name="entity">An <see cref="TotalizadorEntity"/> object containing the data to be inserted.</param>
        /// <returns>The persisted <see cref="TotalizadorEntity"/> object retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the SQL command.</exception>
        public TotalizadorEntity Create(TotalizadorEntity entity)
        {
            try
            {
                var id = this._connection.QuerySingle<int>(
                    sql: "UspInserirTotalizador",
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
        /// Deletes a "Totalizador" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Totalizador" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(int id)
        {
            try
            {
                this._connection.Execute(
                    "UspExcluirTotalizadorPorId",
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
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteMany(Expression<Func<int, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves an "Totalizador" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Totalizador" record.</param>
        /// <returns>The matching <see cref="TotalizadorEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public TotalizadorEntity Get(int id)
        {
            try
            {
                var totalizadorEntity = this._connection.Query<TotalizadorEntity>(
                    "UspObterTotalizadorPorId",
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction);

                return totalizadorEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Totalizador" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TotalizadorEntity}"/> containing all "Totalizador" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<TotalizadorEntity> GetAll()
        {
            try
            {
                var totalizadoresEntities = this._connection.Query<TotalizadorEntity>(
                    "UspObterTotalizadores",
                    transaction: this._transaction);

                return totalizadoresEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<TotalizadorEntity> GetMany(Expression<Func<TotalizadorEntity, bool>> filter = null, Func<IQueryable<TotalizadorEntity>, IOrderedQueryable<TotalizadorEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing "Totalizador" record in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the "Totalizador" record to update.</param>
        /// <param name="entity">An <see cref="TotalizadorEntity"/> object containing the updated values.</param>
        /// <returns>The updated <see cref="TotalizadorEntity"/> retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the update operation.</exception>
        public TotalizadorEntity Update(int id, TotalizadorEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    "UspAtualizarTotalizador",
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
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}