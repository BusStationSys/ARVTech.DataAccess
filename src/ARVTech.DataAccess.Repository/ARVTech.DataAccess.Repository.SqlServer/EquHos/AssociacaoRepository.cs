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

    public class AssociacaoRepository : BaseRepository, IAssociacaoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssociacaoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public AssociacaoRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociacaoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public AssociacaoRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AssociacaoEntity Create(AssociacaoEntity entity)
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
                    this._connection.Database,
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

                //this._connection.Execute(
                //    cmdText,
                //    param: entity,
                //    transaction: this._transaction);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[ASSOCIACOES]
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssociacaoEntity Get(int id)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, AssociacaoEntity> associacaoResult = new Dictionary<int, AssociacaoEntity>();

                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                                 ON [A].[ID] = [C].[IDASSOCIACAO]
                                              WHERE [A].[ID] = {3}Id ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsAssociacoes,
                    columnsCabanhas,
                    this._connection.Database,
                    this.ParameterSymbol);

                this._connection.Query<AssociacaoEntity, CabanhaEntity, AssociacaoEntity>(
                    cmdText,
                    map: (mapAssociacao, mapCabanha) =>
                    {
                        if (!associacaoResult.ContainsKey(mapAssociacao.Id))
                        {
                            mapAssociacao.Cabanhas = new List<CabanhaEntity>();

                            associacaoResult.Add(
                                mapAssociacao.Id,
                                mapAssociacao);
                        }

                        AssociacaoEntity current = associacaoResult[mapAssociacao.Id];

                        if (mapCabanha != null && !current.Cabanhas.Contains(mapCabanha))
                        {
                            mapCabanha.Associacao = mapAssociacao;

                            current.Cabanhas.Add(mapCabanha);
                        }

                        return null;
                    },
                    param: new
                    {
                        Id = id,
                    },
                    splitOn: "ID,GUID",
                    transaction: this._transaction);

                return associacaoResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssociacaoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<int, AssociacaoEntity> associacaoResult = new Dictionary<int, AssociacaoEntity>();

                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");

                string cmdText = @"          SELECT {0},
                                                    {1}
                                               FROM [{2}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                    LEFT OUTER JOIN [{2}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                                 ON [A].[ID] = [C].[IDASSOCIACAO] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsAssociacoes,
                    columnsCabanhas,
                    this._connection.Database);

                this._connection.Query<AssociacaoEntity, CabanhaEntity, AssociacaoEntity>(
                    cmdText,
                    map: (mapAssociacao, mapCabanha) =>
                    {
                        if (!associacaoResult.ContainsKey(mapAssociacao.Id))
                        {
                            mapAssociacao.Cabanhas = new List<CabanhaEntity>();

                            associacaoResult.Add(
                                mapAssociacao.Id,
                                mapAssociacao);
                        }

                        AssociacaoEntity current = associacaoResult[mapAssociacao.Id];

                        if (mapCabanha != null && !current.Cabanhas.Contains(mapCabanha))
                        {
                            mapCabanha.Associacao = mapAssociacao;

                            current.Cabanhas.Add(mapCabanha);
                        }

                        return null;
                    },
                    splitOn: "ID,GUID",
                    transaction: this._transaction);

                return associacaoResult.Values;
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
        public AssociacaoEntity Update(AssociacaoEntity entity)
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

//namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data;
//    using System.Data.SqlClient;
//    using System.Globalization;
//    using System.Linq;
//    using ARVTech.DataAccess.Entities.EquHos;
//    using ARVTech.DataAccess.Repository.Interfaces.EquHos;

//    public class AssociacaoRepository : Repository, IAssociacaoRepository
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="AssociacaoRepository"/> class.
//        /// </summary>
//        /// <param name="connection"></param>
//        /// <param name="transaction"></param>
//        public AssociacaoRepository(SqlConnection connection, SqlTransaction transaction)
//            : base(connection, transaction)
//        {
//            this._connection = connection;
//            this._transaction = transaction;
//        }

//        public AssociacaoEntity Create(AssociacaoEntity entity)
//        {
//            try
//            {
//                string cmdText = @"INSERT INTO [{0}].[dbo].[ASSOCIACOES]
//                                               (Descricao, Observacoes)
//                                        VALUES ({1}Descricao, {1}Observacoes)
//                                        SELECT SCOPE_IDENTITY()";

//                cmdText = string.Format(
//                    CultureInfo.InvariantCulture,
//                    cmdText,
//                    this._connection.Database,
//                    this.ParameterSymbol);

//                using (SqlCommand command = this.CreateCommand(
//                    cmdText.ToString(),
//                    parameters: this.GetDataParameters(
//                        entity).ToArray()))
//                {
//                    int id = Convert.ToInt32(
//                        command.ExecuteScalar());

//                    return this.Get(
//                        Convert.ToInt32(
//                            id));
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public void Delete(int id)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Gets the "Associação" record.
//        /// </summary>
//        /// <param name="id">ID of "Associação" record.</param>
//        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
//        public AssociacaoEntity Get(int id)
//        {
//            try
//            {
//                AssociacaoEntity associacao = null as AssociacaoEntity;

//                string cmdText = @"    SELECT TOP 1 A.ID,
//                                                A.RAZAO_SOCIAL,
//                                                A.SIGLA,
//                                                A.OBSERVACOES,
//                                                A.DESCRICAO_REGISTRO
//                                           FROM [{0}].[dbo].ASSOCIACOES AS A
//                                          WHERE ID = {1}Id";

//                cmdText = string.Format(
//                    CultureInfo.InvariantCulture,
//                    cmdText,
//                    this._connection.Database,
//                    this.ParameterSymbol);

//                using (SqlCommand command = this.CreateCommand(
//                    cmdText))
//                {
//                    command.Parameters.Add($"{this.ParameterSymbol}Id", SqlDbType.Int).Value = id;

//                    using (DataTable dt = this.GetDataTableFromDataReader(
//                        command))
//                    {
//                        if (dt != null && dt.Rows.Count > 0)
//                            associacao = this.ConvertToList<AssociacaoEntity>(dt).ToList().First();
//                    }
//                }

//                return associacao;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        /// <summary>
//        /// Get all "Associações" records.
//        /// </summary>
//        /// <returns>If success, the list with all "Associações" records. Otherwise, an exception detailing the problem.</returns>
//        public IEnumerable<AssociacaoEntity> GetAll()
//        {
//            try
//            {
//                IEnumerable<AssociacaoEntity> associacoes = null as IEnumerable<AssociacaoEntity>;

//                string cmdText = @"    SELECT A.ID,
//                                              A.RAZAO_SOCIAL,
//                                              A.SIGLA,
//                                              A.OBSERVACOES,
//                                              A.DESCRICAO_REGISTRO
//                                         FROM [{0}].[dbo].ASSOCIACOES AS A
//                                        WHERE 1=1
//                                     ORDER BY A.SIGLA,
//                                              A.RAZAO_SOCIAL,
//                                              A.ID";

//                cmdText = string.Format(
//                    CultureInfo.InvariantCulture,
//                    cmdText,
//                    this._connection.Database);

//                using (SqlCommand command = this.CreateCommand(
//                    cmdText))
//                {
//                    using (DataTable dt = this.GetDataTableFromDataReader(
//                        command))
//                    {
//                        if (dt != null && dt.Rows.Count > 0)
//                            associacoes = this.ConvertToList<AssociacaoEntity>(dt).ToList();
//                    }
//                }

//                return associacoes;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public AssociacaoEntity Update(AssociacaoEntity entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

/*
 public IEnumerable<DocumentoEntity> GetAll()

        {

            try

            {

                string columnsDocumento = this.GetAllColumnsFromTable("Documento", "D");

 

                //string columnsItemDocumento = this.GetAllColumnsFromTable("Item do Documento", "I");

 

                //string cmdText = $@"     SELECT {columnsDocumento},

                //                                {columnsItemDocumento}

                //                           FROM [dbo].[Documento] D WITH(NOLOCK)

                //                     INNER JOIN [dbo].[Item do Documento] I WITH(NOLOCK)

                //                             ON D.[ID Documento] = I.[ID Documento]

                //                          WHERE D.[ID Documento] = @IdDocumento ";

 

                string columnsAgenteEmitente = this.GetAllColumnsFromTable("Agente", "E");

 

                string columnsAgenteCliente = this.GetAllColumnsFromTable("Agente", "C");

 

                string columnsAgenteFornecedor = this.GetAllColumnsFromTable("Agente", "F");

 

                string columnsItemDocumento = this.GetAllColumnsFromTable("Item do Documento", "I");

 

                string cmdText = $@"     SELECT TOP 1000 {columnsDocumento},

                                                {columnsAgenteFornecedor},

                                                {columnsAgenteEmitente},

                                                {columnsAgenteCliente},

                                                {columnsItemDocumento}

                                           FROM [dbo].[Documento] D WITH(NOLOCK)

                                     INNER JOIN [dbo].[Agente] F WITH(NOLOCK)

                                             ON D.[Código do Fornecedor] = F.[Código do Agente]

                                     INNER JOIN [dbo].[Agente] E WITH(NOLOCK)

                                             ON D.[Código do Emitente] = E.[Código do Agente]

                                     INNER JOIN [dbo].[Agente] C WITH(NOLOCK)

                                             ON D.[Código do Cliente] = C.[Código do Agente]

                                     INNER JOIN [dbo].[Item do Documento] I WITH(NOLOCK)

                                             ON D.[ID Documento] = I.[ID Documento] ";

 

                var documentoResult = new Dictionary<int, DocumentoEntity>();

 

                this._connection.Query<DocumentoEntity, AgenteEntity, AgenteEntity, AgenteEntity, ItemDocumentoEntity, DocumentoEntity>(

                    cmdText,

                    map: (mapDocumento, mapAgenteFornecedor, mapAgenteEmitente, mapAgenteCliente, mapItemDocumento) =>

                    {

                        //mapItemDocumento.Documento = mapDocumento;

                        //mapItemDocumento.IdDocumento = mapDocumento.IdDocumento;

 

                        if (documentoResult.TryGetValue(mapDocumento.IdDocumento, out DocumentoEntity existingDocumento))

                        {

                            mapDocumento = existingDocumento;

                        }

                        else

                        {

                            mapDocumento.ItensDocumento = new List<ItemDocumentoEntity>();

 

                            mapDocumento.AgenteFornecedor = mapAgenteFornecedor;

                            mapDocumento.AgenteEmitente = mapAgenteEmitente;

                            mapDocumento.AgenteCliente = mapAgenteCliente;

 

                            documentoResult.Add(mapDocumento.IdDocumento, mapDocumento);

                        }

 

                        mapDocumento.ItensDocumento.Add(

                            mapItemDocumento);

 

                        return null;

                    },

                    splitOn: "ID Documento, Código do Agente, Código do Agente, Código do Agente, Sequência do Item do Documento",

                    transaction: this._transaction);

 

                return documentoResult.Values;

            }

            catch

            {

                throw;

            }

        }
 */