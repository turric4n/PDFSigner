using CommandLine;
using CommandLineSelectableMenu;
using PDFSign.Database.Factories;
using PDFSign.Exceptions;
using PDFSign.Models;
using PDFSign.Repositories;
using QuickLogger.Extensions.Wrapper.Application.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using PDFSign.Services;

namespace PDFSign
{
    class Program
    {
        private const string ConnectionString = "Data Source=";
        private static DbConnectionFactory _sqlConnectionFactory;
        private static CertificateDataRepository _certificateDataRepository;
        private static ILoggerService _logger;

        private static void InitLogger()
        {
            var executingPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (executingPath != null)
            {
                var settingsPath = Path.Combine(executingPath, "QuickLogger.json");

                if (!File.Exists(settingsPath))
                {
                    Console.WriteLine($"Logger settings not found {settingsPath}");
                    return;
                }

                var settings = File.ReadAllText(settingsPath);
                _logger = new QuickLoggerService(settings);
            }

            _logger?.Info("InitLogger()", "Logger Init");
        }
        static void CreateMainMenu(SelectableMenuOptions options)
        {
            Console.Clear();
            var mainMenu = new SelectableMenu<Action>(options);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Main Menu");

            // add menu item.
            mainMenu.Add("List Certificates", () =>
            {
                CertificatesListMenu(options);
            });
            mainMenu.Add("Add new certificate settings", () =>
            {
                var certificate = new CertificateData();
                NewCertificate(certificate, options);
            });

            mainMenu.Add("Exit ", () => { System.Environment.Exit(1); });

            mainMenu.Draw().Invoke();
        }

        static void NewCertificate(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"New CertificateData");
            var certificatesOptions = new SelectableMenu<Action>(options);
            var props = cert.GetType().GetProperties();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(cert);

                value = name == "Password" ? "****" : value;

                certificatesOptions.Add($"{name} : {value}", () =>
                {
                    if (name != "Id")
                    {
                        NewCertificateValue(cert, name, options);
                    }
                    else
                    {
                        NewCertificate(cert, options);
                    }                    
                });
            }

            certificatesOptions.Add("Save", () =>
            {
                try
                {
                    _certificateDataRepository.Add(cert);
                    _logger?.Info("NewCertificate()", "Certificate added into repository.");
                    CreateMainMenu(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Validation errors : { ex.Message }");
                    Console.ReadLine();
                    NewCertificate(cert, options);
                }                
            });

            certificatesOptions.Add("Cancel", () =>
            {
                CreateMainMenu(options);
            });

            certificatesOptions.Draw().Invoke();
        }
        static void CertificateDelete(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Are you sure to delete : {cert.BusinessName} ?");

            var certificatesOptions = new SelectableMenu<Action>(options);
            certificatesOptions.Add("Yes", () =>
            {
                _certificateDataRepository.Delete(cert);
                _logger?.Info("CertificateDelete()", "Certificate deleted from repository.");
            });
            certificatesOptions.Add("No", () =>
            {
                CertificateOptions(cert, options);
            });

            certificatesOptions.Draw().Invoke();
        }

        static void NewCertificateValue(CertificateData cert, string Name, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Editing value : {Name}");
            Console.WriteLine($"New value : ");
            var consoleInput = Console.ReadLine();
            var prop = cert
                .GetType()
                .GetProperties()
                .FirstOrDefault(x => x.Name == Name);

            prop.SetValue(cert, consoleInput);
           
            NewCertificate(cert, options);
        }

        static void UpdateCertificateValue(CertificateData cert, string Name, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Editing value : {Name}");
            Console.WriteLine($"New value : ");
            var consoleInput = Console.ReadLine();
            var certificateOptions = new SelectableMenu<Action>(options);

            var certificatePropertyInfo = cert
                .GetType()
                .GetProperties()
                .FirstOrDefault(x => x.Name == Name);

            if (certificatePropertyInfo != null) certificatePropertyInfo.SetValue(cert, consoleInput);

            try
            {
                var newCertificate = new CertificateData(cert.Id, cert.Password, cert.Path, cert.BusinessName);

                certificateOptions.Add("Save", () =>
                {
                    _certificateDataRepository.Update(newCertificate);
                    _logger?.Info("UpdateCertificateValue()", "Certificate updated into repository.");
                    CertificateUpdate(newCertificate, options);
                });

                certificateOptions.Add("Cancel", () =>
                {
                    NewCertificate(newCertificate, options);
                });
                certificateOptions.Draw().Invoke();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Entity validation errors : { ex.Message }");
                Console.ReadLine();
                UpdateCertificateValue(cert, Name, options);
            }
        }

        static void CertificateUpdate(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Certificate edit : {cert.BusinessName}");

            var selectableMenu = new SelectableMenu<Action>(options);
            var props = cert.GetType().GetProperties();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(cert);

                value = name == "Password" ? "****" : value;

                selectableMenu.Add($"{name} : {value}", () =>
               {
                   if (name != "Id")
                   {
                       UpdateCertificateValue(cert, name, options);
                   }
                   else
                   {
                       CertificateUpdate(cert, options);
                   }                   
               });
            }
            selectableMenu.Add("Back", Console.Clear);

