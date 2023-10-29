namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class MatriculaEspelhoPontoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsCalculos;
        private readonly string _columnsMatriculas;
        private readonly string _columnsMatriculasEspelhosPonto;
        private readonly string _columnsMatriculasEspelhosPontoCalculos;
        private readonly string _columnsMatriculasEspelhosPontoMarcacoes;
        private readonly string _columnsPessoasFisicas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaEspelhoPontoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsCalculos = base.GetAllColumnsFromTable(
                base.TableNameCalculos,
                base.TableAliasCalculos);

            this._columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            this._columnsMatriculasEspelhosPonto = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            this._columnsMatriculasEspelhosPontoCalculos = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoCalculos,
                base.TableAliasMatriculasEspelhosPontoCalculos);

            this._columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoMarcacoes,
                base.TableAliasMatriculasEspelhosPontoMarcacoes);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas,
                "PF.FOTO");

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");
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
            return $@"          SELECT {this._columnsMatriculasEspelhosPonto},
                                       {this._columnsMatriculas},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoasJuridicas},
                                       {this._columnsMatriculasEspelhosPontoCalculos},
                                       {this._columnsMatriculasEspelhosPontoMarcacoes},
                                       {this._columnsCalculos}
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
                                    ON [{base.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{base.TableAliasCalculos}].[ID]

                                 WHERE [dbo].[{base.TableNameMatriculasEspelhosPonto}].[COMPETENCIA] = @Competencia
                                   AND [dbo].[{base.TableNameMatriculas}].[MATRICULA] = @Matricula

                              ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                       [{base.TableAliasMatriculas}].[MATRICULA],
                                       [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public string CommandTextGetByGuidColaborador()
        {
            return $@"          SELECT {this._columnsMatriculasEspelhosPonto},
                                       {this._columnsMatriculas},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoasJuridicas},
                                       {this._columnsMatriculasEspelhosPontoCalculos},
                                       {this._columnsMatriculasEspelhosPontoMarcacoes},
                                       {this._columnsCalculos}
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
                                    ON [{base.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{base.TableAliasCalculos}].[ID]
                                 
                                 WHERE [dbo].[{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = @GuidColaborador

                              ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                       [{base.TableAliasMatriculas}].[MATRICULA],
                                       [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetAll()
        {
            return $@"          SELECT {this._columnsMatriculasEspelhosPonto},
                                       {this._columnsMatriculas},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoasJuridicas},
                                       {this._columnsMatriculasEspelhosPontoCalculos},
                                       {this._columnsMatriculasEspelhosPontoMarcacoes},
                                       {this._columnsCalculos}
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
                                    ON [{base.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{base.TableAliasCalculos}].[ID]

                              ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                       [{base.TableAliasMatriculas}].[MATRICULA],
                                       [{base.TableAliasPessoasFisicas}].[NOME] ";
        }

        public override string CommandTextGetById()
        {
            return $@"          SELECT {this._columnsMatriculasEspelhosPonto},
                                       {this._columnsMatriculas},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoasJuridicas},
                                       {this._columnsMatriculasEspelhosPontoCalculos},
                                       {this._columnsMatriculasEspelhosPontoMarcacoes},
                                       {this._columnsCalculos}
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
                                    ON [{base.TableAliasMatriculasEspelhosPontoCalculos}].[IDCALCULO] = [{base.TableAliasCalculos}].[ID]

                                 WHERE UPPER([{base.TableAliasMatriculasEspelhosPonto}].[GUID]) = @Guid

                              ORDER BY [{base.TableAliasMatriculasEspelhosPonto}].[COMPETENCIA] Desc,
                                       [{base.TableAliasMatriculas}].[MATRICULA],
                                       [{base.TableAliasPessoasFisicas}].[NOME] ";
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