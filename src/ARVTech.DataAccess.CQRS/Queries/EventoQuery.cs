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
            return $@" SELECT {this._columnsEventos}
                         FROM [dbo].[EVENTOS] AS E WITH(NOLOCK) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextGetById()
        {
            return $@" SELECT {this._columnsEventos}
                         FROM [dbo].[EVENTOS] AS E WITH(NOLOCK)
                        WHERE E.ID = @Id ";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CommandTextGetLastId()
        {
            return @" SELECT ISNULL(MAX(ID),0) + 1 AS LAST_ID
                        FROM [dbo].[EVENTOS] AS E WITH(NOLOCK) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public EventoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
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