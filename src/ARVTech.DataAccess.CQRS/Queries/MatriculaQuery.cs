namespace ARVTech.DataAccess.CQRS.Queries
{
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
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            string columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas,
                "PF.FOTO");

            string columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas,
                "PJ.LOGOTIPO");

            this._commandTextTemplate = $@"     SELECT {columnsMatriculas},
                                                       {columnsPessoasFisicas},
                                                       {columnsPessoasJuridicas}
                                                  FROM [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                                            INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                                    ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                                            INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                                                    ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID] ";
        }

        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMatricula UniqueIdentifier
                               SET @NewGuidMatricula = NEWID()

                       INSERT INTO [dbo].[{base.TableNameMatriculas}]
                                   ([GUID],
                                    [MATRICULA],
                                    [DATA_ADMISSAO],
                                    [DATA_DEMISSAO],
                                    [DESCRICAO_CARGO],
                                    [DESCRICAO_SETOR],
                                    [GUIDCOLABORADOR],
                                    [GUIDEMPREGADOR],
                                    [FORMA_PAGAMENTO],
                                    [BANCO],                                    
                                    [AGENCIA],
                                    [CONTA],
                                    [DV_CONTA],
                                    [CARGA_HORARIA])
                            VALUES (@NewGuidMatricula,
                                    @Matricula,
                                    @DataAdmissao,
                                    @DataDemissao,
                                    @DescricaoCargo,
                                    @DescricaoSetor,
                                    @GuidColaborador,
                                    @GuidEmpregador,
                                    @FormaPagamento,
                                    @Banco,
                                    @Agencia,
                                    @Conta,
                                    @DvConta,
                                    @CargaHoraria)

                             SELECT @NewGuidMatricula ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{base.TableNameMatriculas}]
                        WHERE [GUID] = @Guid ";
        }

        public override string CommandTextGetAll()
        {
            return this._commandTextTemplate;
        }

        public override string CommandTextGetById()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{base.TableAliasMatriculas}].[GUID] = @Guid ";
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
            return $@" UPDATE [dbo].[{base.TableNameMatriculas}]
                          SET [MATRICULA] = @Matricula,
                              [DATA_ADMISSAO] = @DataAdmissao,
                              [DATA_DEMISSAO] = @DataDemissao,
                              [DESCRICAO_CARGO] = @DescricaoCargo,
                              [DESCRICAO_SETOR] = @DescricaoSetor,
                              [GUIDCOLABORADOR] = @GuidColaborador,
                              [GUIDEMPREGADOR] = @GuidEmpregador,
                              [BANCO] = @Banco,
                              [AGENCIA] = @Agencia,
                              [CONTA] = @Conta,
                              [DV_CONTA] = @DvConta,
                              [FORMA_PAGAMENTO] = @FormaPagamento,
                              [SALARIO_NOMINAL] = @SalarioNominal
                        WHERE [GUID] = @Guid ";
        }

        public string CommandTextDeleteEspelhosPonto()
        {
            return $@" DELETE
                         FROM [dbo].[{base.TableNameMatriculasEspelhosPonto}]
                        WHERE [GUIDMATRICULA] = @GuidMatricula
                          AND [GUID] = @Guid ";
        }

        public string CommandTextGetByMatricula()
        {
            return $@" {this._commandTextTemplate}
                       WHERE [{base.TableAliasMatriculas}].[MATRICULA] = @Matricula ";
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