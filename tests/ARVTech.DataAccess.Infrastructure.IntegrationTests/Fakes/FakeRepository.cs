namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fakes
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer;

    [ExcludeFromCodeCoverage]
    public class FakeRepository : BaseRepository
    {
        public FakeRepository(
            SqlConnection connection,
            SqlTransaction? transaction = null)
            : base(
                  connection, 
                  transaction) { }

        public SqlCommand ExposedCreateCommand(
            string cmdText,
            CommandType commandType = CommandType.Text,
            SqlParameter[]? parameters = null)
        {
            return this.CreateCommand(
                cmdText,
                commandType,
                parameters);
        }

        public SqlParameter ExposedCreateDataParameter(string parameterName, object value)
        { 
            return this.CreateDataParameter(
                parameterName,
                value);
        }

        public DataTable ExposedGetDataTableFromDataAdapter(IDbCommand command)
        {
            return this.GetDataTableFromDataAdapter(command);
        }

        public string ExposedGetAllColumnsFromTable(string tableName, string alias = "", string fieldsToIgnore = "")
        { 
            return this.GetAllColumnsFromTable(tableName, alias, fieldsToIgnore); 
        }

        public IEnumerable<T> ExposedLoadData<T, U>(string sql, U parameters)
        { 
            return this.LoadData<T, U>(sql, parameters); 
        }

        //  Para o teste de Dispose
        public SqlConnection ExposedConnection =>
            (SqlConnection)typeof(BaseRepository)
                .GetField("_connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .GetValue(this)!;

        public void ExposedMapAttributeToField(Type entityType)
        { 
            this.MapAttributeToField(
                entityType);
        }
    }
}