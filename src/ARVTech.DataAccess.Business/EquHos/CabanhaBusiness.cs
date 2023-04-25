namespace ARVTech.DataAccess.Business.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class CabanhaBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public CabanhaBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<IcmsDto, IcmsEntity>()
                //    .ForMember(dest => dest.DataDe, opt => opt.MapFrom(
                //        src => src.DataInicial))
                //    .ForMember(dest => dest.CodigoIcms, opt => opt.MapFrom(
                //        src => src.Id))
                //    .ForMember(dest => dest.Aliquota, opt => opt.MapFrom(
                //        src => src.Taxa))
                //    .ReverseMap();

                cfg.CreateMap<CabanhaDto, CabanhaEntity>().ReverseMap();
                cfg.CreateMap<AssociacaoDto, AssociacaoEntity>().ReverseMap();
                cfg.CreateMap<ContaDto, ContaEntity>().ReverseMap();
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
                connection.BeginTransaction();

                connection.RepositoriesEquHos.CabanhaRepository.Delete(
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
        public CabanhaDto Get(Guid guid)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.CabanhaRepository.Get(
                        guid);

                    return this._mapper.Map<CabanhaDto>(entity);
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
        public IEnumerable<CabanhaDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.CabanhaRepository.GetAll();

                    return this._mapper.Map<IEnumerable<CabanhaDto>>(entity);
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
        public CabanhaDto SaveData(CabanhaDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<CabanhaEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesEquHos.CabanhaRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.CabanhaRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<CabanhaDto>(
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