namespace FlooString.Repository.PostgreSql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using FlooString.Models;
    using FlooString.Repository.Common;
    using FlooString.Repository.Interfaces;
    using FlooString.UnitOfWork.PostgreSql;
    using Npgsql;

    public class PessoaRepository : Repository, IPessoaRepository
    {
        public PessoaRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Pessoa Create(Pessoa model)
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
                string cmdText = @"DELETE FROM [{0}].[dbo].[Pessoas]
                                    WHERE Id = @Id";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue("@Id", id);

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
        public Pessoa Get(int id)
        {
            try
            {
                Pessoa pessoa = null as Pessoa;

                string cmdText = @"SELECT Id,
                                          Nome
                                     FROM [{0}].[dbo].[Pessoas]
                                    WHERE Id = {1}";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    id);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue("@Id", id);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        pessoa = this.Popular(dt).ToList().FirstOrDefault();

                    dt.Dispose();
                }

                return pessoa;
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
        public IEnumerable<Pessoa> GetAll()
        {
            try
            {
                IEnumerable<Pessoa> pessoas = null as IEnumerable<Pessoa>;

                string cmdText = @"SELECT Id,
                                          Nome
                                     FROM [{0}].[dbo].[Pessoas]";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                        pessoas = this.Popular(dt);

                    dt.Dispose();
                }

                return pessoas;
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
        public Pessoa Update(Pessoa model)
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
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="Pessoa"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<Pessoa> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new Pessoa
                   {
                       Id = Convert.ToInt32(row["Id"].ToString()),
                       Nome = row["Nome"].ToString(),
                   };
        }
    }
}