namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class EventoService : BaseService
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public EventoService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
                cfg.CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventoResponseDto Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.EventoRepository.Get(
                        id);

                    return this._mapper.Map<EventoResponseDto>(entity);
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public EventoResponseDto SaveData(EventoRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var eventoResponseDto = default(
                    EventoResponseDto);

                if (dto.Id != null &&
                    !dto.Id.HasValue)
                {
                    eventoResponseDto = this.Get(
                        dto.Id.Value);
                }

                connection.BeginTransaction();

                var entity = default(
                    EventoEntity);

                if (eventoResponseDto != null)
                {
                    entity = this._mapper.Map<EventoEntity>(
                        eventoResponseDto);

                    entity = connection.RepositoriesUniPayCheck.EventoRepository.Update(
                        entity.Id,
                        entity);
                }
                else
                {
                    if (dto.Id != null &&
                        !dto.Id.HasValue)
                    {
                        dto.Id = connection.RepositoriesUniPayCheck.EventoRepository.GetLastId();
                    }

                    entity = this._mapper.Map<EventoEntity>(
                        dto);

                    entity = connection.RepositoriesUniPayCheck.EventoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<EventoResponseDto>(
                    entity);
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