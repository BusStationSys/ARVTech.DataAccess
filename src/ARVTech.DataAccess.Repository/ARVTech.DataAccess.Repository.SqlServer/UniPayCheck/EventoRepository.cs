namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using Dapper;

    public class EventoRepository : BaseRepository, IEventoRepository
    {
        private readonly string _columnsEventos;

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

            this._columnsEventos = base.GetAllColumnsFromTable(
                "EVENTOS",
                "E");
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

            this._columnsEventos = base.GetAllColumnsFromTable(
                "EVENTOS",
                "E");
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
                string cmdText = @" INSERT INTO [{0}].[dbo].[EVENTOS]
                                                ([CODIGO],
                                                 [DESCRICAO],
                                                 [TIPO],
                                                 [OBSERVACOES])
                                         VALUES ({1}Codigo,
                                                 {1}Descricao,
                                                 {1}Tipo,
                                                 {1}Observacoes)
                                         SELECT SCOPE_IDENTITY() ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                var id = this._connection.QuerySingle<int>(
                    sql: cmdText,
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
        /// Deletes the "Evento" record.
        /// </summary>
        /// <param name="id">Id of "Evento" record.</param>
        public void Delete(int id)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[EVENTOS]
                                     WHERE [ID] = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    cmdText,
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

        /*public EventoEntity Get(int id)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, TipoEntity> tipoResult = new Dictionary<int, TipoEntity>();

                string columnsTipos = this.GetAllColumnsFromTable("TIPOS", "T");
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[TIPOS] as T WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                                 ON [T].[ID] = [A].[IDTIPO]
                                              WHERE [T].[ID] = {3}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsTipos,
                    columnsAnimais,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Query<TipoEntity, AnimalEntity, TipoEntity>(
                    cmdText,
                    map: (mapTipo, mapAnimal) =>
                    {
                        if (!tipoResult.ContainsKey(mapTipo.Id))
                        {
                            mapTipo.Animais = new List<AnimalEntity>();

                            tipoResult.Add(
                                mapTipo.Id,
                                mapTipo);
                        }

                        TipoEntity current = tipoResult[mapTipo.Id];

                        if (mapAnimal != null && !current.Animais.Contains(mapAnimal))
                        {
                            mapAnimal.Tipo = mapTipo;

                            current.Animais.Add(
                                mapAnimal);
                        }

                        return null;
                    },
                    param: new
                    {
                        Id = id,
                    },
                    splitOn: "ID,GUID",
                    transaction: this._transaction);

                return tipoResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }*/

        /// <summary>
        /// Gets the "Eventos" record.
        /// </summary>
        /// <param name="guid">Guid of "Evento" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity Get(int id)
        {
            try
            {
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[EVENTOS] AS E WITH(NOLOCK)
                                          WHERE E.ID = {2}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsEventos,
                    base._connection.Database,
                    base.ParameterSymbol);

                var eventoEntity = this._connection.Query<EventoEntity>(
                    cmdText,
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
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[EVENTOS] AS E WITH(NOLOCK) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsEventos,
                    base._connection.Database);

                var eventosEntities = this._connection.Query<EventoEntity>(
                    cmdText,
                    transaction: this._transaction);

                return eventosEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Eventos" record by "Código".
        /// </summary>
        /// <param name="codigo">"Código" of "Evento" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity GetByCodigo(int codigo)
        {
            try
            {
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[EVENTOS] AS E WITH(NOLOCK)
                                          WHERE E.[CODIGO] = {2}Codigo ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsEventos,
                    base._connection.Database,
                    base.ParameterSymbol);

                var eventoEntity = this._connection.Query<EventoEntity>(
                    cmdText,
                    param: new
                    {
                        Codigo = codigo,
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
        /// Updates the "Evento" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public EventoEntity Update(EventoEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[EVENTOS]
                                       SET [DESCRICAO] = {1}Descricao,
                                           [OBSERVACOES] = {1}Observacoes
                                     WHERE ID = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    cmdText,
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
    }
}