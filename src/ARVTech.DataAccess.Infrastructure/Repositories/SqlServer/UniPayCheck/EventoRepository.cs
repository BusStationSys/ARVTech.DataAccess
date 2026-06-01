namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Domain.Common;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer;
    using Dapper;
    using Microsoft.Data.SqlClient;

    public class EventoRepository : BaseRepository, IEventoRepository
    {
        private readonly EventoCommand _eventoCommand;

        private readonly EventoQuery _eventoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public EventoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this.MapAttributeToField(
                typeof(
                    EventoEntity));

            this._eventoCommand = new EventoCommand();

            this._eventoQuery = new EventoQuery(
                connection);
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
                    sql: this._eventoCommand.CommandTextCreate(),
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
                    sql: this._eventoCommand.CommandTextDelete(),
                    param: new
                    {
                        Id = id,
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
                    sql: this._eventoQuery.CommandTextGetById(),
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

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
                    sql: this._eventoQuery.CommandTextGetAll(),
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return eventosEntities;
            }
            catch
            {
                throw;
            }
        }

        public Task<IEnumerable<EventoEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public PagedResult<EventoEntity> GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<EventoEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<EventoEntity> GetAsync(int id)
        {
            throw new NotImplementedException();
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
                    sql: this._eventoQuery.CommandTextGetLastId(),
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
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
                    sql: this._eventoCommand.CommandTextUpdate(),
                    param: entity,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return this.Get(
                    entity.Id);
            }
            catch
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this._eventoQuery.Dispose();

            base.Dispose(disposing);
        }
    }
}