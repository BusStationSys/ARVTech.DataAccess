namespace ARV.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using ARV.DataAccess.Models;
    using ARV.DataAccess.UnitOfWork.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class UsuarioService
    {
        /// <summary>
        /// 
        /// </summary>
        private IUnitOfWork _unitOfWork = null as IUnitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UsuarioService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> Listar()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return null;
                    //return connection.RepositoriesEquHos.UsuarioRepository.GetAll();
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
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario Obter(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return null;
                    //return connection.Repositories.ARV.DataAccess.UsuarioRepository.Get(id);
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
        /// <param name="id"></param>
        public void Excluir(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    //connection.Repositories.ARV.DataAccess.UsuarioRepository.Delete(id);

                    connection.Commit();
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
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario Salvar(Usuario model, int? id = null)
        {
            try
            {
                Usuario usuario = null as Usuario;

                using (var connection = this._unitOfWork.Create())
                {
                    if (id != null &&
                        id.HasValue)
                    {
                        model.Id = id;
                        //usuario = connection.Repositories.ARV.DataAccess.UsuarioRepository.Update(model);
                    }
                    else
                    {
                        //usuario = connection.Repositories.ARV.DataAccess.UsuarioRepository.Create(model);
                    }

                    connection.Commit();
                }

                return usuario;
            }
            catch
            {
                throw;
            }
        }
    }
}