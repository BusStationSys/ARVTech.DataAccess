namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using Dapper;

    public class MatriculaRepository : BaseRepository, IMatriculaRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

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
        }

        /// <summary>
        /// Inserts a new "Matrícula" record into the database.
        /// </summary>
        /// <param name="entity">An <see cref="MatriculaEntity"/> object containing the data to be inserted.</param>
        /// <returns>The persisted <see cref="MatriculaEntity"/> object retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the SQL command.</exception>
        public MatriculaEntity Create(MatriculaEntity entity)
        {
            try
            {
                var guid = base._connection.QuerySingle<Guid>(
                    "UspInserirMatricula",
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
        /// Deletes a "Matrícula" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Matrícula" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                base._connection.Execute(
                    "UspExcluirMatriculaPorId",
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
        /// Deletes all "Espelho Ponto" records associated with the specified "Matrícula".
        /// </summary>
        /// <param name="guidMatricula">The unique identifier (<see cref="Guid"/>) of the "Matrícula" whose records should be deleted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="guidMatricula"/> is empty.</exception>
        /// <exception cref="Exception">Rethrows any exception that occurs during the delete operation.</exception>
        public void DeleteEspelhosPonto(Guid guidMatricula)
        {
            try
            {
                if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                base._connection.Execute(
                    "UspExcluirMatriculaEspelhoPontoPorIdMatricula",
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
        /// Retrieves an "Matricula" record from the database by its ID.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Matrícula" record.</param>
        /// <returns>The matching <see cref="MatriculaEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public MatriculaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEntity = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    "UspObterMatriculaPorId",
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
        /// Retrieves all "Matrícula" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{MatriculaEntity}"/> containing all "Matrícula" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<MatriculaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculasEntities = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    "UspObterMatriculas",
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
        /// Retrieves a list of employees (matriculas) who have birthdays in the specified month, including their related personal and employer information.
        /// </summary>
        /// <param name="mes">The month number (1–12) used to filter birthdays.</param>
        /// <returns>An <see cref="IEnumerable{MatriculaEntity}"/> containing the matched records with related entities.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<MatriculaEntity> GetAniversariantesEmpresa(int mes)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculasEntities = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    sql: "UspObterAniversariantesEmpresa",
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
        /// Retrieves an "Matricula" record from the database by its "Matrícula".
        /// </summary>
        /// <param name="matricula">The "Matrícula" of the "Matrícula" record.</param>
        /// <returns>The matching <see cref="MatriculaEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public MatriculaEntity GetByMatricula(string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(matricula));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEntity = base._connection.Query<MatriculaEntity, PessoaFisicaEntity, PessoaJuridicaEntity, MatriculaEntity>(
                    "UspObterMatriculaPorMatricula",
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
        /// Imports enrollment data for a given company by executing the stored procedure 'UspImportarArquivoMatriculas'.
        /// </summary>
        /// <param name="cnpj">The CNPJ (Brazilian company identifier) of the company related to the enrollment data.</param>
        /// <param name="content">The raw content of the enrollment file to be processed.</param>
        /// <returns>
        /// A tuple containing:
        /// - dataInicio: The date and time when the import process started;
        /// - dataFim: The date and time when the import process finished;
        /// - quantidadeRegistrosAtualizados: The number of records that were updated;
        /// - quantidadeRegistrosInalterados: The number of records that remained unchanged;
        /// - quantidadeRegistrosInseridos: The number of records that were newly inserted.
        /// </returns>
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

        /// <summary>
        /// Updates an existing "Matrícula" record in the database.
        /// </summary>
        /// <param name="guid">The unique identifier of the "Matrícula" record to update.</param>
        /// <param name="entity">An <see cref="MatriculaEntity"/> object containing the updated values.</param>
        /// <returns>The updated <see cref="MatriculaEntity"/> retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the update operation.</exception>
        public MatriculaEntity Update(Guid guid, MatriculaEntity entity)
        {
            try
            {
                entity.Guid = guid;

                base._connection.Execute(
                    "UspAtualizarMatricula",
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

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
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

        public IEnumerable<MatriculaEntity> GetMany(Expression<Func<MatriculaEntity, bool>> filter = null, Func<IQueryable<MatriculaEntity>, IOrderedQueryable<MatriculaEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        //public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        //{
        //    return base.RefreshPagination(
        //        this._commandTextTemplate,
        //        where,
        //        orderBy,
        //        pageNumber,
        //        pageSize);
        //}
    }
}