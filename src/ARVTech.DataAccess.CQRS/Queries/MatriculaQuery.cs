namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class MatriculaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsMatriculas;
        private readonly string _columnsPessoasFisicas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MatriculaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsMatriculas = base.GetAllColumnsFromTable(
                base.TableNameMatriculas,
                base.TableAliasMatriculas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas);
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
                                    [BANCO],
                                    [AGENCIA],
                                    [CONTA],
                                    [CARGA_HORARIA])
                            VALUES (@NewGuidMatricula,
                                    @Matricula,
                                    @DataAdmissao,
                                    @DataDemissao,
                                    @DescricaoCargo,
                                    @DescricaoSetor,
                                    @GuidColaborador,
                                    @GuidEmpregador,
                                    @Banco,
                                    @Agencia,
                                    @Conta,
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
            return $@"     SELECT {this._columnsMatriculas},
                                  {this._columnsPessoasFisicas},
                                  {this._columnsPessoasJuridicas}
                             FROM [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID] ";
        }

        public override string CommandTextGetById()
        {
            return $@"     SELECT {this._columnsMatriculas},
                                  {this._columnsPessoasFisicas},
                                  {this._columnsPessoasJuridicas}
                             FROM [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID]

                            WHERE [{base.TableAliasMatriculas}].[GUID] = @Guid ";
        }

        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{base.TableNameMatriculas}]
                          SET [MATRICULA] = @Matricula,
                              [DATA_ADMISSAO] = @DataAdmissao,
                              [DATA_DEMISSAO] = @DataDemissao,
                              [GUIDCOLABORADOR] = @GuidColaborador,
                              [GUIDEMPREGADOR] = @GuidEmpregador,
                              [BANCO] = @Banco,
                              [AGENCIA] = @Agencia,
                              [CONTA] = @Conta,
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
            return $@"     SELECT {this._columnsMatriculas},
                                  {this._columnsPessoasFisicas},
                                  {this._columnsPessoasJuridicas}
                             FROM [dbo].[{base.TableNameMatriculas}] as {base.TableAliasMatriculas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNamePessoasJuridicas}] as {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                               ON [{base.TableAliasMatriculas}].[GUIDEMPREGADOR] = [{base.TableAliasPessoasJuridicas}].[GUID]

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