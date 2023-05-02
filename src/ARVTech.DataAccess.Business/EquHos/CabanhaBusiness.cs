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
        private bool _disposedValue = false;    // To detect redundant calls.

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
        /// Must update "Conta" and "Cabanha" where the "Usuário" is logged.
        /// </summary>
        /// <param name="dto">Object with the fields.</param>
        public void AtualizarContaECabanhaLogados(CabanhaDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                var entity = this._mapper.Map<CabanhaEntity>(
                    dto);

                connection.RepositoriesEquHos.CabanhaRepository.AtualizarContaECabanhaLogados(
                    entity);

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
        /// Checks if the record exists by "CNPJ" of "Cabanha".
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        /// <param name="cnpj">"CNPJ" of "Cabanha" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteCNPJDuplicado(Guid guid, string cnpj)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return connection.RepositoriesEquHos.CabanhaRepository.ExisteCNPJDuplicado(
                        guid,
                        cnpj);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the record exists by "Razão Social" of "Cabanha".
        /// </summary>
        /// <param name="guid">Guid of "Cabanha" record.</param>
        /// <param name="razaoSocial">"Razão Social" of "Cabanha" record.</param>
        /// <returns>True, for the record existing. False, for the record not found.</returns>
        public bool ExisteRazaoSocialDuplicada(Guid guid, string razaoSocial)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return connection.RepositoriesEquHos.CabanhaRepository.ExisteRazaoSocialDuplicada(
                        guid,
                        razaoSocial);
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
        /// <param name="guidConta"></param>
        /// <returns></returns>
        public IEnumerable<CabanhaDto> GetAllWithPermission(Guid guidConta)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.CabanhaRepository.GetAllByGuidConta(
                        guidConta);

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
        /// <param name="guidConta"></param>
        /// <param name="guidUsuario"></param>
        /// <returns></returns>
        public IEnumerable<CabanhaDto> GetAllWithPermission(Guid guidConta, Guid guidUsuario)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.CabanhaRepository.GetAllWithPermission(
                        guidConta,
                        guidUsuario);

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