namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.Destin
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using ARVTech.DataAccess.CQRS.Commands.Destin;
    using ARVTech.DataAccess.CQRS.Queries.Destin;
    using ARVTech.DataAccess.Domain.Common;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer;
    using Dapper;
    using Microsoft.Data.SqlClient;

    public class ModalidadeRepository : BaseRepository, IModalidadeRepository
    {
        private readonly ModalidadeCommand _modalidadeCommand;

        private readonly ModalidadeQuery _modalidadeQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalidadeRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public ModalidadeRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this.MapAttributeToField(
                typeof(
                    ModalidadeEntity));

            this._modalidadeCommand = new ModalidadeCommand();

            this._modalidadeQuery = new ModalidadeQuery(
                connection);
        }

        /// <summary>
        /// Inserts a new "Modalidade" record into the database.
        /// </summary>
        /// <param name="entity">An <see cref="ModalidadeEntity"/> object containing the data to be inserted.</param>
        /// <returns>The persisted <see cref="ModalidadeEntity"/> object retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the SQL command.</exception>
        public ModalidadeEntity Create(ModalidadeEntity entity)
        {
            try
            {
                this._connection.Execute(
                    sql: this._modalidadeCommand.CommandTextCreate(),
                    param: entity,
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
        /// Deletes an "Modalidade" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Modalidade" record to delete.</param>
        /// <exception cref="Exception">Rethrows any exception that occurs during the execution of the delete operation.</exception>
        public void Delete(int id)
        {
            try
            {
                this._connection.Execute(
                    sql: this._modalidadeCommand.CommandTextDelete(),
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
        /// Retrieves an "Modalidade" record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the "Modalidade" record.</param>
        /// <returns>The matching <see cref="ModalidadeEntity"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public ModalidadeEntity Get(int id)
        {
            try
            {
                var modalidadeEntity = this._connection.Query<ModalidadeEntity>(
                    sql: this._modalidadeQuery.CommandTextGetById(),
                    param: new
                    {
                        Id = id,
                    },
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return modalidadeEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all "Modalidade" records from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ModalidadeEntity}"/> containing all "Modalidade" records.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public IEnumerable<ModalidadeEntity> GetAll()
        {
            try
            {
                var modalidadeEntities = this._connection.Query<ModalidadeEntity>(
                    sql: this._modalidadeQuery.CommandTextGetAll(),
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return modalidadeEntities;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ModalidadeEntity>> GetAllAsync()
        {
            return await this._connection.QueryAsync<ModalidadeEntity>(
                sql: this._modalidadeQuery.CommandTextGetAll(),
                transaction: this._transaction,
                commandType: CommandType.StoredProcedure);
        }

        public PagedResult<ModalidadeEntity> GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ModalidadeEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<ModalidadeEntity> GetAsync(int id)
        {
            var result = await this._connection.QueryAsync<ModalidadeEntity>(
                sql: this._modalidadeQuery.CommandTextGetById(),
                param: new { Id = id },
                transaction: this._transaction,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the highest "Id" currently used in the "Modalidade" records.
        /// </summary>
        /// <returns>The maximum <c>Id</c> value from the "Modalidade" table.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during query execution.</exception>
        public int GetLastId()
        {
            try
            {
                return this._connection.QuerySingle<int>(
                    sql: this._modalidadeQuery.CommandTextGetLastId(),
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing "Modalidade" record in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the "Modalidade" record to update.</param>
        /// <param name="entity">An <see cref="ModalidadeEntity"/> object containing the updated values.</param>
        /// <returns>The updated <see cref="ModalidadeEntity"/> retrieved from the database.</returns>
        /// <exception cref="Exception">Rethrows any exception that occurs during the update operation.</exception>
        public ModalidadeEntity Update(int id, ModalidadeEntity entity)
        {
            try
            {
                entity.Id = id;

                this._connection.Execute(
                    sql: this._modalidadeCommand.CommandTextUpdate(),
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