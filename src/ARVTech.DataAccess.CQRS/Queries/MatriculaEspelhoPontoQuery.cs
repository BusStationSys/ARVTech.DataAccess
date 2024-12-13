namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;
    using ARVTech.Shared;

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
                Constants.TableNameCalculos,
                Constants.TableAliasCalculos);

            string columnsMatriculas = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculas,
                Constants.TableAliasMatriculas);

            string columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPonto,
                Constants.TableAliasMatriculasEspelhosPonto);

            string columnsMatriculasEspelhosPontoCalculos = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPontoCalculos,
                Constants.TableAliasMatriculasEspelhosPontoCalculos);

            string columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                Constants.TableNameMatriculasEspelhosPontoMarcacoes,
                Constants.TableAliasMatriculasEspelhosPontoMarcacoes);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasFisicas,
                Constants.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasJuridicas,
                Constants.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            this._commandTextTemplate = $@"          SELECT {columnsMatriculasEspelhosPonto},
                                                            {columnsMatriculas},
                                                            {columnsPessoasFisicas},
                                                            {columnsPessoasJuridicas},
                                                            {columnsMatriculasEspelhosPontoCalculos},
                                                            {columnsMatriculasEspelhosPontoMarcacoes},
                                                            {columnsCalculos}
                                                       FROM [dbo].[{Constants.TableNameMatriculasEspelhosPonto}] as {Constants.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                        
                                                 INNER JOIN [dbo].[{Constants.TableNameMatriculas}] as {Constants.TableAliasMatriculas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasEspelhosPonto}].[GUIDMATRICULA] = [{Constants.TableAliasMatriculas}].[GUID] 

                                                 INNER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]

                                                 INNER JOIN [dbo].[{Constants.TableNamePessoasJuridicas}] as {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{Constants.TableAliasPessoasJuridicas}].[GUID] 

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}] as {Constants.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasEspelhosPonto}].[GUID] = {Constants.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameMatriculasEspelhosPontoCalculos}] as {Constants.TableAliasMatriculasEspelhosPontoCalculos} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasEspelhosPonto}].[GUID] = {Constants.TableAliasMatriculasEspelhosPontoCalculos}.[GUIDMATRICULA_ESPELHO_PONTO]

                                            LEFT OUTER JOIN [dbo].[{Constants.TableNameCalculos}] as {Constants.TableAliasCalculos} WITH(NOLOCK)
                                                         ON [{Constants.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{Constants.TableAliasCalculos}].[ID] ";
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
                          WHERE [{Constants.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] = @Competencia
                            AND [{Constants.TableAliasMatriculas}].[MATRICULA] = @Matricula

                       ORDER BY [{Constants.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{Constants.TableAliasMatriculas}].[MATRICULA],
                                [{Constants.TableAliasPessoasFisicas}].[NOME] ";
        }

        public string CommandTextGetByGuidColaborador()
        {
            return $@"    {this._commandTextTemplate}
                          WHERE [dbo].[{Constants.TableAliasMatriculas}].[GUIDCOLABORADOR] = @GuidColaborador

                       ORDER BY [{Constants.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{Constants.TableAliasMatriculas}].[MATRICULA],
                                [{Constants.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetAll()
        {
            return $@" {this._commandTextTemplate}
                       ORDER BY [{Constants.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{Constants.TableAliasMatriculas}].[MATRICULA],
                                [{Constants.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetById()
        {
            return $@"    {this._commandTextTemplate}

                          WHERE UPPER([{Constants.TableAliasMatriculasEspelhosPonto}].[GUID]) = @Guid

                       ORDER BY [{Constants.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                [{Constants.TableAliasMatriculas}].[MATRICULA],
                                [{Constants.TableAliasPessoasFisicas}].[NOME] ";
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