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

    public class ContaRepository : BaseRepository, IContaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public ContaRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    ContaEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public ContaRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    ContaEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ContaEntity Create(ContaEntity entity)
        {
            try
            {
                string cmdText = @" INSERT INTO [{0}].[dbo].[ASSOCIACOES]
                                                ([RAZAO_SOCIAL],
                                                 [SIGLA],
                                                 [OBSERVACOES],
                                                 [DESCRICAO_REGISTRO])
                                         VALUES ({1}RazaoSocial,
                                                 {1}Sigla,
                                                 {1}Observacoes,
                                                 {1}DescricaoRegistro)
                                         SELECT SCOPE_IDENTITY() ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                //using (SqlCommand command = this.CreateCommand(
                //    cmdText.ToString(),
                //    parameters: this.GetDataParameters(
                //        entity).ToArray()))
                //{
                //    object id = command.ExecuteScalar();

                //    return this.Get(
                //        int.Parse(
                //            id.ToString()));
                //}

                //base._connection.Execute(
                //    cmdText,
                //    param: entity,
                //    transaction: this._transaction);

                var guid = base._connection.QuerySingle<Guid>(
                    sql: cmdText,
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    guid);
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
        public void Delete(Guid guid)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[ASSOCIACOES]
                                     WHERE [GUID] = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Execute(
                    cmdText,
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
        /// Gets the "Conta" record.
        /// </summary>
        /// <param name="guid">Guid of "Conta" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public ContaEntity Get(Guid guid)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<Guid, ContaEntity> contaResult = new Dictionary<Guid, ContaEntity>();

                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "C");
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "B");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");

                string cmdText = @"          SELECT {0},
                                                    {1},
                                                    {2}
                                               FROM [{3}].[dbo].[CONTAS] as C WITH(NOLOCK)
                                    LEFT OUTER JOIN [{3}].[dbo].[CABANHAS] as B WITH(NOLOCK)
                                                 ON C.[GUID] = [B].[GUIDCONTA]
                                         INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                                 ON B.[IDASSOCIACAO] = A.[ID]
                                              WHERE C.[GUID] = {4}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsContas,
                    columnsCabanhas,
                    columnsAssociacoes,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Query<ContaEntity, CabanhaEntity, AssociacaoEntity, ContaEntity>(
                    cmdText,
                    map: (mapConta, mapCabanha, mapAssociacao) =>
                    {
                        if (!contaResult.ContainsKey(mapConta.Guid))
                        {
                            mapConta.Cabanhas = new List<CabanhaEntity>();

                            contaResult.Add(
                                mapConta.Guid,
                                mapConta);
                        }

                        ContaEntity current = contaResult[mapConta.Guid];

                        if (mapCabanha != null && !current.Cabanhas.Contains(mapCabanha))
                        {
                            mapCabanha.Conta = mapConta;
                            mapCabanha.Associacao = mapAssociacao;

                            current.Cabanhas.Add(
                                mapCabanha);
                        }

                        return null;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return contaResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Contas" records.
        /// </summary>
        /// <returns>If success, the list with all "Contas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<ContaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<Guid, ContaEntity> contaResult = new Dictionary<Guid, ContaEntity>();

                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "C");
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "B");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");

                string cmdText = @"          SELECT {0},
                                                    {1},
                                                    {2}
                                               FROM [{3}].[dbo].[CONTAS] as C WITH(NOLOCK)
                                    LEFT OUTER JOIN [{3}].[dbo].[CABANHAS] as B WITH(NOLOCK)
                                                 ON C.[GUID] = [B].[GUIDCONTA]
                                         INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                                 ON B.[IDASSOCIACAO] = A.ID ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsContas,
                    columnsCabanhas,
                    columnsAssociacoes,
                    base._connection.Database);

                base._connection.Query<ContaEntity, CabanhaEntity, AssociacaoEntity, ContaEntity>(
                    cmdText,
                    map: (mapConta, mapCabanha, mapAssociacao) =>
                    {
                        if (!contaResult.ContainsKey(mapConta.Guid))
                        {
                            mapConta.Cabanhas = new List<CabanhaEntity>();

                            contaResult.Add(
                                mapConta.Guid,
                                mapConta);
                        }

                        ContaEntity current = contaResult[mapConta.Guid];

                        if (mapCabanha != null && !current.Cabanhas.Contains(mapCabanha))
                        {
                            mapCabanha.Conta = mapConta;
                            mapCabanha.Associacao = mapAssociacao;

                            current.Cabanhas.Add(
                                mapCabanha);
                        }

                        return null;
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return contaResult.Values;
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
        public ContaEntity Update(ContaEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[ASSOCIACOES]
                                       SET [RAZAO_SOCIAL] = {1}RazaoSocial,
                                           [SIGLA] = {1}Sigla,
                                           [OBSERVACOES] = {1}Observacoes,
                                           [DESCRICAO_REGISTRO] = {1}DescricaoRegistro
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
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }
    }
}