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
                ////  Primeiramente, insere o registro na tabela "PESSOAS".
                //string cmdText = @"     DECLARE {1}NewGuidPessoa UniqueIdentifier
                //                            SET {1}NewGuidPessoa = NEWID()

                //                    INSERT INTO [{0}].[dbo].[PESSOAS]
                //                               ([GUID],
                //                                [BAIRRO],
                //                                [CEP],
                //                                [CIDADE],
                //                                [COMPLEMENTO],
                //                                [ENDERECO],
                //                                [NUMERO],
                //                                [UF])
                //                        VALUES ({1}NewGuidPessoa,
                //                                {1}Bairro,
                //                                {1}Cep,
                //                                {1}Cidade,
                //                                {1}Complemento,
                //                                {1}Endereco,
                //                                {1}Numero,
                //                                {1}Uf)

                //                         SELECT {1}NewGuidPessoa ";

                //cmdText = string.Format(
                //    CultureInfo.InvariantCulture,
                //    cmdText,
                //    base._connection.Database,
                //    base.ParameterSymbol);

                //var pessoaEntity = entity.Pessoa;

                //entity.GuidPessoa = this._connection.QuerySingle<Guid>(
                //    sql: cmdText,
                //    param: pessoaEntity,
                //    transaction: this._transaction);

                //  Por último, insere o registro na tabela "PESSOAS_FISICAS".
                string cmdText = @"     DECLARE {1}NewGuidPF UniqueIdentifier
                                     SET {1}NewGuidPF = NEWID()

                                  INSERT INTO [{0}].[dbo].[PESSOAS_FISICAS]
                                             ([GUID],
                                              [GUIDPESSOA],
                                              [CPF],
                                              [RG],
                                              [DATA_NASCIMENTO],
                                              [NOME])
                                      VALUES ({1}NewGuidPF,
                                              {1}GuidPessoa,
                                              {1}Cpf,
                                              {1}Rg,
                                              {1}DataNascimento,
                                              {1}Nome)

                                       SELECT {1}NewGuidPF ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

                var guid = this._connection.QuerySingle<Guid>(
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
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PessoaFisicaEntity Update(PessoaFisicaEntity entity)
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

//public class PessoaFisicaRepository : BaseRepository, IPessoaFisicaRepository
//{
//    private readonly string _columnsPessoas;
//    private readonly string _columnsPessoasFisicas;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="PessoaFisicaRepository"/> class.
//    /// </summary>
//    /// <param name="connection"></param>
//    public PessoaFisicaRepository(SqlConnection connection) :
//        base(connection)
//    {
//        base._connection = connection;

//        this.MapAttributeToField(
//            typeof(
//                PessoaFisicaEntity));

//        //this.MapAttributeToField(
//        //    typeof(
//        //        CabanhaEntity));

//        //this.MapAttributeToField(
//        //    typeof(
//        //        ContaEntity));

//        this._columnsPessoas = base.GetAllColumnsFromTable(
//            "PESSOAS",
//            "P");

//        this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
//            "PESSOAS_FISICAS",
//            "PF");
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="PessoaFisicaRepository"/> class.
//    /// </summary>
//    /// <param name="connection"></param>
//    /// <param name="transaction"></param>
//    public PessoaFisicaRepository(SqlConnection connection, SqlTransaction transaction) :
//        base(connection, transaction)
//    {
//        base._connection = connection;
//        this._transaction = transaction;

//        this.MapAttributeToField(
//            typeof(
//                PessoaFisicaEntity));

//        //this.MapAttributeToField(
//        //    typeof(
//        //    CabanhaEntity));

//        //this.MapAttributeToField(
//        //    typeof(
//        //        ContaEntity));

//        this._columnsPessoas = base.GetAllColumnsFromTable(
//            "PESSOAS",
//            "P");

//        this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
//            "PESSOAS_FISICAS",
//            "PF");
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="entity"></param>
//    /// <returns></returns>
//    public PessoaFisicaEntity Create(PessoaFisicaEntity entity)
//    {
//        try
//        {
//            string cmdText = @"     DECLARE @NewGuidAnimal UniqueIdentifier
//                                        SET @NewGuidAnimal = NEWID()
//                                INSERT INTO [{0}].[dbo].[ANIMAIS]
//                                            ([GUID],
//                                             [SBB],
//                                             [RP],
//                                             [NOME],
//                                             [SEXO],
//                                             [DATA_NASCIMENTO],
//                                             [IDPELAGEM],
//                                             [ALTURA],
//                                             [PESO],
//                                             [OPERACAO_BAIXA],
//                                             [DATA_BAIXA],
//                                             [GUIDCONTA],
//                                             [GUIDCABANHA],
//                                             [OBSERVACAO_BAIXA],
//                                             [GUIDANIMAL_PAI],
//                                             [SBB_PAI],
//                                             [RP_PAI],
//                                             [NOME_PAI],
//                                             [GUIDANIMAL_MAE],
//                                             [SBB_MAE],
//                                             [RP_MAE],
//                                             [NOME_MAE],
//                                             [IDTIPO])
//                                     VALUES (@NewGuidAnimal,
//                                             {1}Sbb,
//                                             {1}Rp,
//                                             {1}Nome,
//                                             {1}Sexo,
//                                             {1}DataNascimento,
//                                             {1}IdPelagem,
//                                             {1}Altura,
//                                             {1}Peso,
//                                             {1}OperacaoBaixa,
//                                             {1}DataBaixa,
//                                             {1}GuidConta,
//                                             {1}GuidCabanha,
//                                             {1}ObservacaoBaixa,
//                                             {1}GuidAnimalPai,
//                                             {1}SbbPai,
//                                             {1}RpPai,
//                                             {1}NomePai,
//                                             {1}GuidAnimalMae,
//                                             {1}SbbMae,
//                                             {1}RpMae,
//                                             {1}NomeMae,
//                                             {1}IdTipo)
//                                      SELECT @NewGuidAnimal ";

//            cmdText = string.Format(
//                CultureInfo.InvariantCulture,
//                cmdText,
//                base._connection.Database,
//                base.ParameterSymbol);

//            var guid = base._connection.QuerySingle<Guid>(
//                sql: cmdText,
//                param: entity,
//                transaction: this._transaction);

//            return this.Get(
//                guid);
//        }
//        catch
//        {
//            throw;
//        }
//    }

//    /// <summary>
//    /// Deletes the "Pessoa Física" record.
//    /// </summary>
//    /// <param name="guid">Guid of "Pessoa Física" record.</param>
//    public void Delete(Guid guid)
//    {
//        try
//        {
//            if (guid == Guid.Empty)
//                throw new ArgumentNullException(
//                    nameof(guid));

//            string cmdText = @"     DELETE PF
//                                      FROM [{0}].[dbo].[PESSOAS_FISICAS] PF
//                                INNER JOIN [{0}].[dbo].[PESSOAS] P
//                                        ON PF.[GUIDPESSOA] = P.[GUID]
//                                     WHERE PF.[GUID] = {1}Guid ";

//            cmdText = string.Format(
//                CultureInfo.InvariantCulture,
//                cmdText,
//                base._connection.Database,
//                this.ParameterSymbol);

//            base._connection.Execute(
//                cmdText,
//                new
//                {
//                    Guid = guid,
//                },
//                transaction: this._transaction);
//        }
//        catch
//        {
//            throw;
//        }
//    }

//    /// <summary>
//    /// Gets the "Pessoa Física" record.
//    /// </summary>
//    /// <param name="guid">Guid of "Pessoa Física" record.</param>
//    /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
//    public PessoaFisicaEntity Get(Guid guid)
//    {
//        throw new NotImplementedException();

//        //try
//        //{
//        //    if (guid == Guid.Empty)
//        //        throw new ArgumentNullException(
//        //            nameof(guid));

//        //    //  Maneira utilizada para trazer os relacionamentos 1:N.
//        //    string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

//        //    string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

//        //    string columnsCabanhas = this.GetAllColumnsFromTable(
//        //        "CABANHAS",
//        //        "C",
//        //        "C.[MARCA]");

//        //    string cmdText = @"      SELECT {0},
//        //                                    {1},
//        //                                    {2}
//        //                               FROM [{3}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
//        //                         INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
//        //                                 ON [A].[GUIDCONTA] = [O].[GUID]
//        //                         INNER JOIN [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
//        //                                 ON [A].[GUIDCABANHA] = [C].[GUID]
//        //                              WHERE UPPER(A.GUID) = {4}Guid ";

//        //    cmdText = string.Format(
//        //        CultureInfo.InvariantCulture,
//        //        cmdText,
//        //        columnsAnimais,
//        //        columnsContas,
//        //        columnsCabanhas,
//        //        base._connection.Database,
//        //        base.ParameterSymbol);

//        //    var animal = base._connection.Query<PessoaFisicaEntity, ContaEntity, CabanhaEntity, AnimalEntity>(
//        //        cmdText,
//        //        map: (mapAnimal, mapConta, mapCabanha) =>
//        //        {
//        //            mapAnimal.Conta = mapConta;
//        //            mapAnimal.Cabanha = mapCabanha;

//        //            return mapAnimal;
//        //        },
//        //        param: new
//        //        {
//        //            Guid = guid,
//        //        },
//        //        splitOn: "GUID,GUID,GUID",
//        //        transaction: this._transaction);

//        //    return animal.FirstOrDefault();
//        //}
//        //catch
//        //{
//        //    throw;
//        //}
//    }

//    /// <summary>
//    /// Get all "Pessoas Físicas" records.
//    /// </summary>
//    /// <returns>If success, the list with all "Pessoas Físicas" records. Otherwise, an exception detailing the problem.</returns>
//    public IEnumerable<PessoaFisicaEntity> GetAll()
//    {
//        try
//        {
//            //  Maneira utilizada para trazer os relacionamentos 1:N.
//            string cmdText = @"      SELECT {0},
//                                            {1}
//                                       FROM [{2}].[dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
//                                 INNER JOIN [{2}].[dbo].[PESSOAS] as P WITH(NOLOCK)
//                                         ON [PF].[GUIDPESSOA] = [P].[GUID] ";

//            cmdText = string.Format(
//                CultureInfo.InvariantCulture,
//                cmdText,
//                this._columnsPessoasFisicas,
//                this._columnsPessoas,
//                base._connection.Database);

//            var pessoasFisicas = base._connection.Query<PessoaFisicaEntity, PessoaEntity, PessoaFisicaEntity>(
//                cmdText,
//                map: (mapPessoaFisica, mapPessoa) =>
//                {
//                    mapPessoaFisica.Pessoa = mapPessoa;

//                    return mapPessoaFisica;
//                },
//                splitOn: "GUID,GUID",
//                transaction: this._transaction);

//            return pessoasFisicas;

//            // var pessoaJuridica = base._connection.Query<PessoaJuridicaEntity>(
//            //     cmdText,
//            //     this._transaction);

//            //return pessoaJuridica;
//        }
//        catch
//        {
//            throw;
//        }
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="entity"></param>
//    /// <returns></returns>
//    public PessoaFisicaEntity Update(PessoaFisicaEntity entity)
//    {
//        throw new NotImplementedException();

//        //try
//        //{
//        //    string cmdText = @" UPDATE [{0}].[dbo].[ANIMAIS]
//        //                           SET [SBB] = {1}Sbb,
//        //                               [RP] = {1}Rp,
//        //                               [NOME] = {1}Nome,
//        //                               [SEXO] = {1}Sexo,
//        //                               [DATA_NASCIMENTO] = {1}DataNascimento,
//        //                               [IDPELAGEM] = {1}IdPelagem,
//        //                               [ALTURA] = {1}Altura,
//        //                               [PESO] = {1}Peso,
//        //                               [OPERACAO_BAIXA] = {1}OperacaoBaixa,
//        //                               [DATA_BAIXA] = {1}Data_Baixa,
//        //                               [GUIDCONTA] = {1}GuidConta,
//        //                               [GUIDCABANHA] = {1}GuidCabanha,
//        //                               [OBSERVACAO_BAIXA] = {1}ObservacaoBaixa,
//        //                               [GUIDANIMAL_PAI] = {1}GuidAnimalPai,
//        //                               [SBB_PAI] = {1}SbbPai,
//        //                               [RP_PAI] = {1}RpPai,
//        //                               [NOME_PAI] = {1}NomePai,
//        //                               [GUIDANIMAL_MAE] = {1}GuidAnimalMae,
//        //                               [SBB_MAE] = {1}SbbMae,
//        //                               [RP_MAE] = {1}RpMae,
//        //                               [NOME_MAE] = {1}NomeMae,
//        //                               [IDTIPO] = {1}IdTipo
//        //                         WHERE GUID = {1}Guid ";

//        //    cmdText = string.Format(
//        //        CultureInfo.InvariantCulture,
//        //        cmdText,
//        //        base._connection.Database,
//        //        this.ParameterSymbol);

//        //    base._connection.Execute(
//        //        cmdText,
//        //        param: entity,
//        //        transaction: this._transaction);

//        //    return this.Get(
//        //        entity.Guid);
//        //}
//        //catch
//        //{
//        //    throw;
//        //}
//    }
//}
