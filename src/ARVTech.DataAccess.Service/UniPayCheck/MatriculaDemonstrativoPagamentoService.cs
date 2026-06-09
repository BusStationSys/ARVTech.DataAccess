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
    using ARVTech.Shared.Enums;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoService : BaseService, IMatriculaDemonstrativoPagamentoService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public MatriculaDemonstrativoPagamentoService(IUnitOfWork unitOfWork, IMapper mapper) :
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Delete(
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
        /// <param name="competencia"></param>
        /// <param name="guidMatricula"></param>
        public void Delete(string competencia, Guid guidMatricula)
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.DeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(
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
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoResponse Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoResponse>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponse> Get(string competencia, string matricula)
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
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        competencia,
                        matricula);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponse>>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponse>>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetByGuidColaborador(Guid guidColaborador)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetByGuidColaborador(
                        guidColaborador);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponse>>(
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
        /// <param name="competenciaInicial"></param>
        /// <param name="competenciaFinal"></param>
        /// <param name="situacao"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamento situacao = SituacaoPendenciaDemonstrativoPagamento.Todos)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetPendencias(
                        competenciaInicial,
                        competenciaFinal);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponse>>(
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
        /// <param name="guidUsuario"></param>
        /// <param name="competencia"></param>
        /// <returns></returns>
        public IEnumerable<GraficoComposicaoSalarialResponse> GetSalaryCompositionChart(Guid guidUsuario, string competencia)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var resultado = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetSalaryCompositionChart(
                        guidUsuario,
                        competencia);

                    return resultado.Select(g => new GraficoComposicaoSalarialResponse()
                    {
                        GuidUsuario = g.guidUsuario,
                        Competencia = g.competencia,
                        Valor = g.valor,
                        Matricula = g.matricula,
                        DescricaoEvento = g.descricaoEvento,
                        Tipo = g.tipo,
                        Cor = g.cor,
                    });
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
        /// <param name="guidUsuario"></param>
        /// <param name="quantidadeMesesRetroativos"></param>
        /// <returns></returns>
        public IEnumerable<GraficoEvolucaoSalarialResponse> GetSalaryEvolutionChart(Guid guidUsuario, Int16 quantidadeMesesRetroativos)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var resultado = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetSalaryEvolutionChart(
                        guidUsuario,
                        quantidadeMesesRetroativos);

                    return resultado.Select(g => new GraficoEvolucaoSalarialResponse()
                    {
                        GuidUsuario = g.guidUsuario,
                        Competencia = g.competencia,
                        Valor = g.valor,
                    });
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
        public ResumoImportacaoDemonstrativosPagamentoResponse ImportFileDemonstrativosPagamento(string cnpj, string content)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var (dataInicio, dataFim, quantidadeRegistrosAtualizados, quantidadeRegistrosInalterados, quantidadeRegistrosInseridos, quantidadeRegistrosRejeitados) = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.ImportFileDemonstrativosPagamento(
                        cnpj,
                        content);

                    return new ResumoImportacaoDemonstrativosPagamentoResponse
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
        /// Saves enrollment payment statement data, either by creating a new one or updating an existing one.
        /// </summary>
        /// <param name="createRequest">Object containing the data to create a new enrollment payment statement.</param>
        /// <param name="updateRequest">Object containing the data to update an existing enrollment payment statement.</param>
        /// <returns>Returns the response containing the saved enrollment payment statement data.</returns>
        public MatriculaDemonstrativoPagamentoResponse SaveData(MatriculaDemonstrativoPagamentoCreateRequest? createRequest = null, MatriculaDemonstrativoPagamentoUpdateRequest? updateRequest = null)
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
                    MatriculaDemonstrativoPagamentoEntity);

                connection.BeginTransaction();

                if (updateRequest != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        updateRequest);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createRequest != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        createRequest);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoResponse>(
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