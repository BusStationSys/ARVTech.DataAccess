namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class MatriculaEspelhoPontoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaEspelhoPontoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            string columnsCalculos = base.GetAllColumnsFromTable(
                base.TableNameCalculos,
                base.TableAliasCalculos);

            string columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            string columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            string columnsMatriculasEspelhosPontoCalculos = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoCalculos,
                base.TableAliasMatriculasEspelhosPontoCalculos);

            string columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoMarcacoes,
                base.TableAliasMatriculasEspelhosPontoMarcacoes);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            this._commandTextTemplate = $@"          SELECT {columnsMatriculasEspelhosPonto},
                                                            {columnsMatriculas},
                                                            {columnsPessoasFisicas},
                                                            {columnsPessoasJuridicas},
                                                            {columnsMatriculasEspelhosPontoCalculos},
                                                            {columnsMatriculasEspelhosPontoMarcacoes},
                                                            {columnsCalculos}
                                                       FROM [dbo].[{base.TableNameMatriculasEspelhosPonto}] as {base.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                        
                                                 INNER JOIN [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasEspelhosPonto}].[GUIDMATRICULA] = [{base.TableAliasMatriculas}].[GUID] 

                                                 INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]

                                                 INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID] 

                                            LEFT OUTER JOIN [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}] as {base.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasEspelhosPonto}].[GUID] = {base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]

                                            LEFT OUTER JOIN [dbo].[{base.TableNameMatriculasEspelhosPontoCalculos}] as {base.TableAliasMatriculasEspelhosPontoCalculos} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasEspelhosPonto}].[GUID] = {base.TableAliasMatriculasEspelhosPontoCalculos}.[GUIDMATRICULA_ESPELHO_PONTO]

                                            LEFT OUTER JOIN [dbo].[{base.TableNameCalculos}] as {base.TableAliasCalculos} WITH(NOLOCK)
                                                         ON [{base.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{base.TableAliasCalculos}].[ID] ";
        }

        public override string CommandTextCreate()
        {
            return @"    DECLARE @NewGuidMatriculaEspelhoPonto UniqueIdentifier
                             SET @NewGuidMatriculaEspelhoPonto = NEWID()

                     INSERT INTO [dbo].[MATRICULAS_ESPELHOS_PONTO]
                                 ([GUID],
                                  [GUIDMATRICULA],
                                  [COMPETENCIA])
                          VALUES ( @NewGuidMatriculaEspelhoPonto,
                                   @GuidMatricula,
                                   @Competencia )
                          
                           SELECT @NewGuidMatriculaEspelhoPonto ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[MATRICULAS_ESPELHOS_PONTO]
                        WHERE [GUID] = @Guid ";
        }

        public string CommandTextDeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula()
        {
            return $@" DELETE 
                         FROM [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]
                        WHERE [GUIDMATRICULA_ESPELHO_PONTO] IN ( SELECT [GUID]
                                                                   FROM [dbo].[MATRICULAS_ESPELHOS_PONTO]
                                                                  WHERE [COMPETENCIA] = @Competencia
                                                                    AND [GUIDMATRICULA] = @GuidMatricula )
                       
                       DELETE
                         FROM [dbo].[MATRICULAS_ESPELHOS_PONTO_CALCULOS]
                        WHERE [GUIDMATRICULA_ESPELHO_PONTO] IN ( SELECT [GUID]
                                                                   FROM [dbo].[MATRICULAS_ESPELHOS_PONTO]
                                                                  WHERE [COMPETENCIA] = @Competencia
                                                                    AND [GUIDMATRICULA] = @GuidMatricula ) ";
        }

        public string CommandTextGetByCompetenciaAndMatricula()
        {
            return $@"    {this._commandTextTemplate}
                          WHERE [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] = @Competencia
                            AND [{base.TableAliasMatriculas}].[MATRICULA] = @Matricula

                       ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{base.TableAliasMatriculas}].[MATRICULA],
                                [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public string CommandTextGetByGuidColaborador()
        {
            return $@"    {this._commandTextTemplate}
                          WHERE [dbo].[{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = @GuidColaborador

                       ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{base.TableAliasMatriculas}].[MATRICULA],
                                [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetAll()
        {
            return $@" {this._commandTextTemplate}
                       ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{base.TableAliasMatriculas}].[MATRICULA],
                                [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetById()
        {
            return $@"    {this._commandTextTemplate}

                          WHERE UPPER([{base.TableAliasMatriculasEspelhosPonto}].[GUID]) = @Guid

                       ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{base.TableAliasMatriculas}].[MATRICULA],
                                [{base.TableAliasPessoasFisicas}].[NOME] ";
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
            return $@" UPDATE [dbo].[{base.TableNameMatriculasEspelhosPonto}]
                          SET [GUIDMATRICULA] = @GuidMatricula,
                              [COMPETENCIA] = @Competencia
                        WHERE [GUID] = @Guid ";
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