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

    public class MatriculaDemonstrativoPagamentoTotalizadorRepository : BaseRepository, IMatriculaDemonstrativoPagamentoTotalizadorRepository
    {
        private readonly string _columnsMatriculasDemonstrativoPagamento;

        private readonly string _columnsMatriculasDemonstrativoPagamentoTotalizadores;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoTotalizadorRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaDemonstrativoPagamentoTotalizadorRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoTotalizadorEntity));

            this._columnsMatriculasDemonstrativoPagamento = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamento,
                base.TableAliasMatriculasDemonstrativosPagamento);

            this._columnsMatriculasDemonstrativoPagamentoTotalizadores = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoTotalizadorRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaDemonstrativoPagamentoTotalizadorRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoTotalizadorEntity));

            this._columnsMatriculasDemonstrativoPagamento = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamento,
                base.TableAliasMatriculasDemonstrativosPagamento);

            this._columnsMatriculasDemonstrativoPagamentoTotalizadores = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoTotalizadorEntity Create(MatriculaDemonstrativoPagamentoTotalizadorEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMdpt UniqueIdentifier
                                            SET @NewGuidMdpt = NEWID()

                                    INSERT INTO [{0}].[dbo].[{1}]
                                                ([GUID],
                                                 [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
                                                 [IDTOTALIZADOR],
                                                 [VALOR])
                                         VALUES (@NewGuidMdpt,
                                                 {2}GuidMatriculaDemonstrativoPagamento,
                                                 {2}IdTotalizador,
                                                 {2}Valor)

                                          SELECT @NewGuidMdpt ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    base.ParameterSymbol);

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
        /// <param name="guid"></param>
        public void Delete(Guid guid)
        {
            try
            {
                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[{1}]
                                     WHERE [GUID] = {2}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    base.ParameterSymbol);

                this._connection.Execute(
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
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoTotalizadorEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]
                                          WHERE UPPER({4}.[GUID]) = {7}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento,
                    base.ParameterSymbol);

                var matriculaDemonstrativoPagamentoTotalizadorEntity = base._connection.Query<MatriculaDemonstrativoPagamentoTotalizadorEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoTotalizadorEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoTotalizadorPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoTotalizadorPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoTotalizadorPagamento;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaDemonstrativoPagamentoTotalizadorEntity.FirstOrDefault();
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
        public IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento);

                var matriculasDemonstrativoPagamentoTotalizadoresEntities = base._connection.Query<MatriculaDemonstrativoPagamentoTotalizadorEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoTotalizadorEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoPagamentoTotalizador, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoPagamentoTotalizador.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoPagamentoTotalizador;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculasDemonstrativoPagamentoTotalizadoresEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idTotalizador"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoTotalizadorEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador)
        {
            try
            {
                if (guidMatriculaDemonstrativoPagamento == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaDemonstrativoPagamento));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]
                                          WHERE UPPER({4}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]) = {7}GuidMatriculaDemonstrativoPagamento
                                            AND {4}.[IDTOTALIZADOR] = {7}IdTotalizador ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento,
                    base.ParameterSymbol);

                var matriculaDemonstrativoPagamentoTotalizadorEntity = base._connection.Query<MatriculaDemonstrativoPagamentoTotalizadorEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoTotalizadorEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoTotalizadorPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoTotalizadorPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoTotalizadorPagamento;
                    },
                    param: new
                    {
                        GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                        IdTotalizador = idTotalizador,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaDemonstrativoPagamentoTotalizadorEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
    }
}