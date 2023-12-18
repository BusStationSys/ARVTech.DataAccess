namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Business.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoBusiness : BaseBusiness, IMatriculaDemonstrativoPagamentoBusiness
    {
        private readonly int _idBaseFgts = 1;
        private readonly int _idValorFgts = 2;
        private readonly int _idTotalVencimentos = 3;
        private readonly int _idTotalDescontos = 4;
        private readonly int _idBaseIrrf = 5;
        private readonly int _idBaseInss = 6;
        private readonly int _idTotalLiquido = 7;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaDemonstrativoPagamentoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestCreateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestUpdateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoEventoResponseDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoEventoRequestDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
                cfg.CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
                cfg.CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponseDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequestDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
                cfg.CreateMap<TotalizadorRequestDto, TotalizadorEntity>().ReverseMap();
                cfg.CreateMap<TotalizadorResponseDto, TotalizadorEntity>().ReverseMap();
                cfg.CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Delete(
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
        /// <param name="competencia"></param>
        /// <param name="guidMatricula"></param>
        public void Delete(string competencia, Guid guidMatricula)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.DeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(
                    competencia,
                    guidMatricula);

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
        public MatriculaDemonstrativoPagamentoResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoResponseDto>(
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
        /// <param name="competencia"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> Get(string competencia, string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(
                            competencia));
                else if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(
                            matricula));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        competencia,
                        matricula);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponseDto>>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponseDto>>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetByGuidColaborador(Guid guidColaborador)
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetByGuidColaborador(
                        guidColaborador);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponseDto>>(entity);
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
        /// <param name="demonstrativoPagamentoResult"></param>
        /// <returns></returns>
        public ExecutionResponseDto<MatriculaDemonstrativoPagamentoResponseDto> Import(DemonstrativoPagamentoResult demonstrativoPagamentoResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                //  Verifica se existe o registro da Matrícula.
                var matriculaResponseDto = default(
                    MatriculaResponseDto);

                using (var matriculaBusiness = new MatriculaBusiness(
                    this._unitOfWork))
                {
                    matriculaResponseDto = matriculaBusiness.GetByMatricula(
                        demonstrativoPagamentoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaResponseDto is null)
                    return new ExecutionResponseDto<MatriculaDemonstrativoPagamentoResponseDto>
                    {
                        Message = $"Matrícula {demonstrativoPagamentoResult.Matricula} não encontrada. O registro deve ser cadastrado/importado préviamente.",
                    };

                connection.BeginTransaction();

                //  Verifica se existe o registro do Demonstrativo de Pagamento da Matrícula.
                string competencia = string.Concat(
                    "01/",
                    demonstrativoPagamentoResult.Competencia);

                competencia = Convert.ToDateTime(
                    competencia).ToString("yyyyMMdd");

                //  Independente se existir um ou mais registros de Demonstrativos de Pagamento para a Matrícula, deve forçar a limpeza dos itens dos Demonstrativos de Pagamento que possam estar vinculados à Matrícula dentro da Competência.
                this.Delete(
                    competencia,
                    matriculaResponseDto.Guid);

                IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> matriculasDemonstrativosPagamentoResponseDto = this.Get(
                    competencia,
                    demonstrativoPagamentoResult.Matricula);

                //  Se não existir o registro do Demonstrativo de Pagamento da Matrícula, adiciona.
                if (matriculasDemonstrativosPagamentoResponseDto is null ||
                    matriculasDemonstrativosPagamentoResponseDto.Count() == 0)
                {
                    var matriculaDemonstrativoPagamentoRequestCreateDto = new MatriculaDemonstrativoPagamentoRequestCreateDto
                    {
                        GuidMatricula = matriculaResponseDto.Guid,
                        Competencia = competencia,
                    };

                    var newMdp = this.SaveData(
                        matriculaDemonstrativoPagamentoRequestCreateDto);

                    ((List<MatriculaDemonstrativoPagamentoResponseDto>)matriculasDemonstrativosPagamentoResponseDto).Add(
                        newMdp);
                }

                // Processa os Eventos.
                if (demonstrativoPagamentoResult?.Eventos.Count > 0)
                {
                    foreach (var evento in demonstrativoPagamentoResult.Eventos)
                    {
                        //  Verifica se existe o registro do Evento.
                        var eventoResponseDto = default(
                            EventoResponseDto);

                        using (var eventoBusiness = new EventoBusiness(
                            this._unitOfWork))
                        {
                            eventoResponseDto = eventoBusiness.Get(
                                Convert.ToInt32(
                                    evento.Codigo));
                        }

                        //  Se não existir o registro do Evento, adiciona.
                        if (eventoResponseDto is null)
                        {
                            var eventoRequestDto = new EventoRequestDto
                            {
                                Id = Convert.ToInt32(
                                    evento.Codigo),
                                Descricao = evento.Descricao,
                                Tipo = evento.Tipo,
                            };

                            using (var eventoBusiness = new EventoBusiness(
                                this._unitOfWork))
                            {
                                eventoResponseDto = eventoBusiness.SaveData(
                                    eventoRequestDto);
                            }
                        }

                        // Processa os Vínculos dos Eventos.
                        this.processRecordMDPEvento(
                            matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                            Convert.ToInt32(
                                eventoResponseDto.Id),
                            !string.IsNullOrEmpty(
                                evento.Referencia) ? Convert.ToDecimal(
                                    evento.Referencia) : default(decimal?),
                            Convert.ToDecimal(
                                    evento.Valor));
                    }
                }

                // Processa os Vínculos dos Totalizadores.
                decimal baseFgts = decimal.Zero;
                decimal valorFgts = decimal.Zero;
                decimal totalVencimentos = decimal.Zero;
                decimal totalDescontos = decimal.Zero;
                decimal baseIrrf = decimal.Zero;
                decimal baseInss = decimal.Zero;
                decimal totalLiquido = decimal.Zero;

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseFgts))
                    baseFgts = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseFgts);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.ValorFgts))
                    valorFgts = Convert.ToDecimal(
                        demonstrativoPagamentoResult.ValorFgts);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalVencimentos))
                    totalVencimentos = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalVencimentos);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalDescontos))
                    totalDescontos = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalDescontos);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseIrrf))
                    baseIrrf = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseIrrf);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseInss))
                    baseInss = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseInss);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalLiquido))
                    totalLiquido = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalLiquido);

                //  Processa a Base Fgts.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseFgts,
                    baseFgts);

                //  Processa o Valor Fgts.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idValorFgts,
                    valorFgts);

                //  Processa o Total de Vencimentos.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalVencimentos,
                    totalVencimentos);

                //  Processa o Total de Descontos.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalDescontos,
                    totalDescontos);

                //  Processa a Base Irrf.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseIrrf,
                    baseIrrf);

                //  Processa a Base Inss.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseInss,
                    baseInss);

                //  Processa o Total Líquido.
                this.processRecordMDPTotalizador(
                    matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalLiquido,
                    totalLiquido);

                connection.CommitTransaction();

                return new ExecutionResponseDto<MatriculaDemonstrativoPagamentoResponseDto>
                {
                    Data = matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault(),
                    Success = true,
                };
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
        /// <param name="createDto"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoResponseDto SaveData(MatriculaDemonstrativoPagamentoRequestCreateDto? createDto = null, MatriculaDemonstrativoPagamentoRequestUpdateDto? updateDto = null)
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
                    MatriculaDemonstrativoPagamentoEntity);

                connection.BeginTransaction();

                if (updateDto != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoResponseDto>(
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idEvento"></param>
        /// <param name="referencia"></param>
        /// <param name="valor"></param>
        private void processRecordMDPEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento, decimal? referencia, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Evento.
                var matriculaDemonstrativoPagamentoEventoResponseDto = default(
                    MatriculaDemonstrativoPagamentoEventoResponseDto);

                using (var matriculaDemonstrativoPagamentoEventoBusiness = new MatriculaDemonstrativoPagamentoEventoBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoEventoResponseDto = matriculaDemonstrativoPagamentoEventoBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(
                        guidMatriculaDemonstrativoPagamento,
                        idEvento);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Evento, adiciona.
                    var matriculaDemonstrativoPagamentoEventoRequestDto = default(
                        MatriculaDemonstrativoPagamentoEventoRequestDto);

                    if (matriculaDemonstrativoPagamentoEventoResponseDto is null)
                        matriculaDemonstrativoPagamentoEventoRequestDto = new MatriculaDemonstrativoPagamentoEventoRequestDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdEvento = idEvento,
                        };
                    else
                        matriculaDemonstrativoPagamentoEventoRequestDto = new MatriculaDemonstrativoPagamentoEventoRequestDto
                        {
                            Guid = matriculaDemonstrativoPagamentoEventoResponseDto.Guid,
                            GuidMatriculaDemonstrativoPagamento = matriculaDemonstrativoPagamentoEventoResponseDto.GuidMatriculaDemonstrativoPagamento,
                            IdEvento = matriculaDemonstrativoPagamentoEventoResponseDto.IdEvento,
                        };

                    matriculaDemonstrativoPagamentoEventoRequestDto.Referencia = referencia;
                    matriculaDemonstrativoPagamentoEventoRequestDto.Valor = valor;

                    matriculaDemonstrativoPagamentoEventoBusiness.SaveData(
                        matriculaDemonstrativoPagamentoEventoRequestDto);
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
        /// <param name="valor"></param>
        private void processRecordMDPTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Totalizador.
                var matriculaDemonstrativoPagamentoTotalizadorResponseDto = default(
                    MatriculaDemonstrativoPagamentoTotalizadorResponseDto);

                using (var matriculaDemonstrativoPagamentoTotalizadorBusiness = new MatriculaDemonstrativoPagamentoTotalizadorBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoTotalizadorResponseDto = matriculaDemonstrativoPagamentoTotalizadorBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(
                        guidMatriculaDemonstrativoPagamento,
                        idTotalizador);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Totalizador, adiciona.
                    var matriculaDemonstrativoPagamentoTotalizadorRequestDto = default(
                        MatriculaDemonstrativoPagamentoTotalizadorRequestDto);

                    if (matriculaDemonstrativoPagamentoTotalizadorResponseDto is null)
                    {
                        matriculaDemonstrativoPagamentoTotalizadorRequestDto = new MatriculaDemonstrativoPagamentoTotalizadorRequestDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdTotalizador = idTotalizador,
                        };
                    }

                    matriculaDemonstrativoPagamentoTotalizadorRequestDto.Valor = valor;

                    matriculaDemonstrativoPagamentoTotalizadorBusiness.SaveData(
                        matriculaDemonstrativoPagamentoTotalizadorRequestDto);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}