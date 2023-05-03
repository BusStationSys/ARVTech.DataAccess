namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using Dapper;

    public class AnimalRepository : BaseRepository, IAnimalRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public AnimalRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    AnimalEntity));

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public AnimalRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                    AnimalEntity));

            this.MapAttributeToField(
                typeof(
                CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AnimalEntity Create(AnimalEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidAnimal UniqueIdentifier
                                            SET @NewGuidAnimal = NEWID()
                                    INSERT INTO [{0}].[dbo].[ANIMAIS]
                                                ([GUID],
                                                 [SBB],
                                                 [RP],
                                                 [NOME],
                                                 [SEXO],
                                                 [DATA_NASCIMENTO],
                                                 [IDPELAGEM],
                                                 [ALTURA],
                                                 [PESO],
                                                 [OPERACAO_BAIXA],
                                                 [DATA_BAIXA],
                                                 [GUIDCONTA],
                                                 [GUIDCABANHA],
                                                 [OBSERVACAO_BAIXA],
                                                 [GUIDANIMAL_PAI],
                                                 [SBB_PAI],
                                                 [RP_PAI],
                                                 [NOME_PAI],
                                                 [GUIDANIMAL_MAE],
                                                 [SBB_MAE],
                                                 [RP_MAE],
                                                 [NOME_MAE],
                                                 [IDTIPO])
                                         VALUES (@NewGuidAnimal,
                                                 {1}Sbb,
                                                 {1}Rp,
                                                 {1}Nome,
                                                 {1}Sexo,
                                                 {1}DataNascimento,
                                                 {1}IdPelagem,
                                                 {1}Altura,
                                                 {1}Peso,
                                                 {1}OperacaoBaixa,
                                                 {1}DataBaixa,
                                                 {1}GuidConta,
                                                 {1}GuidCabanha,
                                                 {1}ObservacaoBaixa,
                                                 {1}GuidAnimalPai,
                                                 {1}SbbPai,
                                                 {1}RpPai,
                                                 {1}NomePai,
                                                 {1}GuidAnimalMae,
                                                 {1}SbbMae,
                                                 {1}RpMae,
                                                 {1}NomeMae,
                                                 {1}IdTipo)
                                          SELECT @NewGuidAnimal ";

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
        /// Deletes the "Animal" record.
        /// </summary>
        /// <param name="guid">Guid of "Animal" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[ANIMAIS]
                                     WHERE GUID = {1}Guid ";

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
        /// Gets the "Animal" record.
        /// </summary>
        /// <param name="guid">Guid of "Animal" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public AnimalEntity Get(Guid guid)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string columnsCabanhas = this.GetAllColumnsFromTable(
                    "CABANHAS",
                    "C",
                    "C.[MARCA]");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [A].[GUIDCONTA] = [O].[GUID]
                                     INNER JOIN [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                             ON [A].[GUIDCABANHA] = [C].[GUID]
                                          WHERE UPPER(A.GUID) = @Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsAnimais,
                    columnsContas,
                    columnsCabanhas,
                    base._connection.Database);

                var animal = base._connection.Query<AnimalEntity, ContaEntity, CabanhaEntity, AnimalEntity>(
                    cmdText,
                    map: (mapAnimal, mapConta, mapCabanha) =>
                    {
                        mapAnimal.Conta = mapConta;
                        mapAnimal.Cabanha = mapCabanha;

                        return mapAnimal;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return animal.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Cabanhas" records.
        /// </summary>
        /// <returns>If success, the list with all "Cabanhas" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<AnimalEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsAnimais = this.GetAllColumnsFromTable("ANIMAIS", "A");

                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string columnsCabanhas = this.GetAllColumnsFromTable(
                    "CABANHAS",
                    "C",
                    "C.[MARCA]");

                //string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[ANIMAIS] as A WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [A].[GUIDCONTA] = [O].[GUID]
                                     INNER JOIN [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                             ON [A].[GUIDCABANHA] = [C].[GUID] ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsAnimais,
                    columnsContas,
                    columnsCabanhas,
                    base._connection.Database);

                var animal = base._connection.Query<AnimalEntity, ContaEntity, CabanhaEntity, AnimalEntity>(
                    cmdText,
                    map: (mapAnimal, mapConta, mapCabanha) =>
                    {
                        mapAnimal.Conta = mapConta;
                        mapAnimal.Cabanha = mapCabanha;

                        return mapAnimal;
                    },
                    splitOn: "GUID,GUID,GUID",
                    transaction: this._transaction);

                return animal;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all "Cabanhas" records by "Conta".
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <returns>If success, the list with all "Cabanhas" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<AnimalEntity> GetAllByGuidConta(Guid guidConta)
        {
            throw new NotImplementedException();

            //try
            //{
            //    if (guidConta == Guid.Empty)
            //        throw new ArgumentNullException(
            //            nameof(guidConta));

            //    IEnumerable<CabanhaEntity> cabanhas = null as IEnumerable<CabanhaEntity>;

            //    string cmdText = $@"    SELECT C.GUID,
            //                                   C.CNPJ,
            //                                   C.RAZAO_SOCIAL,
            //                                   C.IDASSOCIACAO,
            //                                   C.GUIDCONTA,
            //                                   C.NOME_FANTASIA,
            //                                   C.MARCA,
            //                                   C.CEP,
            //                                   C.ENDERECO,
            //                                   C.NUMERO,
            //                                   C.COMPLEMENTO,
            //                                   C.PONTO_REFERENCIA,
            //                                   C.BAIRRO,
            //                                   C.CIDADE,
            //                                   C.UF,
            //                                   C.RESPONSAVEL,
            //                                   C.EMAIL,
            //                                   C.TELEFONE,
            //                                   A.DESCRICAO_REGISTRO AS DESCRICAO_REGISTRO_ASSOCIACAO,
            //                                   A.RAZAO_SOCIAL AS RAZAO_SOCIAL_ASSOCIACAO,
            //                                   A.OBSERVACOES AS OBSERVACOES_ASSOCIACAO,
            //                                   A.SIGLA AS SIGLA_ASSOCIACAO
            //                              FROM [{base._connection.Database}].[dbo].[CABANHAS] AS C
            //                        INNER JOIN [{base._connection.Database}].[dbo].[ASSOCIACOES] AS A
            //                                ON C.IDASSOCIACAO = A.ID
            //                             WHERE UPPER(C.GUIDCONTA) = '{guidConta.ToString().ToUpper()}'
            //                          ORDER BY C.RAZAO_SOCIAL,
            //                                   C.GUID";

            //    using (SqlCommand command = this.CreateCommand(
            //        cmdText))
            //    {
            //        using (DataTable dt = this.GetDataTableFromDataReader(
            //            command))
            //        {
            //            //if (dt != null && dt.Rows.Count > 0)
            //            //    cabanhas = this.Popular(dt).ToList();
            //        }
            //    }

            //    return cabanhas;
            //}
            //catch
            //{
            //    throw;
            //}
        }

        /// <summary>
        /// Gets all "Cabanhas" records by "Conta" and Usuário.
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the list with all "Cabanhas" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<AnimalEntity> GetAllWithPermission(Guid guidConta, Guid guidUsuario)
        {
            throw new NotImplementedException();

            //try
            //{
            //    IEnumerable<CabanhaEntity> cabanhas = null as IEnumerable<CabanhaEntity>;

            //    string cmdText = @"CABANHASListarByGuidContaComPermissao";

            //    SqlParameter[] parameters = new SqlParameter[2];

            //    parameters[0] = new SqlParameter
            //    {
            //        ParameterName = "@GUIDCONTA",
            //        SqlDbType = SqlDbType.UniqueIdentifier,
            //        Direction = ParameterDirection.Input,
            //        Value = guidConta,
            //    };

            //    parameters[1] = new SqlParameter
            //    {
            //        ParameterName = "@GUIDUSUARIO",
            //        SqlDbType = SqlDbType.UniqueIdentifier,
            //        Direction = ParameterDirection.Input,
            //        Value = guidUsuario,
            //    };

            //    using (SqlCommand command = this.CreateCommand(
            //        cmdText,
            //        commandType: CommandType.StoredProcedure))
            //    {
            //        command.Parameters.AddRange(parameters.ToArray());

            //        using (DataTable dt = this.GetDataTableFromDataReader(command))
            //        {
            //            //if (dt != null && dt.Rows.Count > 0)
            //            //    cabanhas = this.ConvertToList<CabanhaEntity>(dt).ToList();
            //        }
            //    }

            //    return cabanhas;
            //}
            //catch
            //{
            //    throw;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AnimalEntity Update(AnimalEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[ANIMAIS]
                                       SET [SBB] = {1}Sbb,
                                           [RP] = {1}Rp,
                                           [NOME] = {1}Nome,
                                           [SEXO] = {1}Sexo,
                                           [DATA_NASCIMENTO] = {1}DataNascimento,
                                           [IDPELAGEM] = {1}IdPelagem,
                                           [ALTURA] = {1}Altura,
                                           [PESO] = {1}Peso,
                                           [OPERACAO_BAIXA] = {1}OperacaoBaixa,
                                           [DATA_BAIXA] = {1}Data_Baixa,
                                           [GUIDCONTA] = {1}GuidConta,
                                           [GUIDCABANHA] = {1}GuidCabanha,
                                           [OBSERVACAO_BAIXA] = {1}ObservacaoBaixa,
                                           [GUIDANIMAL_PAI] = {1}GuidAnimalPai,
                                           [SBB_PAI] = {1}SbbPai,
                                           [RP_PAI] = {1}RpPai,
                                           [NOME_PAI] = {1}NomePai,
                                           [GUIDANIMAL_MAE] = {1}GuidAnimalMae,
                                           [SBB_MAE] = {1}SbbMae,
                                           [RP_MAE] = {1}RpMae,
                                           [NOME_MAE] = {1}NomeMae,
                                           [IDTIPO] = {1}IdTipo
                                     WHERE GUID = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    this.ParameterSymbol);

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