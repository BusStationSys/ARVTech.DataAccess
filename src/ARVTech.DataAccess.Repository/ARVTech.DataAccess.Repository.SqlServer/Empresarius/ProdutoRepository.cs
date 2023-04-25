namespace ARVTech.DataAccess.Repository.SqlServer.Empresarius
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.Empresarius;
    using ARVTech.DataAccess.Repository.Interfaces.Empresarius;

    public class ProdutoRepository : Repository, IProdutoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public ProdutoRepository(SqlConnection connection, SqlTransaction transaction)
            : base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public ProdutoEntity Create(ProdutoEntity entity)
        {
            try
            {
                string cmdText = @"INSERT INTO [{0}].[dbo].[PRODUTOS]
                                               (IDEMPRESA, DESCRICAO, PRECO_VENDA, ESTOQUE_MINIMO,
                                                PRECO_CUSTO, CUSTO_MEDIO, MARGEM_LUCRO, OBSERVACAO)
                                        VALUES ({1}IdEmpresa, {1}Descricao, {1}Preco_Venda, {1}Estoque_Minimo,
                                                {1}Preco_Custo, {1}Custo_Medio, {1}Margem_Lucro, {1}Observacao)
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
                    int id = Convert.ToInt32(
                        command.ExecuteScalar());

                    return this.Get(
                        Convert.ToInt32(
                            id));
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete(decimal id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the "Produto" record.
        /// </summary>
        /// <param name="id">ID of "Produto" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public ProdutoEntity Get(decimal id)
        {
            try
            {
                ProdutoEntity produto = null as ProdutoEntity;

                string cmdText = @"    SELECT TOP 1 P.IDPRODUTO,
                                                    P.DESCRICAO,
                                                    P.OBSERVACAO
                                           FROM [{0}].[dbo].PRODUTOS AS P
                                          WHERE IDPRODUTO = {1}IdProduto";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    this.ParameterSymbol);

                using (SqlCommand command = this.CreateCommand(
                    cmdText))
                {
                    command.Parameters.Add($"{this.ParameterSymbol}IdProduto", SqlDbType.Decimal).Value = id;

                    using (DataTable dt = this.GetDataTableFromDataReader(
                        command))
                    {
                        if (dt != null && dt.Rows.Count > 0)
                            produto = this.ConvertToList<ProdutoEntity>(dt).ToList().First();
                    }
                }

                return produto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Produto" records.
        /// </summary>
        /// <returns>If success, the list with all "Associações" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<ProdutoEntity> GetAll()
        {
            try
            {
                IEnumerable<ProdutoEntity> produtos = null as IEnumerable<ProdutoEntity>;

                string cmdText = @"    SELECT A.ID,
                                              A.RAZAO_SOCIAL,
                                              A.SIGLA,
                                              A.OBSERVACOES,
                                              A.DESCRICAO_REGISTRO
                                         FROM [{0}].[dbo].ASSOCIACOES AS A
                                        WHERE 1=1
                                     ORDER BY A.SIGLA,
                                              A.RAZAO_SOCIAL,
                                              A.ID";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database);

                using (SqlCommand command = this.CreateCommand(
                    cmdText))
                {
                    using (DataTable dt = this.GetDataTableFromDataReader(
                        command))
                    {
                        if (dt != null && dt.Rows.Count > 0)
                            produtos = this.ConvertToList<ProdutoEntity>(dt).ToList();
                    }
                }

                return produtos;
            }
            catch
            {
                throw;
            }
        }

        public ProdutoEntity Update(ProdutoEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
