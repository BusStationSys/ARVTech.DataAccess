namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaRepository : BaseRepository, IMatriculaRepository
    {
        private readonly string _columnsMatriculas;
        private readonly string _columnsPessoasFisicas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaEntity));

            this._columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas);
        }

        /// <summary>
        /// Creates the "Matrícula" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEntity Create(MatriculaEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidMatricula UniqueIdentifier
                                            SET @NewGuidMatricula = NEWID()

                                    INSERT INTO [{0}].[dbo].[MATRICULAS]
                                                ([GUID],
                                                 [MATRICULA],
                                                 [DATA_ADMISSAO],
                                                 [DATA_DEMISSAO],
                                                 [DESCRICAO_CARGO],
                                                 [DESCRICAO_SETOR],
                                                 [GUIDCOLABORADOR],
                                                 [GUIDEMPREGADOR],
                                                 [BANCO],
                                                 [AGENCIA],
                                                 [CONTA],
                                                 [CARGA_HORARIA])
                                         VALUES (@NewGuidMatricula,
                                                 {1}Matricula,
                                                 {1}DataAdmissao,
                                                 {1}DataDemissao,
                                                 {1}DescricaoCargo,
                                                 {1}DescricaoSetor,
                                                 {1}GuidColaborador,
                                                 {1}GuidEmpregador,
                                                 {1}Banco,
                                                 {1}Agencia,
                                                 {1}Conta,
                                                 {1}CargaHoraria)

                                          SELECT @NewGuidMatricula ";

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
        /// Deletes the "Matrícula" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[MATRICULAS]
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
        /// Deletes the "Matrícula Espelho Ponto" record.
        /// </summary>
        /// <param name="guidMatricula">Guid of "Matrícula" record.</param>
        /// <param name="guid">Guid of "Espelho Ponto" record.</param>
        public void DeleteEspelhosPonto(Guid guidMatricula)
        {
            try
            {
                if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                     WHERE [GUIDMATRICULA] = {1}GuidMatricula
                                       AND [GUID] = {1}Guid";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

                base._connection.Execute(
                    cmdText,
                    new
                    {
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
        /// Gets the "Matrícula" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public MatriculaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[{4}] as {5} WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[{6}] as {7} WITH(NOLOCK)
                                             ON [{5}].[GUIDCOLABORADOR] = [{7}].[GUID]
                                     INNER JOIN [{3}].[dbo].[{8}] as {9} WITH(NOLOCK)
                                             ON [{5}].[GUIDEMPREGADOR] = [{9}].[GUID]
                                          WHERE UPPER({5}.GUID) = {10}Guid ";

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
                    base.TableAliasPessoasJuridicas,
                    base.ParameterSymbol);

                var matricula = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    cmdText,
                    map: (mapMatricula, mapPessoaFisica, mapPessoaJuridica) =>
                    {
                        mapMatricula.Colaborador = mapPessoaFisica;
                        mapMatricula.Empregador = mapPessoaJuridica;

                        return mapMatricula;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return matricula.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas" records.
        /// </summary>
        /// <returns>If success, the list with all "Matrículas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaEntity> GetAll()
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

                var matriculasEntities = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    cmdText,
                    map: (mapMatricula, mapPessoaFisica, mapPessoaJuridica) =>
                    {
                        mapMatricula.Colaborador = mapPessoaFisica;
                        mapMatricula.Empregador = mapPessoaJuridica;

                        return mapMatricula;
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return matriculasEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public MatriculaEntity GetByMatricula(string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(matricula));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[{4}] as {5} WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[{6}] as {7} WITH(NOLOCK)
                                             ON [{5}].[GUIDCOLABORADOR] = [{7}].[GUID]
                                     INNER JOIN [{3}].[dbo].[{8}] as {9} WITH(NOLOCK)
                                             ON [{5}].[GUIDEMPREGADOR] = [{9}].[GUID]
                                          WHERE {5}.MATRICULA = {10}Matricula ";

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
                    base.TableAliasPessoasJuridicas,
                    base.ParameterSymbol);

                var matriculaEntity = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    cmdText,
                    map: (mapMatricula, mapPessoaFisica, mapPessoaJuridica) =>
                    {
                        mapMatricula.Colaborador = mapPessoaFisica;
                        mapMatricula.Empregador = mapPessoaJuridica;

                        return mapMatricula;
                    },
                    param: new
                    {
                        Matricula = matricula,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return matriculaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the "Matrícula" record.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEntity Update(Guid guid, MatriculaEntity entity)
        {
            try
            {
                entity.Guid = guid;

                string cmdText = @" UPDATE [{0}].[dbo].[MATRICULAS]
                                       SET [MATRICULA] = {1}Matricula,
                                           [DATA_ADMISSAO] = {1}DataAdmissao,
                                           [DATA_DEMISSAO] = {1}DataDemissao,
                                           [GUIDCOLABORADOR] = {1}GuidColaborador,
                                           [GUIDEMPREGADOR] = {1}GuidEmpregador,
                                           [BANCO] = {1}Banco,
                                           [AGENCIA] = {1}Agencia,
                                           [CONTA] = {1}Conta,
                                           [SALARIO_NOMINAL] = {1}SalarioNominal
                                     WHERE GUID = {1}Guid ";

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