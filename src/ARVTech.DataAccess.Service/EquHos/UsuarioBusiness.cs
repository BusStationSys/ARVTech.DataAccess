namespace ARVTech.DataAccess.Business.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.EquHos;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class UsuarioBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public UsuarioBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuarioDto, UsuarioEntity>().ReverseMap();
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

                connection.RepositoriesEquHos.UsuarioRepository.Delete(
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
        public UsuarioDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.UsuarioRepository.Get(
                        guid);

                    return this._mapper.Map<UsuarioDto>(entity);
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
        public IEnumerable<UsuarioDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.UsuarioRepository.GetAll();

                    return this._mapper.Map<IEnumerable<UsuarioDto>>(entity);
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
        /// <param name="perfil"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioDto> GetAll(Guid guidConta, int perfil)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesEquHos.UsuarioRepository.GetAll(
                        guidConta,
                        perfil);

                    return this._mapper.Map<IEnumerable<UsuarioDto>>(entity);
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
        public UsuarioDto SaveData(UsuarioDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<UsuarioEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesEquHos.UsuarioRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesEquHos.UsuarioRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<UsuarioDto>(
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="guidConta"></param>
        /// <param name="guidCabanha"></param>
        /// <returns></returns>
        public UsuarioDto TrocarContaECabanhaLogados(Guid guid, Guid guidConta, Guid guidCabanha)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                if (guidConta == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidConta));

                if (guidCabanha == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidCabanha));

                connection.BeginTransaction();

                var entity = connection.RepositoriesEquHos.UsuarioRepository.TrocarContaECabanhaLogados(
                    guid,
                    guidConta,
                    guidCabanha);

                connection.CommitTransaction();

                return this._mapper.Map<UsuarioDto>(
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