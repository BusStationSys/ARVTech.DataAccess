namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Domain.Enums.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using Dapper;

    public class MatriculaDemonstrativoPagamentoRepository : BaseRepository, IMatriculaDemonstrativoPagamentoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaDemonstrativoPagamentoRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    EventoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoEventoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaDemonstrativoPagamentoTotalizadorEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaJuridicaEntity));

            this.MapAttributeToField(
                typeof(
                    TotalizadorEntity));
        }

        /// <summary>
        /// Creates the "Matrícula Demonstrativo Pagamento" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoEntity Create(MatriculaDemonstrativoPagamentoEntity entity)
        {
            try
            {
                var guid = this._connection.QuerySingle<Guid>(
                    sql: "UspInserirMatriculaDemonstrativoPagamento",
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
        /// Deletes the "Matrícula Demonstrativo Pagamento" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula Demonstrativo Pagamento" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                this._connection.Execute(
                    "UspExcluirMatriculaDemonstrativoPagamentoPorId",
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
        /// Deletes the "Eventos" And "Totalizadores" of "Matrícula Demonstrativo Pagamento" records by "Competência" And "Guid of Matrícula".
        /// </summary>
        /// <param name="competencia">"Competência" of "Matrícula" record.</param>
        /// <param name="guidMatricula">Guid of "Matrícula" record.</param>
        public void DeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                this._connection.Execute(
                    sql: "UspExcluirMatriculaDemonstrativoPagamentoVinculosPorCompetenciaEIdMatricula",
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
        /// Gets the "Matrícula Demonstrativo Pagamento" record.
        /// </summary>
        /// <param name="guid">Guid of "Matrícula Demonstrativo Pagamento" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public MatriculaDemonstrativoPagamentoEntity Get(Guid guid)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasDemonstrativosPagamentoResult = new Dictionary<Guid, MatriculaDemonstrativoPagamentoEntity>();

                this._connection.Query<MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculaDemonstrativoPagamentoPorId",
                    new[]
                    {
                        typeof(MatriculaDemonstrativoPagamentoEntity),
                        typeof(MatriculaEntity),
                        typeof(PessoaFisicaEntity),
                        typeof(PessoaJuridicaEntity),
                        typeof(MatriculaDemonstrativoPagamentoEventoEntity),
                        typeof(EventoEntity),
                        typeof(MatriculaDemonstrativoPagamentoTotalizadorEntity),
                        typeof(TotalizadorEntity),
                    },
                    obj =>
                    {
                        var matriculaDemonstrativoPagamentoEntity = (MatriculaDemonstrativoPagamentoEntity)obj[0];
                        var matriculaEntity = (MatriculaEntity)obj[1];
                        var pessoaFisicaEntity = (PessoaFisicaEntity)obj[2];
                        var pessoaJuridicaEntity = (PessoaJuridicaEntity)obj[3];
                        var matriculaDemonstrativoPagamentoEventoEntity = (MatriculaDemonstrativoPagamentoEventoEntity)obj[4];
                        var eventoEntity = (EventoEntity)obj[5];
                        var matriculaDemonstrativoPagamentoTotalizadorEntity = (MatriculaDemonstrativoPagamentoTotalizadorEntity)obj[6];
                        var totalizadorEntity = (TotalizadorEntity)obj[7];

                        if (!matriculasDemonstrativosPagamentoResult.ContainsKey(matriculaDemonstrativoPagamentoEntity.Guid))
                        {
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoTotalizadores = new List<MatriculaDemonstrativoPagamentoTotalizadorEntity>();

                            matriculaEntity.Colaborador = pessoaFisicaEntity;
                            matriculaEntity.Empregador = pessoaJuridicaEntity;

                            matriculaDemonstrativoPagamentoEntity.Matricula = matriculaEntity;

                            matriculasDemonstrativosPagamentoResult.Add(
                                matriculaDemonstrativoPagamentoEntity.Guid,
                                matriculaDemonstrativoPagamentoEntity);
                        }

                        MatriculaDemonstrativoPagamentoEntity current = matriculasDemonstrativosPagamentoResult[matriculaDemonstrativoPagamentoEntity.Guid];

                        if (matriculaDemonstrativoPagamentoEventoEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoEventos.Any(
                                mdpe => mdpe.IdEvento == matriculaDemonstrativoPagamentoEventoEntity.IdEvento))
                        {
                            matriculaDemonstrativoPagamentoEventoEntity.Evento = eventoEntity;

                            current.MatriculaDemonstrativoPagamentoEventos.Add(
                                matriculaDemonstrativoPagamentoEventoEntity);
                        }

                        if (matriculaDemonstrativoPagamentoTotalizadorEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoTotalizadores.Any(
                                mdpt => mdpt.IdTotalizador == matriculaDemonstrativoPagamentoTotalizadorEntity.IdTotalizador))
                        {
                            matriculaDemonstrativoPagamentoTotalizadorEntity.Totalizador = totalizadorEntity;

                            current.MatriculaDemonstrativoPagamentoTotalizadores.Add(
                                matriculaDemonstrativoPagamentoTotalizadorEntity);
                        }

                        return null;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,ID,GUID,ID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Demonstrativos Pagamento" records by "Competência" And "Matrícula".
        /// </summary>
        /// <param name="competencia"></param>
        /// <param name="matricula"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> Get(string competencia, string matricula)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasDemonstrativosPagamentoResult = new Dictionary<Guid, MatriculaDemonstrativoPagamentoEntity>();

                this._connection.Query<MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculaDemonstrativoPagamentoPorCompetenciaEMatricula",
                    new[]
                    {
                        typeof(MatriculaDemonstrativoPagamentoEntity),
                        typeof(MatriculaEntity),
                        typeof(PessoaFisicaEntity),
                        typeof(PessoaJuridicaEntity),
                        typeof(MatriculaDemonstrativoPagamentoEventoEntity),
                        typeof(EventoEntity),
                        typeof(MatriculaDemonstrativoPagamentoTotalizadorEntity),
                        typeof(TotalizadorEntity),
                    },
                    obj =>
                    {
                        var matriculaDemonstrativoPagamentoEntity = (MatriculaDemonstrativoPagamentoEntity)obj[0];
                        var matriculaEntity = (MatriculaEntity)obj[1];
                        var pessoaFisicaEntity = (PessoaFisicaEntity)obj[2];
                        var pessoaJuridicaEntity = (PessoaJuridicaEntity)obj[3];
                        var matriculaDemonstrativoPagamentoEventoEntity = (MatriculaDemonstrativoPagamentoEventoEntity)obj[4];
                        var eventoEntity = (EventoEntity)obj[5];
                        var matriculaDemonstrativoPagamentoTotalizadorEntity = (MatriculaDemonstrativoPagamentoTotalizadorEntity)obj[6];
                        var totalizadorEntity = (TotalizadorEntity)obj[7];

                        if (!matriculasDemonstrativosPagamentoResult.ContainsKey(matriculaDemonstrativoPagamentoEntity.Guid))
                        {
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoTotalizadores = new List<MatriculaDemonstrativoPagamentoTotalizadorEntity>();

                            matriculaEntity.Colaborador = pessoaFisicaEntity;
                            matriculaEntity.Empregador = pessoaJuridicaEntity;

                            matriculaDemonstrativoPagamentoEntity.Matricula = matriculaEntity;

                            matriculasDemonstrativosPagamentoResult.Add(
                                matriculaDemonstrativoPagamentoEntity.Guid,
                                matriculaDemonstrativoPagamentoEntity);
                        }

                        MatriculaDemonstrativoPagamentoEntity current = matriculasDemonstrativosPagamentoResult[matriculaDemonstrativoPagamentoEntity.Guid];

                        if (matriculaDemonstrativoPagamentoEventoEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoEventos.Any(
                                mdpe => mdpe.IdEvento == matriculaDemonstrativoPagamentoEventoEntity.IdEvento))
                        {
                            matriculaDemonstrativoPagamentoEventoEntity.Evento = eventoEntity;

                            current.MatriculaDemonstrativoPagamentoEventos.Add(
                                matriculaDemonstrativoPagamentoEventoEntity);
                        }

                        if (matriculaDemonstrativoPagamentoTotalizadorEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoTotalizadores.Any(
                                mdpt => mdpt.IdTotalizador == matriculaDemonstrativoPagamentoTotalizadorEntity.IdTotalizador))
                        {
                            matriculaDemonstrativoPagamentoTotalizadorEntity.Totalizador = totalizadorEntity;

                            current.MatriculaDemonstrativoPagamentoTotalizadores.Add(
                                matriculaDemonstrativoPagamentoTotalizadorEntity);
                        }

                        return null;
                    },
                    param: new
                    {
                        Competencia = competencia,
                        Matricula = matricula,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,ID,GUID,ID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Demonstrativos Pagamento" records.
        /// </summary>
        /// <returns>If success, the list with all "Matrículas Demonstrativos Pagamento" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasDemonstrativosPagamentoResult = new Dictionary<Guid, MatriculaDemonstrativoPagamentoEntity>();

                this._connection.Query<MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculasDemonstrativosPagamento",
                    new[]
                    {
                        typeof(MatriculaDemonstrativoPagamentoEntity),
                        typeof(MatriculaEntity),
                        typeof(PessoaFisicaEntity),
                        typeof(PessoaJuridicaEntity),
                        typeof(MatriculaDemonstrativoPagamentoEventoEntity),
                        typeof(EventoEntity),
                        typeof(MatriculaDemonstrativoPagamentoTotalizadorEntity),
                        typeof(TotalizadorEntity),
                    },
                    obj =>
                    {
                        var matriculaDemonstrativoPagamentoEntity = (MatriculaDemonstrativoPagamentoEntity)obj[0];
                        var matriculaEntity = (MatriculaEntity)obj[1];
                        var pessoaFisicaEntity = (PessoaFisicaEntity)obj[2];
                        var pessoaJuridicaEntity = (PessoaJuridicaEntity)obj[3];
                        var matriculaDemonstrativoPagamentoEventoEntity = (MatriculaDemonstrativoPagamentoEventoEntity)obj[4];
                        var eventoEntity = (EventoEntity)obj[5];
                        var matriculaDemonstrativoPagamentoTotalizadorEntity = (MatriculaDemonstrativoPagamentoTotalizadorEntity)obj[6];
                        var totalizadorEntity = (TotalizadorEntity)obj[7];

                        if (!matriculasDemonstrativosPagamentoResult.ContainsKey(matriculaDemonstrativoPagamentoEntity.Guid))
                        {
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();
                            matriculaDemonstrativoPagamentoEntity.MatriculaDemonstrativoPagamentoTotalizadores = new List<MatriculaDemonstrativoPagamentoTotalizadorEntity>();

                            matriculaEntity.Colaborador = pessoaFisicaEntity;
                            matriculaEntity.Empregador = pessoaJuridicaEntity;

                            matriculaDemonstrativoPagamentoEntity.Matricula = matriculaEntity;

                            matriculasDemonstrativosPagamentoResult.Add(
                                matriculaDemonstrativoPagamentoEntity.Guid,
                                matriculaDemonstrativoPagamentoEntity);
                        }

                        MatriculaDemonstrativoPagamentoEntity current = matriculasDemonstrativosPagamentoResult[matriculaDemonstrativoPagamentoEntity.Guid];

                        if (matriculaDemonstrativoPagamentoEventoEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoEventos.Any(
                                mdpe => mdpe.IdEvento == matriculaDemonstrativoPagamentoEventoEntity.IdEvento))
                        {
                            matriculaDemonstrativoPagamentoEventoEntity.Evento = eventoEntity;

                            current.MatriculaDemonstrativoPagamentoEventos.Add(
                                matriculaDemonstrativoPagamentoEventoEntity);
                        }

                        if (matriculaDemonstrativoPagamentoTotalizadorEntity != null &&
                            !current.MatriculaDemonstrativoPagamentoTotalizadores.Any(
                                mdpt => mdpt.IdTotalizador == matriculaDemonstrativoPagamentoTotalizadorEntity.IdTotalizador))
                        {
                            matriculaDemonstrativoPagamentoTotalizadorEntity.Totalizador = totalizadorEntity;

                            current.MatriculaDemonstrativoPagamentoTotalizadores.Add(
                                matriculaDemonstrativoPagamentoTotalizadorEntity);
                        }

                        return null;
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,ID,GUID,ID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Demonstrativos Pagamento" records by "Competência".
        /// </summary>
        /// <param name="competencia"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByCompetencia(string competencia)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculasDemonstrativosPagamentoEntity = this._connection.Query<MatriculaDemonstrativoPagamentoEntity, MatriculaEntity, MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculaDemonstrativoPagamentoPorCompetencia",
                    map: (mapMatriculasDemonstrativoPagamento, mapMatricula) =>
                    {
                        //mapMatricula.Colaborador = mapPessoaFisica;
                        //mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculasDemonstrativoPagamento.Matricula = mapMatricula;

                        return mapMatriculasDemonstrativoPagamento;
                    },
                    param: new
                    {
                        Competencia = competencia,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoEntity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Demonstrativos Pagamento" records by "GuidColaborador".
        /// </summary>
        /// <param name="guidColaborador"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByGuidColaborador(Guid guidColaborador)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var matriculasDemonstrativosPagamentoResult = new Dictionary<Guid, MatriculaDemonstrativoPagamentoEntity>();

                this._connection.Query<MatriculaDemonstrativoPagamentoEntity, MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaDemonstrativoPagamentoEventoEntity, EventoEntity, MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculaDemonstrativoPagamentoPorIdColaborador",
                    map: (mapMatriculaDemonstrativoPagamento, mapMatricula, mapPessoaFisica, mapPessoaJuridica, mapMatriculaDemonstrativoPagamentoEventos, mapEvento) =>
                    {
                        if (!matriculasDemonstrativosPagamentoResult.ContainsKey(mapMatriculaDemonstrativoPagamento.Guid))
                        {
                            mapMatricula.Colaborador = mapPessoaFisica;
                            mapMatricula.Empregador = mapPessoaJuridica;

                            mapMatriculaDemonstrativoPagamento.Matricula = mapMatricula;

                            mapMatriculaDemonstrativoPagamento.MatriculaDemonstrativoPagamentoEventos = new List<MatriculaDemonstrativoPagamentoEventoEntity>();

                            matriculasDemonstrativosPagamentoResult.Add(
                                mapMatriculaDemonstrativoPagamento.Guid,
                                mapMatriculaDemonstrativoPagamento);
                        }

                        MatriculaDemonstrativoPagamentoEntity current = matriculasDemonstrativosPagamentoResult[mapMatriculaDemonstrativoPagamento.Guid];

                        if (mapMatriculaDemonstrativoPagamentoEventos != null && !current.MatriculaDemonstrativoPagamentoEventos.Contains(mapMatriculaDemonstrativoPagamentoEventos))
                        {
                            mapMatriculaDemonstrativoPagamentoEventos.Evento = mapEvento;

                            current.MatriculaDemonstrativoPagamentoEventos.Add(
                                mapMatriculaDemonstrativoPagamentoEventos);
                        }

                        return null;
                    },
                    param: new
                    {
                        GuidColaborador = guidColaborador,
                    },
                    splitOn: "GUID,GUID,GUID,GUID,GUID,ID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Matrículas Demonstrativos Pagamento" records by "Matrícula".
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByMatricula(string matricula)
        {
            try
            {
                var matriculasDemonstrativosPagamentoEntity = this._connection.Query<MatriculaDemonstrativoPagamentoEntity, MatriculaEntity, MatriculaDemonstrativoPagamentoEntity>(
                    sql: "UspObterMatriculaDemonstrativoPagamentoPorMatricula",
                    map: (mapMatriculaDemonstrativoPagamento, mapMatricula) =>
                    {
                        //mapMatricula.Colaborador = mapPessoaFisica;
                        //mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculaDemonstrativoPagamento.Matricula = mapMatricula;

                        return mapMatriculaDemonstrativoPagamento;
                    },
                    param: new
                    {
                        Matricula = matricula,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculasDemonstrativosPagamentoEntity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Imports payroll statement (demonstrativo de pagamento) data for a given company by executing the stored procedure 'UspImportarArquivoDemonstrativosPagamento'.
        /// </summary>
        /// <param name="cnpj">The CNPJ (Brazilian company identifier) of the company related to the payroll statement data.</param>
        /// <param name="content">The raw content of the payroll statement file to be processed.</param>
        /// <returns>
        /// A tuple containing:
        /// - dataInicio: The date and time when the import process started;
        /// - dataFim: The date and time when the import process finished;
        /// - quantidadeRegistrosAtualizados: The number of records that were updated;
        /// - quantidadeRegistrosInalterados: The number of records that remained unchanged;
        /// - quantidadeRegistrosInseridos: The number of records that were newly inserted;
        /// - quantidadeRegistrosRejeitados: The number of records that were rejected during the import.
        /// </returns>
        public (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos, int quantidadeRegistrosRejeitados) ImportFileDemonstrativosPagamento(string cnpj, string content)
        {
            try
            {
                string sql = "UspImportarArquivoDemonstrativosPagamento";

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
        /// Updates the "Matrícula Demonstrativo Pagamento" record.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoEntity Update(Guid guid, MatriculaDemonstrativoPagamentoEntity entity)
        {
            try
            {
                entity.Guid = guid;

                this._connection.Execute(
                    sql: "UspAtualizarMatriculaDemonstrativoPagamento",
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

        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamentoEnum situacao = SituacaoPendenciaDemonstrativoPagamentoEnum.Todos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetMany(Expression<Func<MatriculaDemonstrativoPagamentoEntity, bool>> filter = null, Func<IQueryable<MatriculaDemonstrativoPagamentoEntity>, IOrderedQueryable<MatriculaDemonstrativoPagamentoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}