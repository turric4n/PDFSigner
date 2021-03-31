using System;
using System.IO;
using CommandLine;
using PDFSign.Models;
using PDFSign.Repositories;
using PDFSign.Database.Factories;
using CommandLineSelectableMenu;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Org.BouncyCastle.Asn1.X9;
using PDFSign.Exceptions;

namespace PDFSign
{
    class Program
    {
        static string connectionstring = "Data Source=";
        static DBConnectionFactory sqlconnectionfactory;
        static CertificateDataRepository certificatedatarepo;

        static void CreateMainMenu(SelectableMenuOptions options)
        {
            Console.Clear();
            var mainmenu = new SelectableMenu<Action>(options);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Main Menu");

            // add menu item.
            mainmenu.Add("List Certificates", () =>
            {
                CertificatesListMenu(options);
            });
            mainmenu.Add("Add new certificate settings", () =>
            {
                var certificate = new CertificateData();
                NewCertificate(certificate, options);
            });

            mainmenu.Add("Exit ", () => { System.Environment.Exit(1); });

            mainmenu.Draw().Invoke();
        }

        static void NewCertificate(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"New CertificateData");
            var certificatesoptions = new SelectableMenu<Action>(options);
            var props = cert.GetType().GetProperties();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(cert);

                value = name == "Password" ? "****" : value;

                certificatesoptions.Add($"{name} : {value}", () =>
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

            certificatesoptions.Add("Save", () =>
            {
                try
                {
                    certificatedatarepo.Add(cert);
                    CreateMainMenu(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Validation errors : { ex.Message }");
                    Console.ReadLine();
                    NewCertificate(cert, options);
                }                
            });

            certificatesoptions.Add("Cancel", () =>
            {
                CreateMainMenu(options);
            });

            certificatesoptions.Draw().Invoke();
        }
        static void CertificateDelete(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Are you sure to delete : {cert.Businessname} ?");

            var certificatesoptions = new SelectableMenu<Action>(options);
            certificatesoptions.Add("Yes", () =>
            {
                certificatedatarepo.Delete(cert);
            });
            certificatesoptions.Add("No", () =>
            {
                CertificateOptions(cert, options);
            });

            certificatesoptions.Draw().Invoke();
        }

        static void NewCertificateValue(CertificateData cert, string Name, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Editing value : {Name}");
            Console.WriteLine($"New value : ");
            var stin = Console.ReadLine();
            var prop = cert.GetType().GetProperties().Where(x => x.Name == Name).FirstOrDefault();

            prop.SetValue(cert, stin);
           
            NewCertificate(cert, options);
        }

        static void UpdateCertificateValue(CertificateData cert, string Name, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Editing value : {Name}");
            Console.WriteLine($"New value : ");
            var stin = Console.ReadLine();
            var certificatesoptions = new SelectableMenu<Action>(options);

            var prop = cert.GetType().GetProperties().Where(x => x.Name == Name).FirstOrDefault();

            prop.SetValue(cert, stin);
            try
            {
                var newcertificate = new CertificateData(cert.Id, cert.Password, cert.Path, cert.Businessname);

                certificatesoptions.Add("Save", () =>
                {
                    certificatedatarepo.Update(newcertificate);
                    CertificateUpdate(newcertificate, options);
                });

                certificatesoptions.Add("Cancel", () =>
                {
                    NewCertificate(newcertificate, options);
                });
                certificatesoptions.Draw().Invoke();
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
            Console.WriteLine($"Certificate edit : {cert.Businessname}");

            var certificatesoptions = new SelectableMenu<Action>(options);
            var props = cert.GetType().GetProperties();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(cert);

                value = name == "Password" ? "****" : value;

                certificatesoptions.Add($"{name} : {value}", () =>
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
            certificatesoptions.Add("Back", () =>
            {
                Console.Clear();
            });

            certificatesoptions.Draw().Invoke();
        }
        static void CertificateOptions(CertificateData cert, SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Certificate edit : {cert.Businessname}");

            var certificatesoptions = new SelectableMenu<Action>(options);
            certificatesoptions.Add("Update values", () =>
            {
                CertificateUpdate(cert, options);
            });
            certificatesoptions.Add("Delete", () =>
            {
                CertificateDelete(cert, options);
                CertificatesListMenu(options);
            });

            certificatesoptions.Add("Back", () =>
            {
                Console.Clear();
            });
            certificatesoptions.Draw().Invoke();
        }
        static void CertificatesListMenu(SelectableMenuOptions options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Certificates");

            var certificatesmenu = new SelectableMenu<Action>(options);

            var certificates = certificatedatarepo.GetAll();
            foreach (var cert in certificates)
            {
                certificatesmenu.Add(cert.Businessname, () =>
                {
                    CertificateOptions(cert, options);
                    CertificatesListMenu(options);
                });
            }

            certificatesmenu.Add("Back", () =>
            {
                Console.Clear();
                CreateMainMenu(options);
            });

            certificatesmenu.Draw().Invoke();
        }
        static void Setup()
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
            var cs = $"{connectionstring}{System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PDFSign.db")}";
            Console.WriteLine(cs);
            sqlconnectionfactory = new DBConnectionFactory(cs);
            certificatedatarepo = new CertificateDataRepository(sqlconnectionfactory);
        }
        static void Start(ApplicationParameters applicationParameters)
        {
            var pdfpath = applicationParameters.PdfPath;
            if (!File.Exists(pdfpath)) { throw new FileNotFoundException($"[PDFLocator] PDF Location couldn't be found. : { applicationParameters.PdfPath }"); }

            CertificateData certificatedata = null;

            if (applicationParameters.Id > 0)
            {
                certificatedata = certificatedatarepo.GetById(applicationParameters.Id);
            }

            else if (!string.IsNullOrEmpty(applicationParameters.CertificateName))
            {
                certificatedata = certificatedatarepo.GetByName(applicationParameters.CertificateName);
            }

            if (certificatedata == null) { throw new RepositoryNotFoundException($"[CertificateDataRepository] Certificate info {applicationParameters.Id} {applicationParameters.CertificateName} not found.");  }

            var pdfsignin = new PDFSignerService();

            Stream signedpdf = null;

            using (var certificateStream = new FileStream(certificatedata.Path, FileMode.Open))
            {
                var certificate = new Certificate(certificateStream, certificatedata.Password);

                certificate.Init();

                using (var pdfstream = new FileStream(pdfpath, FileMode.Open))
                {
                    signedpdf = pdfsignin.SignPDF(pdfstream, certificate);
                }
            }

            signedpdf.Position = 0;

            File.Delete(pdfpath);

            using (var newfile = new FileStream(pdfpath, FileMode.OpenOrCreate))
            {
                signedpdf.CopyTo(newfile);
            }

            signedpdf.Dispose();
        }
        static void Main(string[] args)
        {
            try
            {
                InitRepositories();
                Parser.Default.ParseArguments<ApplicationParameters>(args)
                       .WithParsed<ApplicationParameters>(o =>
                       {
                           var validparameters = o.Setup
                                || (o.Id > 0 ^ !(string.IsNullOrEmpty(o.CertificateName)) 
                                && !string.IsNullOrEmpty(o.PdfPath));

                           if (!validparameters) { throw new ArgumentException("[Parameters] >> Invalid parameters supplied."); }
                           if (o.Verbose)
                           {
                               Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                           }
                           if (o.Setup)
                           {
                               Setup();
                               return;
                           }

                           Start(o);
                       });
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
