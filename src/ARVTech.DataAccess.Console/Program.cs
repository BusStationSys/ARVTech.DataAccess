namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using ARVTech.DataAccess.Business.UniPayCheck;
    using ARVTech.DataAccess.Console.Enums;
    using ARVTech.DataAccess.DbManager;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        private static IConfiguration _configuration;

        private readonly static Assembly _assembly = Assembly.GetExecutingAssembly();

        private readonly static FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(
            _assembly.Location);

        private readonly static string _productName = fvi.ProductName;
        private readonly static string _productVersion = fvi.ProductVersion;

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
                writeConsole(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "*** {0} [ Versão {1} ] ***",
                        _productName,
                        _productVersion),
                    bootstrapColor: BootstrapColorEnum.Primary);

                writeConsole("Limpando Log",
                    newLinesBefore: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                apagarLog();

                writeConsole(
                    "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
                    newLinesBefore: 2,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                getOrCreateConfiguration();

                _singletonDbManager = new ContextDbManager(
                    DatabaseTypeEnum.SqlServer,
                    _configuration);

                using (var usuarioBusiness = new UsuarioBusiness(
                    _singletonDbManager.UnitOfWork))
                {
                    string username = "UserMain";

                    IEnumerable<UsuarioResponseDto> usuariosResponseDto = usuarioBusiness.GetByUsername(
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
                        };

                        usuarioBusiness.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                using var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    _singletonDbManager.UnitOfWork);

                _pessoasJuridicas = pessoaJuridicaBusiness.GetAll();

                //  Importa os Demonstrativos de Pagamento.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("DP"))
                    importarDemonstrativosPagamento();

                ////  Importa os Espelhos de Ponto.
                //if (args is null ||
                //    args.Length == 0 ||
                //    args.Contains("EP"))
                //    importarEspelhosPonto();

                //  Importa as Matrículas.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("M"))
                    importarMatriculas();
            }
            catch (Exception ex)
            {
                writeConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
            finally
            {
                writeConsole(
                    "*** Término da execução do ARVTech.DataAccess®. ***",
                    newLinesBefore: 1);
            }
        }

        /// <summary>
        /// Método que importa as Matrículas.
        /// </summary>
        private static void importarMatriculas()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO as Matrículas do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaBusiness = new MatriculaBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                    continue;

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var matriculas = transmissionUniPayCheck.GetMatriculas();

                if (matriculas == null ||
                    matriculas.Count() == 0)
                {
                    writeConsole(
                        $@"Não encontrado nenhum arquivo de importação de Matrículas no diretório {pathDirectoryOrFileNameSource}.",
                        newLinesAfter: 1,
                        newLinesBefore: 0,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                foreach (var matricula in matriculas)
                {
                    writeConsole(
                        $"Matrícula {matricula.Matricula}; Colaborador CPF: {matricula.Cpf}; Nome: {matricula.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        matriculaBusiness.Import(
                            matricula);

                        writeConsole(
                            "OK",
                            newLinesAfter: 1,
                            bootstrapColor: BootstrapColorEnum.Success,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
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
        }

        /// <summary>
        /// Método que importa os Demonstrativos de Pagamento.
        /// </summary>
        private static void importarDemonstrativosPagamento()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO os Demonstrativos de Pagamento do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaDemonstrativoPagamentoBusiness = new MatriculaDemonstrativoPagamentoBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                    continue;

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var demonstrativosPagamento = transmissionUniPayCheck.GetDemonstrativosPagamento();

                if (demonstrativosPagamento == null ||
                    demonstrativosPagamento.Count() == 0)
                {
                    writeConsole(
                        $@"Não encontrado nenhum arquivo de importação de Demonstrativo de Pagamento no diretório {pathDirectoryOrFileNameSource}.",
                        newLinesAfter: 1,
                        newLinesBefore: 0,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                foreach (var dp in demonstrativosPagamento)
                {
                    writeConsole(
                        $"Demonstrativo de Pagamento ref. Competência: {dp.Competencia}; Matrícula: {dp.Matricula}; Nome: {dp.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        matriculaDemonstrativoPagamentoBusiness.Import(
                            dp);

                        writeConsole(
                            "OK",
                            newLinesAfter: 1,
                            bootstrapColor: BootstrapColorEnum.Success,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
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
        }

        /// <summary>
        /// Método que importa os Espelhos de Ponto.
        /// </summary>
        private static void importarEspelhosPonto()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO os Espelho de Ponto do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaEspelhoPontoBusiness = new MatriculaEspelhoPontoBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                    continue;

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var espelhosPonto = transmissionUniPayCheck.GetEspelhosPonto();

                if (espelhosPonto == null ||
                    espelhosPonto.Count() == 0)
                {
                    writeConsole(
                        $@"Não encontrado nenhum arquivo de importação de Espelho de Ponto no diretório {pathDirectoryOrFileNameSource}.",
                        newLinesAfter: 1,
                        newLinesBefore: 0,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                foreach (var ep in espelhosPonto)
                {
                    writeConsole(
                        $"Espelho de Ponto ref. Competência: {ep.Competencia}; Matrícula: {ep.Matricula}; Nome: {ep.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        matriculaEspelhoPontoBusiness.Import(
                            ep);

                        writeConsole(
                            "OK",
                            newLinesAfter: 1,
                            bootstrapColor: BootstrapColorEnum.Success,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void getOrCreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            _configuration = builder.Build();

            if (_configuration == null)
                throw new Exception("[ERRO] Não foi possível carregar as configurações do Integrador.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void apagarLog()
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
        private static void writeConsole(string texto, int newLinesBefore = 0, int newLinesAfter = 0, BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary, bool showDate = true)
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
                        writeFile(
                            System.Environment.NewLine);
                }
            }

            Console.Write(content);
            if (!string.IsNullOrEmpty(_arquivoLog))
                writeFile(content);

            if (newLinesAfter > 0)
            {
                for (int i = 0; i < newLinesAfter; i++)
                {
                    Console.Write(
                        Environment.NewLine);

                    if (!string.IsNullOrEmpty(
                        _arquivoLog))
                        writeFile(
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
        private static void writeFile(string texto)
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