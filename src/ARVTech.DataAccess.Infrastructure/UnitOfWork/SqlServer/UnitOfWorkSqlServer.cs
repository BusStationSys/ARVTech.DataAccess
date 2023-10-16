namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly IConfiguration _configuration;

        public IConfiguration Configuration
        {
            get
            {
                return this._configuration;
            }
        }

        public UnitOfWorkSqlServer(IConfiguration configuration)
        {
            this._configuration = configuration;

            //if (this._configuration["ConnectionStrings:SqlServer"] != null &&
            //    !string.IsNullOrEmpty(this._configuration["ConnectionStrings:SqlServer"].ToString()))
            //    this.ConnectionString = this._configuration["ConnectionStrings:SqlServer"].ToString();
        }

        public IUnitOfWorkAdapter Create(int connectionTimeout = 0, string applicationName = "")
        {
            try
            {
                if (this._configuration == null)
                    throw new Exception("String de Conexão não encontrada.");

                return new UnitOfWorkSqlServerAdapter(
                    this._configuration,
                    connectionTimeout,
                    applicationName);
            }
            catch
            {
                throw;
            }
        }
    }
}