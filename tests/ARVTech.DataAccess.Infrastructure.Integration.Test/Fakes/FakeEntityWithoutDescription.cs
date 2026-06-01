namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fakes
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class FakeEntityWithoutDescription
    {
        public int Id { get; set; }

        public string? Nome { get; set; }
    }
}