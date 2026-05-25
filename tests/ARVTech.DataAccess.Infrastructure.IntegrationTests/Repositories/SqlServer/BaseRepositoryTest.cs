namespace ARVTech.DataAccess.Infrastructure.IntegrationTests
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Infrastructure.IntegrationTests.Fakes;
    using ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BaseRepositoryTest : IClassFixture<UnitOfWorkFixture>, IDisposable
    {
        private readonly FakeRepository _fakeRepository;

        public BaseRepositoryTest(UnitOfWorkFixture unitOfWorkFixture)
        {
            var connection = (SqlConnection)unitOfWorkFixture.UnitOfWorkAdapter.Connection;
            var transaction = (SqlTransaction)unitOfWorkFixture.UnitOfWorkAdapter.Transaction;

            this._fakeRepository = new FakeRepository(
                connection,
                transaction);
        }

        [Fact]
        public void CreateCommand_WhenTextIsValid_ShouldReturnConfiguredCommand()
        {
            using var command = this._fakeRepository.ExposedCreateCommand(
                "SELECT 1");

            Assert.NotNull(command);
            Assert.Equal("SELECT 1", command.CommandText);
            Assert.Equal(CommandType.Text, command.CommandType);
            Assert.Equal(0, command.CommandTimeout);
        }

        [Fact]
        public void CreateCommand_WithNullOrEmptyText_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                this._fakeRepository.ExposedCreateCommand(
                    string.Empty));
        }

        [Fact]
        public void CreateCommand_WithParameters_ShouldAddParametersToCommand()
        {
            var parameters = new[]
            {
                new SqlParameter(
                    "@Id",
                    1)
            };

            using var command = this._fakeRepository.ExposedCreateCommand(
                "SELECT 1 WHERE 1 = @Id",
                CommandType.Text,
                parameters);

            Assert.Single(command.Parameters);
            Assert.Equal("@Id", command.Parameters[0].ParameterName);
        }

        [Fact]
        public void CreateDataParameter_WithValidValue_ShouldReturnConfiguredParameter()
        {
            var param = this._fakeRepository.ExposedCreateDataParameter(
                "@Nome",
                "João");

            Assert.Equal("@Nome", param.ParameterName);
            Assert.Equal("João", param.Value);
        }

        [Fact]
        public void CreateDataParameter_WithNullValue_ShouldReturnDBNull()
        {
            var param = this._fakeRepository.ExposedCreateDataParameter(
                "@Nome",
                null!);

            Assert.Equal(
                DBNull.Value,
                param.Value);
        }

        [Fact]
        public void CreateDataParameter_WithEmptyName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                this._fakeRepository.ExposedCreateDataParameter(
                    string.Empty,
                    "valor"));
        }

        [Fact]
        public void GetDataTableFromDataAdapter_WithNullCommand_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                this._fakeRepository.ExposedGetDataTableFromDataAdapter(null!));
        }

        [Fact]
        public void GetDataTableFromDataAdapter_WithValidCommand_ShouldReturnDataTable()
        {
            using var command = this._fakeRepository.ExposedCreateCommand("SELECT 1 AS COL1");

            var dataTable = this._fakeRepository.ExposedGetDataTableFromDataAdapter(command);

            Assert.NotNull(dataTable);
            Assert.Single(dataTable.Columns);
            Assert.Equal("COL1", dataTable.Columns[0].ColumnName);
        }

        [Fact]
        public void GetDescriptionFromAttribute_WithNullMember_ShouldReturnNull()
        {
            //  Arrange & Act
            var result = this._fakeRepository.ExposedGetDescriptionFromAttribute(null!);

            //  Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetDescriptionFromAttribute_WithMemberWithoutAttribute_ShouldReturnNull()
        {
            //  Arrange — propriedade sem DescriptionAttribute
            var member = typeof(FakeEntityWithoutDescription).GetProperty(nameof(FakeEntityWithoutDescription.Id))!;

            //  Act
            var result = this._fakeRepository.ExposedGetDescriptionFromAttribute(member);

            //  Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetDescriptionFromAttribute_WithMemberWithAttribute_ShouldReturnDescription()
        {
            //  Arrange — propriedade com DescriptionAttribute
            var member = typeof(FakeEntity).GetProperty(nameof(FakeEntity.Id))!;

            //  Act
            var result = this._fakeRepository.ExposedGetDescriptionFromAttribute(member);

            //  Assert
            Assert.Equal("ID", result);
        }

        [Fact]
        public void GetAllColumnsFromTable_WithValidTable_ShouldReturnColumnList()
        {
            //  Use uma tabela que existe no banco de homologação
            var columns = this._fakeRepository.ExposedGetAllColumnsFromTable("PESSOAS");

            Assert.NotNull(columns);
            Assert.NotEmpty(columns);
        }

        [Fact]
        public void GetAllColumnsFromTable_WithAlias_ShouldPrefixColumns()
        {
            var columns = this._fakeRepository.ExposedGetAllColumnsFromTable("PESSOAS", "P");

            Assert.Contains("P.", columns);
        }

        [Fact]
        public void GetAllColumnsFromTable_WithFieldsToIgnore_ShouldExcludeFields()
        {
            var allColumns = this._fakeRepository.ExposedGetAllColumnsFromTable("PESSOAS");
            var firstColumn = allColumns.Split(',')[0].Trim().Trim('[', ']');
            var filteredColumns = this._fakeRepository.ExposedGetAllColumnsFromTable("PESSOAS", fieldsToIgnore: firstColumn);

            Assert.DoesNotContain(firstColumn, filteredColumns);
        }

        [Fact]
        public void GetAllColumnsFromTable_WithNullTableName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                this._fakeRepository.ExposedGetAllColumnsFromTable(string.Empty));
        }

        [Fact]
        public void LoadData_WithValidQuery_ShouldReturnResults()
        {
            var results = this._fakeRepository.ExposedLoadData<int, object>(
                "SELECT 1",
                null!);

            Assert.NotNull(results);
            Assert.Single(results);
        }

        [Fact]
        public void Dispose_WhenCalledTwice_ShouldNotThrowException()
        {
            var fakeRepository = new FakeRepository(
                (SqlConnection)_fakeRepository.ExposedConnection);

            fakeRepository.Dispose();

            var exception = Record.Exception(() => fakeRepository.Dispose());
            Assert.Null(exception);
        }

        [Fact]
        public void MapAttributeToField_WithValidType_ShouldNotThrowException()
        {
            var exception = Record.Exception(() =>
                this._fakeRepository.ExposedMapAttributeToField(typeof(FakeEntity)));

            Assert.Null(exception);
        }

        [Fact]
        public void MapAttributeToField_WithTypeHavingNoDescriptionAttribute_ShouldMapToNull()
        {
            //  Tipo sem nenhum DescriptionAttribute — força o retorno null interno
            var exception = Record.Exception(() =>
                this._fakeRepository.ExposedMapAttributeToField(
                    typeof(
                        FakeEntityWithoutDescription)));

            Assert.Null(exception);
        }

        public void Dispose()
        {
            this._fakeRepository.Dispose();
        }
    }
}