namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Dapper;

    public class PessoaJuridicaRepository : BaseRepository, IPessoaJuridicaRepository
    {
        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaJuridicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaJuridicaRepository(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PessoaJuridicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this._columnsPessoas = base.GetAllColumnsFromTable(
                "PESSOAS",
                "P");

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                "PESSOAS_JURIDICAS",
                "PJ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaJuridicaEntity Create(PessoaJuridicaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                // Insere o registro na tabela "PESSOAS_JURÍDICAS".
                string cmdText = @" INSERT INTO [{0}].[dbo].[PESSOAS_JURIDICAS]
                                             ([GUID],
                                              [GUIDPESSOA],
                                              [CNPJ],
                                              [DATA_FUNDACAO],
                                              [RAZAO_SOCIAL])
                                      VALUES ({1}Guid,
                                              {1}GuidPessoa,
                                              {1}Cnpj,
                                              {1}DataFundacao,
                                              {1}RazaoSocial) ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    sql: cmdText,
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the "Pessoa Jurídica" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Jurídica" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @"     DELETE PJ
                                          FROM [{0}].[dbo].[PESSOAS_JURIDICAS] PJ
                                    INNER JOIN [{0}].[dbo].[PESSOAS] P
                                            ON PJ.[GUIDPESSOA] = P.[GUID]
                                         WHERE PJ.[GUID] = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    cmdText,
                    new
                    {
                        Guid = guid,
                    },
                    transaction: this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Jurídica" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaJuridicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PJ].[GUIDPESSOA] = [P].[GUID]
                                          WHERE UPPER(PJ.GUID) = {3}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasJuridicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaJuridica = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    cmdText,
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaJuridica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Pessoas Jurídicas" records.
        /// </summary>
        /// <returns>If success, the list with all "Pessoas Jurídicas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PessoaJuridicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PJ].[GUIDPESSOA] = [P].[GUID] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasJuridicas,
                    this._columnsPessoas,
                    base._connection.Database);

                var pessoasJuridicas = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    cmdText,
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoasJuridicas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record by "Razão Social".
        /// </summary>
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaJuridicaEntity GetByRazaoSocial(string razaoSocial)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(
                        nameof(
                            razaoSocial));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PJ].[GUIDPESSOA] = [P].[GUID] 
                                          WHERE PJ.RAZAO_SOCIAL = {3}RazaoSocial ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasJuridicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    cmdText,
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Jurídica" record by "Razão Social" And "Cnpj".
        /// </summary>
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaJuridicaEntity GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(
                        nameof(
                            razaoSocial));
                else if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(
                        nameof(
                            cnpj));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PJ].[GUIDPESSOA] = [P].[GUID] 
                                          WHERE PJ.RAZAO_SOCIAL = {3}RazaoSocial
                                            AND PJ.CNPJ = {3}Cnpj ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasJuridicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaJuridicaEntity = this._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
                    cmdText,
                    map: (mapPessoaJuridica, mapPessoa) =>
                    {
                        mapPessoaJuridica.Pessoa = mapPessoa;

                        return mapPessoaJuridica;
                    },
                    param: new
                    {
                        RazaoSocial = razaoSocial,
                        Cnpj = cnpj,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaJuridicaEntity.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaJuridicaEntity Update(Guid guid, PessoaJuridicaEntity entity)
        {
            try
            {
                if (entity.Guid == Guid.Empty)
                    throw new NullReferenceException(
                        nameof(entity.Guid));
                else if (entity.GuidPessoa == Guid.Empty ||
                    entity.Pessoa.Guid == Guid.Empty)
                {
                    throw new NullReferenceException(
                        nameof(entity.GuidPessoa));
                }

                //  Primeiramente, atualiza o registro na tabela "PESSOAS".
                string cmdText = @"     UPDATE [{0}].[dbo].[PESSOAS]
                                           SET [BAIRRO] = {1}Bairro,
                                               [CEP] = {1}Cep,
                                               [CIDADE] = {1}Cidade,
                                               [COMPLEMENTO] = {1}Complemento,
                                               [ENDERECO] = {1}Endereco,
                                               [NUMERO] = {1}Numero,
                                               [UF] = {1}Uf
                                         WHERE [GUID] = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                this._connection.Execute(
                    cmdText,
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, atualiza o registro na tabela "PESSOAS_JURÍDICAS".
                entity.Guid = guid;

                cmdText = @"     UPDATE [{0}].[dbo].[PESSOAS_JURIDICAS]
                                    SET [CNPJ] = {1}Cnpj,
                                        [DATA_FUNDACAO] = {1}DataFundacao,
                                        [RAZAO_SOCIAL] = {1}RazaoSocial
                                  WHERE [GUID] = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                base._connection.Execute(
                    cmdText,
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    entity.Guid);
            }
            catch
            {
                throw;
            }
        }
    }
}