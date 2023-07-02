namespace ARVTech.DataAccess.Business.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class AnimalBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public AnimalBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnimalDto, AnimalEntity>().ReverseMap();
                cfg.CreateMap<ContaDto, ContaEntity>().ReverseMap();
                cfg.CreateMap<CabanhaDto, CabanhaEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

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

                connection.RepositoriesEquHos.AnimalRepository.Delete(
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
        /// <param name="guid"></param>
        /// <returns></returns>
        public AnimalDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AnimalRepository.Get(
                        guid);

                    return this._mapper.Map<AnimalDto>(entity);
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
        public IEnumerable<AnimalDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AnimalRepository.GetAll();

                    return this._mapper.Map<IEnumerable<AnimalDto>>(entity);
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
        /// <param name="guidConta"></param>
        /// <param name="guidCabanha"></param>
        /// <param name="sexo"></param>
        /// <param name="argumento"></param>
        /// <returns></returns>
        public IEnumerable<AnimalDto> GetAllBySexoAndArgumento(Guid guidConta, Guid guidCabanha, string sexo, string argumento)
        {
            try
            {
                if (guidConta == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidConta));

                if (guidCabanha == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidCabanha));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AnimalRepository.GetAllBySexoAndArgumento(
                        guidConta,
                        guidCabanha,
                        sexo,
                        argumento);

                    return this._mapper.Map<IEnumerable<AnimalDto>>(entity);
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
        /// <param name="sexo"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<AnimalDto> GetAllFilhos(string sexo, Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AnimalRepository.GetAllFilhos(
                        sexo,
                        guid);

                    return this._mapper.Map<IEnumerable<AnimalDto>>(entity);
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
        public AnimalDto SaveData(AnimalDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<AnimalEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesEquHos.AnimalRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.AnimalRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<AnimalDto>(
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