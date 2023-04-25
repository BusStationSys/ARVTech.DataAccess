namespace ARVTech.DataAccess.Repository.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class Helpers
    {
        private static Helpers instance = null as Helpers;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Helpers GetInstance()
        {
            if (instance == null)
                instance = new Helpers();

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        private Helpers()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            string providerInvariantName = command.Connection.GetType().Namespace;

            IDbDataAdapter dataAdapter = DbProviderFactories
                .GetFactory(providerInvariantName)
                .CreateDataAdapter();

            dataAdapter.SelectCommand = command;

            return dataAdapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(IDbCommand command)
        {
            try
            {
                if (command == null)
                    throw new ArgumentNullException(nameof(command));

                IDbDataAdapter dataAdapter = this.CreateDataAdapter(command);

                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataAdapter"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet<T>(IDbDataAdapter dataAdapter)
        {
            try
            {
                if (dataAdapter == null)
                    throw new ArgumentNullException(nameof(dataAdapter));

                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet);

                return dataSet;
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
        public DataTable GetDataTableFromDataReader(IDbCommand command)
        {
            try
            {
                if (command == null)
                    throw new ArgumentNullException(nameof(command));

                using (IDataReader dataReader = command.ExecuteReader())
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        dataTable.Load(dataReader);

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
        /// Converts a <see cref="IEnumerable{T}"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public IEnumerable<T> ConvertToList<T>(DataTable dataTable)
        {
            try
            {
                var columnNames = dataTable.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName.ToUpper())
                    .ToList();

                return dataTable.AsEnumerable().Select(row =>
                {
                    var instance = Activator.CreateInstance<T>();

                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                        string itemName = property.Name;

                        if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name))
                            itemName = columnAttribute.Name.ToUpper();

                        if (columnNames.Contains(itemName.ToUpper()))
                        {
                            object objectValue = !row.IsNull(itemName) ? row[itemName] : null;

                            property.SetValue(
                                instance,
                                objectValue);
                        }
                    }

                    return instance;
                });
            }
            catch
            {
                throw;
            }
        }
    }
}