namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;
    using ARVTech.Shared;

    public class UsuarioBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public UsuarioBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuarioRequestCreateDto, UsuarioEntity>().ReverseMap();
                cfg.CreateMap<UsuarioRequestUpdateDto, UsuarioEntity>().ReverseMap();
                cfg.CreateMap<UsuarioResponse, UsuarioEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponse, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponse, PessoaEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.UsuarioRepository.Delete(
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
        public UsuarioResponse Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Get(
                        guid);

                    return this._mapper.Map<UsuarioResponse>(entity);
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
        public IEnumerable<UsuarioResponse> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.GetAll();

                    return this._mapper.Map<IEnumerable<UsuarioResponse>>(entity);
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
        /// <param name="username"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioResponse> GetByUsername(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new ArgumentNullException(
                        nameof(username));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.GetByUsername(
                        username);

                    return this._mapper.Map<IEnumerable<UsuarioResponse>>(entity);
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
        /// <param name="createDto"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public UsuarioResponse SaveData(UsuarioRequestCreateDto? createDto = null, UsuarioRequestUpdateDto? updateDto = null)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (createDto != null && updateDto != null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar preenchidos ao mesmo tempo.");
                else if (createDto is null && updateDto is null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar vazios ao mesmo tempo.");
                else if (updateDto != null && updateDto.Guid == Guid.Empty)
                    throw new InvalidOperationException($"É necessário o preenchimento do {nameof(updateDto.Guid)}.");

                var entity = default(
                    UsuarioEntity);

                connection.BeginTransaction();

                if (updateDto != null)
                {
                    updateDto.Password = PasswordCryptography.GetHashMD5(
                        updateDto.Password);

                    entity = this._mapper.Map<UsuarioEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Update(
                        entity);
                }
                else if (createDto != null)
                {
                    createDto.Password = PasswordCryptography.GetHashMD5(
                        createDto.Password);

                    entity = this._mapper.Map<UsuarioEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<UsuarioResponse>(
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