namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MatriculaEspelhoPontoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public MatriculaEspelhoPontoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {

        }

        public override string CommandTextCreate()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextDelete()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetAll()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetById()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextUpdate()
        {
            throw new NotImplementedException();
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