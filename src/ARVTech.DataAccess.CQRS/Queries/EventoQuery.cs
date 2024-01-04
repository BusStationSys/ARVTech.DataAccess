namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class EventoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsEventos;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return @" INSERT INTO [dbo].[EVENTOS] ([ID],
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
            return @" DELETE
                        FROM [dbo].[EVENTOS]
                       WHERE [ID] = @Id ";
        }

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
        public override string CommandTextUpdate()
        {
            return @" UPDATE [dbo].[EVENTOS]
                         SET [DESCRICAO] = @Descricao,
                             [OBSERVACOES] = @Observacoes
                       WHERE [ID] = @Id ";
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
        public EventoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsEventos = base.GetAllColumnsFromTable(
                "EVENTOS",
                "E");
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