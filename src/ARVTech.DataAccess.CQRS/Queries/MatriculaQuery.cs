namespace ARVTech.DataAccess.CQRS.Queries
{
    using ARVTech.Shared;
    using System.Data.SqlClient;

    public class MatriculaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            string columnsMatriculas = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculas,
                Constants.TableAliasMatriculas);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasFisicas,
                Constants.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasJuridicas,
                Constants.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            this._commandTextTemplate = $@"     SELECT {columnsMatriculas},
                                                       {columnsPessoasFisicas},
                                                       {columnsPessoasJuridicas}
                                                  FROM [dbo].[{Constants.TableNameMatriculas}] as {Constants.TableAliasMatriculas} WITH(NOLOCK)
                                            INNER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                    ON [{Constants.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]
                                            INNER JOIN [dbo].[{Constants.TableNamePessoasJuridicas}] as {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                    ON [{Constants.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{Constants.TableAliasPessoasJuridicas}].[GUID] ";
        }

        public override string CommandTextGetAll()
        {
            return this._commandTextTemplate;
        }

        public override string CommandTextGetById()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{Constants.TableAliasMatriculas}].[GUID] = @Guid ";
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

        public string CommandTextGetAniversariantesEmpresa()
        {
            return $@"     SELECT M.[GUID],
                                  M.[MATRICULA],
                                  M.[DATA_ADMISSAO],
                                  PF.[GUID],
                                  PF.[NOME],
                                  PJ.[GUID],
                                  PJ.[RAZAO_SOCIAL]
                             FROM MATRICULAS as M
                       INNER JOIN PESSOAS_FISICAS as PF
                               ON M.GUIDCOLABORADOR = PF.GUID
                       INNER JOIN PESSOAS_JURIDICAS as PJ
                               ON M.GUIDEMPREGADOR = PJ.GUID
                            WHERE DATA_DEMISSAO IS NULL
                              AND MONTH(M.[DATA_ADMISSAO]) = @Mes ";
        }

        public string CommandTextDeleteEspelhosPonto()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameMatriculasEspelhosPonto}]
                        WHERE [GUIDMATRICULA] = @GuidMatricula
                          AND [GUID] = @Guid ";
        }

        public string CommandTextGetByMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{Constants.TableAliasMatriculas}].[MATRICULA] = @Matricula ";
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