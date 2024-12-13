namespace ARVTech.DataAccess.CQRS.Queries
{
    using ARVTech.Shared;
    using System.Data.SqlClient;

    public class MatriculaDemonstrativoPagamentoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaDemonstrativoPagamentoQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaDemonstrativoPagamentoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            string columnsEventos = base.GetAllColumnsFromTable(
                Constants.TableNameEventos,
                Constants.TableAliasEventos);

            string columnsMatriculas = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculas,
                Constants.TableAliasMatriculas);

            string columnsMatriculasDemonstrativosPagamento = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasDemonstrativosPagamento,
                Constants.TableAliasMatriculasDemonstrativosPagamento);

            string columnsMatriculasDemonstrativosPagamentoEventos = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasDemonstrativosPagamentoEventos,
                Constants.TableAliasMatriculasDemonstrativosPagamentoEventos);

            string columnsMatriculasDemonstrativosPagamentoTotalizadores = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasFisicas,
                Constants.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasJuridicas,
                Constants.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            string columnsTotalizadores = base.GetAllColumnsFromTable(
                Constants.TableNameTotalizadores,
                Constants.TableAliasTotalizadores);

            this._commandTextTemplate = $@"          SELECT {columnsMatriculasDemonstrativosPagamento},
                                                            {columnsMatriculas},
                                                            {columnsPessoasFisicas},
                                                            {columnsPessoasJuridicas},
                                                            {columnsMatriculasDemonstrativosPagamentoEventos},
                                                            {columnsEventos},
                                                            {columnsMatriculasDemonstrativosPagamentoTotalizadores},
                                                            {columnsTotalizadores}
                                                       FROM [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamento}] as {Constants.TableAliasMatriculasDemonstrativosPagamento} WITH(NOLOCK)

                                                 INNER JOIN [dbo].[{Constants.TableNameMatriculas}] as {Constants.TableAliasMatriculas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasDemonstrativosPagamento}].[GUIDMATRICULA] = [{Constants.TableAliasMatriculas}].[GUID] 

                                                 INNER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]

                                                 INNER JOIN [dbo].[{Constants.TableNamePessoasJuridicas}] as {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{Constants.TableAliasPessoasJuridicas}].[GUID] 

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamentoEventos}] as {Constants.TableAliasMatriculasDemonstrativosPagamentoEventos} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasDemonstrativosPagamento}].[GUID] = {Constants.TableAliasMatriculasDemonstrativosPagamentoEventos}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameEventos}] as {Constants.TableAliasEventos} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasDemonstrativosPagamentoEventos}].[IDEVENTO] = [{Constants.TableAliasEventos}].[ID]

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamentoTotalizadores}] as {Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasDemonstrativosPagamento}].[GUID] = {Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameTotalizadores}] as {Constants.TableAliasTotalizadores} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasDemonstrativosPagamentoTotalizadores}].[IDTOTALIZADOR] = [{Constants.TableAliasTotalizadores}].[ID] ";
        }

        public override string CommandTextGetAll()
        {
            return $@" {this._commandTextTemplate}          
                       ORDER BY [{Constants.TableAliasMatriculasDemonstrativosPagamento}].[COMPETENCIA] Desc,
                                [{Constants.TableAliasMatriculas}].[MATRICULA],
                                [{Constants.TableAliasPessoasFisicas}].[NOME],
                                [{Constants.TableAliasEventos}].[ID] ";
        }

        public override string CommandTextGetById()
        {
            return $@" {this._commandTextTemplate}
                       WHERE UPPER([{Constants.TableAliasMatriculasDemonstrativosPagamento}].[GUID]) = @Guid ";
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

        public string CommandTextDeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula()
        {
            return $@" DELETE
                         FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS]
                        WHERE [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] IN ( SELECT [GUID]
                                                                             FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
                                                                            WHERE [COMPETENCIA] = @Competencia
                                                                              AND [GUIDMATRICULA] = @GuidMatricula )
                       DELETE
                         FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]
                        WHERE [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] IN ( SELECT [GUID]
                                                                             FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
                                                                            WHERE [COMPETENCIA] = @Competencia
                                                                              AND [GUIDMATRICULA] = @GuidMatricula ) ";
        }

        public string CommandTextGetByCompetencia()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {Constants.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] = @Competencia ";
        }

        public string CommandTextGetByCompetenciaAndMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {Constants.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] = @Competencia 
                         AND {Constants.TableAliasMatriculas}.[MATRICULA] = @Matricula ";
        }

        public string CommandTextGetByGuidColaborador()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{Constants.TableAliasMatriculas}].[GUIDCOLABORADOR] = @GuidColaborador ";
        }

        public string CommandTextGetByMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {Constants.TableAliasMatriculas}.[MATRICULA] = @Matricula ";
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