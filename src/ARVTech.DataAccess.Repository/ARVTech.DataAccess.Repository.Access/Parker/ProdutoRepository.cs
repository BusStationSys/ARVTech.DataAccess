namespace ARV.DataAccess.Repository.Access.Parker
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.Linq;
    using ARV.DataAccess.Entities.Parker;
    using ARV.DataAccess.Repository.Interfaces.Parker;

    public class ProdutoRepository : Repository, IProdutoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public ProdutoRepository(OleDbConnection connection, OleDbTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProdutoEntity Get(string item)
        {
            try
            {
                ProdutoEntity produto = null as ProdutoEntity;

                string cmdText = @" SELECT [Item],
                                           [Linha de Produto],
                                           [Composto],
                                           [DI], 
                                           [W],
                                           [Preço Última Coluna],
                                           [Regra Desconto],
                                           [Regra Quantidade],
                                           [Posição Última Coluna],
                                           [Lote Mínimo],
                                           [Classificação Fiscal],
                                           [IPI],
                                           [Preço Antigo],
                                           [OG]
                                      FROM [tblItem] 
                                     WHERE [Item] = {1}Item";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue($"{this.ParameterSymbol}Item", item);

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        produto = this.Popular(dt).ToList().FirstOrDefault();

                    dt.Dispose();
                }

                return produto;
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
        public IEnumerable<ProdutoEntity> GetAll()
        {
            try
            {
                IEnumerable<ProdutoEntity> produtos = null as IEnumerable<ProdutoEntity>;

                string cmdText = @" SELECT [Item],
                                           [Linha de Produto],
                                           [Composto],
                                           [DI], 
                                           [W],
                                           [Preço Última Coluna],
                                           [Regra Desconto],
                                           [Regra Quantidade],
                                           [Posição Última Coluna],
                                           [Lote Mínimo],
                                           [Classificação Fiscal],
                                           [IPI],
                                           [Preço Antigo],
                                           [OG]
                                      FROM [tblItem]";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                var cmd = this.CreateCommand(cmdText);
                cmd.CommandText = cmdText;  // Necessário, pois ao criar o command, diferente de outros drivers, o objeto OleDb não armazena a propriedade CommandText nos métodos que o criam.

                using (DataTable dt = this.GetDataTableFromDataReader(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                        produtos = this.Popular(dt);

                    dt.Dispose();
                }

                return produtos;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="PelagemEntity"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<ProdutoEntity> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new ProdutoEntity
                   {
                       Item = row["Item"].ToString(),
                       LinhaProduto = row["Linha de Produto"].ToString(),
                       Composto = row["Composto"].ToString(),
                   };
        }
    }
}