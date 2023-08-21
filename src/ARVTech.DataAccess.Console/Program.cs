namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    //using ARVTech.DataAccess.Business.EquHos;
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

        private static IEnumerable<PessoaJuridicaResponse>? _pessoasJuridicas = default;

        public static void Main(string[] args)
        {
            try
            {
                WriteConsole(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "*** {0} [ Versão {1} ] ***",
                        _productName,
                        _productVersion),
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

                using (var usuariosBusiness = new UsuarioBusiness(
                    _singletonDbManager.UnitOfWork))
                {
                    string username = "UserMain";

                    var usuarioResponse = usuariosBusiness.GetByUsername(
                        username);

                    if (usuarioResponse is null || usuarioResponse.Count() == 0)
                    {
                        var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                        {
                            Username = "UserMain",
                            Password = "(u53rM@1n)",
                            ConfirmPassword = "(u53rM@1n)",
                            DataPrimeiroAcesso = DateTimeOffset.UtcNow,
                        };

                        usuariosBusiness.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                using var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    _singletonDbManager.UnitOfWork);

                _pessoasJuridicas = pessoaJuridicaBusiness.GetAll();

                //  Importa os Demonstrativos de Pagamento.
                // importarDemonstrativosPagamento();

                //  Importa os Espelhos de Ponto.
                importarEspelhosPonto();
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
                    newLinesBefore: 1);
            }
        }

        /// <summary>
        /// Método que importa os Demonstrativos de Pagamento.
        /// </summary>
        private static void importarDemonstrativosPagamento()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                WriteConsole(
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
                {
                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var demonstrativosPagamento = transmissionUniPayCheck.GetDemonstrativosPagamento();

                foreach (var dp in demonstrativosPagamento)
                {
                    WriteConsole(
                        $"Demonstrativo de Pagamento ref. Competência: {dp.Competencia}; Matrícula: {dp.Matricula}; Nome: {dp.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        matriculaDemonstrativoPagamentoBusiness.Import(
                            dp);

                        WriteConsole(
                            "OK",
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
        }

        /// <summary>
        /// Método que importa os Espelhos de Ponto.
        /// </summary>
        private static void importarEspelhosPonto()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                WriteConsole(
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
                {
                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var espelhosPonto = transmissionUniPayCheck.GetEspelhosPonto();

                foreach (var ep in espelhosPonto)
                {
                    WriteConsole(
                        $"Espelho de Ponto ref. Competência: {ep.Competencia}; Matrícula: {ep.Matricula}; Nome: {ep.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        matriculaEspelhoPontoBusiness.Import(
                            ep);

                        WriteConsole(
                            "OK",
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
        }

        private static void GetOrCreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            _configuration = builder.Build();

            if (_configuration == null)
                throw new Exception("[ERRO] Não foi possível carregar as configurações do Integrador.");
        }

        private static void ApagarLog()
        {
            DateTime dataBase = DateTime.Now.AddDays(-7);

            try
            {
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "LOG*.log");

                foreach (string file in files)
                {
                    FileInfo fs = new FileInfo(file);

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
                        System.Environment.NewLine);

                    if (!string.IsNullOrEmpty(_arquivoLog))
                        WriteFile(
                            System.Environment.NewLine);
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

        private static void WriteFile(string texto)
        {
            using (StreamWriter streamWriter = new StreamWriter(
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