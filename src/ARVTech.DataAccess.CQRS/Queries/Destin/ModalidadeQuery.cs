namespace ARVTech.DataAccess.CQRS.Queries.Destin
{
    using Microsoft.Data.SqlClient;

    public class ModalidadeQuery : BaseQuery
    {
        private readonly string _columnsModalidades;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetAll()
        {
            return "UspObterModalidades";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetById()
        {
            return "UspObterModalidadePorId";
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
            return "UspObterModalidades";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CommandTextGetLastId()
        {
            return "UspObterUltimoIdModalidade";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public ModalidadeQuery(SqlConnection connection) :
            base(connection)
        {
            this._columnsModalidades = base.GetAllColumnsFromTable(
                "MODALIDADE",
                "M");
        }

        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}