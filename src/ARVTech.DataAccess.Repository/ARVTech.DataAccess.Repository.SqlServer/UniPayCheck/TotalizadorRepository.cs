namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using Dapper;

    public class TotalizadorRepository : BaseRepository, ITotalizadorRepository
    {
        private readonly string _columnsTotalizadores;

        /// <summary>
        /// Initializes a new instance of the <see cref="TotalizadorRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public TotalizadorRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));

            this._columnsTotalizadores = base.GetAllColumnsFromTable(
                "TOTALIZADORES",
                "T");
        }

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

            this._columnsTotalizadores = base.GetAllColumnsFromTable(
                "TOTALIZADORES",
                "T");
        }

        /// <summary>
        /// Creates the "Totalizador" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public TotalizadorEntity Create(TotalizadorEntity entity)
        {
            try
            {
                string cmdText = @" INSERT INTO [{0}].[dbo].[TOTALIZADORES]
                                                ([DESCRICAO],
                                                 [OBSERVACOES])
                                         VALUES ({1}Descricao,
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
        /// Deletes the "Totalizador" record.
        /// </summary>
        /// <param name="id">Id of "Evento" record.</param>
        public void Delete(int id)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[TOTALIZADORES]
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

        /// <summary>
        /// Gets the "Totalizadores" record.
        /// </summary>
        /// <param name="guid">Guid of "Totalizador" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public TotalizadorEntity Get(int id)
        {
            try
            {
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[TOTALIZADORES] AS E WITH(NOLOCK)
                                          WHERE E.ID = {2}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsTotalizadores,
                    base._connection.Database,
                    base.ParameterSymbol);

                var totalizadorEntity = this._connection.Query<TotalizadorEntity>(
                    cmdText,
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
        /// Get all "Totalizadores" records.
        /// </summary>
        /// <returns>If success, the list with all "Totalizadores" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<TotalizadorEntity> GetAll()
        {
            try
            {
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[TOTALIZADORES] AS T WITH(NOLOCK) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsTotalizadores,
                    base._connection.Database);

                var totalizadoresEntities = this._connection.Query<TotalizadorEntity>(
                    cmdText,
                    transaction: this._transaction);

                return totalizadoresEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the "Totalizador" record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public TotalizadorEntity Update(int id, TotalizadorEntity entity)
        {
            try
            {
                entity.Id = id;

                string cmdText = @" UPDATE [{0}].[dbo].[TOTALIZADOR]
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