namespace FlooString.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using MySqlConnector;

    public abstract class Repository
    {
        protected readonly string ParameterSymbol = "?";

        protected MySqlConnection _connection = null as MySqlConnection;

        protected MySqlTransaction _transaction = null as MySqlTransaction;

        protected MySqlCommand CreateCommand(
            string cmdText, 
            CommandType commandType = CommandType.Text, 
            MySqlParameter[] parameters = null)
        {
            MySqlCommand command = new MySqlCommand(
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

        protected MySqlDataAdapter CreateDataAdapter(MySqlCommand command)
        {
            return new MySqlDataAdapter(command);
        }

        protected MySqlParameter CreateDataParameter(string parameterName, object value)
        {
            MySqlParameter dataParameter = new MySqlParameter();
            dataParameter.ParameterName = parameterName;
            dataParameter.Value = (value ?? DBNull.Value);

            return dataParameter;
        }

        protected MySqlParameter[] GetDataParameters<T>(T model) where T : class
        {
            IList<MySqlParameter> dataParameters = null as IList<MySqlParameter>;

            foreach (var property in model.GetType().GetProperties())
            {
                if (dataParameters == null)
                    dataParameters = new List<MySqlParameter>();

                string parameterName = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    this.ParameterSymbol,
                    property.Name);

                object parameterValue = property.GetValue(
                    model,
                    null);

                MySqlParameter item = this.CreateDataParameter(
                    parameterName,
                    parameterValue);

                dataParameters.Add(item);
            }

            return dataParameters.ToArray();
        }
    }
}