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
    using ARVTech.Shared;
    using Dapper;

    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

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
                    PessoaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

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
                var guid = this._connection.QuerySingle<Guid>(
                    sql: this._usuarioQuery.CommandTextCreate(),
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
        /// Check if the "Password" of "Usuário" record is valid.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="password">"Password" of "Usuário" record.</param>
        /// <returns>If success, the Entity with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity CheckPasswordValid(Guid guid, string password)
        {
            try
            {
                string passwordQuery = PasswordCryptography.GetHashMD5(
                    password);

                var usuarioEntity = this._connection.QueryFirstOrDefault(
                    sql: this._usuarioQuery.CommandTextCheckPasswordValid(),
                    param: new
                    {
                        Guid = guid,
                        PasswordQuery = passwordQuery,
                    });

                if (usuarioEntity != null)
                    return this.Get(
                        usuarioEntity.Guid);

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
                    this._usuarioQuery.CommandTextDelete(),
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

                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var usuarioResult = new Dictionary<Guid, UsuarioEntity>();

                this._connection.Query<UsuarioEntity, PessoaFisicaEntity, PessoaEntity, UsuarioEntity>(
                    sql: this._usuarioQuery.CommandTextGetById(),
                    map: (mapUsuario, mapPessoaFisica, mapPessoa) =>
                    {
                        if (!usuarioResult.ContainsKey(mapUsuario.Guid))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuarioResult.Add(
                                mapUsuario.Guid,
                                mapUsuario);
                        }

                        UsuarioEntity current = usuarioResult[mapUsuario.Guid];

                        if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                        {
                            current.Colaborador = mapPessoaFisica;

                            if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                current.Colaborador.Pessoa = mapPessoa;
                        }

                        //if (mapUsuarioCabanha != null && !current.UsuariosCabanhas.Contains(mapUsuarioCabanha))
                        //{
                        //    mapUsuarioCabanha.Usuario = current;
                        //    // mapCabanha.Conta = mapConta;
                        //    // mapCabanha.Associacao = mapAssociacao;

                        //    current.UsuariosCabanhas.Add(
                        //        mapUsuarioCabanha);
                        //}

                        return null;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

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
                        if (!usuariosResult.ContainsKey(mapUsuario.Guid))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuariosResult.Add(
                                mapUsuario.Guid,
                                mapUsuario);
                        }

                        UsuarioEntity current = usuariosResult[mapUsuario.Guid];

                        if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                        {
                            current.Colaborador = mapPessoaFisica;

                            if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                current.Colaborador.Pessoa = mapPessoa;
                        }

                        //if (mapUsuarioCabanha != null && !current.UsuariosCabanhas.Contains(mapUsuarioCabanha))
                        //{
                        //    mapUsuarioCabanha.Usuario = current;
                        //    // mapCabanha.Conta = mapConta;
                        //    // mapCabanha.Associacao = mapAssociacao;

                        //    current.UsuariosCabanhas.Add(
                        //        mapUsuarioCabanha);
                        //}

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

                //  Maneira utilizada para trazer os relacionamentos 0:N.
                var usuariosResult = new Dictionary<Guid, UsuarioEntity>();

                this._connection.Query<UsuarioEntity, PessoaFisicaEntity, PessoaEntity, UsuarioEntity>(
                    sql: this._usuarioQuery.CommandTextGetByUsername(),
                    map: (mapUsuario, mapPessoaFisica, mapPessoa) =>
                    {
                        if (!usuariosResult.ContainsKey(mapUsuario.Guid))
                        {
                            //mapUsuario.Colaborador = mapPessoaFisica;
                            //mapUsuario.Colaborador.Pessoa = mapPessoa;

                            usuariosResult.Add(
                                mapUsuario.Guid,
                                mapUsuario);
                        }

                        UsuarioEntity current = usuariosResult[mapUsuario.Guid];

                        if (mapPessoaFisica != null && current.Colaborador != mapPessoaFisica)
                        {
                            current.Colaborador = mapPessoaFisica;

                            if (mapPessoa != null && current.Colaborador.Pessoa != mapPessoa)
                                current.Colaborador.Pessoa = mapPessoa;
                        }

                        return null;
                    },
                    param: new
                    {
                        Filtro = cpfEmailUsername,
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
                    sql: this._usuarioQuery.CommandTextUpdate(),
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
                    this._usuarioQuery.Dispose();
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}