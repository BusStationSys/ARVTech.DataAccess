namespace ARVTech.DataAccess.CQRS.Commands.Destin
{
    public class ModalidadeCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return "UspInserirModalidade";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return "UspExcluirModalidadePorId";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return "UspAtualizarModalidade";
        }
    }
}