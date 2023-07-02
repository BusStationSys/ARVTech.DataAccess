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

    public class MatriculaDemonstrativoPagamentoEventoRepository : BaseRepository, IMatriculaDemonstrativoPagamentoEventoRepository
    {
        private readonly string _columnsMatriculasDemonstrativoPagamento;

        private readonly string _columnsMatriculasDemonstrativoPagamentoEventos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoEventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaDemonstrativoPagamentoEventoRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEventoEntity));

            this._columnsMatriculasDemonstrativoPagamento = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamento,
                base.TableAliasMatriculasDemonstrativosPagamento);

            this._columnsMatriculasDemonstrativoPagamentoEventos = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoEventos,
                base.TableAliasMatriculasDemonstrativosPagamentoEventos);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoEventoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaDemonstrativoPagamentoEventoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEventoEntity));

            this._columnsMatriculasDemonstrativoPagamento = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamento,
                base.TableAliasMatriculasDemonstrativosPagamento);

            this._columnsMatriculasDemonstrativoPagamentoEventos = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoEventos,
                base.TableAliasMatriculasDemonstrativosPagamentoEventos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoEventoEntity Create(MatriculaDemonstrativoPagamentoEventoEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMdpe UniqueIdentifier
                                            SET @NewGuidMdpe = NEWID()

                                    INSERT INTO [{0}].[dbo].[{1}]
                                                ([GUID],
                                                 [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
                                                 [IDEVENTO],
                                                 [REFERENCIA],
                                                 [VALOR])
                                         VALUES (@NewGuidMdpe,
                                                 {2}GuidMatriculaDemonstrativoPagamento,
                                                 {2}IdEvento,
                                                 {2}Referencia,
                                                 {2}Valor)

                                          SELECT @NewGuidMdpe ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoEventos,
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
                    base.TableNameMatriculasDemonstrativosPagamentoEventos,
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
        public MatriculaDemonstrativoPagamentoEventoEntity Get(Guid guid)
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
                    this._columnsMatriculasDemonstrativoPagamentoEventos,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoEventos,
                    base.TableAliasMatriculasDemonstrativosPagamentoEventos,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento,
                    base.ParameterSymbol);

                var matriculaDemonstrativoPagamentoEventoEntity = base._connection.Query<MatriculaDemonstrativoPagamentoEventoEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoEventoEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoEventoPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoEventoPagamento;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaDemonstrativoPagamentoEventoEntity.FirstOrDefault();
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
        public IEnumerable<MatriculaDemonstrativoPagamentoEventoEntity> GetAll()
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
                    this._columnsMatriculasDemonstrativoPagamentoEventos,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoEventos,
                    base.TableAliasMatriculasDemonstrativosPagamentoEventos,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento);

                var matriculaDemonstrativoPagamentoEventoEntities = base._connection.Query<MatriculaDemonstrativoPagamentoEventoEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoEventoEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoEventoPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoEventoPagamento;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaDemonstrativoPagamentoEventoEntities;
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
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoEventoEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento)
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
                                            AND {4}.[IDEVENTO] = {7}IdEvento ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasDemonstrativoPagamentoEventos,
                    this._columnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamentoEventos,
                    base.TableAliasMatriculasDemonstrativosPagamentoEventos,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento,
                    base.ParameterSymbol);

                var matriculaDemonstrativoPagamentoEventoEntity = base._connection.Query<MatriculaDemonstrativoPagamentoEventoEntity, MatriculaDemonstrativoPagamentoEntity, MatriculaDemonstrativoPagamentoEventoEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoEventoPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoEventoPagamento;
                    },
                    param: new
                    {
                        GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                        IdEvento = idEvento,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaDemonstrativoPagamentoEventoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
    }
}