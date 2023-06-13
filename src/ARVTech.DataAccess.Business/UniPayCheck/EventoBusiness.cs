namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class EventoBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public EventoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventoDto, EventoEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventoDto Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.EventoRepository.Get(
                        id);

                    return this._mapper.Map<EventoDto>(entity);
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
        public EventoDto SaveData(EventoDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var eventoDto = default(
                    EventoDto);

                if (dto.Id != null &&
                    !dto.Id.HasValue)
                {
                    eventoDto = this.Get(
                        dto.Id.Value);
                }

                connection.BeginTransaction();

                var entity = default(
                    EventoEntity);

                if (eventoDto != null)
                {
                    entity = this._mapper.Map<EventoEntity>(
                        eventoDto);

                    entity = connection.RepositoriesUniPayCheck.EventoRepository.Update(
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

                return this._mapper.Map<EventoDto>(
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