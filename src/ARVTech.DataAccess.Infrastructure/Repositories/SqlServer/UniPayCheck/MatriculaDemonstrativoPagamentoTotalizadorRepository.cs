namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using ARVTech.DataAccess.Domain.Common;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer;
    using ARVTech.Shared;
    using Dapper;
    using Microsoft.Data.SqlClient;

    public class MatriculaDemonstrativoPagamentoTotalizadorRepository : BaseRepository, IMatriculaDemonstrativoPagamentoTotalizadorRepository
    {
        private string? _columnsMatriculasDemonstrativoPagamento;

        private string? _columnsMatriculasDemonstrativoPagamentoTotalizadores;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoTotalizadorRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaDemonstrativoPagamentoTotalizadorRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoTotalizadorEntity));
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
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
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
                    this.ColumnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this.ColumnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableNameMatriculasDemonstrativosPagamento,
                    Constants.TableAliasMatriculasDemonstrativosPagamento,
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
                    this.ColumnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this.ColumnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableNameMatriculasDemonstrativosPagamento,
                    Constants.TableAliasMatriculasDemonstrativosPagamento);

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

        public Task<IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public PagedResult<MatriculaDemonstrativoPagamentoTotalizadorEntity> GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MatriculaDemonstrativoPagamentoTotalizadorEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<MatriculaDemonstrativoPagamentoTotalizadorEntity> GetAsync(Guid id)
        {
            throw new NotImplementedException();
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
                    this.ColumnsMatriculasDemonstrativoPagamentoTotalizadores,
                    this.ColumnsMatriculasDemonstrativoPagamento,
                    base._connection.Database,
                    Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores,
                    Constants.TableNameMatriculasDemonstrativosPagamento,
                    Constants.TableAliasMatriculasDemonstrativosPagamento,
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets all columns names from the "Matrículas Demonstrativo Pagamento" table with alias applied.
        /// </summary>
        private string ColumnsMatriculasDemonstrativoPagamento
        {
            get
            {
                if (this._columnsMatriculasDemonstrativoPagamento is null)
                    this._columnsMatriculasDemonstrativoPagamento = base.GetAllColumnsFromTable(
                        Constants.TableNameMatriculasDemonstrativosPagamento,
                        Constants.TableAliasMatriculasDemonstrativosPagamento);

                return this._columnsMatriculasDemonstrativoPagamento;
            }
        }

        /// <summary>
        /// Gets all column names from the "Matrículas Demonstrativo Pagamento Totalizadores" table with alias applied.
        /// </summary>
        private string ColumnsMatriculasDemonstrativoPagamentoTotalizadores
        {
            get
            {
                if (this._columnsMatriculasDemonstrativoPagamentoTotalizadores is null)
                    this._columnsMatriculasDemonstrativoPagamentoTotalizadores = base.GetAllColumnsFromTable(
                        Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                        Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores);

                return this._columnsMatriculasDemonstrativoPagamentoTotalizadores;
            }
        }
    }
}