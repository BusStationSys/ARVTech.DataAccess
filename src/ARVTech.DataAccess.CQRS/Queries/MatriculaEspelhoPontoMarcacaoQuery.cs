namespace ARVTech.DataAccess.CQRS.Queries
{
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
                base.TableNameMatriculasEspelhosPonto,
                base.TableAliasMatriculasEspelhosPonto);

            this._columnsMatriculasEspelhosPontoMarcacoes = base.GetAllColumnsFromTable(
                base.TableNameMatriculasEspelhosPontoMarcacoes,
                base.TableAliasMatriculasEspelhosPontoMarcacoes);
        }

        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMepm UniqueIdentifier
                                            SET @NewGuidMepm = NEWID()

                                    INSERT INTO [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}]
                                                ([GUID],
                                                 [GUIDMATRICULA_ESPELHO_PONTO],
                                                 [DATA],
                                                 [MARCACAO],
                                                 [HORAS_EXTRAS_050],
                                                 [HORAS_EXTRAS_070],
                                                 [HORAS_EXTRAS_100],
                                                 [HORAS_CREDITO_BH],
                                                 [HORAS_DEBITO_BH],
                                                 [HORAS_FALTAS],
                                                 [HORAS_TRABALHADAS])
                                         VALUES (@NewGuidMepm,
                                                 @GuidMatriculaEspelhoPonto,
                                                 @Data,
                                                 @Marcacao,
                                                 @HorasExtras050,
                                                 @HorasExtras070,
                                                 @HorasExtras100,
                                                 @HorasCreditoBH,
                                                 @HorasDebitoBH,
                                                 @HorasFaltas,
                                                 @HorasTrabalhadas)

                                          SELECT @NewGuidMepm ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}]
                        WHERE [GUID] = @Guid ";
        }

        public override string CommandTextGetAll()
        {
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
                              FROM [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}] as {base.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{base.TableNameMatriculasEspelhosPonto}] as {base.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {base.TableAliasMatriculasEspelhosPonto}.[GUID] = {base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO] ";
        }

        public override string CommandTextGetById()
        {
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
                              FROM [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}] as {base.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{base.TableNameMatriculasEspelhosPonto}] as {base.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {base.TableAliasMatriculasEspelhosPonto}.[GUID] = {base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]
                             WHERE UPPER({base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUID]) = @Guid ";
        }

        public override string CommandTextUpdate()
        {
            throw new NotImplementedException();
        }

        public string CommandTextGetByGuidMatriculaEspelhoPontoAndData()
        {
            return $@"      SELECT {this._columnsMatriculasEspelhosPontoMarcacoes},
                                   {this._columnsMatriculasEspelhosPonto}
                              FROM [dbo].[{base.TableNameMatriculasEspelhosPontoMarcacoes}] as {base.TableAliasMatriculasEspelhosPontoMarcacoes} WITH(NOLOCK)
                        INNER JOIN [dbo].[{base.TableNameMatriculasEspelhosPonto}] as {base.TableAliasMatriculasEspelhosPonto} WITH(NOLOCK)
                                ON {base.TableAliasMatriculasEspelhosPonto}.[GUID] = {base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]
                             WHERE UPPER({base.TableAliasMatriculasEspelhosPontoMarcacoes}.[GUIDMATRICULA_ESPELHO_PONTO]) = @GuidMatriculaEspelhoPonto
                               AND {base.TableAliasMatriculasEspelhosPontoMarcacoes}.[DATA] = @Data ";
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