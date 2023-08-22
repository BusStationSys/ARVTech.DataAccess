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

    public class MatriculaEspelhoPontoRepository : BaseRepository, IMatriculaEspelhoPontoRepository
    {
        private readonly string _columnsMatriculas;
        private readonly string _columnsMatriculasEspelhosPonto;
        private readonly string _columnsPessoasFisicas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaEspelhoPontoRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    MatriculaEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this._columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaEspelhoPontoRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoEntity));

            this._columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas);
        }

        /// <summary>
        /// Creates the "Matrícula Espelho Ponto" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoEntity Create(MatriculaEspelhoPontoEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMatriculaEspelhoPonto UniqueIdentifier
                                            SET @NewGuidMatriculaEspelhoPonto = NEWID()

                                    INSERT INTO [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                                ([GUID],
                                                 [GUIDMATRICULA],
                                                 [COMPETENCIA])
                                         VALUES ( @NewGuidMatriculaEspelhoPonto,
                                                  {1}GuidMatricula,
                                                  {1}Competencia )

                                          SELECT @NewGuidMatriculaEspelhoPonto ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
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
        /// Deletes the "Matrícula Espelho Ponto" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula Espelho Ponto" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
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
        /// Deletes the "Cálculos" of "Matrícula Espelho Ponto" records by "Competência" And "Guid of Matrícula".
        /// </summary>
        /// <param name="competencia">"Competência" of "Matrícula" record.</param>
        /// <param name="guidMatricula">Guid of "Matrícula" record.</param>
        public void DeleteCalculosByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]
                                     WHERE [GUIDMATRICULA_ESPELHO_PONTO] IN ( SELECT [GUID]
                                                                                FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                                                               WHERE [COMPETENCIA] = {1}Competencia
                                                                                 AND [GUIDMATRICULA] = {1}GuidMatricula )

                                    DELETE
                                      FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO_CALCULOS]
                                     WHERE [GUIDMATRICULA_ESPELHO_PONTO] IN ( SELECT [GUID]
                                                                                FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                                                               WHERE [COMPETENCIA] = {1}Competencia
                                                                                 AND [GUIDMATRICULA] = {1}GuidMatricula ) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Execute(
                    cmdText,
                    new
                    {
                        Competencia = competencia,
                        GuidMatricula = guidMatricula,
                    },
                    transaction: this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Matrícula Espelho Ponto" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula Espelho Ponto" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public MatriculaEspelhoPontoEntity Get(Guid guid)
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
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA]
                                          WHERE UPPER({4}.[GUID]) = {7}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPonto,
                    this._columnsMatriculas,
                    base._connection.Database,
                    base.TableNameMatriculasDemonstrativosPagamento,
                    base.TableAliasMatriculasDemonstrativosPagamento,
                    base.TableNameMatriculas,
                    base.TableAliasMatriculas,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoEntity = base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, MatriculaEspelhoPontoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPonto, mapMatricula) =>
                    {
                        //mapMatricula.Colaborador = mapPessoaFisica;
                        //mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                        return mapMatriculaEspelhoPonto;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Espelhos Ponto" records.
        /// </summary>
        /// <returns>If success, the list with all "Matrículas Espelhos Pagamento" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaEspelhoPontoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[{4}] as {5} WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[{6}] as {7} WITH(NOLOCK)
                                             ON [{5}].[GUIDCOLABORADOR] = [{7}].[GUID]
                                     INNER JOIN [{3}].[dbo].[{8}] as {9} WITH(NOLOCK)
                                             ON [{5}].[GUIDEMPREGADOR] = [{9}].[GUID] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculas,
                    this._columnsPessoasFisicas,
                    this._columnsPessoasJuridicas,
                    base._connection.Database,
                    base.TableNameMatriculas,
                    base.TableAliasMatriculas,
                    base.TableNamePessoasFisicas,
                    base.TableAliasPessoasFisicas,
                    base.TableNamePessoasJuridicas,
                    base.TableAliasPessoasJuridicas);

                var matriculasEspelhosPontoEntities = base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEspelhoPontoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPonto, mapMatricula, mapPessoaFisica, mapPessoaJuridica) =>
                    {
                        mapMatricula.Colaborador = mapPessoaFisica;
                        mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                        return mapMatriculaEspelhoPonto;
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return matriculasEspelhosPontoEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Espelhos Ponto" records by "Competência" And "Matrícula".
        /// </summary>
        /// <param name="competencia"></param>
        /// <param name="matricula"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public MatriculaEspelhoPontoEntity GetByCompetenciaAndMatricula(string competencia, string matricula)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[{3}] as {4} WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[{5}] as {6} WITH(NOLOCK)
                                             ON {6}.[GUID] = {4}.[GUIDMATRICULA]
                                          WHERE {4}.[COMPETENCIA] = {7}Competencia 
                                            AND {6}.[MATRICULA] = {7}Matricula ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsMatriculasEspelhosPonto,
                    this._columnsMatriculas,
                    base._connection.Database,
                    base.TableNameMatriculasEspelhosPonto,
                    base.TableAliasMatriculasEspelhosPonto,
                    base.TableNameMatriculas,
                    base.TableAliasMatriculas,
                    base.ParameterSymbol);

                var matriculaEspelhoPontoEntity = base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, MatriculaEspelhoPontoEntity>(
                    cmdText,
                    map: (mapMatriculaEspelhoPonto, mapMatricula) =>
                    {
                        //mapMatricula.Colaborador = mapPessoaFisica;
                        //mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                        return mapMatriculaEspelhoPonto;
                    },
                    param: new
                    {
                        Competencia = competencia,
                        Matricula = matricula,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the "Matrícula Demonstrativo Pagamento" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoEntity Update(MatriculaEspelhoPontoEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                       SET [GUIDMATRICULA] = {1}GuidMatricula,
                                           [COMPETENCIA] = {1}Competencia
                                     WHERE [GUID] = {1}Guid ";

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