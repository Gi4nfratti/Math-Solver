using Math_Solver.DAO;
using Math_Solver.Models;
using Math_Solver.Resources;
using Math_Solver.Services;
using Math_Solver.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Montserrat.ttf", Alias = "Montserrat")]
[assembly: ExportFont("MontserratSemiBold.ttf", Alias = "MontserratSemiBold")]
[assembly: ExportFont("Helvetica.ttf", Alias = "Helvetica")]
[assembly: ExportFont("EBGaramond.ttf", Alias = "Garamond")]
[assembly: ExportFont("Roboto.ttf", Alias = "Roboto")]
[assembly: ExportFont("RobotoBold.ttf", Alias = "RobotoBold")]
[assembly: ExportFont("RobotoMedium.ttf", Alias = "RobotoMedium")]
[assembly: ExportFont("SFUIDisplay-Regular.otf", Alias = "SFUIDisplay-Regular")]

namespace Math_Solver
{
    public partial class App : Application
    {
        public static List<Formula> mathList = new List<Formula>();
        public static List<Base64Images> base64Images = new List<Base64Images>();
        readonly DatabaseAccess access = new DatabaseAccess();

        public App()
        {
            InitializeComponent();
            
            DependencyService.Register<MockDataStore>();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(
                System.Globalization.CultureInfo.InstalledUICulture.Name
            );

            if (VersionTracking.IsFirstLaunchEver)
                MainPage = new OnboardingPage();
            else
                MainPage = new AppShell();

            StartTask();
        }

        protected override void OnStart()
        {
            VersionTracking.Track();
        }

        public void StartTask()
        {
            if (mathList.Count == 0)
                GetXMLContents();

            if (!access.TableFavoritesExists())
            {
                if (access.CreateFavoriteDatabase())
                {
                    List<int> favoriteList = mathList.Where(f => f.Tag == "Calc").Select(g => g.Id).ToList();
                    foreach (int id in favoriteList)
                    {
                        access.InsertFavorites(id, 0);
                    }
                }
            }

            if (!access.TableRecentsExists())
            {
                if (access.CreateRecentDatabase())
                {
                    List<int> recentList = mathList.Where(f => f.Tag == "Calc").Select(g => g.Id).ToList();
                    foreach (int id in recentList)
                    {
                        access.InsertRecents(id, 0);
                    }
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void GetXMLContents()
        {
            Stream stream = GetCulture();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(stream);

            var result = xDoc.GetElementsByTagName("Formula");

            foreach (XmlNode item in result)
            {
                mathList.Add(new Formula()
                {
                    Id = int.Parse(item.Attributes["id"]?.InnerText),
                    IdName = item.Attributes["idName"]?.InnerText,
                    Name = item.Attributes["name"]?.InnerText,
                    Desc = item.Attributes["desc"]?.InnerText,
                    Area = item.Attributes["area"]?.InnerText,
                    Hashtag = item.Attributes["hashtag"]?.InnerText,
                    Tag = item.Attributes["tag"]?.InnerText,
                    UrlImage = item.Attributes["urlImage"]?.InnerText,
                    UrlExample = item.Attributes["urlExample"]?.InnerText,
                    Data = item.Attributes["data"]?.InnerText,
                    Lines = item.Attributes["lines"]?.InnerText,
                });
            }

            var result2 = xDoc.GetElementsByTagName("Category");

            foreach (XmlNode item in result2)
            {
                mathList.Add(new Formula()
                {
                    Id = int.Parse(item.Attributes["id"]?.InnerText),
                    IdName = item.Attributes["idName"]?.InnerText,
                    Name = item.Attributes["name"]?.InnerText,
                    Desc = item.Attributes["desc"]?.InnerText,
                    Area = item.Attributes["area"]?.InnerText,
                    Hashtag = item.Attributes["hashtag"]?.InnerText,
                    Tag = item.Attributes["tag"]?.InnerText,
                    UrlImage = item.Attributes["urlImage"]?.InnerText,
                    UrlExample = item.Attributes["urlExample"]?.InnerText,
                    Data = item.Attributes["data"]?.InnerText,
                    Lines = item.Attributes["lines"]?.InnerText,
                });
            }

            Stream streamImg = GetCulture(true);
            XmlDocument xDocImg = new XmlDocument();
            xDocImg.Load(streamImg);

            var result3 = xDocImg.GetElementsByTagName("appImage");

            foreach (XmlNode item in result3)
            {
                base64Images.Add(new Base64Images()
                {
                    Name = item.Attributes["name"]?.InnerText,
                    Img = item.Attributes["img"]?.InnerText,
                    ImgExample = item.Attributes["imgExample"]?.InnerText
                });
            }

            var result4 = xDocImg.GetElementsByTagName("Base64Image");

            foreach (XmlNode item in result4)
            {
                base64Images.Add(new Base64Images()
                {
                    Name = item.Attributes["name"]?.InnerText,
                    Img = item.Attributes["img"]?.InnerText,
                    ImgExample = item.Attributes["imgExample"]?.InnerText
                });
            }
        }

        public Stream GetCulture(bool isImages = false)
        {
            var assembly = typeof(InitPage).GetTypeInfo().Assembly;

            if(isImages)
                return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML_base.xml");

            string nameCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            switch (nameCulture)
            {
                case "pt":
                    return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML_pt.xml");
                case "es":
                    return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML_es.xml");
                case "fr":
                    return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML_fr.xml");
                case "de":
                    return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML_de.xml");
                default:
                    return assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML.xml");
            }            
        }

        public static void GoToAbout(ToolbarItem lblAbout)
        {
            lblAbout.Clicked += (s, e) => {
                Shell.Current.GoToAsync(nameof(AboutPage));
            };
        }
    }
}
