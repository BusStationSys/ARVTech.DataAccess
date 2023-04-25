namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;

    public class RecursoRepository : Repository, IRecursoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecursoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public RecursoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public RecursoEntity Create(RecursoEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the "Recurso" record.
        /// </summary>
        /// <param name="id">ID of "Recurso" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public RecursoEntity Get(int id)
        {
            try
            {
                try
                {
                    RecursoEntity recursoEntity = null as RecursoEntity;

                    string cmdText = @"     SELECT R.ID,
                                               R.DESCRICAO,
                                               R.IDVINCULO,
                                               R.LINK,
                                               R.ORIENTACAO,
                                               R.ORDEM,
                                               R.ICONE,
                                               R.SITUACAO
                                          FROM [{0}].[dbo].RECURSOS AS R
                                         WHERE ID = {1}Id
                                      ORDER BY R.DESCRICAO,
                                               R.ID";

                    cmdText = string.Format(
                        CultureInfo.InvariantCulture,
                        cmdText,
                        this._connection.Database,
                        this.ParameterSymbol);

                    var cmd = this.CreateCommand(cmdText);
                    cmd.Parameters.Add($"{this.ParameterSymbol}Id", SqlDbType.Int).Value = id;

                    using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                    {
                        if (dt != null && dt.Rows.Count == 1)
                            recursoEntity = this.ConvertToList<RecursoEntity>(dt).ToList().FirstOrDefault();
                    }

                    return recursoEntity;
                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<RecursoEntity> GetAll()
        {
            try
            {
                IEnumerable<RecursoEntity> recursosEntities = null as IEnumerable<RecursoEntity>;

                string cmdText = @"     SELECT R.ID,
                                               R.DESCRICAO,
                                               R.IDVINCULO,
                                               R.LINK,
                                               R.ORIENTACAO,
                                               R.ORDEM,
                                               R.ICONE,
                                               R.SITUACAO
                                          FROM [{0}].[dbo].RECURSOS AS R
                                      ORDER BY R.DESCRICAO,
                                               R.ID";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null)
                        recursosEntities = this.ConvertToList<RecursoEntity>(dt).ToList();
                }

                return recursosEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all "Recursos" records in an orderly and structured manner according to the level.
        /// </summary>
        /// <returns>If success, the list with all "Recursos" records in an orderly and structured manner according to the level. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<RecursoEntity> GetAllHierarchical()
        {
            try
            {
                IEnumerable<RecursoEntity> recursosEntities = null as IEnumerable<RecursoEntity>;

                string cmdText = @"     SELECT R.ID,
                                               R.DESCRICAO,
                                               R.NIVEL,
                                               R.ORDEM,
                                               R.IDVINCULO,
                                               R.ESTRUTURADO,
                                               R.LINK,
                                               R.ORIENTACAO,
                                               R.ICONE,
                                               R.SITUACAO
                                          FROM [{0}].[dbo].[vwRecursos_CTE] AS R
                                      ORDER BY R.ESTRUTURADO,
                                               R.ID";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null)
                        recursosEntities = this.ConvertToList<RecursoEntity>(dt).ToList();
                }

                return recursosEntities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the string for render "Usuários" "Menu Principal" in HTML format for the "Usuário" logged.
        /// </summary>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public string RenderMenuPrincipalHTML(Guid guidUsuario)
        {
            try
            {
                string html = string.Empty;

                string cmdText = @"spRenderMenuPrincipalHTML";

                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@GUIDUSUARIO",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guidUsuario,
                };

                using (var cmd = this.CreateCommand(
                    cmdText,
                    commandType: CommandType.StoredProcedure))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                    {
                        if (dt != null && dt.Rows.Count == 1)
                            html = dt.Rows[0].Field<string>("Html");
                    }
                }

                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the string for render "Usuários" Menu in HTML format for the "Usuário" logged.
        /// </summary>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public string RenderMenuUsuarioHTML(Guid guidUsuario)
        {
            try
            {
                string html = string.Empty;

                string cmdText = @"spRenderMenuUsuarioHTML";

                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@GUIDUSUARIO",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guidUsuario,
                };

                using (SqlCommand command = this.CreateCommand(
                    cmdText,
                    commandType: CommandType.StoredProcedure))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (DataTable dt = this.GetDataTableFromDataReader(command))
                    {
                        if (dt != null && dt.Rows.Count == 1)
                            html = dt.Rows[0].Field<string>("Html");
                    }
                }

                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the string for render "Configurações" Menu in HTML format for the "Usuário" logged.
        /// </summary>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public string RenderMenuConfiguracoesHTML(Guid guidUsuario)
        {
            try
            {
                string html = string.Empty;

                string cmdText = @"spRenderMenuConfiguracoesHTML";

                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@GUIDUSUARIO",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guidUsuario,
                };

                using (SqlCommand command = this.CreateCommand(
                    cmdText,
                    commandType: CommandType.StoredProcedure))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (DataTable dt = this.GetDataTableFromDataReader(command))
                    {
                        if (dt != null && dt.Rows.Count == 1)
                            html = dt.Rows[0].Field<string>("Html");
                    }
                }

                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RecursoEntity Update(RecursoEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change the value of the "Situação" field, that is, if it is "A" ("Ativo") value, change it to "I" ("Inativo") value and vice versa.
        /// </summary>
        /// <param name="id">ID of "Recurso" record.</param>
        public RecursoEntity ExecutarStatus(int id)
        {
            try
            {
                RecursoEntity entity = this.Get(id);

                if (entity != null)
                {
                    string valueSituacao = "A";

                    if (entity.Situacao == "A")
                        valueSituacao = "I";

                    this.AlterarSituacao(id, valueSituacao);

                    entity = this.Get(id);
                }

                return entity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method responsible by change the "Situação" field.
        /// </summary>
        /// <param name="id">ID of "Recurso" record.</param>
        /// <param name="valueSituacao">Value to be modified.</param>
        private void AlterarSituacao(int id, string valueSituacao)
        {
            try
            {
                string cmdText = @"RECURSOSAtualizarSituacao";

                SqlParameter[] parameters = new SqlParameter[2];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@IDRecurso",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                };

                parameters[1] = new SqlParameter
                {
                    ParameterName = "@Valor",
                    Direction = ParameterDirection.Input,
                    Size = 1,
                    SqlDbType = SqlDbType.VarChar,
                    Value = valueSituacao,
                };

                using (var cmd = this.CreateCommand(
                    cmdText,
                    commandType: CommandType.StoredProcedure))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}