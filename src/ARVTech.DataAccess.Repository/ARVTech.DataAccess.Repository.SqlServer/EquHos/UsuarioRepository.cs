namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Common;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using Dapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsuarioRepository"/> class.
    /// </summary>
    /// <param name="connection"></param>
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        private readonly string _columnsCategoriasUsuarios;
        private readonly string _columnsContas;
        private readonly string _columnsUsuarios;
        private readonly string _columnsUsuariosCabanhas;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuarioRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public UsuarioRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    UsuarioEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                    CategoriaUsuarioEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));

            this._columnsCategoriasUsuarios = base.GetAllColumnsFromTable(
                "CATEGORIAS_USUARIOS",
                "CU");

            this._columnsContas = base.GetAllColumnsFromTable(
                "CONTAS",
                "O");

            this._columnsUsuarios = base.GetAllColumnsFromTable(
                "USUARIOS",
                "U");

            this._columnsUsuariosCabanhas = base.GetAllColumnsFromTable(
                "USUARIOS_CABANHAS",
                "UC");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuarioRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public UsuarioRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    UsuarioEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                    CategoriaUsuarioEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));

            this._columnsCategoriasUsuarios = base.GetAllColumnsFromTable(
                "CATEGORIAS_USUARIOS",
                "CU");

            this._columnsContas = base.GetAllColumnsFromTable(
                "CONTAS",
                "O");

            this._columnsUsuarios = base.GetAllColumnsFromTable(
                "USUARIOS",
                "U");

            this._columnsUsuariosCabanhas = base.GetAllColumnsFromTable(
                "USUARIOS_CABANHAS",
                "UC");
        }

        /// <summary>
        /// Checks if the Username and Password match the registration in the "Usuários" table.
        /// </summary>
        /// <param name="apelidousuario">"Apelido" of "Usuário" record.</param>
        /// <param name="email">"E-Mail" of "Usuário" record.</param>
        /// <param name="senha">"Senha" of "Usuário" record.</param>
        /// <returns>>If success, the duly authenticated Entity. Otherwise, an exception is generated stating what happened.</returns>
        public UsuarioEntity Autenticar(string apelidousuario, string email, string senha)
        {
            try
            {
                string filtro = apelidousuario.ToLower();

                if (!string.IsNullOrEmpty(email))
                    filtro = email.ToLower();

                string cmdText = @" SELECT TOP 1 Guid,
                                                 Apelido_Usuario,
                                                 Email,
                                                 Senha
                                      FROM [{0}].[dbo].USUARIOS
                                     WHERE ( LOWER(Apelido_Usuario) = {1}Filtro
                                             OR LOWER(Email) = {1}Filtro ) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var usuario = base._connection.QueryFirstOrDefault(
                    cmdText,
                    param: new
                    {
                        Filtro = filtro,
                    },
                    transaction: this._transaction);

                if (usuario != null)
                {
                    return this.VerificarSenhaValida(
                            ((UsuarioEntity)usuario).Guid,
                            senha);
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the "Usuário" record.
        /// </summary>
        /// <param name="entity">Entity with the fields.</param>
        /// <returns>If success, the Entity with the persistent database record. Otherwise, the exception.</returns>
        public UsuarioEntity Create(UsuarioEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the "Usuário" record.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        public void Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the record exists by "Apelido" of "Usuário".
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="apelidoUsuario">"Apelido" of "Usuário" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteApelidoUsuarioDuplicado(Guid guid, string apelidoUsuario)
        {
            try
            {
                var cmdText = new StringBuilder();

                cmdText.Append("      SELECT U.GUID ");

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "        FROM [{0}].[dbo].[USUARIOS] as U WITH(NOLOCK) ",
                    base._connection.Database);

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "       WHERE UPPER(U.[APELIDO_USUARIO]) = {0}ApelidoUsuario ",
                    base.ParameterSymbol);

                if (guid != Guid.Empty)
                {
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND UPPER(U.GUID) != {0}Guid ",
                        base.ParameterSymbol);
                }

                var usuario = base._connection.QueryFirstOrDefault(
                    cmdText.ToString(),
                    param: new
                    {
                        ApelidoUsuario = apelidoUsuario.ToUpper(),
                        Guid = guid,
                    },
                    transaction: this._transaction);

                return usuario != null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the record exists by "CPF" of "Usuário".
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="cpf">"CPF" of "Usuário" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteCPFDuplicado(Guid guid, string cpf)
        {
            try
            {
                var cmdText = new StringBuilder();

                cmdText.Append("      SELECT U.GUID ");

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "        FROM [{0}].[dbo].[USUARIOS] as U WITH(NOLOCK) ",
                    base._connection.Database);

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "       WHERE U.[CPF] = {0}Cpf ",
                    base.ParameterSymbol);

                if (guid != Guid.Empty)
                {
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND U.[GUID] != {0}Guid ",
                        base.ParameterSymbol);
                }

                var usuario = base._connection.QueryFirstOrDefault(
                    cmdText.ToString(),
                    param: new
                    {
                        Cpf = cpf,
                        Guid = guid,
                    },
                    transaction: this._transaction);

                return usuario != null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the record exists by "E-Mail" of "Usuário".
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="email">"E-Mail" of "Usuário" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteEmailDuplicado(Guid guid, string email)
        {
            try
            {
                var cmdText = new StringBuilder();

                cmdText.Append("      SELECT U.GUID ");

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "        FROM [{0}].[dbo].[USUARIOS] as U WITH(NOLOCK) ",
                    base._connection.Database);

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "       WHERE UPPER(U.[EMAIL]) = {0}Email ",
                    base.ParameterSymbol);

                if (guid != Guid.Empty)
                {
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND U.[GUID] != {0}Guid ",
                        base.ParameterSymbol);
                }

                var usuario = base._connection.QueryFirstOrDefault(
                    cmdText.ToString(),
                    param: new
                    {
                        Email = email.ToLower(),
                        Guid = guid,
                    },
                    transaction: this._transaction);

                return usuario != null;
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
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT TOP 1 {0},
                                                      {1},
                                                      {2}
                                           FROM [{3}].[dbo].[USUARIOS] as U WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].CATEGORIAS_USUARIOS AS CU
                                             ON U.[IDCATEGORIA_USUARIO] = CU.[ID]
                                     INNER JOIN [{3}].[dbo].CONTAS AS O
                                             ON U.[GUIDCONTA] = O.[GUID]
                                          WHERE U.[GUID] = {4}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsUsuarios,
                    this._columnsContas,
                    this._columnsCategoriasUsuarios,
                    base._connection.Database,
                    base.ParameterSymbol);

                var usuario = base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioEntity>(
                    cmdText,
                    map: (mapUsuario, mapConta, mapCategoriaUsuario) =>
                    {
                        mapUsuario.Conta = mapConta;
                        mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                        return mapUsuario;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return usuario.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Usuários" records".
        /// </summary>
        /// <returns>If success, the list with all "Usuários" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<UsuarioEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 0:N.
                Dictionary<Guid, UsuarioEntity> usuariosResult = new Dictionary<Guid, UsuarioEntity>();

                string cmdText = @"          SELECT {0},
                                                    {1},
                                                    {2},
                                                    {3}
                                               FROM [{4}].[dbo].[USUARIOS] as U WITH(NOLOCK)
                                         INNER JOIN [{4}].[dbo].CATEGORIAS_USUARIOS AS CU
                                                 ON U.IDCATEGORIA_USUARIO = CU.ID
                                         INNER JOIN [{4}].[dbo].CONTAS AS O
                                                 ON U.GUIDCONTA = O.GUID
                                    LEFT OUTER JOIN [{4}].[dbo].[USUARIOS_CABANHAS] AS UC WITH(NOLOCK)
                                                 ON U.GUID = UC.GUIDUSUARIO ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsUsuarios,
                    this._columnsContas,
                    this._columnsCategoriasUsuarios,
                    this._columnsUsuariosCabanhas,
                    base._connection.Database);

                //var usuarios = base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioEntity>(
                //    cmdText,
                //    map: (mapUsuario, mapConta, mapCategoriaUsuario) =>
                //    {
                //        mapUsuario.Conta = mapConta;
                //        mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                //        return mapUsuario;
                //    },
                //    splitOn: "GUID,GUID,ID",
                //    transaction: this._transaction);

                //return usuarios;

                base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioCabanhaEntity, UsuarioEntity>(
                    cmdText,
                    map: (mapUsuario, mapConta, mapCategoriaUsuario, mapUsuarioCabanha) =>
                    {
                        if (!usuariosResult.ContainsKey(mapUsuario.Guid))
                        {
                            mapUsuario.Conta = mapConta;
                            mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                            mapUsuario.UsuariosCabanhas = new List<UsuarioCabanhaEntity>();

                            usuariosResult.Add(
                                mapUsuario.Guid,
                                mapUsuario);
                        }

                        UsuarioEntity current = usuariosResult[mapUsuario.Guid];

                        if (mapUsuarioCabanha != null && !current.UsuariosCabanhas.Contains(mapUsuarioCabanha))
                        {
                            mapUsuarioCabanha.Usuario = current;
                            // mapCabanha.Conta = mapConta;
                            // mapCabanha.Associacao = mapAssociacao;

                            current.UsuariosCabanhas.Add(
                                mapUsuarioCabanha);
                        }

                        return null;
                    },
                    splitOn: "GUID,GUID,ID,GUID",
                    transaction: this._transaction);

                return usuariosResult.Values;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Usuários" records by "Conta" and "Perfil".
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <param name="perfil">"Perfil" of "Usuário".</param>
        /// <returns>If success, the list with all "Usuários" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<UsuarioEntity> GetAll(Guid guidConta, int perfil)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                var cmdText = new StringBuilder();

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"      SELECT {0},
                                   {1},
                                   {2}
                              FROM [{3}].[dbo].[USUARIOS] as U WITH(NOLOCK)
                        INNER JOIN [{3}].[dbo].CATEGORIAS_USUARIOS AS CU
                                ON U.[IDCATEGORIA_USUARIO] = CU.[ID]
                        INNER JOIN [{0}].[dbo].CONTAS AS O
                                ON U.[GUIDCONTA] = O.[GUID] ",
                    this._columnsUsuarios,
                    this._columnsContas,
                    this._columnsCategoriasUsuarios,
                    base._connection.Database);

                // Se o Perfil Logado não for MASTER, vai mostrar apenas os Usuário vinculados à Conta Logada e com o Perfil diferente de MASTER. Perfil MASTER visualiza todos os registros da Conta.
                if (perfil >= 2)
                {
                    // Gerentes enxergarão tudo relacionado à Conta.
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND UPPER(U.[GUIDCONTA]) = {0}GuidConta ",
                        base.ParameterSymbol);

                    // Administradores enxergarão outros veterinários e usuários obedecendo os níveis de perfis.
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND CU.PERFIL >= {0}Perfil ",
                        base.ParameterSymbol);
                }

                var usuarios = base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioEntity>(
                    cmdText.ToString(),
                    map: (mapUsuario, mapConta, mapCategoriaUsuario) =>
                    {
                        mapUsuario.Conta = mapConta;
                        mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                        return mapUsuario;
                    },
                    param: new
                    {
                        GuidConta = guidConta,
                        Perfil = perfil,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return usuarios;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Usuário" record by "Apelido".
        /// </summary>
        /// <param name="apelidoUsuario">"Apelido" of "Usuário" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity GetByApelidoUsuario(string apelidoUsuario)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT TOP 1 {0},
                                                      {1},
                                                      {2}
                                           FROM [{3}].[dbo].[USUARIOS] as U WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[CATEGORIAS_USUARIOS] AS CU
                                             ON U.[IDCATEGORIA_USUARIO] = CU.ID
                                     INNER JOIN [{3}].[dbo].CONTAS AS O
                                             ON U.[GUIDCONTA] = O.[GUID]
                                          WHERE U.[APELIDO_USUARIO] = {4}ApelidoUsuario ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsUsuarios,
                    this._columnsContas,
                    this._columnsCategoriasUsuarios,
                    base._connection.Database,
                    base.ParameterSymbol);

                var usuario = base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioEntity>(
                    cmdText,
                    map: (mapUsuario, mapConta, mapCategoriaUsuario) =>
                    {
                        mapUsuario.Conta = mapConta;
                        mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                        return mapUsuario;
                    },
                    param: new
                    {
                        ApelidoUsuario = apelidoUsuario,
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return usuario.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Usuário" record by "E-Mail".
        /// </summary>
        /// <param name="email">"E-Mail" of "Usuário" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity GetByEmail(string email)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT TOP 1 {0},
                                                      {1},
                                                      {2}
                                           FROM [{3}].[dbo].[USUARIOS] as U WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[CATEGORIAS_USUARIOS] AS CU
                                             ON U.[IDCATEGORIA_USUARIO] = CU.ID
                                     INNER JOIN [{3}].[dbo].CONTAS AS O
                                             ON U.[GUIDCONTA] = O.[GUID]
                                          WHERE LOWER(U.[EMAIL]) = {4}Email ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsUsuarios,
                    this._columnsContas,
                    this._columnsCategoriasUsuarios,
                    base._connection.Database,
                    base.ParameterSymbol);

                var usuario = base._connection.Query<UsuarioEntity, ContaEntity, CategoriaUsuarioEntity, UsuarioEntity>(
                    cmdText,
                    map: (mapUsuario, mapConta, mapCategoriaUsuario) =>
                    {
                        mapUsuario.Conta = mapConta;
                        mapUsuario.CategoriaUsuario = mapCategoriaUsuario;

                        return mapUsuario;
                    },
                    param: new
                    {
                        Email = email.ToLower(),
                    },
                    splitOn: "GUID,GUID,ID",
                    transaction: this._transaction);

                return usuario.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Change the "Conta" and "Cabanha" that "Usuário" is logged.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="guidConta">Guid of "Conta" record.</param>
        /// <param name="guidCabanha">Guid of "Cabanha" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public UsuarioEntity TrocarContaECabanhaLogados(Guid guid, Guid guidConta, Guid guidCabanha)
        {
            try
            {
                Guid? guidCabanhaLogado = null;
                byte[] marcaCabanhaLogado = null;
                string nomeFantasiaCabanhaLogado = string.Empty;

                string cmdText = @" UPDATE [{0}].[dbo].[USUARIOS]
                                       SET GUIDCONTA_LOGADO = {1}GuidContaLogado,
                                           GUIDCABANHA_LOGADO = {1}GuidCabanhaLogado,
                                           MARCA_CABANHA_LOGADO = {1}MarcaCabanhaLogado,
                                           NOME_FANTASIA_CABANHA_LOGADO = {1}NomeFantasiaCabanhaLogado
                                     WHERE GUID = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                if (guidCabanha != Guid.Empty)
                {
                    CabanhaRepository cabanhaRepository = new CabanhaRepository(
                        this._connection,
                        this._transaction);

                    CabanhaEntity cabanhaEntity = cabanhaRepository.Get(
                        guidCabanha);

                    if (cabanhaEntity != null)
                    {
                        guidCabanhaLogado = cabanhaEntity.Guid;
                        marcaCabanhaLogado = cabanhaEntity.Marca;
                        nomeFantasiaCabanhaLogado = cabanhaEntity.NomeFantasia;
                    }
                }

                base._connection.Execute(
                    cmdText,
                    param: new
                    {
                        Guid = guid != Guid.Empty ? guid : (object)DBNull.Value,
                        GuidContaLogado = guidConta != Guid.Empty ? guidConta : (object)DBNull.Value,
                        GuidCabanhaLogado = guidCabanhaLogado != null ? guidCabanhaLogado : (object)DBNull.Value,
                        MarcaCabanhaLogado = marcaCabanhaLogado ?? (object)DBNull.Value,
                        NomeFantasiaLogado = !string.IsNullOrEmpty(nomeFantasiaCabanhaLogado) ? nomeFantasiaCabanhaLogado : (object)DBNull.Value,
                    },
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
        /// Updates the "Usuário" record.
        /// </summary>
        /// <param name="entity">Entity with the fields.</param>
        /// <returns>If success, the Entity with the persistent database record. Otherwise, the exception.</returns>
        public UsuarioEntity Update(UsuarioEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the "Senha" of "Usuário" record is valid.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="senha">"Senha" of "Usuário" record.</param>
        /// <returns>If success, the Entity with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        private UsuarioEntity VerificarSenhaValida(Guid guid, string senha)
        {
            try
            {
                string senhaQuery = PasswordCryptography.GetHashMD5(senha);

                string cmdText = @" SELECT TOP 1 Guid
                                      FROM [{0}].[dbo].USUARIOS
                                     WHERE GUID = {1}Guid
                                       AND SENHA = {1}SenhaQuery COLLATE SQL_Latin1_General_CP1_CS_AS ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);


                var usuario = base._connection.QueryFirstOrDefault(
                    cmdText,
                    param: new
                    {
                        Guid = guid,
                        SenhaQuery = senhaQuery,
                    },
                    transaction: this._transaction);

                if (usuario != null)
                {
                    return this.Get(
                        ((UsuarioEntity)usuario).Guid);
                }
                else
                {
                    //EquHosException equHosException = new EquHosException(
                    //    title: "Autenticação/Validação",
                    //    text: "Login/Email e Senha não conferem, verifique.",
                    //    method: "DAUsuarios.SenhaValida()",
                    //    bsAlert: BsAlertsEnum.Warning);

                    //throw new Exception(
                    //    JsonMethods.Serializar(
                    //        equHosException));
                    throw new Exception("Login/Email e Senha não conferem, verifique.");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}