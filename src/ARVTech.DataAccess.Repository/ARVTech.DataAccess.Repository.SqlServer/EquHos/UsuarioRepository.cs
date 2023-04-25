namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Common;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;

    /// <summary>
    /// Class responsible for the Repository Data Access Layer methods of the <see cref="UsuarioRepository"/> to records in the "USUARIOS" table.
    /// </summary>
    public class UsuarioRepository : Repository, IUsuarioRepository
    {
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
        }

        public UsuarioEntity Create(UsuarioEntity entity)
        {
            throw new NotImplementedException();
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
                UsuarioEntity entity = null as UsuarioEntity;

                byte[] marcaCabanhaLogado = null;
                string nomeFantasiaCabanhaLogado = string.Empty;

                string cmdText = @" UPDATE [{0}].[dbo].[USUARIOS]
                                       SET GUIDCONTA_LOGADO = @GuidContaLogado,
                                           GUIDCABANHA_LOGADO = @GuidCabanhaLogado,
                                           MARCA_CABANHA_LOGADO = @MarcaCabanhaLogado,
                                           NOME_FANTASIA_CABANHA_LOGADO = @NomeFantasiaCabanhaLogado
                                     WHERE GUID = @Guid";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@Guid",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guid != Guid.Empty ? guid : (object)DBNull.Value,
                };

                parameters[1] = new SqlParameter
                {
                    ParameterName = "@GuidContaLogado",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guidConta != Guid.Empty ? guidConta : (object)DBNull.Value,
                };

                parameters[2] = new SqlParameter
                {
                    ParameterName = "@GuidCabanhaLogado",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                };

                if (guidCabanha != Guid.Empty)
                {
                    CabanhaRepository cabanhaRepository = new CabanhaRepository(
                        this._connection,
                        this._transaction);

                    CabanhaEntity cabanhaEntity = cabanhaRepository.Get(
                        guidCabanha);

                    if (cabanhaEntity != null)
                    {
                        marcaCabanhaLogado = cabanhaEntity.Marca;
                        nomeFantasiaCabanhaLogado = cabanhaEntity.NomeFantasia;
                    }

                    parameters[2].Value = guidCabanha;
                }
                else
                {
                    parameters[2].Value = DBNull.Value;
                }

                parameters[3] = new SqlParameter
                {
                    ParameterName = "@MarcaCabanhaLogado",
                    SqlDbType = SqlDbType.VarBinary,
                    Direction = ParameterDirection.Input,
                    Value = marcaCabanhaLogado != null ? marcaCabanhaLogado : (object)DBNull.Value,
                };

                parameters[4] = new SqlParameter
                {
                    ParameterName = "@NomeFantasiaCabanhaLogado",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = !string.IsNullOrEmpty(nomeFantasiaCabanhaLogado) ? nomeFantasiaCabanhaLogado : (object)DBNull.Value,
                };

                using (SqlCommand command = this.CreateCommand(
                    cmdText))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    command.ExecuteNonQuery();

                    //using (DataTable dt = this.GetDataTableFromDataReader(command))
                    //{
                    //    // if (dt != null && dt.Rows.Count > 0)
                    //    // cabanhas = this.ConvertToList<CabanhaEntity>(dt).ToList();
                    //}
                }

                entity = this.Get(guid);

                return entity;
            }
            catch
            {
                throw;
            }

        }

        public void Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public UsuarioEntity Get(Guid guid)
        {
            try
            {
                UsuarioEntity usuarioEntity = null as UsuarioEntity;

                string cmdText = @"SELECT TOP 1 U.GUID,
                                                U.EMAIL,
                                                U.APELIDO_USUARIO,
                                                U.SENHA,
                                                U.BLOQUEADO,
                                                U.IDCATEGORIA_USUARIO,
                                                U.ENVIAR_LINK_ATIVACAO,
                                                U.DATA_EXPIRACAO_SENHA,
                                                U.INTERVALO_EXPIRACAO_SENHA,
                                                U.NOME,
                                                U.SOBRENOME,
                                                U.CPF,
                                                U.GUIDCONTA_LOGADO,
                                                U.GUIDCABANHA_LOGADO,
                                                U.MARCA_CABANHA_LOGADO,
                                                U.NOME_FANTASIA_CABANHA_LOGADO,
                                                U.GUIDCONTA,
                                                CU.DESCRICAO AS DESCRICAO_CATEGORIA_USUARIO,
                                                CU.PERFIL AS PERFIL_CATEGORIA_USUARIO
                                          FROM [{0}].[dbo].USUARIOS AS U
                                    INNER JOIN [{0}].[dbo].CATEGORIAS_USUARIOS AS CU
                                            ON U.IDCATEGORIA_USUARIO = CU.ID
                                    INNER JOIN [{0}].[dbo].CONTAS AS C
                                            ON U.GUIDCONTA = C.GUID
                                         WHERE UPPER(U.GUID) = {1}Guid";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.Add($"{this.ParameterSymbol}Guid", SqlDbType.UniqueIdentifier).Value = guid;

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        usuarioEntity = this.Popular(dt).ToList().FirstOrDefault();
                }

                return usuarioEntity;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<UsuarioEntity> GetAll()
        {
            throw new NotImplementedException();
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
                UsuarioEntity usuarioEntity = null as UsuarioEntity;

                string cmdText = @"SELECT TOP 1 U.GUID,
                                                U.EMAIL,
                                                U.APELIDO_USUARIO,
                                                U.SENHA,
                                                U.BLOQUEADO,
                                                U.IDCATEGORIA_USUARIO,
                                                U.ENVIAR_LINK_ATIVACAO,
                                                U.DATA_EXPIRACAO_SENHA,
                                                U.INTERVALO_EXPIRACAO_SENHA,
                                                U.NOME,
                                                U.SOBRENOME,
                                                U.CPF,
                                                U.GUIDCONTA_LOGADO,
                                                U.GUIDCABANHA_LOGADO,
                                                U.MARCA_CABANHA_LOGADO,
                                                U.NOME_FANTASIA_CABANHA_LOGADO,
                                                U.GUIDCONTA,
                                                CU.DESCRICAO AS DESCRICAO_CATEGORIA_USUARIO,
                                                CU.PERFIL AS PERFIL_CATEGORIA_USUARIO
                                          FROM [{0}].[dbo].USUARIOS AS U
                                    INNER JOIN [{0}].[dbo].CATEGORIAS_USUARIOS AS CU
                                            ON U.IDCATEGORIA_USUARIO = CU.ID
                                    INNER JOIN [{0}].[dbo].CONTAS AS C
                                            ON U.GUIDCONTA = C.GUID
                                         WHERE U.APELIDO_USUARIO = {1}ApelidoUsuario";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.Add($"{this.ParameterSymbol}ApelidoUsuario", SqlDbType.VarChar).Value = apelidoUsuario;

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        usuarioEntity = this.Popular(dt).ToList().FirstOrDefault();
                }

                return usuarioEntity;
            }
            catch
            {
                throw;
            }
        }

        public UsuarioEntity Update(UsuarioEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the Username and Password match the registration in the "Usuários" table.
        /// </summary>
        /// <param name="apelidousuario">"Apelido" of "Usuário" record.</param>
        /// <param name="email">"E-Mail" of "Usuário" record.</param>
        /// <param name="senha">"Senha" of "Usuário" record.</param>
        /// <returns>>If success, the duly authenticated object. Otherwise, an exception is generated stating what happened.</returns>
        public UsuarioEntity Autenticar(string apelidousuario, string email, string senha)
        {
            try
            {
                UsuarioEntity usuarioEntity = null as UsuarioEntity;

                string filtro = apelidousuario.ToLower();

                if (!string.IsNullOrEmpty(email))
                    filtro = email.ToLower();

                string cmdText = @"SELECT TOP 1 Guid,
                                                Apelido_Usuario,
                                                Email,
                                                Senha
                                     FROM [{0}].[dbo].USUARIOS
                                    WHERE (LOWER(Apelido_Usuario) = {1}Filtro
                                       OR LOWER(Email) = {1}Filtro)";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue($"{ this.ParameterSymbol }Filtro", filtro);

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        usuarioEntity = this.VerificarSenhaValida(
                            Guid.Parse(dt.Rows[0]["GUID"].ToString()),
                            senha);
                }

                return usuarioEntity;
            }
            catch
            {
                throw;
            }
        }

        //protected override int Somar()
        //{
        //    return 5;
        //}

        /// <summary>
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="UsuarioEntity"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<UsuarioEntity> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new UsuarioEntity
                   {
                       Guid = !row.IsNull("Guid") ? (Guid?)row.Field<Guid>("Guid") : null,
                       MarcaCabanhaLogado = !row.IsNull("MARCA_CABANHA_LOGADO") ? row.Field<byte[]>("MARCA_CABANHA_LOGADO") : null,
                       Bloqueado = !row.IsNull("Bloqueado") ? row.Field<bool>("Bloqueado") : false,
                       EnviarLinkAtivacao = !row.IsNull("ENVIAR_LINK_ATIVACAO") ? row.Field<bool>("ENVIAR_LINK_ATIVACAO") : false,
                       DataExpiracaoSenha = !row.IsNull("DATA_EXPIRACAO_SENHA") ? (DateTime?)row.Field<DateTime>("DATA_EXPIRACAO_SENHA") : null,
                       IntervaloExpiracaoSenha = !row.IsNull("INTERVALO_EXPIRACAO_SENHA") ? (int?)row.Field<int>("INTERVALO_EXPIRACAO_SENHA") : null,
                       ApelidoUsuario = row.Field<string>("Apelido_Usuario"),
                       CPF = row.Field<string>("CPF"),
                       Email = row.Field<string>("Email"),
                       Nome = row.Field<string>("Nome"),
                       NomeFantasiaLogado = row.Field<string>("NOME_FANTASIA_CABANHA_LOGADO"),
                       Senha = row.Field<string>("Senha"),
                       Sobrenome = row.Field<string>("Sobrenome"),
                       CategoriaUsuario = !row.IsNull("IDCATEGORIA_USUARIO") ?
                            new CategoriaUsuarioEntity()
                            {
                                Id = !row.IsNull("IDCATEGORIA_USUARIO") ? (int?)row.Field<int>("IDCATEGORIA_USUARIO") : null,
                                Descricao = row.Field<string>("DESCRICAO_CATEGORIA_USUARIO"),
                                Perfil = !row.IsNull("PERFIL_CATEGORIA_USUARIO") ? (int?)row.Field<int>("PERFIL_CATEGORIA_USUARIO") : null,
                            } :
                            null as CategoriaUsuarioEntity,
                       GuidContaLogado = !row.IsNull("GUIDCONTA_LOGADO") ? (Guid?)row.Field<Guid>("GUIDCONTA_LOGADO") : null,
                       GuidCabanhaLogado = !row.IsNull("GUIDCABANHA_LOGADO") ? (Guid?)row.Field<Guid>("GUIDCABANHA_LOGADO") : null,
                   };
        }

        /// <summary>
        /// Check if the "Senha" of "Usuário" record is valid.
        /// </summary>
        /// <param name="guid">Guid of "Usuário" record.</param>
        /// <param name="senha">"Senha" of "Usuário" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        private UsuarioEntity VerificarSenhaValida(Guid guid, string senha)
        {
            try
            {
                UsuarioEntity usuarioEntity = null as UsuarioEntity;

                string senhaQuery = PasswordCryptography.GetHashMD5(senha);

                string cmdText = @" SELECT TOP 1 Guid
                                      FROM [{0}].[dbo].USUARIOS
                                     WHERE UPPER(GUID) = {1}Guid
                                       AND SENHA = {1}Senha COLLATE SQL_Latin1_General_CP1_CS_AS";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.Add($"{this.ParameterSymbol}Guid", SqlDbType.UniqueIdentifier).Value = guid;
                cmd.Parameters.Add($"{this.ParameterSymbol}Senha", SqlDbType.VarChar).Value = senhaQuery;

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        usuarioEntity = this.Get(
                            Guid.Parse(dt.Rows[0]["GUID"].ToString()));
                }

                if (usuarioEntity == null)
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

                return usuarioEntity;
            }
            catch
            {
                throw;
            }
        }
    }
}