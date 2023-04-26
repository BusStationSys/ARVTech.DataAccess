namespace ARVTech.DataAccess.Business.EquHos
{
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class PelagemBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public PelagemBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PelagemDto, PelagemEntity>().ReverseMap();
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

                connection.RepositoriesEquHos.PelagemRepository.Delete(
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
        public PelagemDto Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.PelagemRepository.Get(
                        id);

                    return this._mapper.Map<PelagemDto>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all "Pelagens" records.
        /// </summary>
        /// <returns>If success, the list with all "Pelagens" records. Otherwise, an exception detailing the problem.</returns>
        public IEnumerable<PelagemDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.PelagemRepository.GetAll();

                    return this._mapper.Map<IEnumerable<PelagemDto>>(entity);
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
        public PelagemDto SaveData(PelagemDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<PelagemEntity>(dto);

                connection.BeginTransaction();

                if (dto.Id != null)
                {
                    entity = connection.RepositoriesEquHos.PelagemRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.PelagemRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<PelagemDto>(
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