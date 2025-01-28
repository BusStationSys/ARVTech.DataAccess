namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaRepository : BaseRepository, IMatriculaRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly MatriculaCommand _matriculaCommand;
        private readonly MatriculaQuery _matriculaQuery;

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

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaJuridicaEntity));

            this._matriculaCommand = new MatriculaCommand();

            this._matriculaQuery = new MatriculaQuery(
                connection,
                transaction);
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
                var guid = base._connection.QuerySingle<Guid>(
                    sql: this._matriculaCommand.CommandTextCreate(),
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

                base._connection.Execute(
                    sql: this._matriculaCommand.CommandTextDelete(),
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
        public void DeleteEspelhosPonto(Guid guidMatricula)
        {
            try
            {
                if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                base._connection.Execute(
                    sql: this._matriculaQuery.CommandTextDeleteEspelhosPonto(),
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
                var matriculaEntity = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    sql: this._matriculaQuery.CommandTextGetById(),
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

                return matriculaEntity.FirstOrDefault();
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
                var matriculasEntities = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    sql: this._matriculaQuery.CommandTextGetAll(),
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
        /// <param name="mes"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaEntity> GetAniversariantesEmpresa(int mes)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculasEntities = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    sql: this._matriculaQuery.CommandTextGetAniversariantesEmpresa(),
                    map: (mapMatricula, mapPessoaFisica, mapPessoaJuridica) =>
                    {
                        mapMatricula.Colaborador = mapPessoaFisica;
                        mapMatricula.Empregador = mapPessoaJuridica;

                        return mapMatricula;
                    },
                    param: new
                    {
                        Mes = mes,
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

                string sql = "UspObterMatriculaPorMatricula";

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEntity = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    sql: sql,
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

                base._connection.Execute(
                    sql: this._matriculaCommand.CommandTextUpdate(),
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
                    this._matriculaQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public IEnumerable<MatriculaEntity> GetMany(Expression<Func<MatriculaEntity, bool>> filter = null, Func<IQueryable<MatriculaEntity>, IOrderedQueryable<MatriculaEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) ImportFileMatriculas(string cnpj, string content)
        {
            try
            {
                string sql = "UspImportarArquivoMatriculas";

                var parameters = new
                {
                    Cnpj = cnpj,
                    Conteudo = content,
                };

                return this._connection.QueryFirstOrDefault<(DateTime, DateTime, int, int, int)>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }
    }
}