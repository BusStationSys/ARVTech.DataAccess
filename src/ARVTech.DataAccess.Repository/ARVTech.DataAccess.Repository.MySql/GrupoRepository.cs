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

    public class GrupoRepository : Repository, IGrupoRepository
    {
        public GrupoRepository(MySqlConnection connection, MySqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Grupo Create(Grupo model)
        {
            try
            {
                string cmdText = @"INSERT INTO [{0}].[dbo].[Pessoas]
                                               (Nome)
                                        VALUES ({1}Nome)
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
                string cmdText = @"DELETE FROM {0}.TbPerfisUsuarios
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
        public Grupo Get(int id)
        {
            try
            {
                Grupo grupoUsuario = null as Grupo;

                string cmdText = @"SELECT Id,
                                          Nome
                                     FROM {0}.TbPerfisUsuarios
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
                        grupoUsuario = this.Popular(dt).ToList().FirstOrDefault();

                    dt.Dispose();
                }

                return grupoUsuario;
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
        public IEnumerable<Grupo> GetAll()
        {
            try
            {
                IEnumerable<Grupo> gruposUsuarios = null as IEnumerable<Grupo>;

                string cmdText = @"SELECT Id,
                                          Title
                                     FROM {0}.TbGruposUsuarios";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                        gruposUsuarios = this.Popular(dt);

                    dt.Dispose();
                }

                return gruposUsuarios;
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
        public Grupo Update(Grupo model)
        {
            try
            {
                string cmdText = @"UPDATE [{0}].[dbo].[Pessoas]
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
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="Grupo"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<Grupo> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new Grupo
                   {
                       Id = Convert.ToInt32(row["Id"].ToString()),
                       Title = row["Title"].ToString(),
                   };
        }
    }
}