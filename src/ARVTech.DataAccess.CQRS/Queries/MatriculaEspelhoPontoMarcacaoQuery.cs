namespace ARVTech.DataAccess.CQRS.Queries
{
    using ARVTech.Shared;
    using System;
    using System.Data.SqlClient;

    public class MatriculaEspelhoPontoMarcacaoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsMatriculasEspelhosPonto;

        private readonly string _columnsMatriculasEspelhosPontoMarcacoes;

        public MatriculaEspelhoPontoMarcacaoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPonto,
                Constants.TableAliasMatriculasEspelhosPonto);

            this._columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPontoMarcacoes,
                Constants.TableAliasMatriculasEspelhosPontoMarcacoes);
        }

        public override string CommandTextGetAll()
        {
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
                              FROM [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}] as {Constants.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPonto}] as {Constants.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {Constants.TableAliasMatriculasEspelhosPonto}.[GUID] = {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO] ";
        }

        public override string CommandTextGetById()
        {
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
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
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
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
    }
}