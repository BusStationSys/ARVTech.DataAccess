namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class EventoService : BaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public EventoService(IUnitOfWork unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventoResponse Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.EventoRepository.Get(
                        id);

                    return this._mapper.Map<EventoResponse>(entity);
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

                connection.RepositoriesUniPayCheck.EventoRepository.Delete(
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public EventoResponse SaveData(EventoRequest dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                var entity = default(EventoEntity);

                if (dto.Id.HasValue)
                {
                    var existingEntity = connection.RepositoriesUniPayCheck.EventoRepository.Get(
                        dto.Id.Value);

                    entity = this._mapper.Map<EventoEntity>(
                        existingEntity);

                    entity = connection.RepositoriesUniPayCheck.EventoRepository.Update(
                        entity.Id,
                        entity);
                }
                else
                {
                    dto.Id = connection.RepositoriesUniPayCheck.EventoRepository.GetLastId();

                    entity = this._mapper.Map<EventoEntity>(
                        dto);

                    entity = connection.RepositoriesUniPayCheck.EventoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<EventoResponse>(
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