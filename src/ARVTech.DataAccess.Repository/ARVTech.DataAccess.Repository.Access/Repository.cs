namespace ARV.DataAccess.Repository.Access
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.Linq;
    using ARV.DataAccess.Repository.Common;

    public abstract class Repository
    {
        // private bool _disposedValue = false; // To detect redundant calls

        protected readonly string ParameterSymbol = "@";

        protected OleDbConnection _connection = null as OleDbConnection;

        protected OleDbTransaction _transaction = null as OleDbTransaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected Repository(OleDbConnection connection, OleDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected DataTable GetDataTableFromDataAdapter(IDbCommand command)
        {
            try
            {
                if (command == null)
                    throw new ArgumentNullException(nameof(command));

                using (OleDbCommand oleDbCommand = new OleDbCommand(
                    command.CommandText.ToString(),
                    this._connection,
                    this._transaction))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(oleDbCommand))
                        {
                            adapter.Fill(dataTable);
                        }

                        return dataTable;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected DataTable GetDataTableFromDataReader(IDbCommand command)
        {
            try
            {
                using (OleDbCommand oleDbCommand = new OleDbCommand(
                    command.CommandText.ToString(),
                    this._connection,
                    this._transaction))
                {
                    return Helpers.GetInstance().GetDataTableFromDataReader(
                        oleDbCommand);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected OleDbCommand CreateCommand(
            string cmdText,
            CommandType commandType = CommandType.Text,
            OleDbParameter[] parameters = null)
        {
            try
            {
                using (OleDbCommand oleDbcommand = new OleDbCommand(
                    cmdText,
                    this._connection,
                    this._transaction))
                {
                    oleDbcommand.CommandText = cmdText;
                    oleDbcommand.CommandType = commandType;

                    if (parameters != null &&
                        parameters.Any())
                        foreach (var parameter in parameters)
                            oleDbcommand.Parameters.AddWithValue(
                                parameter.ParameterName,
                                parameter.Value);

                    return oleDbcommand;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected OleDbParameter CreateDataParameter(string parameterName, object value)
        {
            OleDbParameter dataParameter = new OleDbParameter();
            dataParameter.ParameterName = parameterName;
            dataParameter.Value = (value ?? DBNull.Value);

            return dataParameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected OleDbParameter[] GetDataParameters<T>(T model) where T : class
        {
            IList<OleDbParameter> dataParameters = null as IList<OleDbParameter>;

            foreach (var property in model.GetType().GetProperties())
            {
                if (dataParameters == null)
                    dataParameters = new List<OleDbParameter>();

                string parameterName = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    this.ParameterSymbol,
                    property.Name);

                object parameterValue = property.GetValue(
                    model,
                    null);

                OleDbParameter item = this.CreateDataParameter(
                    parameterName,
                    parameterValue);

                dataParameters.Add(item);
            }

            return dataParameters.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="IEnumerable{T}"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        protected IEnumerable<T> ConvertToList<T>(DataTable dataTable)
        {
            try
            {
                return Helpers.GetInstance().ConvertToList<T>(dataTable);
            }
            catch
            {
                throw;
            }
        }

        //protected virtual int Somar()
        //{
        //    return 2;
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this._disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // TODO: fazer dispose dos managed objects.

        //            if (this._connection != null && this._connection.State == ConnectionState.Open)
        //            {
        //                this._connection.Dispose();
        //                this._connection = null;
        //            }

        //            if (this._transaction != null)
        //            {
        //                this._transaction.Dispose();
        //                this._transaction = null;
        //            }
        //        }

        //        // TODO: liberar recursos unmanaged (unmanaged objects) e fazer override do finalizador.
        //        // TODO: campos grandes devem receber valor null.

        //        this._disposedValue = true;
        //    }
        //}

        //public void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //~Repository()
        //{
        //    this.Dispose(false);
        //}
    }
}