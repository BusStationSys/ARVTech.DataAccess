namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaDemonstrativoPagamentoRepository : BaseRepository, IMatriculaDemonstrativoPagamentoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly MatriculaDemonstrativoPagamentoQuery _matriculaDemonstrativoPagamentoQuery;

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

            this._matriculaDemonstrativoPagamentoQuery = new MatriculaDemonstrativoPagamentoQuery(
                connection,
                transaction);
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextCreate(),
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
                    this._matriculaDemonstrativoPagamentoQuery.CommandTextDelete(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextDeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetById(),
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
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculasDemonstrativosPagamentoEntity = this._connection.Query<MatriculaDemonstrativoPagamentoEntity, MatriculaEntity, MatriculaDemonstrativoPagamentoEntity>(
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetByCompetenciaAndMatricula(),
                    map: (mapMatriculaDemonstrativoPagamento, mapMatricula) =>
                    {
                        //mapMatricula.Colaborador = mapPessoaFisica;
                        //mapMatricula.Empregador = mapPessoaJuridica;

                        mapMatriculaDemonstrativoPagamento.Matricula = mapMatricula;

                        return mapMatriculaDemonstrativoPagamento;
                    },
                    param: new
                    {
                        Competencia = competencia,
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetAll(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetByCompetencia(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetByGuidColaborador(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextGetByMatricula(),
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
                    sql: this._matriculaDemonstrativoPagamentoQuery.CommandTextUpdate(),
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
                    this._matriculaDemonstrativoPagamentoQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}