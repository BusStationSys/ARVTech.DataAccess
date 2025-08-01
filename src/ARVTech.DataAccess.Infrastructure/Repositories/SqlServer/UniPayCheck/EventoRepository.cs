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

    public class EventoRepository : BaseRepository, IEventoRepository
    {
        //  To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public EventoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));
        }

        /// <summary>
        /// Inserts a new "Evento" record into the database.
        /// </summary>
        /// <param name="entity">An <see cref="EventoEntity"/> object containing the data to be inserted.</param>
        /// <returns>The persisted <see cref="EventoEntity"/> object retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the SQL command.</exception>
        public EventoEntity Create(EventoEntity entity)
        {
            try
            {
                this._connection.Execute(
                    sql: "UspInserirEvento",
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

        /// <summary>
        /// Deletes an "Evento" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Evento" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(int id)
        {
            try
            {
                this._connection.Execute(
                    "UspExcluirEventoPorId",
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
        /// Retrieves an "Evento" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Evento" record.</param>
        /// <returns>The matching <see cref="EventoEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public EventoEntity Get(int id)
        {
            try
            {
                var eventoEntity = this._connection.Query<EventoEntity>(
                    "UspObterEventoPorId",
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction);

                return eventoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Evento" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{EventoEntity}"/> containing all "Evento" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<EventoEntity> GetAll()
        {
            try
            {
                var eventosEntities = this._connection.Query<EventoEntity>(
                    "UspObterEventos",
                    transaction: this._transaction);

                return eventosEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the highest "Id" currently used in the "Evento" records.
        /// </summary>
        /// <returns>The maximum <c>Id</c> value from the "Evento" table.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public int GetLastId()
        {
            try
            {
                return this._connection.QuerySingle<int>(
                    sql: "UspObterUltimoIdEvento",
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
        /// <param name="orderBy"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<EventoEntity> GetMany(Expression<Func<EventoEntity, bool>> filter = null, Func<IQueryable<EventoEntity>, IOrderedQueryable<EventoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing "Evento" record in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the "Evento" record to update.</param>
        /// <param name="entity">An <see cref="EventoEntity"/> object containing the updated values.</param>
        /// <returns>The updated <see cref="EventoEntity"/> retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the update operation.</exception>
        public EventoEntity Update(int id, EventoEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    "UspAtualizarEvento",
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