            selectableMenu
                .Draw()
                .Invoke();
        }
        static void CertificateOptions(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Certificate edit : {cert.BusinessName}");

            var certificateOptions = new SelectableMenu<Action>(options);
            certificateOptions.Add("Update values", () =>
            {
                CertificateUpdate(cert, options);
            });
            certificateOptions.Add("Delete", () =>
            {
                CertificateDelete(cert, options);
                CertificatesListMenu(options);
            });

            certificateOptions.Add("Back", Console.Clear);

            certificateOptions.Draw().Invoke();
        }
        static void CertificatesListMenu(SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Certificates");

            var certificatesMenu = new SelectableMenu<Action>(options);

            var certificates = _certificateDataRepository.GetAll();
            foreach (var cert in certificates)
            {
                certificatesMenu.Add(cert.BusinessName, () =>
                {
                    CertificateOptions(cert, options);
                    CertificatesListMenu(options);
                });
            }

            certificatesMenu.Add("Back", () =>
            {
                Console.Clear();
                CreateMainMenu(options);
            });

            certificatesMenu.Draw().Invoke();
        }

        private static void Setup()
        {
            var menuOptions = new SelectableMenuOptions()
            {
                SelectedType = new ArrowSelectedType()
                {
                    SelectedColor = ConsoleColor.Green,
                },
                IsClearAfterSelection = true,
            };
            CreateMainMenu(menuOptions);
        }
        static void InitRepositories()
        {
            var cs = $"{ConnectionString}{System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "PDFSign.db")}";
            cs = cs.Contains("\\") ? cs.Replace("\\", "\\\\") : cs;
            _sqlConnectionFactory = new DbConnectionFactory(cs);
            _certificateDataRepository = new CertificateDataRepository(_sqlConnectionFactory);
        }
        static void Start(ApplicationParameters applicationParameters)
        {
            var pdfpath = applicationParameters.PdfPath;
            if (!File.Exists(pdfpath)) { throw new FileNotFoundException($"[PDFLocator] PDF Location couldn't be found. : { applicationParameters.PdfPath }"); }

            _logger?.Info("Start()", $"Target PDF Path : {pdfpath}");

            CertificateData certificateData = null;

            if (applicationParameters.Id > 0)
            {
                _logger?.Info("Start()", $"Certificate Id passed : {applicationParameters.Id}");
                certificateData = _certificateDataRepository.GetById(applicationParameters.Id);
            }

            else if (!string.IsNullOrEmpty(applicationParameters.CertificateName))
            {
                _logger?.Info("Start()", $"Certificate Name passed : {applicationParameters.CertificateName}");
                certificateData = _certificateDataRepository.GetByName(applicationParameters.CertificateName);
            }

            if (certificateData == null) { throw new RepositoryNotFoundException($"[CertificateDataRepository] Certificate info {applicationParameters.Id} {applicationParameters.CertificateName} not found.");  }

            _logger?.Info("Start()", $"Create PDF Engine.");
            var pdfSignerService = new PdfSignerService();

            Stream signedPdfStream = null;

            _logger?.Info("Start()", $"Loading Certificate Data : {certificateData.Path}");
            using (var certificateStream = new FileStream(certificateData.Path, FileMode.Open))
            {
                var certificate = new Certificate(certificateStream, certificateData.Password);

                _logger?.Info("Start()", $"Initialize Certificate Data : {certificateData.Path}");
                certificate.Init();

                _logger?.Info("Start()", $"Loading PDF [memory] : {pdfpath}");
                using (var pdfstream = new FileStream(pdfpath, FileMode.Open))
                {
                    _logger?.Info("Start()", $"Sign PDF [memory]");
                    signedPdfStream = pdfSignerService.SignPdf(pdfstream, certificate);
                }
            }

            signedPdfStream.Position = 0;

            if (applicationParameters.SaveOriginal)
            {
                _logger?.Info("Start()","Keep original file flagged.");
                var oldFilenamePath = Path.Combine(Path.GetDirectoryName(pdfpath) ?? string.Empty, (Path.GetFileNameWithoutExtension(pdfpath) + $"_original{Path.GetExtension(pdfpath)}"));

                _logger?.Info("Start()", $"Copy original file : {pdfpath} to : {oldFilenamePath}");
                File.Copy(pdfpath, oldFilenamePath);
            }

            _logger?.Info("Start()", $"Delete PDF File : {pdfpath}");

            File.Delete(pdfpath);

            _logger?.Info("Start()", $"Create signed PDF File : {pdfpath}");
            using (var fileStream = new FileStream(pdfpath, FileMode.OpenOrCreate))
            {
                signedPdfStream.CopyTo(fileStream);
            }

            signedPdfStream.Dispose();
        }
        static void Main(string[] args)
        {
            try
            {
                InitLogger();
                InitRepositories();
                Parser.Default.ParseArguments<ApplicationParameters>(args)
                       .WithParsed<ApplicationParameters>(o =>
                       {
                           var validParameters = o.Setup
                                || (o.Id > 0 ^ !(string.IsNullOrEmpty(o.CertificateName)) 
                                && !string.IsNullOrEmpty(o.PdfPath));

                           if (!validParameters) { throw new ArgumentException("[Parameters] >> Invalid parameters supplied."); }
                           if (o.Verbose)
                           {
                               _logger?.Info("Main()",$"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                           }
                           if (o.Setup)
                           {
                               _logger?.Info("Main()", $"Invoke setup.");
                               Setup();
                               return;
                           }

                           Start(o);
                       });
                _logger?.Info("Main()", $"Exit");
            }
            catch (Exception ex)
            {
                _logger?.Error("Main()",ex.Message);
            }
        }
    }
}
