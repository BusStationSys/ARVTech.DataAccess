namespace ARVTech.DataAccess.DbManager
{
    using System;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.UnitOfWork.SqlServer;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class ContextDbManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration = null as IConfiguration;

        /// <summary>
        /// 
        /// </summary>
        private readonly DatabaseTypeEnum? _databaseType = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = null as IUnitOfWork;

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
        public DatabaseTypeEnum? DatabaseType
        {
            get
            {
                return this._databaseType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return this._unitOfWork;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ContextDbManager(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (configuration["DatabaseType"] == null ||
                string.IsNullOrEmpty(configuration["DatabaseType"].ToString()))
                throw new Exception("[ERRO] Variável DatabaseType não encontrada ou não preenchida.");

            string databaseType = configuration["DatabaseType"].ToString();

            if (!Enum.IsDefined(typeof(DatabaseTypeEnum), databaseType))
                throw new Exception($"[ERRO] DatabaseType [ {databaseType} ] não configurado.");

            this._databaseType = (DatabaseTypeEnum)Enum.Parse(typeof(DatabaseTypeEnum), databaseType);

            switch (this._databaseType)
            {
                case DatabaseTypeEnum.Access:
                    //this._unitOfWork = new UnitOfWorkAccess(
                    //    this._configuration);
                    break;
                case DatabaseTypeEnum.Firebird:
                    break;
                case DatabaseTypeEnum.Interbase:
                    break;
                case DatabaseTypeEnum.MySql:
                    //this._unitOfWork = new UnitOfWorkMySql(
                    //    this._configuration);
                    break;
                case DatabaseTypeEnum.Oracle:
                    break;
                case DatabaseTypeEnum.Pervasive:
                    break;
                case DatabaseTypeEnum.Postgresql:
                    break;
                case DatabaseTypeEnum.SqlServer:
                    this._unitOfWork = new UnitOfWorkSqlServer(
                        this._configuration);
                    break;
                default:
                    break;
            }

            if (this._unitOfWork == null)
                throw new Exception($"[ERRO] Conexão ao {databaseType} não configurado.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="configuration"></param>
        public ContextDbManager(DatabaseTypeEnum databaseType, IConfiguration configuration)
        {
            this._databaseType = databaseType;
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            switch (this._databaseType)
            {
                case DatabaseTypeEnum.Access:
                    //this._unitOfWork = new UnitOfWorkAccess(
                    //    this._configuration);
                    break;
                case DatabaseTypeEnum.Firebird:
                    break;
                case DatabaseTypeEnum.Interbase:
                    break;
                case DatabaseTypeEnum.MySql:
                    break;
                case DatabaseTypeEnum.Oracle:
                    break;
                case DatabaseTypeEnum.Pervasive:
                    break;
                case DatabaseTypeEnum.Postgresql:
                    break;
                case DatabaseTypeEnum.SqlServer:
                    this._unitOfWork = new UnitOfWorkSqlServer(
                        this._configuration);
                    break;
                default:
                    break;
            }

            if (this._unitOfWork == null)
                throw new Exception($"[ERRO] Conexão ao {databaseType} não configurado.");
        }
    }
}