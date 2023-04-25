namespace ARVTech.DataAccess.Business.Empresarius
{
    using System;
    using ARVTech.DataAccess.Entities.Empresarius;
    using ARVTech.DataAccess.DTOS.Empresarius;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public class ProdutoService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ProdutoService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            this._mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProdutoEntity, ProdutoModel>();
                cfg.CreateMap<ProdutoModel, ProdutoEntity>();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ProdutoModel Salvar(ProdutoModel model, int? id = null)
        {
            try
            {
                ProdutoEntity entity = null as ProdutoEntity;

                var mapper = new Mapper(this._mapperConfiguration);

                using (var connection = this._unitOfWork.Create())
                {
                    entity = mapper.Map<ProdutoEntity>(model);

                    if (id != null &&
                        id.HasValue)
                    {
                        entity.Id = id;
                        entity = connection.RepositoriesEmpresarius.ProdutoRepository.Update(entity);
                    }
                    else
                    {
                        entity = connection.RepositoriesEmpresarius.ProdutoRepository.Create(entity);
                    }

                    connection.Commit();
                }

                model = mapper.Map<ProdutoModel>(entity);

                return model;
            }
            catch
            {
                throw;
            }
        }
    }
}
