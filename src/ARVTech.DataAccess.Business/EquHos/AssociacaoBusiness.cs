namespace ARVTech.DataAccess.Business.EquHos
{
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class AssociacaoBusiness : BaseBusiness
    {
        public AssociacaoBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
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

                cfg.CreateMap<AssociacaoDto, AssociacaoEntity>().ReverseMap();
                cfg.CreateMap<CabanhaDto, CabanhaEntity>().ReverseMap();
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

                connection.RepositoriesEquHos.AssociacaoRepository.Delete(
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
        public AssociacaoDto Get(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AssociacaoRepository.Get(
                        id);

                    return this._mapper.Map<AssociacaoDto>(entity);
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
        public IEnumerable<AssociacaoDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.AssociacaoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<AssociacaoDto>>(entity);
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
        public AssociacaoDto SaveData(AssociacaoDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<AssociacaoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Id != null)
                {
                    entity = connection.RepositoriesEquHos.AssociacaoRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.AssociacaoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<AssociacaoDto>(
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
    }
}

//namespace Services
//{
//    using System;
//    using System.Collections.Generic;
//    using AutoMapper;
//    using DTOs;
//    using Entities;
//    using UnitOfWork.Interfaces;

//    public class IcmsService
//    {
//        private readonly IUnitOfWork _unitOfWork = null;

//        // private readonly Mapper _mapper = null;
//        private Mapper _mapper = null;

//        public IcmsService(IUnitOfWork unitOfWork)
//        {
//            this._unitOfWork = unitOfWork;

//            var mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                //cfg.CreateMap<IcmsDto, IcmsEntity>()
//                //    .ForMember(dest => dest.DataDe, opt => opt.MapFrom(
//                //        src => src.DataInicial))
//                //    .ForMember(dest => dest.CodigoIcms, opt => opt.MapFrom(
//                //        src => src.Id))
//                //    .ForMember(dest => dest.Aliquota, opt => opt.MapFrom(
//                //        src => src.Taxa))
//                //    .ReverseMap();

//                cfg.CreateMap<IcmsDto, IcmsEntity>().ReverseMap();
//            });

//            this._mapper = new Mapper(mapperConfiguration);
//        }

//        //public ValidadeIcmsDto SaveData(ValidadeIcmsDto dto)
//        //{
//        //    var connection = this._unitOfWork.Create();

//        //    try
//        //    {
//        //        var entity = this._mapper.Map<ValidadeIcmsEntity>(dto);

//        //        ValidadeIcmsDto ValidadeIcmsDto = null;

//        //        if (dto.Id != null)
//        //        {
//        //            short id = (short)dto.Id;

//        //            ValidadeIcmsDto = this.Get(
//        //                id,
//        //                dto.DataInicial);
//        //        }

//        //        connection.BeginTransaction();

//        //        if (ValidadeIcmsDto != null)
//        //        {
//        //            entity = this._mapper.Map<ValidadeIcmsEntity>(ValidadeIcmsDto);

//        //            entity = connection.Repositories.ValidadeIcmsRepository.Update(entity);
//        //        }
//        //        else
//        //        {
//        //            entity = connection.Repositories.ValidadeIcmsRepository.Create(entity);
//        //        }

//        //        connection.CommitTransaction();

//        //        return this._mapper.Map<ValidadeIcmsDto>(entity);
//        //    }
//        //    catch
//        //    {
//        //        if (connection.Transaction != null)
//        //        {
//        //            connection.Rollback();
//        //        }

//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Dispose();
//        //    }
//        //}

//        //public void Delete(short id, DateTime dataInicial)
//        //{
//        //    var connection = this._unitOfWork.Create();

//        //    try
//        //    {
//        //        connection.BeginTransaction();

//        //        connection.Repositories.ValidadeIcmsRepository.Delete(
//        //            id,
//        //            dataInicial);

//        //        connection.CommitTransaction();
//        //    }
//        //    catch
//        //    {
//        //        connection.Rollback();

//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Dispose();
//        //    }
//        //}

//        public IcmsDto Get(short codigoIcms)
//        {
//            try
//            {
//                using (var connection = this._unitOfWork.Create())
//                {
//                    var entity = connection.Repositories.IcmsRepository.Get(
//                        codigoIcms);

//                    return this._mapper.Map<IcmsDto>(entity);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        //public IEnumerable<ValidadeIcmsDto> GetAll(short id)
//        //{
//        //    try
//        //    {
//        //        using (var connection = this._unitOfWork.Create())
//        //        {
//        //            //return connection.Repositories.ValidadeIcmsRepository.GetAll();
//        //            return null;
//        //        }
//        //    }
//        //    catch
//        //    {
//        //        throw;
//        //    }
//        //}

//        //public ValidadeIcmsDto Update(ValidadeIcmsDto model)
//        //{
//        //    var connection = this._unitOfWork.Create();

//        //    try
//        //    {
//        //        connection.BeginTransaction();

//        //        //model = connection.Repositories.ValidadeIcmsRepository.Update(model);

//        //        connection.CommitTransaction();

//        //        //return model;
//        //        return model;
//        //    }
//        //    catch
//        //    {
//        //        connection.Rollback();

//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Dispose();
//        //    }
//        //}
//    }
//}