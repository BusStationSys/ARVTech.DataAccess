namespace ARVTech.DataAccess.CQRS.Commands.Destin
{
    public class ConcursoCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return "UspInserirConcurso";
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