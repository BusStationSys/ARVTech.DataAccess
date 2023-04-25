namespace ARVTech.DataAccess.Business
{
    using System;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public abstract class BaseBusiness : IDisposable
    {
        private bool _disposedValue = false;

        protected IUnitOfWork _unitOfWork = null;

        protected Mapper _mapper = null;

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return this._unitOfWork;
            }
        }

        protected Mapper Mapper
        {
            get
            {
                return this._mapper;
            }
        }

        protected BaseBusiness(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }
    }
}

/*
 namespace Services
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using DataTransferObject;
    using Entities;
    using UnitOfWork.Interfaces;

    public class IcmsService
    {
        private readonly IUnitOfWork _unitOfWork = null;

        // private readonly Mapper _mapper = null;
        private Mapper _mapper = null;

        public IcmsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<IcmsDTO, IcmsEntity>()
                //    .ForMember(dest => dest.DataDe, opt => opt.MapFrom(
                //        src => src.DataInicial))
                //    .ForMember(dest => dest.CodigoIcms, opt => opt.MapFrom(
                //        src => src.Id))
                //    .ForMember(dest => dest.Aliquota, opt => opt.MapFrom(
                //        src => src.Taxa))
                //    .ReverseMap();

                cfg.CreateMap<IcmsDTO, IcmsEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

        //public ValidadeIcmsDTO SaveData(ValidadeIcmsDTO dto)
        //{
        //    var connection = this._unitOfWork.Create();

        //    try
        //    {
        //        var entity = this._mapper.Map<ValidadeIcmsEntity>(dto);

        //        ValidadeIcmsDTO validadeIcmsDTO = null;

        //        if (dto.Id != null)
        //        {
        //            short id = (short)dto.Id;

        //            validadeIcmsDTO = this.Get(
        //                id,
        //                dto.DataInicial);
        //        }

        //        connection.BeginTransaction();

        //        if (validadeIcmsDTO != null)
        //        {
        //            entity = this._mapper.Map<ValidadeIcmsEntity>(validadeIcmsDTO);

        //            entity = connection.Repositories.ValidadeIcmsRepository.Update(entity);
        //        }
        //        else
        //        {
        //            entity = connection.Repositories.ValidadeIcmsRepository.Create(entity);
        //        }

        //        connection.CommitTransaction();

        //        return this._mapper.Map<ValidadeIcmsDTO>(entity);
        //    }
        //    catch
        //    {
        //        if (connection.Transaction != null)
        //        {
        //            connection.Rollback();
        //        }

        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }
        //}

        //public void Delete(short id, DateTime dataInicial)
        //{
        //    var connection = this._unitOfWork.Create();

        //    try
        //    {
        //        connection.BeginTransaction();

        //        connection.Repositories.ValidadeIcmsRepository.Delete(
        //            id,
        //            dataInicial);

        //        connection.CommitTransaction();
        //    }
        //    catch
        //    {
        //        connection.Rollback();

        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }
        //}

        public IcmsDTO Get(short codigoIcms)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.Repositories.IcmsRepository.Get(
                        codigoIcms);

                    return this._mapper.Map<IcmsDTO>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        //public IEnumerable<ValidadeIcmsDTO> GetAll(short id)
        //{
        //    try
        //    {
        //        using (var connection = this._unitOfWork.Create())
        //        {
        //            //return connection.Repositories.ValidadeIcmsRepository.GetAll();
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public ValidadeIcmsDTO Update(ValidadeIcmsDTO model)
        //{
        //    var connection = this._unitOfWork.Create();

        //    try
        //    {
        //        connection.BeginTransaction();

        //        //model = connection.Repositories.ValidadeIcmsRepository.Update(model);

        //        connection.CommitTransaction();

        //        //return model;
        //        return model;
        //    }
        //    catch
        //    {
        //        connection.Rollback();

        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }
        //}
    }
}
 */