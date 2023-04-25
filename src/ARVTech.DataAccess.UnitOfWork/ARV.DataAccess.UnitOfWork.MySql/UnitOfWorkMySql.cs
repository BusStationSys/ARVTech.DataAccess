namespace FlooString.UnitOfWork.MySql
{
    using System;
    using FlooString.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkMySql : IUnitOfWork
    {
        private readonly IConfiguration _configuration = null as IConfiguration;

        public IConfiguration Configuration
        {
            get
            {
                return this._configuration;
            }
        }

        public string ConnectionString { get; private set; } = string.Empty;

        public UnitOfWorkMySql(IConfiguration configuration = null)
        {
            this._configuration = configuration;

            if (this._configuration["ConnectionStrings:MySql"] != null &&
                !string.IsNullOrEmpty(this._configuration["ConnectionStrings:MySql"].ToString()))
                this.ConnectionString = this._configuration["ConnectionStrings:MySql"].ToString();
        }

        public IUnitOfWorkAdapter Create()
        {
            try
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new Exception("[ERRO] String de Conexão para MySQL não encontrada.");

                return new UnitOfWorkMySqlAdapter(this.ConnectionString);
            }
            catch
            {
                throw;
            }
        }
    }
}