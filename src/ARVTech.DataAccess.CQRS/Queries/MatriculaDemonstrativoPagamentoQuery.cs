namespace ARVTech.DataAccess.CQRS.Queries
{
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
                base.TableNameEventos,
                base.TableAliasEventos);

            string columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            string columnsMatriculasDemonstrativosPagamento = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamento,
                base.TableAliasMatriculasDemonstrativosPagamento);

            string columnsMatriculasDemonstrativosPagamentoEventos = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoEventos,
                base.TableAliasMatriculasDemonstrativosPagamentoEventos);

            string columnsMatriculasDemonstrativosPagamentoTotalizadores = base.GetAllColumnsFromTable(
                base.TableNameMatriculasDemonstrativosPagamentoTotalizadores,
                base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            string columnsTotalizadores = base.GetAllColumnsFromTable(
                base.TableNameTotalizadores,
                base.TableAliasTotalizadores);

            this._commandTextTemplate = $@"          SELECT {columnsMatriculasDemonstrativosPagamento},
                                                            {columnsMatriculas},
                                                            {columnsPessoasFisicas},
                                                            {columnsPessoasJuridicas},
                                                            {columnsMatriculasDemonstrativosPagamentoEventos},
                                                            {columnsEventos},
                                                            {columnsMatriculasDemonstrativosPagamentoTotalizadores},
                                                            {columnsTotalizadores}
                                                       FROM [dbo].[{base.TableNameMatriculasDemonstrativosPagamento}] as {base.TableAliasMatriculasDemonstrativosPagamento} WITH(NOLOCK)

                                                 INNER JOIN [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasDemonstrativosPagamento}].[GUIDMATRICULA] = [{base.TableAliasMatriculas}].[GUID] 

                                                 INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]

                                                 INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID] 

                                            LEFT OUTER JOIN [dbo].[{base.TableNameMatriculasDemonstrativosPagamentoEventos}] as {base.TableAliasMatriculasDemonstrativosPagamentoEventos} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasDemonstrativosPagamento}].[GUID] = {base.TableAliasMatriculasDemonstrativosPagamentoEventos}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]

                                            LEFT OUTER JOIN [dbo].[{base.TableNameEventos}] as {base.TableAliasEventos} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasDemonstrativosPagamentoEventos}].[IDEVENTO] = [{base.TableAliasEventos}].[ID]

                                            LEFT OUTER JOIN [dbo].[{base.TableNameMatriculasDemonstrativosPagamentoTotalizadores}] as {base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasDemonstrativosPagamento}].[GUID] = {base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores}.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]

                                            LEFT OUTER JOIN [dbo].[{base.TableNameTotalizadores}] as {base.TableAliasTotalizadores} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasDemonstrativosPagamentoTotalizadores}].[IDTOTALIZADOR] = [{base.TableAliasTotalizadores}].[ID] ";
        }

        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMatriculaDemonstrativoPagamento UniqueIdentifier
                               SET @NewGuidMatriculaDemonstrativoPagamento = NEWID()

                            INSERT INTO [dbo].[{base.TableNameMatriculasDemonstrativosPagamento}]
                                        ([GUID],
                                         [GUIDMATRICULA],
                                         [COMPETENCIA])
                                 VALUES (@NewGuidMatriculaDemonstrativoPagamento,
                                         @GuidMatricula,
                                         @Competencia)

                                 SELECT @NewGuidMatriculaDemonstrativoPagamento ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{base.TableNameMatriculasDemonstrativosPagamento}]
                        WHERE [GUID] = @Guid ";
        }

        public override string CommandTextGetAll()
        {
            return $@" {this._commandTextTemplate}          
                       ORDER BY [{base.TableAliasMatriculasDemonstrativosPagamento}].[COMPETENCIA] Desc,
                                [{base.TableAliasMatriculas}].[MATRICULA],
                                [{base.TableAliasPessoasFisicas}].[NOME],
                                [{base.TableAliasEventos}].[ID] ";
        }

        public override string CommandTextGetById()
        {
            return $@" {this._commandTextTemplate}
                       WHERE UPPER([{base.TableAliasMatriculasDemonstrativosPagamento}].[GUID]) = @Guid ";
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

        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{base.TableNameMatriculasDemonstrativosPagamento}]
                          SET [GUIDMATRICULA] = @GuidMatricula,
                              [COMPETENCIA] = @Competencia,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [DATA_CONFIRMACAO] = @DataConfirmacao,
                              [IP_CONFIRMACAO] = @IpConfirmacao
                        WHERE [GUID] = @Guid ";
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
                       WHERE {base.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] = @Competencia ";
        }

        public string CommandTextGetByCompetenciaAndMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {base.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] = @Competencia 
                         AND {base.TableAliasMatriculas}.[MATRICULA] = @Matricula ";
        }

        public string CommandTextGetByGuidColaborador()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = @GuidColaborador ";
        }

        public string CommandTextGetByMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {base.TableAliasMatriculas}.[MATRICULA] = @Matricula ";
        }

        public string CommandTextGetPendencias()
        {
            return $@" {this._commandTextTemplate}
                       WHERE {base.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] >= CONVERT(VARCHAR(8), @CompetenciaInicial, 112)
                         AND {base.TableAliasMatriculasDemonstrativosPagamento}.[COMPETENCIA] <= CONVERT(VARCHAR(8), @CompetenciaFinal, 112) ";
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