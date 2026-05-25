namespace ARVTech.DataAccess.Infrastructure.IntegrationTests.Fixtures
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer;
    using Microsoft.Extensions.Configuration;

    [ExcludeFromCodeCoverage]
    public class UnitOfWorkFixture : IDisposable
    {
        private bool _disposedValue;

        public IUnitOfWorkAdapter UnitOfWorkAdapter { get; private set; }

        public UnitOfWorkFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var unitOfWork = new UnitOfWorkSqlServer(config);

            this.UnitOfWorkAdapter = unitOfWork.Create();

            this.UnitOfWorkAdapter.BeginTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                //  UnitOfWorkFixture não possui recursos não gerenciados.
                //  O if (disposing) é desnecessário aqui.
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