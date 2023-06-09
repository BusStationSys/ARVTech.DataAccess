namespace ARVTech.DataAccess.UnitOfWork.SqlServer
{
    using System;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly IConfiguration _configuration = null;

        public IConfiguration Configuration
        {
            get
            {
                return this._configuration;
            }
        }

        public string ConnectionString { get; private set; } = string.Empty;

        public UnitOfWorkSqlServer(IConfiguration configuration = null)
        {
            this._configuration = configuration;

            if (this._configuration["ConnectionStrings:SqlServer"] != null &&
                !string.IsNullOrEmpty(this._configuration["ConnectionStrings:SqlServer"].ToString()))
                this.ConnectionString = this._configuration["ConnectionStrings:SqlServer"].ToString();
        }

        public IUnitOfWorkAdapter Create()
        {
            try
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new Exception("[ERRO] String de Conexão para SQL Server não encontrada.");

                return new UnitOfWorkSqlServerAdapter(
                    this.ConnectionString);
            }
            catch
            {
                throw;
            }
        }
    }
}