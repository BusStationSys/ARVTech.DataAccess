namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Shared;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoEventoService : BaseService
    {
        public MatriculaDemonstrativoPagamentoEventoService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoEventoRequestDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoEventoResponseDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();

                cfg.CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
                cfg.CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Delete(
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
        public MatriculaDemonstrativoPagamentoEventoResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoEventoResponseDto>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoEventoResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoEventoResponseDto>>(
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
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoEventoResponseDto GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento)
        {
            try
            {
                if (guidMatriculaDemonstrativoPagamento == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaDemonstrativoPagamento));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(
                        guidMatriculaDemonstrativoPagamento,
                        idEvento);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoEventoResponseDto>(
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
        public MatriculaDemonstrativoPagamentoEventoResponseDto SaveData(MatriculaDemonstrativoPagamentoEventoRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEventoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    string x = string.Empty;

                    //entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Update(
                    //    entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Create(
                        entity);
                }

                //  Atualiza o Valor criptografando a informação usando como chave o GuidMatricula do Demonstrativo de Pagamento Evento.
                var key = entity.Guid.ToString("N").ToUpper();

                //entity.Valor = PasswordCryptography.EncryptString(
                //    key,
                //    dto.Valor.ToString("#,###,###,##0.00"));

                entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Update(
                    entity.Guid,
                    entity);

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoEventoResponseDto>(
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
    }
}