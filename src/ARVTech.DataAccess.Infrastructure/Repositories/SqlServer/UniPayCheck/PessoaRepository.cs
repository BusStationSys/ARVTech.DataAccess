namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class PessoaRepository : BaseRepository, IPessoaRepository
    {
        private readonly string _columnsPessoas;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this._columnsPessoas = base.GetAllColumnsFromTable(
                "PESSOAS",
                "P");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaEntity Create(PessoaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                //  Insere o registro na tabela "PESSOAS".
                string cmdText = @" INSERT INTO [{0}].[dbo].[PESSOAS]
                                               ([GUID],
                                                [BAIRRO],
                                                [CEP],
                                                [CIDADE],
                                                [COMPLEMENTO],
                                                [ENDERECO],
                                                [NUMERO],
                                                [UF])
                                        VALUES ({1}Guid,
                                                {1}Bairro,
                                                {1}Cep,
                                                {1}Cidade,
                                                {1}Complemento,
                                                {1}Endereco,
                                                {1}Numero,
                                                {1}Uf) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    sql: cmdText,
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

        /// <summary>
        /// Gets the "Pessoa" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0}
                                           FROM [{1}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                          WHERE UPPER(P.GUID) = {2}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoa = this._connection.Query<PessoaEntity>(
                    cmdText,
                    param: new
                    {
                        Guid = guid,
                    },
                    this._transaction);

                return pessoa.FirstOrDefault();
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
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<PessoaEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PessoaEntity> GetMany(Expression<Func<PessoaEntity, bool>> filter = null, Func<IQueryable<PessoaEntity>, IOrderedQueryable<PessoaEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}