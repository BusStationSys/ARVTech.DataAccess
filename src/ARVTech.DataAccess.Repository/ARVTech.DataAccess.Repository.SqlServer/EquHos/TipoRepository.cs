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

    public class TipoRepository : Repository, ITipoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TipoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public TipoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public TipoEntity Create(TipoEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TipoEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoEntity> GetAll(Guid guidConta, Guid guidCabanha)
        {
            try
            {
                IEnumerable<TipoEntity> tiposEntities = null as IEnumerable<TipoEntity>;

                string cmdText = @"     SELECT T.ID,
                                               T.DESCRICAO,
                                               T.SEXO,
                                               T.OBSERVACOES,
                                               T.ORDEM,
                                               T.COR,
                                               T.ICONE,
                                               T.EXIBIR_QUADRO_ANIMAIS,
                                               COALESCE((SELECT COUNT(A.GUID)
                                                           FROM ANIMAIS A
                                                          WHERE A.IDTIPO = T.ID
                                                            AND A.GUIDCONTA = {1}GuidConta
                                                            AND A.GUIDCABANHA = {1}GuidCabanha), 0) AS _QUANTIDADE_ANIMAIS
                                          FROM [{0}].[dbo].TIPOS AS T
                                      ORDER BY T.ORDEM,
                                               T.DESCRICAO,
                                               T.ID";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.Add($"{this.ParameterSymbol}GuidConta", SqlDbType.UniqueIdentifier).Value = guidConta;
                cmd.Parameters.Add($"{this.ParameterSymbol}GuidCabanha", SqlDbType.UniqueIdentifier).Value = guidCabanha;

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null)
                        tiposEntities = this.ConvertToList<TipoEntity>(dt).ToList();
                }

                return tiposEntities;
            }
            catch
            {
                throw;
            }
        }

        public TipoEntity Update(TipoEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
