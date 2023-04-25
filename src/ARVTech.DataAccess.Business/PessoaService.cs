namespace ARV.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using ARV.DataAccess.Models;
    using ARV.DataAccess.UnitOfWork.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class PessoaService
    {
        private IUnitOfWork _unitOfWork = null as IUnitOfWork;

        public PessoaService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Pessoa> Listar()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return connection.RepositoriesEquHos.PessoaRepository.GetAll();
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
        public Pessoa Obter(int id)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    return connection.RepositoriesEquHos.PessoaRepository.Get(id);
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
                    connection.RepositoriesEquHos.PessoaRepository.Delete(id);

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
        public Pessoa Salvar(Pessoa model, int? id = null)
        {
            try
            {
                Pessoa pessoa = null as Pessoa;

                using (var connection = this._unitOfWork.Create())
                {
                    if (id != null &&
                        id.HasValue)
                    {
                        model.Id = id;
                        pessoa = connection.RepositoriesEquHos.PessoaRepository.Update(model);
                    }
                    else
                    {
                        pessoa = connection.RepositoriesEquHos.PessoaRepository.Create(model);
                    }

                    connection.Commit();
                }

                return pessoa;
            }
            catch
            {
                throw;
            }
        }
    }
}