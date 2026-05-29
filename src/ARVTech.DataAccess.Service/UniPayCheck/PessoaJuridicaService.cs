namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class PessoaJuridicaService : BaseService, IPessoaJuridicaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public PessoaJuridicaService(IUnitOfWork unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        { }

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
        public PessoaJuridicaResponseDto? Get(Guid guid)
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

                    return this._mapper.Map<PessoaJuridicaResponseDto?>(entity);
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
        /// <param name="content"></param>
        /// <returns></returns>
        public ResumoImportacaoEmpregadoresResponseDto ImportFileEmpregadores(string content)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var (dataInicio, dataFim, quantidadeRegistrosAtualizados, quantidadeRegistrosInalterados, quantidadeRegistrosInseridos) = connection.RepositoriesUniPayCheck.PessoaJuridicaRepository.ImportFileEmpregadores(
                        content);

                    return new ResumoImportacaoEmpregadoresResponseDto
                    {
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        QuantidadeRegistrosAtualizados = quantidadeRegistrosAtualizados,
                        QuantidadeRegistrosInalterados = quantidadeRegistrosInalterados,
                        QuantidadeRegistrosInseridos = quantidadeRegistrosInseridos,
                    };

                    //return new ExecutionResponseDto<ResumoImportacaoEmpregadoresResponseDto>
                    //{
                    //    Data = responseDto,
                    //    Success = true,
                    //};
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
        //public ExecutionResponseDto<PessoaJuridicaResponseDto> Import(EmpregadorResult pessoaJuridicaResult)
        //{
        //    var connection = this._unitOfWork.Create();

        //    try
        //    {
        //        connection.BeginTransaction();

        //        //  Verifica se existe o registro do Empregador pelo CNPJ.
        //        var pessoaJuridicaResponseDto = default(
        //            PessoaJuridicaResponseDto);

        //        using (var pessoaFisicaService = new PessoaFisicaService(
        //            this._unitOfWork))
        //        {
        //            var idUnidadeNegocio = (UnidadeNegocioEnum)Enum.Parse(
        //                typeof(
        //                    UnidadeNegocioEnum),
        //                pessoaJuridicaResult.UnidadeNegocio.RemoveDiacritics());

        //            string cep = pessoaJuridicaResult.Cep.Replace(
        //                ".",
        //                string.Empty).Replace(
        //                    "-",
        //                    string.Empty);

        //            string cnpj = pessoaJuridicaResult.Cnpj.Replace(
        //                ".",
        //                string.Empty).Replace(
        //                    "/",
        //                    string.Empty).Replace(
        //                        "-",
        //                        string.Empty);

        //            pessoaJuridicaResponseDto = this.GetByCnpj(
        //                cnpj);

        //            //  Se não existir o registro do Empregador, deve incluir o registro.
        //            if (pessoaJuridicaResponseDto is null)
        //            {
        //                var pessoaJuridicaRequestCreateDto = new PessoaJuridicaRequestCreateDto
        //                {
        //                    IdUnidadeNegocio = idUnidadeNegocio,
        //                    RazaoSocial = pessoaJuridicaResult.RazaoSocial,
        //                    DataFundacao = Convert.ToDateTime(
        //                        pessoaJuridicaResult.DataFundacao),
        //                    Cnpj = cnpj,
        //                    Pessoa = new PessoaRequestCreateDto()
        //                    {
        //                        Bairro = pessoaJuridicaResult.Bairro,
        //                        Cep = cep,
        //                        Cidade = pessoaJuridicaResult.Cidade,
        //                        Uf = pessoaJuridicaResult.Uf,
        //                        Endereco = pessoaJuridicaResult.Logradouro,
        //                        Numero = pessoaJuridicaResult.NumeroLogradouro,
        //                        Complemento = pessoaJuridicaResult.Complemento,
        //                        Email = pessoaJuridicaResult.Email,
        //                        Telefone = pessoaJuridicaResult.Telefone,
        //                    },
        //                };

        //                pessoaJuridicaResponseDto = this.SaveData(
        //                    pessoaJuridicaRequestCreateDto);
        //            }
        //            else    //  Se existir, apenas atualiza as informações.
        //            {
        //                var pessoaJuridicaRequestUpdateDto = new PessoaJuridicaRequestUpdateDto
        //                {
        //                    Guid = pessoaJuridicaResponseDto.Guid,
        //                    IdUnidadeNegocio = idUnidadeNegocio,
        //                    RazaoSocial = pessoaJuridicaResult.RazaoSocial,
        //                    DataFundacao = Convert.ToDateTime(
        //                        pessoaJuridicaResult.DataFundacao),
        //                    Cnpj = pessoaJuridicaResponseDto.Cnpj,
        //                    GuidPessoa = pessoaJuridicaResponseDto.GuidPessoa,
        //                    Pessoa = new PessoaRequestUpdateDto()
        //                    {
        //                        Bairro = pessoaJuridicaResult.Bairro,
        //                        Cep = cep,
        //                        Cidade = pessoaJuridicaResult.Cidade,
        //                        Uf = pessoaJuridicaResult.Uf,
        //                        Endereco = pessoaJuridicaResult.Logradouro,
        //                        Numero = pessoaJuridicaResult.NumeroLogradouro,
        //                        Complemento = pessoaJuridicaResult.Complemento,
        //                        Email = pessoaJuridicaResult.Email,
        //                        Telefone = pessoaJuridicaResult.Telefone,
        //                    },
        //                };

        //                pessoaJuridicaResponseDto = this.SaveData(
        //                    updateDto: pessoaJuridicaRequestUpdateDto);
        //            }
        //        }

        //        connection.CommitTransaction();

        //        return new ExecutionResponseDto<PessoaJuridicaResponseDto>
        //        {
        //            Data = pessoaJuridicaResponseDto,
        //            Success = true,
        //        };
        //    }
        //    catch
        //    {
        //        if (connection.Transaction != null)
        //            connection.Rollback();

        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }
        //}

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
        [ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}