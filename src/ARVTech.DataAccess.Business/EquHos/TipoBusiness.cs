namespace ARVTech.DataAccess.Business.EquHos
{
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class TipoBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public TipoBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TipoDto, TipoEntity>().ReverseMap();
                cfg.CreateMap<AnimalDto, AnimalEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
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

                connection.RepositoriesEquHos.TipoRepository.Delete(
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
        /// <param name="id"></param>
        /// <returns></returns>
        public TipoDto Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.TipoRepository.Get(
                        id);

                    return this._mapper.Map<TipoDto>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Tipos" records.
        /// </summary>
        /// <returns>If success, the list with all "Tipos" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<TipoDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.TipoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<TipoDto>>(entity);
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public TipoDto SaveData(TipoDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<TipoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Id != null)
                {
                    entity = connection.RepositoriesEquHos.TipoRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.TipoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<TipoDto>(
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
            if (!_disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                _disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}