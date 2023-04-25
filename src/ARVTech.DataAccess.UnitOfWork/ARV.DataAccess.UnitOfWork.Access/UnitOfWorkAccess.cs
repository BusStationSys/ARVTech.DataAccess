namespace ARV.DataAccess.UnitOfWork.Access
{
    using System;
    using System.Globalization;
    using System.IO;
    using ARV.DataAccess.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWorkAccess : IUnitOfWork
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
        public UnitOfWorkAccess(IConfiguration configuration = null)
        {
            this._configuration = configuration;

            if (this._configuration["ConnectionStrings:Access"] != null &&
                !string.IsNullOrEmpty(this._configuration["ConnectionStrings:Access"].ToString()))
            {
                this.ConnectionString = this._configuration["ConnectionStrings:Access"].ToString();

                this.ConnectionString = string.Format(
                    CultureInfo.InvariantCulture,
                    this.ConnectionString,
                    Directory.GetCurrentDirectory());
            }
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
                    throw new Exception("[ERRO] String de Conexão para Access não encontrada.");

                return new UnitOfWorkAccessAdapter(this.ConnectionString);
            }
            catch
            {
                throw;
            }
        }
    }
}