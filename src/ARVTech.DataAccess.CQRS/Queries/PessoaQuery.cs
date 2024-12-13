namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Data.SqlClient;

    public class PessoaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        public override string CommandTextGetAll()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetById()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            return base.RefreshPagination(
                this._commandTextTemplate,
                where,
                orderBy,
                pageNumber,
                pageSize);
        }

        public PessoaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._commandTextTemplate = "";
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