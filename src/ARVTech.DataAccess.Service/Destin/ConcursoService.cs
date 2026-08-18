namespace ARVTech.DataAccess.Service.Destin
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.Destin.Requests;
    using ARVTech.DataAccess.Contracts.Destin.Responses;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class ConcursoService : BaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ConcursoService(IUnitOfWork unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConcursoResponse Get(Guid id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesDestin.ConcursoRepository.Get(
                        id);

                    return this._mapper.Map<ConcursoResponse>(entity);
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
        /// <param name="idModalidade"></param>
        /// <param name="numero"></param>
        /// <param name="dataApuracao"></param>
        /// <returns></returns>
        public ConcursoResponse GetByIdModalidadeAndNumeroAndDataApuracao(int idModalidade, int numero, DateTime dataApuracao)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesDestin.ConcursoRepository.GetByIdModalidadeAndNumeroAndDataApuracao(idModalidade,
                        numero,
                        dataApuracao);

                    return this._mapper.Map<ConcursoResponse>(entity);
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
        public IEnumerable<ConcursoResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entities = connection.RepositoriesDestin.ConcursoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<ConcursoResponse>>(entities);
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
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                connection.RepositoriesDestin.ConcursoRepository.Delete(
                    id);

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
        /// <param name="request"></param>
        /// <returns></returns>
        public ConcursoResponse SaveData(ConcursoRequest request)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var newRecord = false;

                connection.BeginTransaction();

                var entity = default(ConcursoEntity);

                if (request.Id.HasValue)
                {
                    var existingEntity = connection.RepositoriesDestin.ConcursoRepository.Get(
                        request.Id.Value);

                    if (existingEntity != null)
                    {
                        entity = this._mapper.Map<ConcursoEntity>(
                            request);

                        entity = connection.RepositoriesDestin.ConcursoRepository.Update(
                            entity.Id,
                            entity);
                    }
                    else
                        newRecord = true;
                }
                else
                    newRecord = true;

                if (newRecord)
                {
                    if (!request.Id.HasValue)   //  Se não houver ID, significa que é um novo registro, então atribuí-se o próximo ID disponível.
                        request.Id = Guid.NewGuid();

                    entity = this._mapper.Map<ConcursoEntity>(
                        request);

                    entity = connection.RepositoriesDestin.ConcursoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<ConcursoResponse>(
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