namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using ARVTech.Shared;
    using Microsoft.Data.SqlClient;

    public class MatriculaEspelhoPontoMarcacaoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private string? _columnsMatriculasEspelhosPonto;

        private string? _columnsMatriculasEspelhosPontoMarcacoes;

        public MatriculaEspelhoPontoMarcacaoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        { }

        public override string CommandTextGetAll()
        {
            return $@"      SELECT {this.ColumnsMatriculasEspelhosPontoMarcacoes},
                                   {this.ColumnsMatriculasEspelhosPonto}
                              FROM [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}] as {Constants.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPonto}] as {Constants.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {Constants.TableAliasMatriculasEspelhosPonto}.[GUID] = {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO] ";
        }

        public override string CommandTextGetById()
        {
            return $@"      SELECT {this.ColumnsMatriculasEspelhosPontoMarcacoes},
                                   {this.ColumnsMatriculasEspelhosPonto}
                              FROM [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}] as {Constants.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPonto}] as {Constants.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {Constants.TableAliasMatriculasEspelhosPonto}.[GUID] = {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]
                             WHERE UPPER({Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUID]) = @Guid ";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public string CommandTextGetByGuidMatriculaEspelhoPontoAndData()
        {
            return $@"      SELECT {this.ColumnsMatriculasEspelhosPontoMarcacoes},
                                   {this.ColumnsMatriculasEspelhosPonto}
                              FROM [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}] as {Constants.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPonto}] as {Constants.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {Constants.TableAliasMatriculasEspelhosPonto}.[GUID] = {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]
                             WHERE UPPER({Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]) = @GuidMatriculaEspelhoPonto
                               AND {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[DATA] = @Data ";
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

        /// <summary>
        /// Gets all column names from the "Matrículas Espelhos Ponto" table with alias applied.
        /// </summary>
        private string ColumnsMatriculasEspelhosPonto
        {
            get
            {
                if (this._columnsMatriculasEspelhosPonto is null)
                    this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                        Constants.TableNameMatriculasEspelhosPonto,
                        Constants.TableAliasMatriculasEspelhosPonto);

                return this._columnsMatriculasEspelhosPonto;
            }
        }

        /// <summary>
        /// Gets all column names from the "Matrículas Espelhos Ponto Marcações" table with alias applied.
        /// </summary>
        private string ColumnsMatriculasEspelhosPontoMarcacoes
        {
            get
            {
                if (this._columnsMatriculasEspelhosPontoMarcacoes is null)
                    this._columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                        Constants.TableNameMatriculasEspelhosPontoMarcacoes,
                        Constants.TableAliasMatriculasEspelhosPontoMarcacoes);

                return this._columnsMatriculasEspelhosPontoMarcacoes;
            }
        }
    }
}