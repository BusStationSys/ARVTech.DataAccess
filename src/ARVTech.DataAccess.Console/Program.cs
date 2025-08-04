namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using ARVTech.DataAccess.Service.UniPayCheck;
    using ARVTech.DataAccess.Service.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Console.Enums;
    using ARVTech.DataAccess.DbManager;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck.Enums;
    using ARVTech.Transmission.Engine.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        private static IConfiguration _configuration;

        private readonly static Assembly _assembly = Assembly.GetExecutingAssembly();

        private readonly static FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(
            _assembly.Location);

        private readonly static string _productName = fvi.ProductName;

        private readonly static string _fileVersion = fvi.FileVersion;

        private readonly static string _arquivoLog = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}\\Log{1}{2}.log",
            AppDomain.CurrentDomain.BaseDirectory,
            _assembly.GetName().Name?.Replace(
                ".Console",
                string.Empty),
            DateTime.Now.ToString("yyyyMMddHHmm"));

        private static ContextDbManager? _singletonDbManager = default;

        private static IEnumerable<PessoaJuridicaResponseDto>? _pessoasJuridicas = default;

        public static void Main(string[] args)
        {
            try
            {
                WriteConsole(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "*** {0} [ Versão {1} ] ***",
                        _productName,
                        _fileVersion),
                    bootstrapColor: BootstrapColorEnum.Primary);

                WriteConsole("Limpando Log",
                    newLinesBefore: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                ApagarLog();

                WriteConsole(
                    "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
                    newLinesBefore: 2,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                GetOrCreateConfiguration();

                _singletonDbManager = new ContextDbManager(
                    DatabaseTypeEnum.SqlServer,
                    _configuration);

                using (var usuarioService = new UsuarioService(
                    _singletonDbManager.UnitOfWork))
                {
                    string username = "UserMain";

                    IEnumerable<UsuarioResponseDto> usuariosResponseDto = usuarioService.GetByUsername(
                        username);

                    if (usuariosResponseDto is null ||
                        usuariosResponseDto.Count() == 0)
                    {
                        var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                        {
                            Username = "UserMain",
                            Password = "(u53rM@1n)",
                            ConfirmPassword = "(u53rM@1n)",
                            DataPrimeiroAcesso = DateTimeOffset.UtcNow,
                            IdPerfilUsuario = PerfilUsuarioEnum.UserMain,
                        };

                        usuarioService.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                using var pessoaJuridicaService = new PessoaJuridicaService(
                    _singletonDbManager.UnitOfWork);

                //  Importa os Empregadores.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("E"))
                    ImportarEmpregadores();

                _pessoasJuridicas = pessoaJuridicaService.GetAll();

                //  Importa as Matrículas.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("M"))
                    ImportarMatriculas();

                //  Importa os Espelhos de Ponto.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("EP"))
                    ImportarEspelhosPonto();

                //  Importa os Demonstrativos de Pagamento.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("DP"))
                    ImportarDemonstrativosPagamento();
            }
            catch (Exception ex)
            {
                WriteConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
            finally
            {
                WriteConsole(
                    "*** Término da execução do ARVTech.DataAccess®. ***",
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);
            }
        }

        /// <summary>
        /// Método que importa os Demonstrativos de Pagamento.
        /// </summary>
        private static void ImportarDemonstrativosPagamento()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                WriteConsole(
                    $"PROCESSANDO os Demonstrativos de Pagamento do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaDemonstrativoPagamentoService = new MatriculaDemonstrativoPagamentoService(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                {
                    WriteConsole(
                        $"Diretório {pathDirectoryOrFileNameSource} não encontrado.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var conteudoDemonstrativosPagamento = transmissionUniPayCheck.GetConteudoDemonstrativosPagamento();

                if (string.IsNullOrEmpty(
                    conteudoDemonstrativosPagamento))
                {
                    WriteConsole(
                        $@"Arquivo de importação de Demonstrativos de Pagamento no diretório {pathDirectoryOrFileNameSource} encontra-se vazio.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);
                    return;
                }

                try
                {
                    var resumoImportacaoDemonstrativosPagamentoResponseDto = matriculaDemonstrativoPagamentoService.ImportFileDemonstrativosPagamento(
                        pessoaJuridica.Cnpj,
                        conteudoDemonstrativosPagamento);

                    WriteConsole(
                        $"Demonstrativos de Pagamento Atualizados: {resumoImportacaoDemonstrativosPagamentoResponseDto.QuantidadeRegistrosAtualizados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Info,
                        showDate: false);

                    WriteConsole(
                        $"Demonstrativos de Pagamento Inalterados: {resumoImportacaoDemonstrativosPagamentoResponseDto.QuantidadeRegistrosInalterados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning,
                        showDate: false);

                    WriteConsole(
                        $"Demonstrativos de Pagamento Inseridos: {resumoImportacaoDemonstrativosPagamentoResponseDto.QuantidadeRegistrosInseridos}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Success,
                        showDate: false);

                    WriteConsole(
                        $"Demonstrativos de Pagamento Rejeitados: {resumoImportacaoDemonstrativosPagamentoResponseDto.QuantidadeRegistrosRejeitados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Secondary,
                        showDate: false);
                }
                catch (Exception ex)
                {
                    WriteConsole(
                        string.Concat(
                            ex.Message,
                            " ",
                            ex.InnerException?.InnerException),
                        newLinesAfter: 1,
                        newLinesBefore: 1,
                        bootstrapColor: BootstrapColorEnum.Danger);
                }
            }
        }

        /// <summary>
        /// Método que importa os Empregadores.
        /// </summary>
        private static void ImportarEmpregadores()
        {
            WriteConsole(
                $"PROCESSANDO os Empregadores",
                newLinesBefore: 1,
                newLinesAfter: 2,
                bootstrapColor: BootstrapColorEnum.Dark);

            using var pessoaJuridicaService = new PessoaJuridicaService(
                _singletonDbManager.UnitOfWork);

            var pathDirectoryOrFileNameSource =
                $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\Empregadores";

            if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                !File.Exists(pathDirectoryOrFileNameSource))
                return;

            var transmissionUniPayCheck = new TransmissionUniPayCheck(
                pathDirectoryOrFileNameSource);

            var conteudoEmpregadores = transmissionUniPayCheck.GetConteudoEmpregadores();

            if (string.IsNullOrEmpty(
                conteudoEmpregadores))
            {
                WriteConsole(
                    $@"Não encontrado nenhum arquivo de importação de Empregadores no diretório {pathDirectoryOrFileNameSource}.",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Warning);
                return;
            }

            try
            {
                var resumoImportacaoEmpregadoresResponseDto = pessoaJuridicaService.ImportFileEmpregadores(
                    conteudoEmpregadores);

                WriteConsole(
                    $"Empregadores Atualizados: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosAtualizados}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Info,
                    showDate: false);

                WriteConsole(
                    $"Empregadores Inalterados: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosInalterados}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Warning,
                    showDate: false);

                WriteConsole(
                    $"Empregadores Inseridos: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosInseridos}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Success,
                    showDate: false);
            }
            catch (Exception ex)
            {
                WriteConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesAfter: 1,
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
        }

        /// <summary>
        /// Método que importa os Espelhos de Ponto.
        /// </summary>
        private static void ImportarEspelhosPonto()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                WriteConsole(
                    $"PROCESSANDO os Espelhos de Ponto do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaEspelhoPontoService = new MatriculaEspelhoPontoService(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                {
                    WriteConsole(
                        $"Diretório {pathDirectoryOrFileNameSource} não encontrado.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var conteudoEspelhosPonto = transmissionUniPayCheck.GetConteudoEspelhosPonto();

                if (string.IsNullOrEmpty(
                    conteudoEspelhosPonto))
                {
                    WriteConsole(
                        $@"Arquivo de importação de Espelhos Ponto no diretório {pathDirectoryOrFileNameSource} encontra-se vazio.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);
                    return;
                }

                try
                {
                    var resumoImportacaoEspelhosPontoResponseDto = matriculaEspelhoPontoService.ImportFileEspelhosPonto(
                        pessoaJuridica.Cnpj,
                        conteudoEspelhosPonto);

                    WriteConsole(
                        $"Espelhos Ponto Atualizados: {resumoImportacaoEspelhosPontoResponseDto.QuantidadeRegistrosAtualizados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Info,
                        showDate: false);

                    WriteConsole(
                        $"Espelhos Ponto Inalterados: {resumoImportacaoEspelhosPontoResponseDto.QuantidadeRegistrosInalterados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning,
                        showDate: false);

                    WriteConsole(
                        $"Espelhos Ponto Inseridos: {resumoImportacaoEspelhosPontoResponseDto.QuantidadeRegistrosInseridos}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Success,
                        showDate: false);

                    WriteConsole(
                        $"Espelhos Ponto Rejeitados: {resumoImportacaoEspelhosPontoResponseDto.QuantidadeRegistrosRejeitados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Secondary,
                        showDate: false);
                }
                catch (Exception ex)
                {
                    WriteConsole(
                        string.Concat(
                            ex.Message,
                            " ",
                            ex.InnerException?.InnerException),
                        newLinesAfter: 1,
                        newLinesBefore: 1,
                        bootstrapColor: BootstrapColorEnum.Danger);
                }
            }
        }

        /// <summary>
        /// Método que importa as Matrículas.
        /// </summary>
        private static void ImportarMatriculas()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                WriteConsole(
                    $"PROCESSANDO as Matrículas do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaService = new MatriculaService(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                {
                    WriteConsole(
                        $"Diretório {pathDirectoryOrFileNameSource} não encontrado.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var conteudoMatriculas = transmissionUniPayCheck.GetConteudoMatriculas();

                if (string.IsNullOrEmpty(
                    conteudoMatriculas))
                {
                    WriteConsole(
                        $@"Arquivo de importação de Matrículas no diretório {pathDirectoryOrFileNameSource} encontra-se vazio.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);
                    return;
                }

                try
                {
                    var resumoImportacaoMatriculasResponseDto = matriculaService.ImportFileMatriculas(
                        pessoaJuridica.Cnpj,
                        conteudoMatriculas);

                    WriteConsole(
                        $"Matrículas Atualizadas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosAtualizados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Info,
                        showDate: false);

                    WriteConsole(
                        $"Matrículas Inalteradas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosInalterados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning,
                        showDate: false);

                    WriteConsole(
                        $"Matrículas Inseridas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosInseridos}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Success,
                        showDate: false);
                }
                catch (Exception ex)
                {
                    WriteConsole(
                        string.Concat(
                            ex.Message,
                            " ",
                            ex.InnerException?.InnerException),
                        newLinesAfter: 1,
                        newLinesBefore: 1,
                        bootstrapColor: BootstrapColorEnum.Danger);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void GetOrCreateConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"; // "Production" é o default

            // Configura o builder de configurações
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // arquivo padrão
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);  // arquivo específico do ambiente

            _configuration = builder.Build();

            if (_configuration == null)
                throw new Exception("[ERRO] Não foi possível carregar as configurações do Integrador.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ApagarLog()
        {
            DateTime dataBase = DateTime.Now.AddDays(-7);

            try
            {
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "LOG*.log");

                foreach (string file in files)
                {
                    var fs = new FileInfo(file);

                    if (fs.LastWriteTime < dataBase)
                        File.Delete(file);
                }
            }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="newLinesBefore"></param>
        /// <param name="newLinesAfter"></param>
        /// <param name="bootstrapColor"></param>
        /// <param name="showDate"></param>
        private static void WriteConsole(string texto, int newLinesBefore = 0, int newLinesAfter = 0, BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary, bool showDate = true)
        {
            Console.ForegroundColor = GetColorFromBootstrap(bootstrapColor);

            string content = showDate ? string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff"),
                texto) : string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}",
                    texto);

            if (newLinesBefore > 0)
            {
                for (int i = 0; i < newLinesBefore; i++)
                {
                    Console.Write(
                        System.Environment.NewLine);

                    if (!string.IsNullOrEmpty(_arquivoLog))
                        WriteFile(
                            System.Environment.NewLine);
                }
            }

            Console.Write(content);
            if (!string.IsNullOrEmpty(_arquivoLog))
                WriteFile(content);

            if (newLinesAfter > 0)
            {
                for (int i = 0; i < newLinesAfter; i++)
                {
                    Console.Write(
                        Environment.NewLine);

                    if (!string.IsNullOrEmpty(
                        _arquivoLog))
                        WriteFile(
                            Environment.NewLine);
                }
            }

            static ConsoleColor GetColorFromBootstrap(BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary)
            {
                Console.ResetColor();

                if (bootstrapColor != BootstrapColorEnum.Secondary)
                {
                    switch (bootstrapColor)
                    {
                        case BootstrapColorEnum.Primary:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;

                        case BootstrapColorEnum.Success:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;

                        case BootstrapColorEnum.Danger:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;

                        case BootstrapColorEnum.Warning:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;

                        case BootstrapColorEnum.Info:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;

                        case BootstrapColorEnum.Light:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case BootstrapColorEnum.Dark:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                    }
                }

                return Console.ForegroundColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        private static void WriteFile(string texto)
        {
            using (var streamWriter = new StreamWriter(
                _arquivoLog,
                true))
            {
                streamWriter.Write(texto);
                streamWriter.Flush();

                streamWriter.Close();
            }
        }
    }
}