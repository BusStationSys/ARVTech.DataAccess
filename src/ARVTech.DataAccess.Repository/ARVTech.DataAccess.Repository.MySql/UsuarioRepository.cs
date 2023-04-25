namespace FlooString.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using FlooString.Models;
    using FlooString.Repository.Common;
    using FlooString.Repository.Interfaces;
    using MySqlConnector;

    public class UsuarioRepository : Repository, IUsuarioRepository
    {
        public UsuarioRepository(MySqlConnection connection, MySqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Usuario Create(Usuario model)
        {
            try
            {
                string cmdText = @"INSERT INTO {0}.TbUsuarios
                                               (Nome, Celular, Email, Login, Senha, Sobre, Perfil)
                                        VALUES ({1}Nome,
                                                {1}Celular,
                                                {1}Email,
                                                {1}Login,
                                                {1}Senha,
                                                {1}Sobre,
                                                {1}Perfil)
                                         SELECT SCOPE_IDENTITY()";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(
                    cmdText.ToString(),
                    parameters: this.GetDataParameters(
                        model).ToArray());

                int id = Convert.ToInt32(
                    cmd.ExecuteScalar());

                return this.Get(
                    Convert.ToInt32(
                        id));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                string cmdText = @"DELETE FROM {0}.TbUsuarios
                                    WHERE Id = {1}Id";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue($"{this.ParameterSymbol}Id", id);

                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario Get(int id)
        {
            try
            {
                Usuario usuario = null as Usuario;

                string cmdText = @"SELECT Id,
                                          Nome,
                                          Celular,
                                          Email,
                                          Login,
                                          Senha,
                                          DataInclusao,
                                          DataAlteracao,
                                          DataUltimoLogin,
                                          Sobre,
                                          Perfil
                                     FROM {0}.TbUsuarios
                                    WHERE Id = {1}Id";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue($"{this.ParameterSymbol}Id", id);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        usuario = this.Popular(dt).ToList().FirstOrDefault();

                    dt.Dispose();
                }

                return usuario;
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
        public IEnumerable<Usuario> GetAll()
        {
            try
            {
                IEnumerable<Usuario> usuarios = null as IEnumerable<Usuario>;

                string cmdText = @"SELECT Id,
                                          Nome,
                                          Celular,
                                          Email,
                                          Login,
                                          Senha,
                                          DataInclusao,
                                          DataAlteracao,
                                          DataUltimoLogin,
                                          Sobre,
                                          Perfil
                                     FROM {0}.TbUsuarios";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                        usuarios = this.Popular(dt);

                    dt.Dispose();
                }

                return usuarios;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Usuario Update(Usuario model)
        {
            try
            {
                string cmdText = @"UPDATE {0}.TbUsuarios
                                      SET Nome = {1}Nome
                                    WHERE Id = {1}Id";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(
                    cmdText,
                    parameters: this.GetDataParameters(
                        model).ToArray());

                cmd.ExecuteNonQuery();

                return model;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="Usuario"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<Usuario> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new Usuario
                   {
                       Id = Convert.ToInt32(row["Id"].ToString()),
                       Nome = row["Nome"].ToString(),
                       Celular = row["Celular"].ToString(),
                       Email = row["Celular"].ToString(),
                       Senha = row["Senha"].ToString(),
                       DataInclusao = (DateTimeOffset?)(row["DataInclusao"] ?? null),
                       DataAlteracao = (DateTimeOffset?)(row["DataAlteracao"] ?? null),
                       DataUltimoLogin = (DateTimeOffset?)(row["DataUltimoLogin"] ?? null),
                       Login = row["Senha"].ToString(),
                       Sobre = row["Sobre"].ToString(),
                       Perfil = row["Perfil"].ToString(),
                   };
        }
    }
}