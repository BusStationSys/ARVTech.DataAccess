namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoTotalizadorService : BaseService
    {
        public MatriculaDemonstrativoPagamentoTotalizadorService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequestDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponseDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestCreateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestUpdateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<TotalizadorRequestDto, TotalizadorEntity>().ReverseMap();
                cfg.CreateMap<TotalizadorResponseDto, TotalizadorEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.Delete(
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
        public MatriculaDemonstrativoPagamentoTotalizadorResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoTotalizadorResponseDto>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorResponseDto>>(
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
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idTotalizador"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoTotalizadorResponseDto GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador)
        {
            try
            {
                if (guidMatriculaDemonstrativoPagamento == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaDemonstrativoPagamento));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(
                        guidMatriculaDemonstrativoPagamento,
                        idTotalizador);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoTotalizadorResponseDto>(
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoTotalizadorResponseDto SaveData(MatriculaDemonstrativoPagamentoTotalizadorRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaDemonstrativoPagamentoTotalizadorEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    string x = string.Empty;

                    //entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.Update(
                    //    entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoTotalizadorResponseDto>(
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
    }
}