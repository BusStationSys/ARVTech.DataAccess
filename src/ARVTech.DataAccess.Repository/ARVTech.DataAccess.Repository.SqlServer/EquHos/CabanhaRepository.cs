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
    using Dapper;

    public class CabanhaRepository : BaseRepository, ICabanhaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CabanhaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public CabanhaRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabanhaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public CabanhaRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CabanhaEntity Create(CabanhaEntity entity)
        {
            try
            {
                string cmdText = @"INSERT INTO [{0}].[dbo].[PELAGENS]
                                               (Descricao, Observacoes)
                                        VALUES ({1}Descricao, {1}Observacoes)
                                        SELECT SCOPE_IDENTITY()";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                using (SqlCommand command = this.CreateCommand(
                    cmdText.ToString(),
                    parameters: this.GetDataParameters(
                        entity).ToArray()))
                {
                    object guid = command.ExecuteScalar();

                    return this.Get(
                        Guid.Parse(
                            guid.ToString()));
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the "Cabanha" record.
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @"DELETE FROM [{0}].[dbo].[CABANHAS] 
                                    WHERE GUID = {1}Guid";

                // model.CABANHAS.RemoveRange(model.CABANHAS.Where(q => q.GUID.ToString().ToUpper().Equals(guid.ToString().ToUpper())));

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                using (SqlCommand command = this.CreateCommand(cmdText))
                {
                    command.Parameters.AddWithValue($"{this.ParameterSymbol}Guid", guid);

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Cabanha" record.
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public CabanhaEntity Get(Guid guid)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string cmdText = @"      SELECT TOP 1 {0},
                                                      {1},
                                                      {2}
                                           FROM [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                             ON [C].[IDASSOCIACAO] = [A].[ID]
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [C].[GUIDCONTA] = [O].[GUID]
                                          WHERE UPPER(C.GUID) = @Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsCabanhas,
                    columnsAssociacoes,
                    columnsContas,
                    this._connection.Database);

                var cabanha = this._connection.Query<CabanhaEntity, AssociacaoEntity, ContaEntity, CabanhaEntity>(
                    cmdText,
                    map: (mapCabanha, mapAssociacao, mapConta) =>
                    {
                        mapCabanha.Associacao = mapAssociacao;
                        mapCabanha.Conta = mapConta;

                        return mapCabanha;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,ID,GUID",
                    transaction: this._transaction);

                return cabanha.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Cabanhas" records.
        /// </summary>
        /// <returns>If success, the list with all "Cabanhas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<CabanhaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                             ON [C].[IDASSOCIACAO] = [A].[ID]
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [C].[GUIDCONTA] = [O].[GUID]";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsCabanhas,
                    columnsAssociacoes,
                    columnsContas,
                    this._connection.Database);

                var cabanhas = this._connection.Query<CabanhaEntity, AssociacaoEntity, ContaEntity, CabanhaEntity>(
                    cmdText,
                    map: (mapCabanha, mapAssociacao, mapConta) =>
                    {
                        mapCabanha.Associacao = mapAssociacao;
                        mapCabanha.Conta = mapConta;

                        return mapCabanha;
                    },
                    splitOn: "GUID,ID,GUID",
                    transaction: this._transaction);

                return cabanhas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all "Cabanhas" records by "Conta".
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <returns>If success, the list with all "Cabanhas" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<CabanhaEntity> GetAllByGuidConta(Guid guidConta)
        {
            try
            {
                if (guidConta == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidConta));

                IEnumerable<CabanhaEntity> cabanhas = null as IEnumerable<CabanhaEntity>;

                string cmdText = $@"    SELECT C.GUID,
                                               C.CNPJ,
                                               C.RAZAO_SOCIAL,
                                               C.IDASSOCIACAO,
                                               C.GUIDCONTA,
                                               C.NOME_FANTASIA,
                                               C.MARCA,
                                               C.CEP,
                                               C.ENDERECO,
                                               C.NUMERO,
                                               C.COMPLEMENTO,
                                               C.PONTO_REFERENCIA,
                                               C.BAIRRO,
                                               C.CIDADE,
                                               C.UF,
                                               C.RESPONSAVEL,
                                               C.EMAIL,
                                               C.TELEFONE,
                                               A.DESCRICAO_REGISTRO AS DESCRICAO_REGISTRO_ASSOCIACAO,
                                               A.RAZAO_SOCIAL AS RAZAO_SOCIAL_ASSOCIACAO,
                                               A.OBSERVACOES AS OBSERVACOES_ASSOCIACAO,
                                               A.SIGLA AS SIGLA_ASSOCIACAO
                                          FROM [{this._connection.Database}].[dbo].[CABANHAS] AS C
                                    INNER JOIN [{this._connection.Database}].[dbo].[ASSOCIACOES] AS A
                                            ON C.IDASSOCIACAO = A.ID
                                         WHERE UPPER(C.GUIDCONTA) = '{guidConta.ToString().ToUpper()}'
                                      ORDER BY C.RAZAO_SOCIAL,
                                               C.GUID";

                using (SqlCommand command = this.CreateCommand(
                    cmdText))
                {
                    using (DataTable dt = this.GetDataTableFromDataReader(
                        command))
                    {
                        //if (dt != null && dt.Rows.Count > 0)
                        //    cabanhas = this.Popular(dt).ToList();
                    }
                }

                return cabanhas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all "Cabanhas" records by "Conta" and Usuário.
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the list with all "Cabanhas" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<CabanhaEntity> GetAllWithPermission(Guid guidConta, Guid guidUsuario)
        {
            try
            {
                IEnumerable<CabanhaEntity> cabanhas = null as IEnumerable<CabanhaEntity>;

                string cmdText = @"CABANHASListarByGuidContaComPermissao";

                SqlParameter[] parameters = new SqlParameter[2];

                parameters[0] = new SqlParameter
                {
                    ParameterName = "@GUIDCONTA",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = guidConta,
                };

                parameters[1] = new SqlParameter
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
                        //if (dt != null && dt.Rows.Count > 0)
                        //    cabanhas = this.ConvertToList<CabanhaEntity>(dt).ToList();
                    }
                }

                return cabanhas;
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
        public CabanhaEntity Update(CabanhaEntity entity)
        {
            try
            {
                string cmdText = @"UPDATE [{0}].[dbo].[PELAGENS]
                                      SET Descricao = {1}Descricao,
                                          Observacoes = {1}Observacoes
                                    WHERE Id = {1}Id";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                using (SqlCommand command = this.CreateCommand(
                    cmdText,
                    parameters: this.GetDataParameters(
                        entity).ToArray()))
                {
                    command.ExecuteNonQuery();
                }

                return entity;
            }
            catch
            {
                throw;
            }
        }
    }
}