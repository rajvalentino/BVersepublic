using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VviewUserControls.Abstraction;
using VviewUserControls.Views;

namespace VviewUserControls.ViewModels
{
    class LiveScreenViewModel : BindableBase
    {
        #region Variables
        private readonly IRegionManager _regionManager;
        public IUnityContainer _container;
        List<string> BookNames;
        IFetchFromXml ifetch;
        //public List<string> MyItems { get; set; }
        ICommand LostfBooknamecmd;
        List<string> Verselist = new List<string>();
        ICommand SearchforVerse;
        MainSearch ms;
        #endregion

        #region Properties
        private string _chapternameno;
        public string Chapternameno
        {
            get { return _chapternameno; }
            set
            {
                SetProperty(ref _chapternameno, value);
                OnPropertyChanged("Chapternameno");
            }         
        }

        
        private string _bgImagePath;
        public string BgImagePath
        {
            get { return _bgImagePath; }
            set
            {
                SetProperty(ref _bgImagePath, value);
                OnPropertyChanged("BgImagePath");
            }
        }

        private string _verse;
        public string Verse
        {
            get { return _verse; }
            set { SetProperty(ref _verse, value);
                if(!string.IsNullOrEmpty(value))
                {
                    //Chapters = ifetch.FetchChapterNos("kjv",value);
                }
            }
        }
       
        private double fontsize = 100;
        public double Fontsize
        {
            get { return fontsize; }
            set
            {
                SetProperty(ref fontsize, value);
               // ms = new MainSearch();
                //Fontsize = ms.fontsizeslide.Value;
                //if (!string.IsNullOrEmpty(value))
                //{
                //    //Chapters = ifetch.FetchChapterNos("kjv",value);
                //}
            }
        }

        private Brush fontcolor= Brushes.Red;
        public Brush FontColor
        {
            get { return fontcolor; }
            set
            {
                SetProperty(ref fontcolor, value);
             
                //if (!string.IsNullOrEmpty(value))
                //{
                //    //Chapters = ifetch.FetchChapterNos("kjv",value);
                //}
            }
        }
        #endregion

        #region Methods
        public LiveScreenViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            if (Properties.Settings.Default.BackgroundImage != null)
            {
                BgImagePath = Properties.Settings.Default.BackgroundImage;
            }
            else
            {
                BgImagePath = @"bg6b.jpg";
            }
            if (Properties.Settings.Default.FontColor != "" && Properties.Settings.Default.FontColor != "0")
            {
                Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                FontColor = brush;
            }
            if (Properties.Settings.Default.VerseFontSize>0)
            {
                Fontsize = Properties.Settings.Default.VerseFontSize;
            }

        }

        private void searchforverse()
        {            
        }
        #endregion
    }
}
