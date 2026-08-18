namespace ARVTech.DataAccess.CQRS.Queries.Destin
{
    using Microsoft.Data.SqlClient;

    public class ConcursoQuery : BaseQuery
    {
        private readonly string _columnsConcursos;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetAll()
        {
            return "UspObterConcursos";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetById()
        {
            return "UspObterConcursoPorId";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            return "UspObterConcursos";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CommandTextGetByIdModalidadeAndNumeroAndDataApuracao()
        {
            return "UspObterConcursoPorIdModalidadeENumeroEDataApuracao";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public ConcursoQuery(SqlConnection connection) :
            base(connection)
        {
            this._columnsConcursos = base.GetAllColumnsFromTable(
                "CONCURSO",
                "C");
        }

        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}