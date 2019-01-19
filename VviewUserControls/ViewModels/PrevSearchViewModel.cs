using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VviewUserControls.Abstraction;
using VviewUserControls.Utils;
using VviewUserControls.Views;

namespace VviewUserControls.ViewModels
{
    class PrevSearchViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Variables
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IRegionManager _regionManager;
        public IUnityContainer _container;
        IFetchFromXml ifetch;
        LiveScreen ls;
        PrevSearch ps;
        Window livewindow;
        public bool showlive = false;
        BitmapImage image = null;
        #endregion

        #region Properties
        private string _chapName;
        public string ChapName
        {
            get { return _chapName; }
            set
            {
                SetProperty(ref _chapName, value);
                OnPropertyChanged("ChapName");
            }
        }

        private string _selectedVerse;
        public string SelectedVerse
        {
            get { return _selectedVerse; }
            set
            {
                SetProperty(ref _selectedVerse, value);
                OnPropertyChanged("SelectedVerse");
                ls = new LiveScreen();
        
                if (!string.IsNullOrEmpty(value))
                {
                    if (Properties.Settings.Default.ShowLive)
                    {
                        if (!WindowExt.IsWindowOpen<Window>("LiveWindow") && ls != null)
                        {
                            livewindow = new Window();
                            livewindow.Name = "LiveWindow";
                            livewindow.Title = "Live";
                            livewindow.Content = ls;
                            WindowExt.MaximizeToSecondaryMonitor(livewindow);
                            livewindow.WindowStyle = WindowStyle.None;
                            livewindow.Show();
                        }
                        Window wnd = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("LiveWindow")).FirstOrDefault();
                        Window wnd1 = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("shellwindow")).FirstOrDefault();
                        ls.verseContent.Text = _selectedVerse;
                        ls.chapterlabel.Text = _chapName;
                        Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                        ls.chapterlabel.Foreground = brush;
                        ls.verseContent.Foreground = brush;
                        ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                        ls.verseContent.FontSize = Properties.Settings.Default.VerseFontSize;
                        //image = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                        //ls.livebackgroundimage.Source = image;
                        ls.UpdateLayout();
                        wnd.Content = ls;
                        wnd.UpdateLayout();
                      
                    }
                    Properties.Settings.Default.CurVerse = _selectedVerse;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private List<string> _versesList;
        public List<string> VersesList
        {
            get { return _versesList; }
            set
            {
                SetProperty(ref _versesList, value);
                OnPropertyChanged("VesesList");
            }
        }
        #endregion

        #region Methods
        public PrevSearchViewModel(RegionManager regionManager, IFetchFromXml ifetch_)
        {
            _regionManager = regionManager;
            ifetch = ifetch_;            
        }

        public void OpenLiveScreen()
        {

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

