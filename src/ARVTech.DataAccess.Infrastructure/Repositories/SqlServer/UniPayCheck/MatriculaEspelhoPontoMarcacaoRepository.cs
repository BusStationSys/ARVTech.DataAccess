namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaEspelhoPontoMarcacaoRepository : BaseRepository, IMatriculaEspelhoPontoMarcacaoRepository
    {
        private readonly string _columnsMatriculasEspelhosPonto;

        private readonly string _columnsMatriculasEspelhosPontoMarcacoes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoMarcacaoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaEspelhoPontoMarcacaoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoMarcacaoEntity));

            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            this._columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoMarcacoes,
                base.TableAliasMatriculasEspelhosPontoMarcacoes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity Create(MatriculaEspelhoPontoMarcacaoEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMepm UniqueIdentifier
                                            SET @NewGuidMepm = NEWID()

                                    INSERT INTO [{0}].[dbo].[{1}]
                                                ([GUID],
                                                 [GUIDMATRICULA_ESPELHO_PONTO],
                                                 [DATA],
                                                 [MARCACAO],
                                                 [HORAS_EXTRAS_050],
                                                 [HORAS_EXTRAS_070],
                                                 [HORAS_EXTRAS_100],
                                                 [HORAS_CREDITO_BH],
                                                 [HORAS_DEBITO_BH],
                                                 [HORAS_FALTAS],
                                                 [HORAS_TRABALHADAS])
                                         VALUES (@NewGuidMepm,
                                                 {2}GuidMatriculaEspelhoPonto,
                                                 {2}Data,
                                                 {2}Marcacao,
                                                 {2}HorasExtras050,
                                                 {2}HorasExtras070,
                                                 {2}HorasExtras100,
                                                 {2}HorasCreditoBH,
                                                 {2}HorasDebitoBH,
                                                 {2}HorasFaltas,
                                                 {2}HorasTrabalhadas)

                                          SELECT @NewGuidMepm ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.TableNameMatriculasEspelhosPontoMarcacoes,
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
                    base.TableNameMatriculasEspelhosPontoMarcacoes,
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

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity Get(Guid guid)
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
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_ESPELHO_PONTO]
                                          WHERE UPPER({4}.[GUID]) = {7}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPontoMarcacoes,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    base.TableNameMatriculasEspelhosPontoMarcacoes,
                    base.TableAliasMatriculasEspelhosPontoMarcacoes,
                    base.TableNameMatriculasEspelhosPonto,
                    base.TableAliasMatriculasEspelhosPonto,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoMarcacaoEntity = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPontoMarcacao, mapMatriculaEspelhoPonto) =>
                    {
                        //mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoMarcacao;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntity.FirstOrDefault();
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
        public IEnumerable<MatriculaEspelhoPontoMarcacaoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_ESPELHO_PONTO] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPontoMarcacoes,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    base.TableNameMatriculasEspelhosPontoMarcacoes,
                    base.TableAliasMatriculasEspelhosPontoMarcacoes,
                    base.TableNameMatriculasEspelhosPonto,
                    base.TableAliasMatriculasEspelhosPonto);

                var matriculaEspelhoPontoMarcacaoEntities = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    cmdText,
                    map: (mapMatriculaDemonstrativoEventoPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoEventoPagamento;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaEspelhoPonto"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity GetByGuidMatriculaEspelhoPontoAndData(Guid guidMatriculaEspelhoPonto, DateTime data)
        {
            try
            {
                if (guidMatriculaEspelhoPonto == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaEspelhoPonto));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA_ESPELHO_PONTO]
                                          WHERE UPPER({4}.[GUIDMATRICULA_ESPELHO_PONTO]) = {7}GuidMatriculaEspelhoPonto
                                            AND {4}.[DATA] = {7}Data ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPontoMarcacoes,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    base.TableNameMatriculasEspelhosPontoMarcacoes,
                    base.TableAliasMatriculasEspelhosPontoMarcacoes,
                    base.TableNameMatriculasEspelhosPonto,
                    base.TableAliasMatriculasEspelhosPonto,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoMarcacaoEntity = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPontoMarcacao, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoMarcacao;
                    },
                    param: new
                    {
                        GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                        Data = data,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<MatriculaEspelhoPontoMarcacaoEntity> GetMany(Expression<Func<MatriculaEspelhoPontoMarcacaoEntity, bool>> filter = null, Func<IQueryable<MatriculaEspelhoPontoMarcacaoEntity>, IOrderedQueryable<MatriculaEspelhoPontoMarcacaoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}