namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Business.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Enums;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Shared;
    using ARVTech.Shared.Extensions;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class PessoaJuridicaBusiness : BaseBusiness, IPessoaJuridicaBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public PessoaJuridicaBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaRequestUpdateDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<UnidadeNegocioResponseDto, UnidadeNegocioEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
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

                connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.Delete(
                    guid);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

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
        /// <param name="guid"></param>
        /// <returns></returns>
        public PessoaJuridicaResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.Get(
                        guid);

                    return this._mapper.Map<PessoaJuridicaResponseDto>(entity);
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
        /// <returns></returns>
        public IEnumerable<PessoaJuridicaResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.GetAll();

                    return this._mapper.Map<IEnumerable<PessoaJuridicaResponseDto>>(entity);
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
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public PessoaJuridicaResponseDto GetByCnpj(string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(cnpj))
                    throw new ArgumentNullException(
                        nameof(
                            cnpj));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.GetByCnpj(
                        cnpj);

                    return this._mapper.Map<PessoaJuridicaResponseDto>(
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
        /// <param name="razaoSocial"></param>
        /// <returns></returns>
        public PessoaJuridicaResponseDto GetByRazaoSocial(string razaoSocial)
        {
            try
            {
                if (string.IsNullOrEmpty(razaoSocial))
                    throw new ArgumentNullException(
                        nameof(
                            razaoSocial));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.GetByRazaoSocial(
                        razaoSocial);

                    return this._mapper.Map<PessoaJuridicaResponseDto>(entity);
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
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public PessoaJuridicaResponseDto GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj)
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

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.GetByRazaoSocialAndCnpj(
                        razaoSocial,
                        cnpj);

                    return this._mapper.Map<PessoaJuridicaResponseDto>(entity);
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
        /// <param name="pessoaJuridicaResult"></param>
        /// <returns></returns>
        public ExecutionResponseDto<PessoaJuridicaResponseDto> Import(EmpregadorResult pessoaJuridicaResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Empregador pelo CNPJ.
                var pessoaJuridicaResponseDto = default(
                    PessoaJuridicaResponseDto);

                using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(
                    this._unitOfWork))
                {
                    var idBandeiraComercial = (UnidadeNegocioEnum)Enum.Parse(
                        typeof(
                            UnidadeNegocioEnum),
                        pessoaJuridicaResult.BandeiraComercial.RemoveDiacritics());

                    string cep = pessoaJuridicaResult.Cep.Replace(
                        ".",
                        string.Empty).Replace(
                            "-",
                            string.Empty);

                    string cnpj = pessoaJuridicaResult.Cnpj.Replace(
                        ".",
                        string.Empty).Replace(
                            "/",
                            string.Empty).Replace(
                                "-",
                                string.Empty);

                    pessoaJuridicaResponseDto = this.GetByCnpj(
                        cnpj);

                    //  Se não existir o registro do Empregador, deve incluir o registro.
                    if (pessoaJuridicaResponseDto is null)
                    {
                        var pessoaJuridicaRequestCreateDto = new PessoaJuridicaRequestCreateDto
                        {
                            IdBandeiraComercial = idBandeiraComercial,
                            RazaoSocial = pessoaJuridicaResult.RazaoSocial,
                            DataFundacao = Convert.ToDateTime(
                                pessoaJuridicaResult.DataFundacao),
                            Cnpj = cnpj,
                            Pessoa = new PessoaRequestCreateDto()
                            {
                                Bairro = pessoaJuridicaResult.Bairro,
                                Cep = cep,
                                Cidade = pessoaJuridicaResult.Cidade,
                                Uf = pessoaJuridicaResult.Uf,
                                Endereco = pessoaJuridicaResult.Logradouro,
                                Numero = pessoaJuridicaResult.NumeroLogradouro,
                                Complemento = pessoaJuridicaResult.Complemento,
                                Email = pessoaJuridicaResult.Email,
                                Telefone = pessoaJuridicaResult.Telefone,
                            },
                        };

                        pessoaJuridicaResponseDto = this.SaveData(
                            pessoaJuridicaRequestCreateDto);
                    }
                    else    //  Se existir, apenas atualiza as informações.
                    {
                        var pessoaJuridicaRequestUpdateDto = new PessoaJuridicaRequestUpdateDto
                        {
                            Guid = pessoaJuridicaResponseDto.Guid,
                            IdBandeiraComercial = idBandeiraComercial,
                            RazaoSocial = pessoaJuridicaResult.RazaoSocial,
                            DataFundacao = Convert.ToDateTime(
                                pessoaJuridicaResult.DataFundacao),
                            Cnpj = pessoaJuridicaResponseDto.Cnpj,
                            GuidPessoa = pessoaJuridicaResponseDto.GuidPessoa,
                            Pessoa = new PessoaRequestUpdateDto()
                            {
                                Bairro = pessoaJuridicaResult.Bairro,
                                Cep = cep,
                                Cidade = pessoaJuridicaResult.Cidade,
                                Uf = pessoaJuridicaResult.Uf,
                                Endereco = pessoaJuridicaResult.Logradouro,
                                Numero = pessoaJuridicaResult.NumeroLogradouro,
                                Complemento = pessoaJuridicaResult.Complemento,
                                Email = pessoaJuridicaResult.Email,
                                Telefone = pessoaJuridicaResult.Telefone,
                            },
                        };

                        pessoaJuridicaResponseDto = this.SaveData(
                            updateDto: pessoaJuridicaRequestUpdateDto);
                    }
                }

                connection.CommitTransaction();

                return new ExecutionResponseDto<PessoaJuridicaResponseDto>
                {
                    Data = pessoaJuridicaResponseDto,
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
        /// <param name="createDto"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public PessoaJuridicaResponseDto SaveData(PessoaJuridicaRequestCreateDto? createDto = null, PessoaJuridicaRequestUpdateDto? updateDto = null)
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
                    PessoaJuridicaEntity);

                connection.BeginTransaction();

                if (updateDto != null)
                {
                    entity = this._mapper.Map<PessoaJuridicaEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    entity = this._mapper.Map<PessoaJuridicaEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<PessoaJuridicaResponseDto>(
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