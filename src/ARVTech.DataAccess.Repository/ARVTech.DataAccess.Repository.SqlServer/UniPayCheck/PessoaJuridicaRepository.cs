namespace ARVTech.DataAccess.Repository.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.UniPayCheck;
    using Dapper;

    public class PessoaJuridicaRepository : BaseRepository, IPessoaJuridicaRepository
    {
        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasJuridicas;

        /// <summary>
        /// Initializes a new instance of the <see cref="PessoaJuridicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public PessoaJuridicaRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

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
        /// Initializes a new instance of the <see cref="PessoaJuridicaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaJuridicaRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            base._connection = connection;
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
                //  Primeiramente, insere o registro na tabela "PESSOAS".
                string cmdText = @"     DECLARE {1}NewGuidPessoa UniqueIdentifier
                                            SET {1}NewGuidPessoa = NEWID()

                                    INSERT INTO [{0}].[dbo].[PESSOAS]
                                               ([GUID],
                                                [BAIRRO],
                                                [CEP],
                                                [CIDADE],
                                                [COMPLEMENTO],
                                                [ENDERECO],
                                                [NUMERO],
                                                [UF])
                                        VALUES ({1}NewGuidPessoa,
                                                {1}Bairro,
                                                {1}Cep,
                                                {1}Cidade,
                                                {1}Complemento,
                                                {1}Endereco,
                                                {1}Numero,
                                                {1}Uf)

                                         SELECT {1}NewGuidPessoa ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                    sql: cmdText,
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_JURÍDICAS".
                cmdText = @"     DECLARE {1}NewGuidPJ UniqueIdentifier
                                     SET {1}NewGuidPJ = NEWID()

                                  INSERT INTO [{0}].[dbo].[PESSOAS_JURIDICAS]
                                             ([GUID],
                                              [GUIDPESSOA],
                                              [CNPJ],
                                              [DATA_FUNDACAO],
                                              [RAZAO_SOCIAL])
                                      VALUES ({1}NewGuidPJ,
                                              {1}GuidPessoa,
                                              {1}Cnpj,
                                              {1}DataFundacao,
                                              {1}RazaoSocial)

                                       SELECT {1}NewGuidPJ ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                var guid = base._connection.QuerySingle<Guid>(
                    sql: cmdText,
                    param: entity,
                    transaction: this._transaction);

                return this.Get(
                    guid);
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
                    this.ParameterSymbol);

                base._connection.Execute(
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

                var pessoaJuridica = base._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
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

                var pessoasJuridicas = base._connection.Query<PessoaJuridicaEntity, PessoaEntity, PessoaJuridicaEntity>(
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
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaJuridicaEntity Update(PessoaJuridicaEntity entity)
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

                //entity.GuidPessoa = base._connection.QuerySingle<Guid>(
                //    sql: cmdText,
                //    param: entity.Pessoa,
                //    transaction: this._transaction);

                base._connection.Execute(
                    cmdText,
                    param: entity.Pessoa,
                    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_JURÍDICAS".
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

                //var guid = base._connection.QuerySingle<Guid>(
                //    sql: cmdText,
                //    param: entity,
                //    transaction: this._transaction);

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

            //try
            //{
            //    string cmdText = @" UPDATE [{0}].[dbo].[ANIMAIS]
            //                           SET [SBB] = {1}Sbb,
            //                               [RP] = {1}Rp,
            //                               [NOME] = {1}Nome,
            //                               [SEXO] = {1}Sexo,
            //                               [DATA_NASCIMENTO] = {1}DataNascimento,
            //                               [IDPELAGEM] = {1}IdPelagem,
            //                               [ALTURA] = {1}Altura,
            //                               [PESO] = {1}Peso,
            //                               [OPERACAO_BAIXA] = {1}OperacaoBaixa,
            //                               [DATA_BAIXA] = {1}Data_Baixa,
            //                               [GUIDCONTA] = {1}GuidConta,
            //                               [GUIDCABANHA] = {1}GuidCabanha,
            //                               [OBSERVACAO_BAIXA] = {1}ObservacaoBaixa,
            //                               [GUIDANIMAL_PAI] = {1}GuidAnimalPai,
            //                               [SBB_PAI] = {1}SbbPai,
            //                               [RP_PAI] = {1}RpPai,
            //                               [NOME_PAI] = {1}NomePai,
            //                               [GUIDANIMAL_MAE] = {1}GuidAnimalMae,
            //                               [SBB_MAE] = {1}SbbMae,
            //                               [RP_MAE] = {1}RpMae,
            //                               [NOME_MAE] = {1}NomeMae,
            //                               [IDTIPO] = {1}IdTipo
            //                         WHERE GUID = {1}Guid ";

            //    cmdText = string.Format(
            //        CultureInfo.InvariantCulture,
            //        cmdText,
            //        base._connection.Database,
            //        this.ParameterSymbol);

            //    base._connection.Execute(
            //        cmdText,
            //        param: entity,
            //        transaction: this._transaction);

            //    return this.Get(
            //        entity.Guid);
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}