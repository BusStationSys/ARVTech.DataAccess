namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Business.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Shared;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class MatriculaBusiness : BaseBusiness, IMatriculaBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaBusiness"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
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
        /// <param name="matriculaResult"></param>
        /// <returns></returns>
        public ExecutionResponseDto<MatriculaResponseDto> Import(MatriculaResult matriculaResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Empregador pelo CNPJ.
                var pessoaJuridicaResponseDto = default(
                    PessoaJuridicaResponseDto);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    this._unitOfWork))
                {
                    pessoaJuridicaResponseDto = pessoaJuridicaBusiness.GetByCnpj(
                        matriculaResult.Cnpj.Replace(
                            ".",
                            string.Empty).Replace(
                                "/",
                                string.Empty).Replace(
                                    "-",
                                    string.Empty));

                    //  Se não existir o registro do Empregador, volta para a chamada anterior, exibe uma mensagem e passa para o próximo registro.
                    if (pessoaJuridicaResponseDto is null)
                        return new ExecutionResponseDto<MatriculaResponseDto>
                        {
                            Message = $"Pessoa Jurídica não encontrada para o CNPJ {matriculaResult.Cnpj}. O registro deve ser cadastrado/importado préviamente.",
                        };
                }

                //  Verifica se existe o registro do Colaborador pelo CPF.
                var pessoaFisicaResponseDto = default(
                    PessoaFisicaResponseDto);

                using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(
                    this._unitOfWork))
                {
                    string cep = matriculaResult.Cep.Replace(
                        ".",
                        string.Empty).Replace(
                            "-",
                            string.Empty);

                    string cpf = matriculaResult.Cpf.Replace(
                        ".",
                        string.Empty).Replace(
                            "-",
                            string.Empty);

                    pessoaFisicaResponseDto = pessoaFisicaBusiness.GetByCpf(
                        cpf);

                    //  Se não existir o registro do Colaborador, deve incluir o registro.
                    if (pessoaFisicaResponseDto is null)
                    {
                        var pessoaFisicaRequestCreateDto = new PessoaFisicaRequestCreateDto
                        {
                            Nome = matriculaResult.Nome,
                            DataNascimento = Convert.ToDateTime(
                                matriculaResult.DataNascimento),
                            Cpf = cpf,
                            NumeroCtps = matriculaResult.NumeroCtps,
                            SerieCtps = matriculaResult.SerieCtps,
                            UfCtps = matriculaResult.UfCtps,
                            Rg = matriculaResult.Rg,
                            Pessoa = new PessoaRequestCreateDto()
                            {
                                Bairro = matriculaResult.Bairro,
                                Cep = cep,
                                Cidade = matriculaResult.Cidade,
                                Uf = matriculaResult.Uf,
                                Endereco = matriculaResult.Logradouro,
                                Numero = matriculaResult.NumeroLogradouro,
                                Complemento = matriculaResult.Complemento,
                                Email = matriculaResult.Email,
                                Telefone = matriculaResult.Telefone,
                            },
                        };

                        pessoaFisicaResponseDto = pessoaFisicaBusiness.SaveData(
                            pessoaFisicaRequestCreateDto);
                    }
                }

                // Verifica se existe o registro do Usuário.
                string username = string.Empty;

                string password = matriculaResult.Matricula;

                var usuariosResponseDto = default(
                    IEnumerable<UsuarioResponseDto>);

                using (var usuarioBusiness = new UsuarioBusiness(
                    this._unitOfWork))
                {
                    var firstName = Common.GetFirstName(
                        pessoaFisicaResponseDto.Nome);

                    var lastName = Common.GetLastName(
                        pessoaFisicaResponseDto.Nome);

                    username = string.Concat(
                        firstName.ToLower(),
                        '.',
                        lastName.ToLower());

                    usuariosResponseDto = usuarioBusiness.GetByUsername(
                        username);
                }

                //  Se não existir o registro do Usuário, deve incluir o registro.
                if (usuariosResponseDto?.Count() == 0)
                {
                    var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                    {
                        GuidColaborador = pessoaFisicaResponseDto.Guid,
                        Username = username,
                        ConfirmPassword = password,
                        Password = password,
                    };

                    using (var usuarioBusiness = new UsuarioBusiness(
                        this._unitOfWork))
                    {
                        usuarioBusiness.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                //  Verifica se existe o registro da Matrícula pelo Número.
                var matriculaResponseDto = default(
                    MatriculaResponseDto);

                using (var matriculaBusiness = new MatriculaBusiness(
                    this._unitOfWork))
                {
                    matriculaResponseDto = matriculaBusiness.GetByMatricula(
                        matriculaResult.Matricula);

                    //  Se não existir o registro da Matrícula, deve incluir o registro.
                    if (matriculaResponseDto is null)
                    {
                        var matriculaRequestCreateDto = new MatriculaRequestCreateDto
                        {
                            DataAdmissao = Convert.ToDateTime(
                                matriculaResult.DataAdmissao),
                            DataDemissao = matriculaResult.DataDemissao != "00/00/0000" ?
                                Convert.ToDateTime(
                                    matriculaResult.DataDemissao) :
                                null,
                            DescricaoCargo = matriculaResult.DescricaoCargo,
                            DescricaoSetor = matriculaResult.DescricaoSetor,
                            FormaPagamento = matriculaResult.FormaPagamento,
                            Agencia = matriculaResult.Agencia,
                            Banco = matriculaResult.Banco,
                            Conta = matriculaResult.Conta,
                            DvConta = matriculaResult.DvConta,
                            GuidColaborador = pessoaFisicaResponseDto.Guid,
                            GuidEmpregador = pessoaJuridicaResponseDto.Guid,
                            Matricula = matriculaResult.Matricula,
                            SalarioNominal = Convert.ToDecimal(
                                matriculaResult.SalarioNominal),
                        };

                        //  if (matriculaRequestCreateDto.FormaPagamento != "R" &&
                        //        string.IsNullOrEmpty(matriculaRequestCreateDto.DvConta))
                        //       matriculaRequestCreateDto.DvConta = "0";

                        matriculaResponseDto = matriculaBusiness.SaveData(
                            createDto: matriculaRequestCreateDto);
                    }
                }

                connection.CommitTransaction();

                return new ExecutionResponseDto<MatriculaResponseDto>
                {
                    Data = matriculaResponseDto,
                    Success = true,
                };
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

                entity.SalarioNominal = PasswordCryptography.EncryptString(
                    key,
                    salarioNominal.ToString("#,###,###,##0.00"));

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
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}