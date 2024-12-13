namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class EventoCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{Constants.TableNameEventos}] ([ID],
                                                   [DESCRICAO],
                                                   [TIPO],
                                                   [OBSERVACOES])
                                           VALUES (@Id,
                                                   @Descricao,
                                                   @Tipo,
                                                   @Observacoes) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@" DELETE
                        FROM [dbo].[{Constants.TableNameEventos}]
                       WHERE [ID] = @Id ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNameEventos}]
                         SET [DESCRICAO] = @Descricao,
                             [OBSERVACOES] = @Observacoes
                       WHERE [ID] = @Id ";
        }
    }
}