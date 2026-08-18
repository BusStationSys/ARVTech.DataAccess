namespace ARVTech.DataAccess.Service.Destin
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.Destin.Requests;
    using ARVTech.DataAccess.Contracts.Destin.Responses;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class ModalidadeService : BaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ModalidadeService(IUnitOfWork unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModalidadeResponse Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesDestin.ModalidadeRepository.Get(
                        id);

                    return this._mapper.Map<ModalidadeResponse>(entity);
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
        public IEnumerable<ModalidadeResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entities = connection.RepositoriesDestin.ModalidadeRepository.GetAll();

                    return this._mapper.Map<IEnumerable<ModalidadeResponse>>(entities);
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
        public void Delete(int id)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                connection.RepositoriesDestin.ModalidadeRepository.Delete(
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
        public ModalidadeResponse SaveData(ModalidadeRequest request)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                bool newRecord = false;

                var entity = default(ModalidadeEntity);

                if (request.Id.HasValue)
                {
                    var existingEntity = connection.RepositoriesDestin.ModalidadeRepository.Get(
                        request.Id.Value);

                    if (existingEntity != null)
                    {
                        entity = this._mapper.Map<ModalidadeEntity>(
                            request);

                        entity = connection.RepositoriesDestin.ModalidadeRepository.Update(
                            entity.Id,
                            entity);
                    }
                    else
                        newRecord = true;
                }

                if (newRecord)
                {
                    if (!request.Id.HasValue)   //  Se não houver ID, significa que é um novo registro, então atribuí-se o próximo ID disponível.
                        request.Id = connection.RepositoriesDestin.ModalidadeRepository.GetLastId();

                    entity = this._mapper.Map<ModalidadeEntity>(
                        request);

                    entity = connection.RepositoriesDestin.ModalidadeRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<ModalidadeResponse>(
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