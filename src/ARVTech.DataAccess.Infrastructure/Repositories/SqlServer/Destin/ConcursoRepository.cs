namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.Destin
{
    using ARVTech.DataAccess.CQRS.Commands.Destin;
    using ARVTech.DataAccess.CQRS.Queries.Destin;
    using ARVTech.DataAccess.Domain.Common;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Domain.Enums.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ConcursoRepository : BaseRepository, IConcursoRepository
    {
        private readonly ConcursoCommand _concursoCommand;

        private readonly ConcursoQuery _concursoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcursoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public ConcursoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this.MapAttributeToField(
                typeof(
                    ConcursoEntity));

            this._concursoCommand = new ConcursoCommand();

            this._concursoQuery = new ConcursoQuery(
                connection);
        }

        /// <summary>
        /// Inserts a new "Concurso" record into the database.
        /// </summary>
        /// <param name="entity">An <see cref="ConcursoEntity"/> object containing the data to be inserted.</param>
        /// <returns>The persisted <see cref="ConcursoEntity"/> object retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the SQL command.</exception>
        public ConcursoEntity Create(ConcursoEntity entity)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@Id",
                    entity.Id);

                param.Add("@IdModalidade",
                    entity.IdModalidade);

                param.Add("@Numero",
                    entity.Numero);

                param.Add("@DataApuracao",
                    entity.DataApuracao);

                param.Add("@Dezenas",
                    entity.Dezenas != null &&
                    entity.Dezenas.Count() > 0 ?
                        JsonConvert.SerializeObject(entity.Dezenas) :
                        null,
                    DbType.String);

                this._connection.Execute(
                    sql: this._concursoCommand.CommandTextCreate(),
                    param: param,
                    transaction: this._transaction);

                return this.Get(
                    entity.Id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes an "Concurso" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Concurso" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(Guid id)
        {
            try
            {
                this._connection.Execute(
                    sql: this._concursoCommand.CommandTextDelete(),
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves an "Concurso" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Concurso" record.</param>
        /// <returns>The matching <see cref="ConcursoEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public ConcursoEntity Get(Guid id)
        {
            try
            {
                var concursoEntity = this._connection.Query<ConcursoEntity, string, ConcursoEntity>(
                    sql: this._concursoQuery.CommandTextGetById(),
                    map: (concurso, dezenas) =>
                    {
                        concurso.Dezenas = !string.IsNullOrEmpty(dezenas) ?
                            JsonConvert.DeserializeObject<List<ConcursoDezenaEntity>>(dezenas) :
                            null;

                        return concurso;
                    },
                    splitOn: "Dezenas",
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return concursoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Concurso" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ConcursoEntity}"/> containing all "Concurso" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<ConcursoEntity> GetAll()
        {
            try
            {
                var concursoEntities = this._connection.Query<ConcursoEntity>(
                    sql: this._concursoQuery.CommandTextGetAll(),
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return concursoEntities;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ConcursoEntity>> GetAllAsync()
        {
            return await this._connection.QueryAsync<ConcursoEntity>(
                sql: this._concursoQuery.CommandTextGetAll(),
                transaction: this._transaction,
                commandType: CommandType.StoredProcedure);
        }

        public PagedResult<ConcursoEntity> GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ConcursoEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<ConcursoEntity> GetAsync(Guid id)
        {
            var result = await this._connection.QueryAsync<ConcursoEntity>(
                sql: this._concursoQuery.CommandTextGetById(),
                param: new { Id = id },
                transaction: this._transaction,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idModalidade"></param>
        /// <param name="numero"></param>
        /// <param name="dataApuracao"></param>
        /// <returns></returns>
        public ConcursoEntity GetByIdModalidadeAndNumeroAndDataApuracao(int idModalidade, int numero, DateTime dataApuracao)
        {
            try
            {
                var concursoEntity = this._connection.Query<ConcursoEntity, string, ConcursoEntity>(
                    sql: this._concursoQuery.CommandTextGetByIdModalidadeAndNumeroAndDataApuracao(),
                    map: (concurso, dezenas) =>
                    {
                        concurso.Dezenas = !string.IsNullOrEmpty(dezenas) ?
                            JsonConvert.DeserializeObject<List<ConcursoDezenaEntity>>(dezenas) :
                            null;

                        return concurso;
                    },
                    splitOn: "Dezenas",
                    param: new
                    {
                        IdModalidade = idModalidade,
                        Numero = numero,
                        DataApuracao = dataApuracao
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return concursoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing "Concurso" record in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the "Concurso" record to update.</param>
        /// <param name="entity">An <see cref="ConcursoEntity"/> object containing the updated values.</param>
        /// <returns>The updated <see cref="ConcursoEntity"/> retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the update operation.</exception>
        public ConcursoEntity Update(Guid id, ConcursoEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    sql: this._concursoCommand.CommandTextUpdate(),
                    param: entity,
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return this.Get(
                    entity.Id);
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
    }
}