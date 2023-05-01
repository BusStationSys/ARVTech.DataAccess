namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using Dapper;

    public class TipoRepository : BaseRepository, ITipoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TipoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public TipoRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    PelagemEntity));

            this.MapAttributeToField(
                typeof(
                    AnimalEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public TipoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;
        }

        public TipoEntity Create(TipoEntity entity)
        {
            try
            {
                string cmdText = @" INSERT INTO [{0}].[dbo].[TIPOS]
                                                ([DESCRICAO],
                                                 [SEXO],
                                                 [OBSERVACOES],
                                                 [ORDEM],
                                                 [COR],
                                                 [ICONE],
                                                 [EXIBIR_QUADRO_ANIMAIS])
                                         VALUES ({1}Descricao,
                                                 {1}Sexo,
                                                 {1}Observacoes,
                                                 {1}Ordem,
                                                 {1}Cor,
                                                 {1}Icone,
                                                 {1}ExibirQuadroAnimais)
                                         SELECT SCOPE_IDENTITY() ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                var id = base._connection.QuerySingle<int>(
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

        public void Delete(int id)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[TIPOS]
                                     WHERE [ID] = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Execute(
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

        public TipoEntity Get(int id)
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
        }

        public IEnumerable<TipoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, TipoEntity> tiposResult = new Dictionary<int, TipoEntity>();

                string columnsTipos = this.GetAllColumnsFromTable("TIPOS", "T");
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[TIPOS] as T WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                                 ON [T].[ID] = [A].[IDTIPO] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsTipos,
                    columnsAnimais,
                    base._connection.Database);

                base._connection.Query<TipoEntity, AnimalEntity, TipoEntity>(
                    cmdText,
                    map: (mapTipo, mapAnimal) =>
                    {
                        if (!tiposResult.ContainsKey(mapTipo.Id))
                        {
                            mapTipo.Animais = new List<AnimalEntity>();

                            tiposResult.Add(
                                mapTipo.Id,
                                mapTipo);
                        }

                        TipoEntity current = tiposResult[mapTipo.Id];

                        if (mapAnimal != null && !current.Animais.Contains(mapAnimal))
                        {
                            mapAnimal.Tipo = mapTipo;

                            current.Animais.Add(
                                mapAnimal);
                        }

                        return null;
                    },
                    splitOn: "ID,GUID",
                    transaction: this._transaction);

                return tiposResult.Values;
            }
            catch
            {
                throw;
            }
        }

        public TipoEntity Update(TipoEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[TIPOS]
                                       SET [DESCRICAO] = {1}Descricao,
                                           [SEXO] = {1}Sexo,
                                           [OBSERVACOES] = {1}Observacoes,
                                           [ORDEM] = {1}Ordem,
                                           [COR] = {1}Cor,
                                           [ICONE] = {1}Icone,
                                           [EXIBIR_QUADRO_ANIMAIS] = {1}ExibirQuadroAnimais
                                     WHERE ID = {1}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Execute(
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