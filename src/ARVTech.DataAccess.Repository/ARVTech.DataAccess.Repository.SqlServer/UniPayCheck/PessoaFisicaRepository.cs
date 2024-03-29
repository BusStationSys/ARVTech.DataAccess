﻿namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using Dapper;

    public class PessoaFisicaRepository : BaseRepository, IPessoaFisicaRepository
    {
        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasFisicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaFisicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public PessoaFisicaRepository(SqlConnection connection) :
            base(connection)
        {
            this._connection = connection;

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this._columnsPessoas = base.GetAllColumnsFromTable(
                "PESSOAS",
                "P");

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                "PESSOAS_FISICAS",
                "PF");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaFisicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaFisicaRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            this._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    PessoaFisicaEntity));

            this.MapAttributeToField(
                typeof(
                    PessoaEntity));

            this._columnsPessoas = base.GetAllColumnsFromTable(
                "PESSOAS",
                "P");

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                "PESSOAS_FISICAS",
                "PF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaFisicaEntity Create(PessoaFisicaEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid();

                //  Insere o registro na tabela "PESSOAS_FISICAS".
                string cmdText = @" INSERT INTO [{0}].[dbo].[PESSOAS_FISICAS]
                                             ([GUID],
                                              [GUIDPESSOA],
                                              [CPF],
                                              [RG],
                                              [DATA_NASCIMENTO],
                                              [NOME],
                                              [NUMERO_CTPS],
                                              [SERIE_CTPS],
                                              [UF_CTPS])
                                      VALUES ({1}Guid,
                                              {1}GuidPessoa,
                                              {1}Cpf,
                                              {1}Rg,
                                              {1}DataNascimento,
                                              {1}Nome,
                                              {1}NumeroCtps,
                                              {1}SerieCtps,
                                              {1}UfCtps) ";

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
        /// Deletes the "Pessoa Física" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Física" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @"     DELETE PF
                                          FROM [{0}].[dbo].[PESSOAS_FISICAS] PF
                                    INNER JOIN [{0}].[dbo].[PESSOAS] P
                                            ON PF.[GUIDPESSOA] = P.[GUID]
                                         WHERE PF.[GUID] = {1}Guid ";

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
        /// Gets the "Pessoa Física" record.
        /// </summary>
        /// <param name="guid">Guid of "Pessoa Física" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaFisicaEntity Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PF].[GUIDPESSOA] = [P].[GUID]
                                          WHERE UPPER(PF.GUID) = {3}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasFisicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    cmdText,
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Pessoas Físicas" records.
        /// </summary>
        /// <returns>If success, the list with all "Pessoas Físicas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PessoaFisicaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PF].[GUIDPESSOA] = [P].[GUID] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasFisicas,
                    this._columnsPessoas,
                    base._connection.Database);

                var pessoasFisicas = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    cmdText,
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoasFisicas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Física" record by "Nome".
        /// </summary>
        /// <param name="nome">"Nome" of "Pessoa Física" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaFisicaEntity GetByNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PF].[GUIDPESSOA] = [P].[GUID]
                                          WHERE PF.NOME = {3}Nome ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasFisicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    cmdText,
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Nome = nome,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Pessoa Física" record by "Nome", "Número Ctps", "Série Ctps" and "Uf Ctps".
        /// </summary>
        /// <param name="nome">"Nome" of "Pessoa Física" record.</param>
        /// <param name="numeroCtps"></param>
        /// <param name="serieCtps"></param>
        /// <param name="ufCtps"></param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public PessoaFisicaEntity GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));
                else if (string.IsNullOrEmpty(numeroCtps))
                    throw new ArgumentNullException(
                        nameof(
                            numeroCtps));
                else if (string.IsNullOrEmpty(serieCtps))
                    throw new ArgumentNullException(
                        nameof(
                            serieCtps));
                else if (string.IsNullOrEmpty(ufCtps))
                    throw new ArgumentNullException(
                        nameof(
                            ufCtps));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string cmdText = @"      SELECT {0},
                                                {1}
                                           FROM [{2}].[dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
                                     INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
                                             ON [PF].[GUIDPESSOA] = [P].[GUID]
                                          WHERE PF.NOME = {3}Nome 
                                            AND PF.NUMERO_CTPS = {3}NumeroCtps
                                            AND PF.SERIE_CTPS = {3}SerieCtps
                                            AND PF.UF_CTPS = {3}UfCtps ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._columnsPessoasFisicas,
                    this._columnsPessoas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var pessoaFisica = this._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
                    cmdText,
                    map: (mapPessoaFisica, mapPessoa) =>
                    {
                        mapPessoaFisica.Pessoa = mapPessoa;

                        return mapPessoaFisica;
                    },
                    param: new
                    {
                        Nome = nome,
                        NumeroCtps = numeroCtps,
                        SerieCtps = serieCtps,
                        UfCtps = ufCtps,
                    },
                    splitOn: "GUID,GUID",
                    transaction: this._transaction);

                return pessoaFisica.FirstOrDefault();
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
        public PessoaFisicaEntity Update(Guid guid, PessoaFisicaEntity entity)
        {
            try
            {
                if (entity.Guid == Guid.Empty)
                    throw new NullReferenceException(
                        nameof(entity.Guid));
                else if (entity.GuidPessoa == Guid.Empty)
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
                                         WHERE [GUID] = {1}GuidPessoa ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                //entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                //    sql: cmdText,
                //    param: entity.Pessoa,
                //    transaction: this._transaction);

                this._connection.Execute(
                    cmdText,
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_FISICAS".
                entity.Guid = guid;

                cmdText = @"     UPDATE [{0}].[dbo].[PESSOAS_FISICAS]
                                    SET [CPF] = {1}Cpf,
                                        [RG] = {1}Rg,
                                        [DATA_NASCIMENTO] = {1}DataNascimento,
                                        [NOME] = {1}Nome
                                  WHERE [GUID] = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                //var guid = base._connection.QuerySingle<Guid>(
                //    sql: cmdText,
                //    param: entity,
                //    transaction: this._transaction);

                this._connection.Execute(
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