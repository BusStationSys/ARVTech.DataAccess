namespace ARVTech.DataAccess.CQRS.Queries
{
    using Microsoft.Data.SqlClient;

    public class EventoQuery : BaseQuery
    {
        private readonly string _columnsEventos;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetAll()
        {
            //return $@" SELECT {this._columnsEventos}
            //             FROM [dbo].[EVENTOS] AS E WITH(NOLOCK) ";

            return "UspObterEventos";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetById()
        {
            //return $@" SELECT {this._columnsEventos}
            //             FROM [dbo].[EVENTOS] AS E WITH(NOLOCK)
            //            WHERE E.ID = @Id ";

            return "UspObterEventoPorId";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            //throw new NotImplementedException();

            return "UspObterEventos";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CommandTextGetLastId()
        {
            return "UspObterUltimoIdEvento";

            //return @" SELECT ISNULL(MAX(ID),0) + 1 AS LAST_ID
            //            FROM [dbo].[EVENTOS] AS E WITH(NOLOCK) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public EventoQuery(SqlConnection connection) :
            base(connection)
        {
            this._columnsEventos = base.GetAllColumnsFromTable(
                "EVENTOS",
                "E");
        }

        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}