namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using Dapper;

    public class PelagemRepository : BaseRepository, IPelagemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PelagemRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public PelagemRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    PelagemEntity));

            this.MapAttributeToField(
                typeof(
                    AnimalEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PelagemRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PelagemRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PelagemEntity));

            this.MapAttributeToField(
                typeof(
                    AnimalEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PelagemEntity Create(PelagemEntity entity)
        {
            try
            {
                string cmdText = @" INSERT INTO [{0}].[dbo].[PELAGENS]
                                                (Descricao, Observacoes)
                                         VALUES ({1}Descricao, {1}Observacoes)
                                         SELECT SCOPE_IDENTITY() ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

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
        /// Deletes the "Pelagem" record.
        /// </summary>
        /// <param name="id">Id of "Pelagem" record.</param>
        public void Delete(int id)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[PELAGENS]
                                     WHERE [ID] = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

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
        /// Gets the "Pelagem" record.
        /// </summary>
        /// <param name="id">Id of "Pelagem" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PelagemEntity Get(int id)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, PelagemEntity> pelagemResult = new Dictionary<int, PelagemEntity>();

                string columnsPelagens = this.GetAllColumnsFromTable("PELAGENS", "P");
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[PELAGENS] as P WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                                 ON [P].[ID] = [A].[IDPELAGEM]
                                              WHERE [P].[ID] = {3}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsPelagens,
                    columnsAnimais,
                    this._connection.Database,
                    this.ParameterSymbol);

                this._connection.Query<PelagemEntity, AnimalEntity, PelagemEntity>(
                    cmdText,
                    map: (mapPelagem, mapAnimal) =>
                    {
                        if (!pelagemResult.ContainsKey(mapPelagem.Id))
                        {
                            mapPelagem.Animais = new List<AnimalEntity>();

                            pelagemResult.Add(
                                mapPelagem.Id,
                                mapPelagem);
                        }

                        PelagemEntity current = pelagemResult[mapPelagem.Id];

                        if (mapAnimal != null && !current.Animais.Contains(mapAnimal))
                        {
                            mapAnimal.Pelagem = mapPelagem;

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

                return pelagemResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Pelagens" records.
        /// </summary>
        /// <returns>If success, the list with all "Pelagens" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PelagemEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, PelagemEntity> pelagemResult = new Dictionary<int, PelagemEntity>();

                string columnsPelagens = this.GetAllColumnsFromTable("PELAGENS", "P");
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[PELAGENS] as P WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                                 ON [P].[ID] = [A].[IDPELAGEM] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsPelagens,
                    columnsAnimais,
                    this._connection.Database);

                this._connection.Query<PelagemEntity, AnimalEntity, PelagemEntity>(
                    cmdText,
                    map: (mapPelagem, mapAnimal) =>
                    {
                        if (!pelagemResult.ContainsKey(mapPelagem.Id))
                        {
                            mapPelagem.Animais = new List<AnimalEntity>();

                            pelagemResult.Add(
                                mapPelagem.Id,
                                mapPelagem);
                        }

                        PelagemEntity current = pelagemResult[mapPelagem.Id];

                        if (mapAnimal != null && !current.Animais.Contains(mapAnimal))
                        {
                            mapAnimal.Pelagem = mapPelagem;

                            current.Animais.Add(mapAnimal);
                        }

                        return null;
                    },
                    splitOn: "ID,GUID",
                    transaction: this._transaction);

                return pelagemResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PelagemEntity Update(PelagemEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[PELAGENS]
                                       SET [DESCRICAO] = {1}Descricao,
                                           [OBSERVACOES] = {1}Observacoes
                                     WHERE ID = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

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