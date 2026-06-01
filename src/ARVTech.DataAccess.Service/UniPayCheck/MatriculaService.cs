namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using ARVTech.Shared;
    using ARVTech.Shared.Security.Interfaces;
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class MatriculaService : BaseService, IMatriculaService
    {
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="passwordHasher"></param>
        public MatriculaService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher) :
            base(unitOfWork, mapper)
        {
            this._passwordHasher = passwordHasher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaResponseDto>(
                        entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaResponseDto> GetAniversariantesEmpresa(int mes)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entities = connection.RepositoriesUniPayCheck.MatriculaRepository.GetAniversariantesEmpresa(
                        mes);

                    return this._mapper.Map<IEnumerable<MatriculaResponseDto>>(
                        entities);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public MatriculaResponseDto GetByMatricula(string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(
                            matricula));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaRepository.GetByMatricula(
                        matricula);

                    return this._mapper.Map<MatriculaResponseDto>(
                        entity);
                }
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
        public void Delete(Guid guid)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.MatriculaRepository.Delete(
                    guid);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ResumoImportacaoMatriculasResponseDto ImportFileMatriculas(string cnpj, string content)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var (dataInicio, dataFim, quantidadeRegistrosAtualizados, quantidadeRegistrosInalterados, quantidadeRegistrosInseridos) = connection.RepositoriesUniPayCheck.MatriculaRepository.ImportFileMatriculas(
                        cnpj,
                        content);

                    var matriculasCredenciaisUsuarios = connection.RepositoriesUniPayCheck.MatriculaRepository.GetToCredenciaisUsuarios(
                        dataInclusao: dataInicio);

                    if (matriculasCredenciaisUsuarios != null &&
                        matriculasCredenciaisUsuarios.Any())    //  Retorna as Matrículas aptas para criar as Credenciais (UserName e Password para acesso à aplicação) entre Matrículas x Usuarios.
                    {
                        var dataTable = new DataTable();

                        dataTable.Columns.Add("GuidColaborador", typeof(Guid));
                        dataTable.Columns.Add("Username", typeof(string));
                        dataTable.Columns.Add("PasswordHash", typeof(string));

                        foreach (var matriculaCredencialUsuario in matriculasCredenciaisUsuarios)
                        {
                            //  Processa o Nome do Colaborador para criar o UserName seguindo a regra: primeiro nome + "." + sobrenome, tudo em letras minúsculas.
                            string username = matriculaCredencialUsuario.Colaborador.Nome;

                            string firstName = Common.GetFirstName(
                                username).ToLower();
                            string lastName = Common.GetLastName(
                                username).ToLower();

                            username = string.Concat(
                                firstName,
                                ".",
                                lastName);

                            //  Criptografa a Matrícula usando o Hash para criar o PasswordHash para acesso à aplicação.
                            string passwordHash = this._passwordHasher.Hash(
                                matriculaCredencialUsuario.Matricula);

                            dataTable.Rows.Add(
                                matriculaCredencialUsuario.GuidColaborador, 
                                username, 
                                passwordHash);
                        }

                        //  Implementar método de sincronização de Credenciais entre Matrículas x Usuarios passando o dataTable.
                        var (quantidadeRegistrosAtualizadosSincronizacao, quantidadeRegistrosInalteradosSincronizacao, quantidadeRegistrosInseridosSincronizacao) = connection.RepositoriesUniPayCheck.MatriculaRepository.SincronizarCredenciaisMatriculasUsuarios(
                            dataTable);

                        dataTable.Dispose();
                    }

                    return new ResumoImportacaoMatriculasResponseDto
                    {
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        QuantidadeRegistrosAtualizados = quantidadeRegistrosAtualizados,
                        QuantidadeRegistrosInalterados = quantidadeRegistrosInalterados,
                        QuantidadeRegistrosInseridos = quantidadeRegistrosInseridos,
                    };
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MatriculaResponseDto SaveData(MatriculaRequestCreateDto? createDto = null, MatriculaRequestUpdateDto? updateDto = null)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (createDto != null && updateDto != null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar preenchidos ao mesmo tempo.");
                else if (createDto is null && updateDto is null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar vazios ao mesmo tempo.");
                else if (updateDto != null && updateDto.Guid == Guid.Empty)
                    throw new InvalidOperationException($"É necessário o preenchimento do {nameof(updateDto.Guid)}.");

                var entity = default(
                    MatriculaEntity);

                connection.BeginTransaction();

                decimal salarioNominal = 0.01m;

                if (updateDto != null)
                {
                    salarioNominal = updateDto.SalarioNominal;

                    entity = this._mapper.Map<MatriculaEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    salarioNominal = createDto.SalarioNominal;

                    entity = this._mapper.Map<MatriculaEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaRepository.Create(
                        entity);
                }

                //  Atualiza o Salário Nominal criptografando a informação usando como chave o GuidMatricula.
                var key = entity.Guid.ToString("N").ToUpper();

                //entity.SalarioNominal = PasswordCryptography.EncryptString(
                //    key,
                //    salarioNominal.ToString("#,###,###,##0.00"));

                entity = connection.RepositoriesUniPayCheck.MatriculaRepository.Update(
                    entity.Guid,
                    entity);

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaResponseDto>(
                    entity);
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}