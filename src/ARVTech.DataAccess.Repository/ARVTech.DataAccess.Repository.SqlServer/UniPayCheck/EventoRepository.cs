namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using Dapper;

    public class EventoRepository : BaseRepository, IEventoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly EventoQuery _eventoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public EventoRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));

            this._eventoQuery = new EventoQuery(
                connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public EventoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));

            this._eventoQuery = new EventoQuery(
                connection,
                transaction);
        }

        /// <summary>
        /// Creates the "Evento" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity Create(EventoEntity entity)
        {
            try
            {
                this._connection.Execute(
                    sql: this._eventoQuery.CommandTextCreate(),
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
        /// Deletes the "Evento" record.
        /// </summary>
        /// <param name="id">Id of "Evento" record.</param>
        public void Delete(int id)
        {
            try
            {
                this._connection.Execute(
                    this._eventoQuery.CommandTextDelete(),
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
        /// Gets the "Eventos" record by "Id".
        /// </summary>
        /// <param name="id">"Id" of "Evento" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity Get(int id)
        {
            try
            {
                var eventoEntity = this._connection.Query<EventoEntity>(
                    this._eventoQuery.CommandTextGetById(),
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
        /// Get all "Eventos" records.
        /// </summary>
        /// <returns>If success, the list with all "Eventos" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<EventoEntity> GetAll()
        {
            try
            {
                var eventosEntities = this._connection.Query<EventoEntity>(
                    this._eventoQuery.CommandTextGetAll(),
                    transaction: this._transaction);

                return eventosEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the Max Id available for use in "Eventos" records.
        /// </summary>
        /// <returns>If success, return the Max Id available for use in "Eventos" records. Otherwise, an exception detailing the problem.</returns>
        public int GetLastId()
        {
            try
            {
                return this._connection.QuerySingle<int>(
                    sql: this._eventoQuery.CommandTextGetLastId(),
                    transaction: this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the "Evento" record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity Update(int id, EventoEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    this._eventoQuery.CommandTextUpdate(),
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
                    this._eventoQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}