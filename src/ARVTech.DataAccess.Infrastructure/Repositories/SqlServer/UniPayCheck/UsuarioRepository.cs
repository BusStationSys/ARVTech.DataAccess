namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using ARVTech.DataAccess.CQRS.Commands;
    using ARVTech.DataAccess.CQRS.Queries;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using Dapper;
    using Newtonsoft.Json;

    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly UsuarioCommand _usuarioCommand;

        private readonly UsuarioQuery _usuarioQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuarioRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public UsuarioRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    UsuarioEntity));

            this.MapAttributeToField(
                typeof(
                    UsuarioNotificacaoEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this._usuarioCommand = new UsuarioCommand();

            this._usuarioQuery = new UsuarioQuery(
                connection,
                transaction);
        }

        /// <summary>
        /// Creates the "Usuário" record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UsuarioEntity Create(UsuarioEntity entity)
        {
            try
            {
                string dataJson = JsonConvert.SerializeObject(entity,
                    Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });

                string sql = "uspSalvarUsuario";

                var guid = this._connection.QuerySingle<Guid>(
                    sql: sql,
                    param: new
                    {
                        DataJson = dataJson,
                    },
                    commandType: CommandType.StoredProcedure,
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
        /// Check if the "Password" of "Usuário" record is valid.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="password">"Password" of "Usuário" record.</param>
        /// <returns>If success, the Entity with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity CheckPasswordValid(Guid guid, string password)
        {
            try
            {
                string sql = "uspVerificarPasswordValido";

                var usuarioEntity = this._connection.QueryFirstOrDefault(
                    sql: sql,
                    param: new
                    {
                        GuidUsuario = guid,
                        PasswordInput = password,
                    },
                    commandType: CommandType.StoredProcedure);

                if (usuarioEntity != null)
                    return this.Get(
                        usuarioEntity.GUID);

                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the "Usuário" record.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                this._connection.Execute(
                    this._usuarioCommand.CommandTextDelete(),
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
        /// Gets the "Usuário" record.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(
                            guid));

                string sql = "uspObterUsuarioPorId";

                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var usuarioResult = new Dictionary<Guid, UsuarioEntity>();

                this._connection.Query<UsuarioEntity, PessoaFisicaEntity, PessoaEntity, UsuarioEntity>(
                    sql: sql,
                    map: (mapUsuario, mapPessoaFisica, mapPessoa) =>
                    {
                        if (mapUsuario.Guid.HasValue &&
                            !usuarioResult.ContainsKey(
                                mapUsuario.Guid.Value))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuarioResult.Add(
                                mapUsuario.Guid.Value,
                                mapUsuario);
                        }

                        if (mapUsuario.Guid.HasValue)
                        {
                            UsuarioEntity current = usuarioResult[mapUsuario.Guid.Value];

                            if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                            {
                                current.Colaborador = mapPessoaFisica;

                                if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                    current.Colaborador.Pessoa = mapPessoa;
                            }
                        }

                        return null;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction,
                    commandType:CommandType.StoredProcedure);

                return usuarioResult.Values.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Usuários" records.
        /// </summary>
        /// <returns>If success, the list with all "Usuários" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<UsuarioEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var usuariosResult = new Dictionary<Guid, UsuarioEntity>();

                this._connection.Query<UsuarioEntity, PessoaFisicaEntity, PessoaEntity, UsuarioEntity>(
                    sql: this._usuarioQuery.CommandTextGetAll(),
                    map: (mapUsuario, mapPessoaFisica, mapPessoa) =>
                    {
                        if (mapUsuario.Guid.HasValue &&
                            !usuariosResult.ContainsKey(
                                mapUsuario.Guid.Value))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuariosResult.Add(
                                (Guid)mapUsuario.Guid,
                                mapUsuario);
                        }

                        if (mapUsuario.Guid.HasValue)
                        {
                            UsuarioEntity current = usuariosResult[mapUsuario.Guid.Value];

                            if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                            {
                                current.Colaborador = mapPessoaFisica;

                                if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                    current.Colaborador.Pessoa = mapPessoa;
                            }
                        }

                        return null;
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return usuariosResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the Username and Password match the registration in the "Usuários" table.
        /// </summary>
        /// <param name="cpfEmailUsername">CPF, Email or Username values.</param>
        /// <returns>If success, the duly authenticated object. Otherwise, an exception is generated stating what happened.</returns>
        public IEnumerable<UsuarioEntity> GetByUsername(string cpfEmailUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(cpfEmailUsername))
                    throw new ArgumentNullException(
                        nameof(
                            cpfEmailUsername));

                string sql = "uspObterUsuarioPorCpfEmailUsername";

                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var usuariosResult = new Dictionary<Guid, UsuarioEntity>();

                this._connection.Query<UsuarioEntity, PessoaFisicaEntity, PessoaEntity, UsuarioEntity>(
                    sql: sql,
                    map: (mapUsuario, mapPessoaFisica, mapPessoa) =>
                    {
                        if (mapUsuario.Guid.HasValue &&
                            !usuariosResult.ContainsKey(
                                mapUsuario.Guid.Value))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuariosResult.Add(
                                mapUsuario.Guid.Value,
                                mapUsuario);
                        }

                        if (mapUsuario.Guid.HasValue)
                        {
                            UsuarioEntity current = usuariosResult[mapUsuario.Guid.Value];

                            if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                            {
                                current.Colaborador = mapPessoaFisica;

                                if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                    current.Colaborador.Pessoa = mapPessoa;
                            }
                        }

                        return null;
                    },
                    param: new
                    {
                        Filtro = cpfEmailUsername,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction,
                    commandType: CommandType.StoredProcedure);

                return usuariosResult.Values;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves user notification records from the database by executing the stored procedure 
        /// <c>UspObterUsuariosNotificacoes</c>. Optional parameters can be provided to filter the results.
        /// </summary>
        /// <param name="tipo">Optional filter for the notification type.</param>
        /// <param name="guidUsuario">Optional filter for the user identifier.</param>
        /// <param name="guidMatriculaDemonstrativoPagamento">Optional filter for the payment statement enrollment identifier.</param>
        /// <param name="guidEmpregador">Optional filter for the employer identifier.</param>
        /// <param name="guidColaborador">Optional filter for the employee identifier.</param>
        /// <returns>A collection of <see cref="UsuarioNotificacaoEntity"/> that match the specified criteria.</returns>
        public IEnumerable<UsuarioNotificacaoEntity> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null)
        {
            try
            {
                //if (string.IsNullOrEmpty(cpfEmailUsername))
                //    throw new ArgumentNullException(
                //        nameof(
                //            cpfEmailUsername));

                string sql = "UspObterUsuariosNotificacoes";

                return this._connection.Query<UsuarioNotificacaoEntity>(
                    sql: sql,
                    param: new
                    {
                        Tipo = tipo,
                        GuidUsuario = guidUsuario,
                        GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                        GuidEmpregador = guidEmpregador,
                        GuidColaborador = guidColaborador,
                    },
                    commandType: CommandType.StoredProcedure);
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
        /// <param name="entity"></param>
        /// <returns></returns>
        public UsuarioEntity Update(Guid guid, UsuarioEntity entity)
        {
            try
            {
                this._connection.Execute(
                    sql: this._usuarioCommand.CommandTextUpdate(),
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    entity.Guid.Value);
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
                    this._usuarioQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public IEnumerable<UsuarioEntity> GetMany(Expression<Func<UsuarioEntity, bool>> filter = null, Func<IQueryable<UsuarioEntity>, IOrderedQueryable<UsuarioEntity>> orderBy = null, int? top = null, int? skip = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<Guid, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}