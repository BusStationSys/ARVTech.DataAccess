namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using AutoMapper;

    public class UsuarioService : BaseService, IUsuarioService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UsuarioResponseDto CheckPasswordValid(Guid guid, string password)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));
                else if (string.IsNullOrEmpty(password))
                    throw new ArgumentNullException(
                        nameof(password));

                var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.CheckPasswordValid(
                    guid,
                    password);

                return this._mapper.Map<UsuarioResponseDto>(
                    entity);
            }
            catch
            {
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
                    connection.Rollback();

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
        public UsuarioResponseDto Get(Guid guid)
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

                    return this._mapper.Map<UsuarioResponseDto>(entity);
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
        public IEnumerable<UsuarioResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.GetAll();

                    return this._mapper.Map<IEnumerable<UsuarioResponseDto>>(entity);
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
        /// <param name="tipo"></param>
        /// <param name="guidUsuario"></param>
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="guidEmpregador"></param>
        /// <param name="guidColaborador"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioNotificacaoResponseDto> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null)
        {
            try
            {
                //if (string.IsNullOrEmpty(cpfEmailUsername))
                //    throw new ArgumentNullException(
                //        nameof(cpfEmailUsername));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.GetNotificacoes(
                        tipo,
                        guidUsuario,
                        guidMatriculaDemonstrativoPagamento,
                        guidEmpregador,
                        guidColaborador);

                    return this._mapper.Map<IEnumerable<UsuarioNotificacaoResponseDto>>(entity);
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
        /// <param name="cpfEmailUsername"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioResponseDto> GetByUsername(string cpfEmailUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(cpfEmailUsername))
                    throw new ArgumentNullException(
                        nameof(cpfEmailUsername));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.GetByUsername(
                        cpfEmailUsername);

                    return this._mapper.Map<IEnumerable<UsuarioResponseDto>>(entity);
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
        public UsuarioResponseDto SaveData(UsuarioRequestCreateDto? createDto = null, UsuarioRequestUpdateDto? updateDto = null)
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
                    entity = this._mapper.Map<UsuarioEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Update(
                        (Guid)entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    entity = this._mapper.Map<UsuarioEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<UsuarioResponseDto>(
                    entity);
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        [ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}