namespace FlooString.UnitOfWork.PostgreSql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Npgsql;

    public abstract class Repository
    {
        protected readonly string ParameterSymbol = ":";

        protected NpgsqlConnection _connection = null as NpgsqlConnection;

        protected NpgsqlTransaction _transaction = null as NpgsqlTransaction;

        protected NpgsqlCommand CreateCommand(
            string cmdText, 
            CommandType commandType = CommandType.Text, 
            NpgsqlParameter[] parameters = null)
        {
            NpgsqlCommand command = new NpgsqlCommand(
                cmdText,
                this._connection,
                this._transaction);

            command.CommandType = commandType;

            if (parameters != null &&
                parameters.Count() > 0)
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);

            return command;
        }

        protected NpgsqlDataAdapter CreateDataAdapter(NpgsqlCommand command)
        {
            return new NpgsqlDataAdapter(command);
        }

        protected NpgsqlParameter CreateDataParameter(string parameterName, object value)
        {
            NpgsqlParameter dataParameter = new NpgsqlParameter();
            dataParameter.ParameterName = parameterName;
            dataParameter.Value = (value ?? DBNull.Value);

            return dataParameter;
        }

        protected NpgsqlParameter[] GetDataParameters<T>(T model) where T : class
        {
            IList<NpgsqlParameter> dataParameters = null as IList<NpgsqlParameter>;

            foreach (var property in model.GetType().GetProperties())
            {
                if (dataParameters == null)
                    dataParameters = new List<NpgsqlParameter>();

                string parameterName = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    this.ParameterSymbol,
                    property.Name);

                object parameterValue = property.GetValue(
                    model,
                    null);

                NpgsqlParameter item = this.CreateDataParameter(
                    parameterName,
                    parameterValue);

                dataParameters.Add(item);
            }

            return dataParameters.ToArray();
        }
    }
}