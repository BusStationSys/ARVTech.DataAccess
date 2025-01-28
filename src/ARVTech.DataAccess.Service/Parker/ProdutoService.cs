//namespace ARVTech.DataAccess.Business.Parker
//{
//    using System.Collections.Generic;
//    using ARVTech.DataAccess.Entities.Parker;
//    using ARVTech.DataAccess.DTOS.Parker;
//    //using ARVTech.DataAccess.Repository.Access.Parker;
//    using ARVTech.DataAccess.UnitOfWork.Interfaces;
//    using AutoMapper;

//    public class ProdutoService: BaseService
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ProdutoService"/> class.
//        /// </summary>
//        /// <param name="unitOfWork"></param>
//        public ProdutoService(IUnitOfWork unitOfWork)
//            : base(unitOfWork)
//        {
//            this._unitOfWork = unitOfWork;

//            this._mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<ProdutoEntity, ProdutoModel>();
//                cfg.CreateMap<ProdutoModel, ProdutoEntity>();
//            });
//        }

//        /// <summary>
//        /// Gets all "Produtos" records.
//        /// </summary>
//        /// <returns>If success, the list with all "Produtos" records. Otherwise, an exception detailing the problem.</returns>
//        public IEnumerable<ProdutoModel> Listar()
//        {
//            try
//            {
//                IEnumerable<ProdutoModel> dtos = null as IEnumerable<ProdutoModel>;

//                using (var connection = this._unitOfWork.Create())
//                {
//                    IEnumerable<ProdutoEntity> entities = ((ProdutoRepository)connection.RepositoriesParker.ProdutoRepository).GetAll();

//                    var mapper = new Mapper(this._mapperConfiguration);

//                    dtos = mapper.Map<IEnumerable<ProdutoModel>>(entities);
//                }

//                return dtos;
//            }
//            catch
//            {
//                throw;
//            }
//        }
//    }
//}