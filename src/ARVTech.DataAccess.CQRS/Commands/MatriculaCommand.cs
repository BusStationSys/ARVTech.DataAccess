namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class MatriculaCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMatricula UniqueIdentifier
                               SET @NewGuidMatricula = NEWID()

                       INSERT INTO [dbo].[{Constants.TableNameMatriculas}]
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameMatriculas}]
                        WHERE [GUID] = @Guid ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNameMatriculas}]
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
    }
}