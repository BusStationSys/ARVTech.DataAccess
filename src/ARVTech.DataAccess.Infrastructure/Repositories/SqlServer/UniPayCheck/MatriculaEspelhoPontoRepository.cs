namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaEspelhoPontoRepository : BaseRepository, IMatriculaEspelhoPontoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly MatriculaEspelhoPontoCommand _matriculaEspelhoPontoCommand;

        private readonly MatriculaEspelhoPontoQuery _matriculaEspelhoPontoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaEspelhoPontoRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    CalculoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoCalculoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoMarcacaoEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaJuridicaEntity));

            this._matriculaEspelhoPontoCommand = new MatriculaEspelhoPontoCommand();

            this._matriculaEspelhoPontoQuery = new MatriculaEspelhoPontoQuery(
                connection,
                transaction);
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
                var guid = base._connection.QuerySingle<Guid>(
                    sql: this._matriculaEspelhoPontoCommand.CommandTextCreate(),
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

                base._connection.Execute(
                    this._matriculaEspelhoPontoCommand.CommandTextDelete(),
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
        /// Deletes the "Cálculos" And "Marcações" of "Matrícula Espelho Ponto" records by "Competência" And "Guid of Matrícula".
        /// </summary>
        /// <param name="competencia">"Competência" of "Matrícula" record.</param>
        /// <param name="guidMatricula">Guid of "Matrícula" record.</param>
        public void DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                base._connection.Execute(
                    this._matriculaEspelhoPontoQuery.CommandTextDeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(),
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
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasEspelhoPontoResult = new Dictionary<Guid, MatriculaEspelhoPontoEntity>();

                base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoMarcacaoEntity, CalculoEntity, MatriculaEspelhoPontoEntity>(
                    this._matriculaEspelhoPontoQuery.CommandTextGetById(),
                    map: (mapMatriculaEspelhoPonto, mapMatricula, mapPessoaFisica, mapPessoaJuridica, mapMatriculaEspelhoPontoCalculos, mapMatriculaEspelhoPontoMarcacoes, mapCalculo) =>
                    {
                        if (!matriculasEspelhoPontoResult.ContainsKey(mapMatriculaEspelhoPonto.Guid))
                        {
                            mapMatricula.Colaborador = mapPessoaFisica;
                            mapMatricula.Empregador = mapPessoaJuridica;

                            mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                            mapMatriculaEspelhoPonto.MatriculaEspelhoPontoMarcacoes = new List<MatriculaEspelhoPontoMarcacaoEntity>();

                            mapMatriculaEspelhoPonto.MatriculaEspelhoPontoCalculos = new List<MatriculaEspelhoPontoCalculoEntity>();

                            matriculasEspelhoPontoResult.Add(
                                mapMatriculaEspelhoPonto.Guid,
                                mapMatriculaEspelhoPonto);
                        }

                        MatriculaEspelhoPontoEntity current = matriculasEspelhoPontoResult[mapMatriculaEspelhoPonto.Guid];

                        if (mapMatriculaEspelhoPontoMarcacoes != null && !current.MatriculaEspelhoPontoMarcacoes.Contains(mapMatriculaEspelhoPontoMarcacoes))
                        {
                            current.MatriculaEspelhoPontoMarcacoes.Add(
                                mapMatriculaEspelhoPontoMarcacoes);
                        }

                        if (mapMatriculaEspelhoPontoCalculos != null && !current.MatriculaEspelhoPontoCalculos.Contains(mapMatriculaEspelhoPontoCalculos))
                        {
                            mapMatriculaEspelhoPontoCalculos.Calculo = mapCalculo;

                            current.MatriculaEspelhoPontoCalculos.Add(
                                mapMatriculaEspelhoPontoCalculos);
                        }

                        return null;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,GUID,ID",
                    transaction: this._transaction);

                return matriculasEspelhoPontoResult.Values.FirstOrDefault();
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
        public IEnumerable<MatriculaEspelhoPontoEntity> Get(string competencia, string matricula)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasEspelhoPontoResult = new Dictionary<Guid, MatriculaEspelhoPontoEntity>();

                base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoMarcacaoEntity, CalculoEntity, MatriculaEspelhoPontoEntity>(
                    this._matriculaEspelhoPontoQuery.CommandTextGetByCompetenciaAndMatricula(),
                    map: (mapMatriculaEspelhoPonto, mapMatricula, mapPessoaFisica, mapPessoaJuridica, mapMatriculaEspelhoPontoCalculos, mapMatriculaEspelhoPontoMarcacoes, mapCalculo) =>
                    {
                        if (!matriculasEspelhoPontoResult.ContainsKey(mapMatriculaEspelhoPonto.Guid))
                        {
                            mapMatricula.Colaborador = mapPessoaFisica;
                            mapMatricula.Empregador = mapPessoaJuridica;

                            mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                            mapMatriculaEspelhoPonto.MatriculaEspelhoPontoMarcacoes = new List<MatriculaEspelhoPontoMarcacaoEntity>();

                            mapMatriculaEspelhoPonto.MatriculaEspelhoPontoCalculos = new List<MatriculaEspelhoPontoCalculoEntity>();

                            matriculasEspelhoPontoResult.Add(
                                mapMatriculaEspelhoPonto.Guid,
                                mapMatriculaEspelhoPonto);
                        }

                        MatriculaEspelhoPontoEntity current = matriculasEspelhoPontoResult[mapMatriculaEspelhoPonto.Guid];

                        if (mapMatriculaEspelhoPontoMarcacoes != null && !current.MatriculaEspelhoPontoMarcacoes.Contains(mapMatriculaEspelhoPontoMarcacoes))
                        {
                            current.MatriculaEspelhoPontoMarcacoes.Add(
                                mapMatriculaEspelhoPontoMarcacoes);
                        }

                        if (mapMatriculaEspelhoPontoCalculos != null && !current.MatriculaEspelhoPontoCalculos.Contains(mapMatriculaEspelhoPontoCalculos))
                        {
                            mapMatriculaEspelhoPontoCalculos.Calculo = mapCalculo;

                            current.MatriculaEspelhoPontoCalculos.Add(
                                mapMatriculaEspelhoPontoCalculos);
                        }

                        return null;
                    },
                    param: new
                    {
                        Competencia = competencia,
                        Matricula = matricula,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,GUID,ID",
                    transaction: this._transaction);

                return matriculasEspelhoPontoResult.Values;
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
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<Guid, MatriculaEspelhoPontoEntity> matriculasEspelhosPontoResult = new Dictionary<Guid, MatriculaEspelhoPontoEntity>();

                base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEspelhoPontoCalculoEntity, MatriculaEspelhoPontoMarcacaoEntity, CalculoEntity, MatriculaEspelhoPontoEntity>(
                    this._matriculaEspelhoPontoQuery.CommandTextGetAll(),
                    map: (mapMatriculaEspelhoPonto, mapMatricula, mapPessoaFisica, mapPessoaJuridica, mapMatriculaEspelhoPontoCalculos, mapMatriculaEspelhoPontoMarcacoes, mapCalculos) =>
                    {
                        if (!matriculasEspelhosPontoResult.ContainsKey(mapMatriculaEspelhoPonto.Guid))
                        {
                            mapMatricula.Colaborador = mapPessoaFisica;
                            mapMatricula.Empregador = mapPessoaJuridica;

                            mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                            //mapMatriculaEspelhoPonto.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();

                            matriculasEspelhosPontoResult.Add(
                                mapMatriculaEspelhoPonto.Guid,
                                mapMatriculaEspelhoPonto);
                        }

                        MatriculaEspelhoPontoEntity current = matriculasEspelhosPontoResult[mapMatriculaEspelhoPonto.Guid];

                        //if (mapMatriculaDemonstrativoPagamentoEventos != null && !current.MatriculaDemonstrativoPagamentoEventos.Contains(mapMatriculaDemonstrativoPagamentoEventos))
                        //{
                        //    mapMatriculaDemonstrativoPagamentoEventos.Evento = mapEvento;

                        //    current.MatriculaDemonstrativoPagamentoEventos.Add(
                        //        mapMatriculaDemonstrativoPagamentoEventos);
                        //}

                        return null;
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,GUID,ID",
                    transaction: this._transaction);

                return matriculasEspelhosPontoResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Espelhos Ponto" records by "GuidColaborador".
        /// </summary>
        /// <param name="guidColaborador"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaEspelhoPontoEntity> GetByGuidColaborador(Guid guidColaborador)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasEspelhosPontoResult = new Dictionary<Guid, MatriculaEspelhoPontoEntity>();

                base._connection.Query<MatriculaEspelhoPontoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity>(
                    sql: this._matriculaEspelhoPontoQuery.CommandTextGetByGuidColaborador(),
                    map: (mapMatriculaEspelhoPonto, mapMatricula, mapPessoaFisica, mapPessoaJuridica, mapMatriculaEspelhoPontoMarcacao) =>
                    {
                        if (!matriculasEspelhosPontoResult.ContainsKey(mapMatriculaEspelhoPonto.Guid))
                        {
                            mapMatricula.Colaborador = mapPessoaFisica;
                            mapMatricula.Empregador = mapPessoaJuridica;

                            mapMatriculaEspelhoPonto.Matricula = mapMatricula;

                            // mapMatriculaEspelhoPonto.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();

                            matriculasEspelhosPontoResult.Add(
                                mapMatriculaEspelhoPonto.Guid,
                                mapMatriculaEspelhoPonto);
                        }

                        MatriculaEspelhoPontoEntity current = matriculasEspelhosPontoResult[mapMatriculaEspelhoPonto.Guid];

                        //if (mapMatriculaDemonstrativoPagamentoEventos != null && !current.MatriculaDemonstrativoPagamentoEventos.Contains(mapMatriculaDemonstrativoPagamentoEventos))
                        //{
                        //    mapMatriculaDemonstrativoPagamentoEventos.Evento = mapEvento;

                        //    current.MatriculaDemonstrativoPagamentoEventos.Add(
                        //        mapMatriculaDemonstrativoPagamentoEventos);
                        //}

                        return null;
                    },
                    param: new
                    {
                        GuidColaborador = guidColaborador,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,ID",
                    transaction: this._transaction);

                return matriculasEspelhosPontoResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos, int quantidadeRegistrosRejeitados) ImportFileEspelhosPonto(string cnpj, string content)
        {
            try
            {
                string sql = "UspImportarArquivoEspelhosPonto";

                var parameters = new
                {
                    Cnpj = cnpj,
                    Conteudo = content,
                };

                return this._connection.QueryFirstOrDefault<(DateTime, DateTime, int, int, int, int)>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the "Matrícula Espelho Ponto" record.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoEntity Update(Guid guid, MatriculaEspelhoPontoEntity entity)
        {
            try
            {
                entity.Guid = guid;

                string cmdText = @" UPDATE [{0}].[dbo].[MATRICULAS_ESPELHOS_PONTO]
                                       SET [GUIDMATRICULA] = {1}GuidMatricula,
                                           [COMPETENCIA] = {1}Competencia,
                                           [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                                           [DATA_CONFIRMACAO] = @DataConfirmacao,
                                           [IP_CONFIRMACAO] = @IpConfirmacao
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

        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                    this._matriculaEspelhoPontoQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MatriculaEspelhoPontoEntity> GetMany(Expression<Func<MatriculaEspelhoPontoEntity, bool>> filter = null, Func<IQueryable<MatriculaEspelhoPontoEntity>, IOrderedQueryable<MatriculaEspelhoPontoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}