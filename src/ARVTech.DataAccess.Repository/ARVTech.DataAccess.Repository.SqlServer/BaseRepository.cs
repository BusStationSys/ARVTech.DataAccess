namespace ARVTech.DataAccess.Repository.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using ARVTech.DataAccess.Repository.Common;
    using Dapper;

    public abstract class BaseRepository : IDisposable
    {
        private bool _disposedValue = false;    //  To detect redundant calls.

        protected readonly string ParameterSymbol = "@";

        protected SqlConnection _connection = null;

        protected SqlTransaction _transaction = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        protected BaseRepository(SqlConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected BaseRepository(SqlConnection connection, SqlTransaction transaction)
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

                using (SqlCommand sqlCommand = new SqlCommand(
                    command.CommandText.ToString(),
                    this._connection))
                {
                    DataTable dataTable = new DataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {
                        adapter.Fill(dataTable);
                    }

                    return dataTable;
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
                return Helpers.GetInstance().GetDataTableFromDataReader(command);
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
        protected SqlCommand CreateCommand(string cmdText, CommandType commandType = CommandType.Text, SqlParameter[] parameters = null)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    cmdText,
                    this._connection,
                    this._transaction)
                {

                    CommandTimeout = 0,
                    CommandType = commandType,
                };

                if (parameters != null &&
                    parameters.Any())
                    foreach (var parameter in parameters)
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);

                return command;
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
        protected SqlParameter CreateDataParameter(string parameterName, object value)
        {
            return new SqlParameter()
            {
                ParameterName = parameterName,
                Value = (value ?? DBNull.Value),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected SqlParameter[] GetDataParameters<T>(T entity) where T : class
        {
            IList<SqlParameter> dataParameters = null as IList<SqlParameter>;

            foreach (var property in entity.GetType().GetProperties())
            {
                if (dataParameters == null)
                    dataParameters = new List<SqlParameter>();

                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                string itemName = property.Name;

                if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name))
                    itemName = columnAttribute.Name.ToUpper();

                string parameterName = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    this.ParameterSymbol,
                    itemName);

                object parameterValue = property.GetValue(
                    entity,
                    null);

                SqlParameter item = this.CreateDataParameter(
                    parameterName,
                    parameterValue);

                dataParameters.Add(item);
            }

            return dataParameters.ToArray();
        }

        //protected virtual int Somar()
        //{
        //    return 2;
        //}

        protected string GetAllColumnsFromTable(string tableName, string alias = "")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(
                    nameof(tableName));

            StringBuilder sbColumns = new StringBuilder();

            string cmdText = $@" SELECT TOP 0 *
                                   FROM {tableName}
                                  WHERE 0 = 1 ";

            using (var reader = this.CreateCommand(
                cmdText).ExecuteReader())
            {
                reader.Read();

                using (var schemaTable = reader.GetSchemaTable())
                {
                    foreach (DataRow column in schemaTable.Rows)
                    {
                        if (sbColumns.Length > 0)
                        {
                            sbColumns.Append(", ");
                        }

                        if (!string.IsNullOrEmpty(alias))
                        {
                            sbColumns.AppendFormat(
                                CultureInfo.InvariantCulture,
                                "{0}.",
                                alias);
                        }

                        sbColumns.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "[{0}]",
                            column["ColumnName"].ToString());
                    }
                }

                reader.Close();
            }

            return sbColumns.ToString();
        }

        protected void MapAttributeToField(Type entityType)
        {
            var map = new CustomPropertyTypeMap(
                entityType,
                (type, columnName) => type.GetProperties().FirstOrDefault(prop => this.GetDescriptionFromAttribute(prop) == columnName));

            SqlMapper.SetTypeMap(entityType, map);
        }

        private string GetDescriptionFromAttribute(MemberInfo member)
        {
            if (member == null) return null;

            var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);
            return attrib == null ? null : attrib.Description;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    if (this._connection != null &&
                        this._connection.State == ConnectionState.Open)
                    {
                        this._connection.Dispose();
                        this._connection = null;
                    }

                    if (this._transaction != null)
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

/*
namespace Repository.SqlServer
{
    using Dapper;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class RepositoryBase
    {
        protected SqlConnection _connection = null;

        protected SqlTransaction _transaction = null;

        //protected SqlCommand CreateCommand(string cmdText)
        //{
        //    if (this._transaction != null)
        //    {
        //        return new SqlCommand(
        //            cmdText,
        //            base._connection,
        //            this._transaction);
        //    }

        //    return new SqlCommand(
        //        cmdText,
        //        base._connection);
        //}

        protected SqlCommand CreateCommand(string cmdText)
        {
            return new SqlCommand(
                cmdText,
                base._connection,
                this._transaction);
        }

        protected void MapAttributeToField(Type entityType)
        {
            var map = new CustomPropertyTypeMap(
                entityType,
                (type, columnName) => type.GetProperties().FirstOrDefault(prop => this.GetDescriptionFromAttribute(prop) == columnName));

            SqlMapper.SetTypeMap(entityType, map);
        }

        private string GetDescriptionFromAttribute(MemberInfo member)
        {
            if (member == null) return null;

            var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);
            return attrib == null ? null : attrib.Description;
        }

        protected string GetAllColumnsFromTable(string tableName, string alias = "")
        {
            StringBuilder sbColumns = new StringBuilder();

            string cmdText = $@" SELECT TOP 0 *
                                   FROM {tableName}
                                  WHERE 0 = 1 ";

            //            SELECT o.Name, c.Name
            //FROM     sys.columns c
            //         JOIN sys.objects o ON o.object_id = c.object_id
            //WHERE o.type = 'U'
            //ORDER BY o.Name, c.Name

            using (var reader = this.CreateCommand(
                cmdText).ExecuteReader())
            {
                reader.Read();

                using (var schemaTable = reader.GetSchemaTable())
                {
                    foreach (DataRow column in schemaTable.Rows)
                    {
                        if (sbColumns.Length > 0)
                        {
                            sbColumns.Append(", ");
                        }

                        if (!string.IsNullOrEmpty(alias))
                        {
                            sbColumns.AppendFormat(
                                CultureInfo.InvariantCulture,
                                "{0}.",
                                alias);
                        }

                        sbColumns.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "[{0}]",
                            column["ColumnName"].ToString());
                    }
                }
            }

            return sbColumns.ToString();
        }
    }
}
 */