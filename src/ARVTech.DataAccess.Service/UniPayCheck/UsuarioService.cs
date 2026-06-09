namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using ARVTech.Shared.Security.Interfaces;
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="passwordHasher"></param>
        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher) :
            base(unitOfWork, mapper)
        {
            this._passwordHasher = passwordHasher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UsuarioResponse CheckPasswordValid(Guid guid, string password)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(nameof(guid));

                if (string.IsNullOrEmpty(password))
                    throw new ArgumentNullException(nameof(password));

                //  Busca usuário.
                var entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Get(
                    guid);

                if (entity is null)
                    return null;

                // Valida a senha no C#
                var isValid = this._passwordHasher.Verify(
                    password,
                    entity.PasswordHash);

                if (!isValid)
                    return null;

                //  Retorna usuário.
                return this._mapper.Map<UsuarioResponse>(
                    entity);
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

                    return this._mapper.Map<UsuarioResponse>(
                        entity);
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

                    return this._mapper.Map<IEnumerable<UsuarioResponse>>(
                        entity);
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
        public IEnumerable<UsuarioNotificacaoResponse> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null)
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

                    return this._mapper.Map<IEnumerable<UsuarioNotificacaoResponse>>(entity);
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
        public IEnumerable<UsuarioResponse> GetByUsername(string cpfEmailUsername)
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

                    return this._mapper.Map<IEnumerable<UsuarioResponse>>(
                        entity);
                }
            }
            catch
            {
                throw;
            }
        }

        public UsuarioResponse SaveData(UsuarioCreateRequest? createRequest = null, UsuarioUpdateRequest? updateRequest = null)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (createRequest != null && updateRequest != null)
                    throw new InvalidOperationException($"{nameof(createRequest)} e {nameof(updateRequest)} não podem estar preenchidos ao mesmo tempo.");
                else if (createRequest is null && updateRequest is null)
                    throw new InvalidOperationException($"{nameof(createRequest)} e {nameof(updateRequest)} não podem estar vazios ao mesmo tempo.");
                else if (updateRequest != null && updateRequest.Guid == Guid.Empty)
                    throw new InvalidOperationException($"É necessário o preenchimento do {nameof(updateRequest.Guid)}.");

                var entity = default(
                    UsuarioEntity);

                connection.BeginTransaction();

                if (updateRequest != null)
                {
                    entity = this._mapper.Map<UsuarioEntity>(
                        updateRequest);

                    entity.PasswordHash = this._passwordHasher.Hash(
                        updateRequest.Password);

                    entity = connection.RepositoriesUniPayCheck.UsuarioRepository.Update(
                        (Guid)entity.Guid,
                        entity);
                }
                else if (createRequest != null)
                {
                    entity = this._mapper.Map<UsuarioEntity>(
                        createRequest);

                    entity.PasswordHash = this._passwordHasher.Hash(
                        createRequest.Password);    //  

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