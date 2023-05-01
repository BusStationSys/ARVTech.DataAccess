namespace ARVTech.DataAccess.Repository.SqlServer.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using Dapper;

    public class CabanhaRepository : BaseRepository, ICabanhaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CabanhaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        public CabanhaRepository(SqlConnection connection) :
            base(connection)
        {
            base._connection = connection;

            this.MapAttributeToField(
                typeof(
                    CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabanhaRepository"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public CabanhaRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
            base._connection = connection;
            this._transaction = transaction;

            this.MapAttributeToField(
                typeof(
                CabanhaEntity));

            this.MapAttributeToField(
                typeof(
                AssociacaoEntity));

            this.MapAttributeToField(
                typeof(
                    ContaEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CabanhaEntity Create(CabanhaEntity entity)
        {
            try
            {
                string cmdText = @"     DECLARE @NewGuidCabanha UniqueIdentifier
                                            SET @NewGuidCabanha = NEWID()
                                    INSERT INTO [{0}].[dbo].[CABANHAS]
                                                ([GUID],
                                                 [CNPJ],
                                                 [RAZAO_SOCIAL],
                                                 [IDASSOCIACAO],
                                                 [GUIDCONTA],
                                                 [NOME_FANTASIA],
                                                 [MARCA],
                                                 [CEP],
                                                 [ENDERECO],
                                                 [NUMERO],
                                                 [COMPLEMENTO],
                                                 [PONTO_REFERENCIA],
                                                 [BAIRRO],
                                                 [CIDADE],
                                                 [UF],
                                                 [RESPONSAVEL],
                                                 [EMAIL],
                                                 [TELEFONE])
                                         VALUES (@NewGuidCabanha,
                                                 {1}Cnpj,
                                                 {1}RazaoSocial,
                                                 {1}IdAssociacao,
                                                 {1}GuidConta,
                                                 {1}NomeFantasia,
                                                 {1}Marca,
                                                 {1}Cep,
                                                 {1}Endereco,
                                                 {1}Numero,
                                                 {1}Complemento,
                                                 {1}PontoReferencia,
                                                 {1}Bairro,
                                                 {1}Cidade,
                                                 {1}Uf,
                                                 {1}Responsavel,
                                                 {1}EMail,
                                                 {1}Telefone)
                                          SELECT @NewGuidCabanha ";

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
        /// Deletes the "Cabanha" record.
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        public void Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                string cmdText = @" DELETE
                                      FROM [{0}].[dbo].[CABANHAS]
                                     WHERE GUID = {1}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    base._connection.Database,
                    base.ParameterSymbol);

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
        /// Checks if the record exists by "CNPJ" of "Cabanha".
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        /// <param name="cnpj">"CNPJ" of "Cabanha" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteCNPJDuplicado(Guid guid, string cnpj)
        {
            try
            {
                var cmdText = new StringBuilder();

                cmdText.Append("      SELECT C.GUID ");

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "        FROM [{0}].[dbo].[CABANHAS] as C WITH(NOLOCK) ",
                    base._connection.Database);

                cmdText.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "       WHERE C.CNPJ = {0}Cnpj ",
                    base.ParameterSymbol);

                if (guid != Guid.Empty)
                {
                    cmdText.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "         AND UPPER(C.GUID) != {0}Guid ",
                        base.ParameterSymbol);
                }

                var cabanha = base._connection.QueryFirstOrDefault(
                    cmdText.ToString(),
                    param: new
                    {
                        Cnpj = cnpj,
                        Guid = guid,
                    },
                    transaction: this._transaction);

                return cabanha != null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the "Cabanha" record.
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        /// <returns>If success, the object with the persistent database record. Otherwise, an exception detailing the problem.</returns>
        public CabanhaEntity Get(Guid guid)
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                             ON [C].[IDASSOCIACAO] = [A].[ID]
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [C].[GUIDCONTA] = [O].[GUID]
                                          WHERE UPPER(C.GUID) = {4}Guid ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsCabanhas,
                    columnsAssociacoes,
                    columnsContas,
                    base._connection.Database,
                    base.ParameterSymbol);

                var cabanha = base._connection.Query<CabanhaEntity, AssociacaoEntity, ContaEntity, CabanhaEntity>(
                    cmdText,
                    map: (mapCabanha, mapAssociacao, mapConta) =>
                    {
                        mapCabanha.Associacao = mapAssociacao;
                        mapCabanha.Conta = mapConta;

                        return mapCabanha;
                    },
                    param: new
                    {
                        Guid = guid,
                    },
                    splitOn: "GUID,ID,GUID",
                    transaction: this._transaction);

                return cabanha.FirstOrDefault();
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
        public IEnumerable<CabanhaEntity> GetAll()
        {
            try
            {
                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                             ON [C].[IDASSOCIACAO] = [A].[ID]
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [C].[GUIDCONTA] = [O].[GUID]";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsCabanhas,
                    columnsAssociacoes,
                    columnsContas,
                    base._connection.Database);

                var cabanhas = base._connection.Query<CabanhaEntity, AssociacaoEntity, ContaEntity, CabanhaEntity>(
                    cmdText,
                    map: (mapCabanha, mapAssociacao, mapConta) =>
                    {
                        mapCabanha.Associacao = mapAssociacao;
                        mapCabanha.Conta = mapConta;

                        return mapCabanha;
                    },
                    splitOn: "GUID,ID,GUID",
                    transaction: this._transaction);

                return cabanhas;
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
        public IEnumerable<CabanhaEntity> GetAllByGuidConta(Guid guidConta)
        {
            try
            {
                if (guidConta == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidConta));

                //  Maneira utilizada para trazer os relacionamentos 1:N.
                string columnsCabanhas = this.GetAllColumnsFromTable("CABANHAS", "C");
                string columnsAssociacoes = this.GetAllColumnsFromTable("ASSOCIACOES", "A");
                string columnsContas = this.GetAllColumnsFromTable("CONTAS", "O");

                string cmdText = @"      SELECT {0},
                                                {1},
                                                {2}
                                           FROM [{3}].[dbo].[CABANHAS] as C WITH(NOLOCK)
                                     INNER JOIN [{3}].[dbo].[ASSOCIACOES] as A WITH(NOLOCK)
                                             ON [C].[IDASSOCIACAO] = [A].[ID]
                                     INNER JOIN [{3}].[dbo].[CONTAS] as O WITH(NOLOCK)
                                             ON [C].[GUIDCONTA] = [O].[GUID]
                                          WHERE UPPER(C.GUIDCONTA) = '{4}' ";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    columnsCabanhas,
                    columnsAssociacoes,
                    columnsContas,
                    base._connection.Database,
                    guidConta.ToString().ToUpper());

                var cabanhas = base._connection.Query<CabanhaEntity, AssociacaoEntity, ContaEntity, CabanhaEntity>(
                    cmdText,
                    map: (mapCabanha, mapAssociacao, mapConta) =>
                    {
                        mapCabanha.Associacao = mapAssociacao;
                        mapCabanha.Conta = mapConta;

                        return mapCabanha;
                    },
                    splitOn: "GUID,ID,GUID",
                    transaction: this._transaction);

                return cabanhas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all "Cabanhas" records by "Conta" and "Usuário".
        /// </summary>
        /// <param name="guidConta">Guid of "Conta".</param>
        /// <param name="guidUsuario">Guid of "Usuário".</param>
        /// <returns>If success, the list with all "Cabanhas" records according to the parameters. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<CabanhaEntity> GetAllWithPermission(Guid guidConta, Guid guidUsuario)
        {
            try
            {
                string cmdText = @"CABANHASListarByGuidContaComPermissao";

                var parameters = new DynamicParameters();

                parameters.Add(
                    "GUIDCONTA",
                    guidConta,
                    DbType.Guid,
                    ParameterDirection.Input);

                parameters.Add(
                    "GUIDUSUARIO",
                    guidUsuario,
                    DbType.Guid,
                    ParameterDirection.Input);

                var cabanhas = base._connection.Query<CabanhaEntity>(
                    cmdText,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return cabanhas;
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
        public CabanhaEntity Update(CabanhaEntity entity)
        {
            try
            {
                string cmdText = @" UPDATE [{0}].[dbo].[CABANHAS]
                                       SET [CNPJ] = {1}Cnpj,
                                           [RAZAO_SOCIAL] = {1}RazaoSocial,
                                           [IDASSOCIACAO] = {1}IdAssociacao,
                                           [GUIDCONTA] = {1}GuidConta,
                                           [NOME_FANTASIA] = {1}NomeFantasia,
                                           [MARCA] = {1}Marca,
                                           [CEP] = {1}Cep,
                                           [ENDERECO] = {1}Endereco,
                                           [NUMERO] = {1}Numero,
                                           [COMPLEMENTO] = {1}Complemento,
                                           [PONTO_REFERENCIA] = {1}PontoReferencia,
                                           [BAIRRO] = {1}Bairro,
                                           [CIDADE] = {1}Cidade,
                                           [UF] = {1}Uf,
                                           [RESPONSAVEL] = {1}Responsavel,
                                           [EMAIL] = {1}EMail,
                                           [TELEFONE] = {1}Telefone
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