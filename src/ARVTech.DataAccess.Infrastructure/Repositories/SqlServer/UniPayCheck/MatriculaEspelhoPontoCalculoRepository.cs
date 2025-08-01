namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Shared;
    using Dapper;

    public class MatriculaEspelhoPontoCalculoRepository : BaseRepository, IMatriculaEspelhoPontoCalculoRepository
    {
        private readonly string _columnsMatriculasEspelhosPonto;

        private readonly string _columnsMatriculasEspelhosPontoCalculos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoCalculoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaEspelhoPontoCalculoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoCalculoEntity));

            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPonto,
                Constants.TableAliasMatriculasEspelhosPonto);

            this._columnsMatriculasEspelhosPontoCalculos = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPontoCalculos,
                Constants.TableAliasMatriculasEspelhosPontoCalculos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoCalculoEntity Create(MatriculaEspelhoPontoCalculoEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMepc UniqueIdentifier
                                            SET @NewGuidMepc = NEWID()

                                    INSERT INTO [{0}].[dbo].[{1}]
                                                ([GUID],
                                                 [GUIDMATRICULA_ESPELHO_PONTO],
                                                 [IDCALCULO],
                                                 [VALOR])
                                         VALUES (@NewGuidMepc,
                                                 {2}GuidMatriculaEspelhoPonto,
                                                 {2}IdCalculo,
                                                 {2}Valor)

                                          SELECT @NewGuidMepc ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    Constants.TableNameMatriculasEspelhosPontoCalculos,
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
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
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
        public MatriculaEspelhoPontoCalculoEntity Get(Guid guid)
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
                    this._columnsMatriculasEspelhosPontoCalculos,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    Constants.TableNameMatriculasEspelhosPontoCalculos,
                    Constants.TableAliasMatriculasEspelhosPontoCalculos,
                    Constants.TableNameMatriculasEspelhosPonto,
                    Constants.TableAliasMatriculasEspelhosPonto,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoCalculoEntity = base._connection.Query<MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoCalculoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPontoCalculo, mapMatriculaEspelhoPonto) =>
                    {
                        //mapMatriculaDemonstrativoTotalizadorPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoCalculo;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoCalculoEntity.FirstOrDefault();
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
        public IEnumerable<MatriculaEspelhoPontoCalculoEntity> GetAll()
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
                    this._columnsMatriculasEspelhosPontoCalculos,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableNameMatriculasDemonstrativosPagamento,
                    Constants.TableAliasMatriculasDemonstrativosPagamento,
                    base.ParameterSymbol);

                var matriculasEspelhosPontoCalculosEntities = base._connection.Query<MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoCalculoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPontoCalculo, mapMatriculaEspelhoPonto) =>
                    {
                        //mapMatriculaDemonstrativoTotalizadorPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoCalculo;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculasEspelhosPontoCalculosEntities;
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
        /// <param name="idCalculo"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoCalculoEntity GetByGuidMatriculaEspelhoPontoAndIdCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo)
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
                                            AND {4}.[IDCALCULO] = {7}IdCalculo ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPontoCalculos,
                    this._columnsMatriculasEspelhosPonto,
                    base._connection.Database,
                    Constants.TableNameMatriculasEspelhosPontoCalculos,
                    Constants.TableAliasMatriculasEspelhosPontoCalculos,
                    Constants.TableNameMatriculasEspelhosPonto,
                    Constants.TableAliasMatriculasEspelhosPonto,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoCalculoEntity = base._connection.Query<MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoCalculoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPontoCalculo, mapMatriculaEspelhoPonto) =>
                    {
                        //mapMatriculaDemonstrativoTotalizadorPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoCalculo;
                    },
                    param: new
                    {
                        GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                        IdCalculo = idCalculo,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoCalculoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<MatriculaEspelhoPontoCalculoEntity> GetMany(Expression<Func<MatriculaEspelhoPontoCalculoEntity, bool>> filter = null, Func<IQueryable<MatriculaEspelhoPontoCalculoEntity>, IOrderedQueryable<MatriculaEspelhoPontoCalculoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}