namespace ARV.DataAccess.Services
{
    using ARV.DataAccess.Models;
    using ARV.DataAccess.UnitOfWork.Interfaces;
    using System.Collections.Generic;
    //using ARV.DataAccess.Models;
    //using ARV.DataAccess.UnitOfWork.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class GrupoService
    {
        private IUnitOfWork _unitOfWork = null as IUnitOfWork;

        public GrupoService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Grupo> Listar()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    //return connection.Repositories.ARV.DataAccess.GrupoRepository.GetAll();
                    return null;
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
        public Grupo Obter(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return null;
                    //return connection.Repositories.ARV.DataAccess.GrupoRepository.Get(id);
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
                    //connection.Repositories.ARV.DataAccess.GrupoRepository.Delete(id);

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
        public Grupo Salvar(Grupo model, int? id = null)
        {
            try
            {
                Grupo grupoUsuario = null as Grupo;

                using (var connection = this._unitOfWork.Create())
                {
                    if (id != null &&
                        id.HasValue)
                    {
                        model.Id = id;
                        //perfilUsuario = connection.Repositories.GrupoUsuarioRepository.Update(model);
                    }
                    else
                    {
                        //perfilUsuario = connection.Repositories.GrupoUsuarioRepository.Create(model);
                    }

                    connection.Commit();
                }

                return grupoUsuario;
            }
            catch
            {
                throw;
            }
        }
    }
}