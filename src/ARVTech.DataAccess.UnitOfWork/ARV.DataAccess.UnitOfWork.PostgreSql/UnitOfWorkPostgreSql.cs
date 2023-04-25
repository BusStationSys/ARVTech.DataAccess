namespace FlooString.UnitOfWork.PostgreSql
{
    using System;
    using FlooString.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkPostgreSql  : IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration = null as IConfiguration;

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration
        {
            get
            {
                return this._configuration;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; private set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public UnitOfWorkPostgreSql(IConfiguration configuration = null)
        {
            this._configuration = configuration;

            if (this._configuration["ConnectionStrings:PostgreSql"] != null &&
                !string.IsNullOrEmpty(this._configuration["ConnectionStrings:PostgreSql"].ToString()))
                this.ConnectionString = this._configuration["ConnectionStrings:PostgreSql"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IUnitOfWorkAdapter Create()
        {
            try
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new Exception("[ERRO] String de Conexão para PostgreSQL não encontrada.");

                return new UnitOfWorkPostgreSqlAdapter(this.ConnectionString);
            }
            catch
            {
                throw;
            }
        }
    }
}