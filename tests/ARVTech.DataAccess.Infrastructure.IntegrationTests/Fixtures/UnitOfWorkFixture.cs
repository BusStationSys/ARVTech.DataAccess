namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures
{
    using System;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkFixture : IDisposable
    {
        private bool _disposedValue;

        public IUnitOfWorkAdapter UnitOfWorkAdapter { get; private set; }

        public UnitOfWorkFixture()
        {
            // Aponta para o appsettings.json da aplicação principal
            //var appSettingsPath = Path.Combine(
            //    AppDomain.CurrentDomain.BaseDirectory,
            //    "..",  //  tests/
            //    "..",  //  src/
            //    "..",  //  raiz da solução
            //    "src",
            //    "ARVTech.DataAccess.Console");

            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            //    ?? "Production";

            //var config = new ConfigurationBuilder().SetBasePath(
            //    Path.GetFullPath(
            //        appSettingsPath)).AddJsonFile(
            //    "appsettings.json",
            //    optional: false).AddJsonFile(
            //        $"appsettings.{environment}.json",
            //        optional: true).AddEnvironmentVariables().Build();   // CI/CD.

            //var config = new ConfigurationBuilder()
            //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .AddJsonFile($"appsettings.{environment}.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var unitOfWork = new UnitOfWorkSqlServer(
                config);

            this.UnitOfWorkAdapter = unitOfWork.Create();

            this.UnitOfWorkAdapter.BeginTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        this.UnitOfWorkAdapter?.Rollback();
                    }
                    catch
                    {
                        //  Qualquer falha no Rollback é ignorada durante o Dispose.
                        //  O finally garante a limpeza dos recursos.
                    }
                    finally
                    {
                        this.UnitOfWorkAdapter?.Dispose();
                        this.UnitOfWorkAdapter = null;
                    }
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}