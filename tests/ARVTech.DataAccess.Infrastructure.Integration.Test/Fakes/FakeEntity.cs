namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fakes
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class FakeEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("NOME")]
        public string? Nome { get; set; }
    }
}