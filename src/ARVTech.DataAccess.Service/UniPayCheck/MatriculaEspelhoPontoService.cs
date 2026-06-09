namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using AutoMapper;

    public class MatriculaEspelhoPontoService : BaseService, IMatriculaEspelhoPontoService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaEspelhoPontoService(IUnitOfWork unitOfWork, IMapper mapper) :
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

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Delete(
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
        /// <param name="competencia"></param>
        /// <param name="guidMatricula"></param>
        public void DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(
                    competencia,
                    guidMatricula);

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
        /// <returns></returns>
        public IEnumerable<MatriculaEspelhoPontoResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoResponse>>(
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
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaEspelhoPontoResponse>(
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
        /// <param name="competencia"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaEspelhoPontoResponse> Get(string competencia, string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(
                            competencia));
                else if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(
                            matricula));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Get(
                        competencia,
                        matricula);

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoResponse>>(
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
        public IEnumerable<MatriculaEspelhoPontoResponse> GetByGuidColaborador(Guid guidColaborador)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.GetByGuidColaborador(
                        guidColaborador);

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoResponse>>(
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
        /// <param name="cnpj"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ResumoImportacaoEspelhosPontoResponse ImportFileEspelhosPonto(string cnpj, string content)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var (dataInicio, dataFim, quantidadeRegistrosAtualizados, quantidadeRegistrosInalterados, quantidadeRegistrosInseridos, quantidadeRegistrosRejeitados) = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.ImportFileEspelhosPonto(
                        cnpj,
                        content);

                    return new ResumoImportacaoEspelhosPontoResponse
                    {
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        QuantidadeRegistrosAtualizados = quantidadeRegistrosAtualizados,
                        QuantidadeRegistrosInalterados = quantidadeRegistrosInalterados,
                        QuantidadeRegistrosInseridos = quantidadeRegistrosInseridos,
                        QuantidadeRegistrosRejeitados = quantidadeRegistrosRejeitados,
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
        /// <param name="createRequest"></param>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse SaveData(MatriculaEspelhoPontoCreateRequest? createRequest = null, MatriculaEspelhoPontoUpdateRequest? updateRequest = null)
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
                    MatriculaEspelhoPontoEntity);

                connection.BeginTransaction();

                if (updateRequest != null)
                {
                    entity = this._mapper.Map<MatriculaEspelhoPontoEntity>(
                        updateRequest);

                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createRequest != null)
                {
                    entity = this._mapper.Map<MatriculaEspelhoPontoEntity>(
                        createRequest);

                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaEspelhoPontoResponse>(
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