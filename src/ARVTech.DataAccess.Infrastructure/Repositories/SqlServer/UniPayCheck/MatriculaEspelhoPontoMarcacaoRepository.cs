namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class MatriculaEspelhoPontoMarcacaoRepository : BaseRepository, IMatriculaEspelhoPontoMarcacaoRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly MatriculaEspelhoPontoMarcacaoCommand _matriculaEspelhoPontoMarcacaoCommand;
        private readonly MatriculaEspelhoPontoMarcacaoQuery _matriculaEspelhoPontoMarcacaoQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoMarcacaoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaEspelhoPontoMarcacaoRepository(SqlConnection connection, SqlTransaction? transaction = null)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoEntity));

            this.MapAttributeToField(
                typeof(
                    MatriculaEspelhoPontoMarcacaoEntity));

            this._matriculaEspelhoPontoMarcacaoCommand = new MatriculaEspelhoPontoMarcacaoCommand();

            this._matriculaEspelhoPontoMarcacaoQuery = new MatriculaEspelhoPontoMarcacaoQuery(
                connection,
                transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity Create(MatriculaEspelhoPontoMarcacaoEntity entity)
        {
            try
            {
                var guid = base._connection.QuerySingle<Guid>(
                    sql: this._matriculaEspelhoPontoMarcacaoCommand.CommandTextCreate(),
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
                this._connection.Execute(
                    this._matriculaEspelhoPontoMarcacaoCommand.CommandTextDelete(),
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
        /// <param name="filter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEspelhoPontoMarcacaoEntity = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    sql: this._matriculaEspelhoPontoMarcacaoQuery.CommandTextGetById(),
                    map: (mapMatriculaEspelhoPontoMarcacao, mapMatriculaEspelhoPonto) =>
                    {
                        //mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoMarcacao;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntity.FirstOrDefault();
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
        public IEnumerable<MatriculaEspelhoPontoMarcacaoEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEspelhoPontoMarcacaoEntities = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    sql: this._matriculaEspelhoPontoMarcacaoQuery.CommandTextGetAll(),
                    map: (mapMatriculaDemonstrativoEventoPagamento, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaDemonstrativoEventoPagamento;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaEspelhoPonto"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoEntity GetByGuidMatriculaEspelhoPontoAndData(Guid guidMatriculaEspelhoPonto, DateTime data)
        {
            try
            {
                if (guidMatriculaEspelhoPonto == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaEspelhoPonto));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var matriculaEspelhoPontoMarcacaoEntity = base._connection.Query<MatriculaEspelhoPontoMarcacaoEntity, MatriculaEspelhoPontoEntity, MatriculaEspelhoPontoMarcacaoEntity>(
                    sql: this._matriculaEspelhoPontoMarcacaoQuery.CommandTextGetByGuidMatriculaEspelhoPontoAndData(),
                    map: (mapMatriculaEspelhoPontoMarcacao, mapMatriculaDemonstrativoPagamento) =>
                    {
                        //  mapMatriculaDemonstrativoEventoPagamento.MatriculaDemonstrativoPagamento = mapMatriculaDemonstrativoPagamento;

                        return mapMatriculaEspelhoPontoMarcacao;
                    },
                    param: new
                    {
                        GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                        Data = data,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return matriculaEspelhoPontoMarcacaoEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<MatriculaEspelhoPontoMarcacaoEntity> GetMany(Expression<Func<MatriculaEspelhoPontoMarcacaoEntity, bool>> filter = null, Func<IQueryable<MatriculaEspelhoPontoMarcacaoEntity>, IOrderedQueryable<MatriculaEspelhoPontoMarcacaoEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
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
                    this._matriculaEspelhoPontoMarcacaoQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}