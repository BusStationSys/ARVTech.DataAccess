namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ARVTech.Shared;

    public class PessoaFisicaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasFisicas;

        private readonly string _commandTextTemplate;

        public override string CommandTextGetAll()
        {
            return this._commandTextTemplate;
        }

        public override string CommandTextGetById()
        {
            return $@"     {this._commandTextTemplate} 
                            WHERE [{Constants.TableAliasPessoasFisicas}].[GUID] = @Guid  ";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            return base.RefreshPagination(
                this._commandTextTemplate,
                where,
                orderBy,
                pageNumber,
                pageSize);
        }

        public string CommandTextGetAniversariantes()
        {
            return $@"     {this._commandTextTemplate} 
                            WHERE SUBSTRING(CONVERT(VARCHAR, [{Constants.TableAliasPessoasFisicas}].[DATA_NASCIMENTO], 112), 5, 4) >= @PeriodoInicial
                              AND SUBSTRING(CONVERT(VARCHAR, [{Constants.TableAliasPessoasFisicas}].[DATA_NASCIMENTO], 112), 5, 4) <= @PeriodoFinal ";
        }

        public string CommandTextGetByCpf()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{Constants.TableNamePessoasFisicas}] AS {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] 
                            WHERE [{Constants.TableAliasPessoasFisicas}].[CPF] = @Cpf ";
        }

        public string CommandTextGetByNome()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{Constants.TableNamePessoasFisicas}] AS {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] 
                            WHERE [{Constants.TableAliasPessoasFisicas}].[NOME] = @Nome ";
        }

        public string CommandTextGetByNomeNumeroCtpsSerieCtpsAndUfCtps()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{Constants.TableNamePessoasFisicas}] AS {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] 
                            WHERE [{Constants.TableAliasPessoasFisicas}].[NOME] = @Nome 
                              AND [{Constants.TableAliasPessoasFisicas}].[NUMERO_CTPS] = @NumeroCtps
                              AND [{Constants.TableAliasPessoasFisicas}].[SERIE_CTPS] = @SerieCtps
                              AND [{Constants.TableAliasPessoasFisicas}].[UF_CTPS] = @UfCtps ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaFisicaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsPessoas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoas,
                Constants.TableAliasPessoas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasFisicas,
                Constants.TableAliasPessoasFisicas);

            this._commandTextTemplate = $@"     SELECT {this._columnsPessoasFisicas},
                                                       {this._columnsPessoas}
                                                  FROM [dbo].[{Constants.TableNamePessoasFisicas}] AS {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                            INNER JOIN [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas} WITH(NOLOCK)
                                                    ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] ";
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}