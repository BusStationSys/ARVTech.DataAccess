namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;

    public class PessoaFisicaService : BaseService, IPessoaFisicaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public PessoaFisicaService(IUnitOfWork unitOfWork, IMapper mapper) :
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

                connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Delete(
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
        /// <param name="guid"></param>
        /// <returns></returns>
        public PessoaFisicaResponse? Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Get(
                        guid);

                    return this._mapper.Map<PessoaFisicaResponse?>(
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
        /// <returns></returns>
        public IEnumerable<PessoaFisicaResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetAll();

                    return this._mapper.Map<IEnumerable<PessoaFisicaResponse>>(
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
        /// <param name="periodoInicialString">MMdd.</param>
        /// <param name="periodoFinalString">MMdd.</param>
        /// <returns></returns>
        public IEnumerable<PessoaFisicaResponse> GetAniversariantes(string periodoInicialString, string periodoFinalString)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetAniversariantes(
                        periodoInicialString,
                        periodoFinalString);

                    return this._mapper.Map<IEnumerable<PessoaFisicaResponse>>(
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
        /// <param name="cpf"></param>
        /// <returns></returns>
        public PessoaFisicaResponse GetByCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrEmpty(cpf))
                    throw new ArgumentNullException(
                        nameof(
                            cpf));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetByCpf(
                        cpf);

                    return this._mapper.Map<PessoaFisicaResponse?>(
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
        /// <param name="nome"></param>
        /// <returns></returns>
        public PessoaFisicaResponse GetByNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetByNome(
                        nome);

                    return this._mapper.Map<PessoaFisicaResponse>(
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
        /// <param name="nome"></param>
        /// <param name="numeroCtps"></param>
        /// <param name="serieCtps"></param>
        /// <param name="ufCtps"></param>
        /// <returns></returns>
        public PessoaFisicaResponse GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps)
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

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetByNomeNumeroCtpsSerieCtpsAndUfCtps(
                        nome,
                        numeroCtps,
                        serieCtps,
                        ufCtps);

                    return this._mapper.Map<PessoaFisicaResponse?>(
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
        /// <param name="createRequest"></param>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        public PessoaFisicaResponse SaveData(PessoaFisicaCreateRequest? createRequest = null, PessoaFisicaUpdateRequest? updateRequest = null)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (createRequest != null && updateRequest != null)
                    throw new InvalidOperationException($"{nameof(createRequest)} e {nameof(updateRequest)} não podem estar preenchidos ao mesmo tempo.");
                else if (createRequest is null && updateRequest is null)
                    throw new InvalidOperationException($"{nameof(createRequest)} e {nameof(updateRequest)} não podem estar vazios ao mesmo tempo.");
                else if (updateRequest != null && updateRequest.Guid == Guid.Empty)
                    throw new InvalidOperationException($"É necessário o preenchimento do {nameof(updateRequest.Guid)}.");

                var entity = default(
                    PessoaFisicaEntity);

                connection.BeginTransaction();

                if (updateRequest != null)
                {
                    entity = this._mapper.Map<PessoaFisicaEntity>(
                        updateRequest);

                    entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createRequest != null)
                {
                    entity = this._mapper.Map<PessoaFisicaEntity>(
                        createRequest);

                    entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<PessoaFisicaResponse>(
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