using Microsoft.Practices.Unity;
using Microsoft.Win32;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VviewUserControls.Abstraction;
using VviewUserControls.Utils;
using VviewUserControls.Views;
using WpfScreenHelper;

namespace VviewUserControls.ViewModels
{
    class MainSearchViewModel:BindableBase
    {
        #region Variables
        private readonly IRegionManager _regionManager;
        public IUnityContainer _container;
        List<string> BookNames;
        List<string> OtherLangBooks;
        IFetchFromXml ifetch;
        //public List<string> MyItems { get; set; }
       public ICommand LostfBooknamecmd;
        List<string> Verselist = new List<string>();
       public ICommand SearchforVerse { get;
            set; }
        public ICommand ShoworHideLiveScreen { get; set; }
        public ICommand BrowseBackgroundImage { get; set; }        
        public string current_language ="Tamil";
        PrevSearch pv;
        List<List<string>> myverselist;
        LiveScreen ls;
        Window livewindow;
        bool showlive=false;
        int index = 0;
        #endregion

        #region Properties
        public ICommand LangSelection { get; set; }

        private int _bookselected;
        public int Bookselected
        {
            get { return _bookselected; }
            set {
                SetProperty(ref _bookselected, value);              
                Chapters = ifetch.FetchChapterNos(current_language, _bookselected+1);
                Verseselected = null;
                showlive = false;
                Properties.Settings.Default.ShowLive = showlive;
                Properties.Settings.Default.CurBook = _bookselected;
                Properties.Settings.Default.Save();
                Chapterselected = null;
                Verseselected = null;
            }

        }

        private List<string> _books;
        public List<string> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        
        private int _sliderValueBinding=75;
        public int SliderValueBinding
        {
            get { return _sliderValueBinding; }
            set
            {
                SetProperty(ref _sliderValueBinding, value);
                //if (current_language != "English")
                //{
                //    ls.chapterlabel.Text = OtherLangBooks[_bookselected] + " " + _chapterselected;
                //}
                //else
                //{
                //    ls.chapterlabel.Text = Books[_bookselected] + " " + _chapterselected;
                //}

                // ls.UpdateLayout();
                Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                ls.chapterlabel.Foreground = brush;
                ls.verseContent.Foreground = brush;
                if (Verselist.Count > 0)
                {
                    ls.verseContent.Text = Properties.Settings.Default.CurVerse;

                }
                if (current_language != "English")
                {
                    if ( Properties.Settings.Default.CurChapter != "")
                    {
                        ls.chapterlabel.Text = OtherLangBooks[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;

                    }
                                    }
                else
                {
                    ls.chapterlabel.Text = Books[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;
                }
                //ls.verseContent.Text = Verselist[index].ToString();               
                ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                Window wnd = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("LiveWindow")).FirstOrDefault();
                Window wnd1 = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("shellwindow")).FirstOrDefault();
                Properties.Settings.Default.VerseFontSize = Convert.ToDouble(SliderValueBinding);
                Properties.Settings.Default.Save();
                ls.UpdateLayout();
                wnd.Content = ls;
                wnd.UpdateLayout();
            }

        }
        
        private string _chapterselected;
        public string Chapterselected
        {
            get { return _chapterselected; }
            set
            {
                SetProperty(ref _chapterselected, value);
                Verseselected = null;
                if (!string.IsNullOrEmpty(value))
                {
                    showlive = false;
                    Properties.Settings.Default.ShowLive = showlive;
                    myverselist = ifetch.FetchVerseNos(current_language, _bookselected+1, value);
                   Verses= myverselist[0];
                    Verselist = myverselist[1];
                    pv.vlist.ItemsSource = Verselist;
                    if (current_language != "English")
                    {
                        pv.prev_chaptername.Text = OtherLangBooks[_bookselected] + " " + _chapterselected;
                    }
                    else
                    {
                        pv.prev_chaptername.Text = Books[_bookselected] + " " + _chapterselected;
                    }                 
                    pv.UpdateLayout();                    
                    if (_regionManager.Regions["PreviewRegion"].Views.Count() != 0)
                    {
                        var Preview = _regionManager.Regions["PreviewRegion"].Views.ElementAt(0);
                        _regionManager.Regions["PreviewRegion"].Remove(Preview);
                    }
                    Properties.Settings.Default.CurChapter= _chapterselected;
                    Properties.Settings.Default.Save();
                    _regionManager.Regions["PreviewRegion"].Add(pv);
                }
            }
        }

        private string _verseselected;
        public string Verseselected
        {
            get { return _verseselected; }
            set
            {
                SetProperty(ref _verseselected, value);
                if (!string.IsNullOrEmpty(value))
                {
                    pv.vlist.ItemsSource = Verselist;
                     index = Convert.ToInt32(Verseselected) - 1;
                    int itemCount = pv.vlist.Items.Count;
                    if (itemCount == 0)
                        return;
                    if (index >= itemCount)
                        index = itemCount - 1;
                    double listHeight = pv.vlist.Height;
                    double yPos = (listHeight / itemCount) * index;
                    if (current_language != "English")
                    {
                        pv.prev_chaptername.Text = OtherLangBooks[_bookselected] + " " + _chapterselected;
                    }
                    else
                    {
                        pv.prev_chaptername.Text = Books[_bookselected] + " " + _chapterselected;
                    }
                    pv.vlist.SelectedIndex = index;
                    pv.vlist.ScrollIntoView(pv.vlist.SelectedItem);
                    pv.UpdateLayout();
                    if (_regionManager.Regions["PreviewRegion"].Views.Count() != 0)
                    {
                        var Preview = _regionManager.Regions["PreviewRegion"].Views.ElementAt(0);
                        _regionManager.Regions["PreviewRegion"].Remove(Preview);
                    }
                    _regionManager.Regions["PreviewRegion"].Add(pv);
                    if (showlive)
                    {
                        if (!WindowExt.IsWindowOpen<Window>("LiveWindow") && ls != null)
                        {
                            livewindow = new Window();
                            ls = new LiveScreen();
                            livewindow.Name = "LiveWindow";
                            livewindow.Title = "Live";
                            livewindow.Content = ls;
                            WindowExt.MaximizeToSecondaryMonitor(livewindow);
                            livewindow.WindowStyle = WindowStyle.None;
                            livewindow.Show();
                        }
                        ls.verseContent.Text = Verselist[index].ToString();
                        ls.chapterlabel.Text = _bookselected + " " + _chapterselected;
                        ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                        Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                        ls.verseContent.Foreground = brush;
                        ls.chapterlabel.Foreground = brush;
                        ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                        Properties.Settings.Default.VerseFontSize = Convert.ToDouble(SliderValueBinding);
                        Properties.Settings.Default.CurVerse = Verselist[index].ToString();
                        Properties.Settings.Default.Save();
                        ls.UpdateLayout();
                    }
                    showlive = true;
                    Properties.Settings.Default.ShowLive = showlive;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private Color? _fontColor;
        public Color? FontColor
        {
            get { return _fontColor; }
            set
            {
                // BREAKPOINT
                _fontColor = value;
                if (_fontColor.HasValue)
                  //  Application.Current.Resources[Name] = new SolidColorBrush(_brushColor.Value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FontColor"));
                //First update the properties and set color
                Properties.Settings.Default.FontColor = _fontColor.Value.ToString();
                Properties.Settings.Default.Save();
                ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                ls.Foreground = brush;
                ls.verseContent.Foreground = brush;
                ls.chapterlabel.Foreground = brush;
                ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                if (Verselist.Count > 0)
                {
                    ls.verseContent.Text = Properties.Settings.Default.CurVerse;

                }
                if (current_language != "English")
                {
                    if (Properties.Settings.Default.CurBook != 0 && Properties.Settings.Default.CurChapter != "")
                    {
                        ls.chapterlabel.Text = OtherLangBooks[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;

                    }
                               }
                else
                {
                    ls.chapterlabel.Text = Books[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;
                }
                //ls.chapterlabel.Text = _bookselected + " " + _chapterselected;
                ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);

                ls.UpdateLayout();
                Window wnd = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("LiveWindow")).FirstOrDefault();

                wnd.Content = ls;
                wnd.UpdateLayout();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> _chapters;
        public List<string> Chapters
        {
            get{ return _chapters; }
            set { SetProperty(ref _chapters, value); }
        }

        private List<string> _verses;
        public List<string> Verses
        {
            get { return _verses; }
            set { SetProperty(ref _verses, value); }
        }
        #endregion

        #region Methods
        public  MainSearchViewModel(RegionManager regionManager, IFetchFromXml ifetch_)
        {
            _regionManager = regionManager;            
            ifetch = ifetch_;
            OtherLangBooks = ifetch.FetchOtherLangBooknames(current_language);
            Books = ifetch.FetchBooknames(current_language);
            Bookselected = 0;
            SearchforVerse = new DelegateCommand(searchforverse);
            ShoworHideLiveScreen = new DelegateCommand(showorhideLive);
            BrowseBackgroundImage = new DelegateCommand(BrowseImage);
            livewindow = new Window();
            ls = new LiveScreen();
            pv = new PrevSearch();
            livewindow.Name = "LiveWindow";
            livewindow.Title = "Live";
            livewindow.Content = ls;
            ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);            
            Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
            ls.chapterlabel.Foreground = brush;
            ls.verseContent.Foreground = brush;
            ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative)); 
            LangSelection = new DelegateCommand<object>(languagechanged);
            WindowExt.MaximizeToSecondaryMonitor(livewindow);
            ls.UpdateLayout();
            livewindow.Content=ls;
            livewindow.WindowStyle = WindowStyle.None;
            livewindow.Hide();

            FontColor =  (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor);
            ClearTempVerse();
        }
        private void ClearTempVerse()
        {
            Properties.Settings.Default.CurBook = 0;
            Properties.Settings.Default.CurChapter = "";
            Properties.Settings.Default.CurLang = "";
            Properties.Settings.Default.CurVerse = "";
            Properties.Settings.Default.Save();
        }

        private void BrowseImage()
        {
            // throw new NotImplementedException();
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "jpg",
                Filter = "Image files (*.jpeg)|*.jpg;*.png",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                BitmapImage image=null;
                if (openFileDialog1.FileName != "")
                     image = new BitmapImage(new Uri(openFileDialog1.FileName, UriKind.Relative));
                Properties.Settings.Default.BackgroundImage = openFileDialog1.FileName;
                Properties.Settings.Default.Save();

                Window wnd = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("LiveWindow")).FirstOrDefault();

                ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                ls.chapterlabel.Foreground = brush;
                ls.verseContent.Foreground = brush;
                ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));

                if (Verselist.Count > 0)
                {
                    ls.verseContent.Text = Properties.Settings.Default.CurVerse;

                }
                if (current_language != "English")
                {
                    if (Properties.Settings.Default.CurBook != 0 && Properties.Settings.Default.CurChapter != "")
                    {
                        ls.chapterlabel.Text = OtherLangBooks[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;

                    }
                }
                else
                {
                    ls.chapterlabel.Text = Books[Properties.Settings.Default.CurBook] + " " + Properties.Settings.Default.CurChapter;
                }
                //ls.chapterlabel.Text = _bookselected + " " + _chapterselected;
                //ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                ls.UpdateLayout();
                wnd.Content = ls;
                wnd.UpdateLayout();



            }
        }
        //private void showorhideLive(object obj)
        //{
        //    //throw new NotImplementedException();
        //}

        private void showorhideLive()
        {
            //throw new NotImplementedException();
            if (WindowExt.IsWindowOpen<Window>("LiveWindow"))
            {               
                if (!livewindow.IsVisible)
                {
                    livewindow.Show();
                }
                else
                {
                    livewindow.Hide();
                }
            }
        }

        private bool canexecutemethod(object arg)
        {
            if (arg != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void languagechanged(object obj)
        {
            current_language = (string)obj;
            OtherLangBooks = ifetch.FetchOtherLangBooknames(current_language);          
        }

        private void searchforverse()
        {
            //if (!string.IsNullOrEmpty(value))
            //{
            myverselist = ifetch.FetchVerseNos(current_language, _bookselected + 1, _chapterselected);
            Verses = myverselist[0];
            Verselist = myverselist[1];
            pv.vlist.ItemsSource = Verselist;
                index = Convert.ToInt32(Verseselected) - 1;
                int itemCount = pv.vlist.Items.Count;
                if (itemCount == 0)
                    return;
                if (index >= itemCount)
                    index = itemCount - 1;
                double listHeight = pv.vlist.Height;
                double yPos = (listHeight / itemCount) * index;
                if (current_language != "English")
                {
                    pv.prev_chaptername.Text = OtherLangBooks[_bookselected] + " " + _chapterselected;
                }
                else
                {
                    pv.prev_chaptername.Text = Books[_bookselected] + " " + _chapterselected;
                }
                pv.vlist.SelectedIndex = index;
                pv.vlist.ScrollIntoView(pv.vlist.SelectedItem);
                pv.UpdateLayout();
                if (_regionManager.Regions["PreviewRegion"].Views.Count() != 0)
                {
                    var Preview = _regionManager.Regions["PreviewRegion"].Views.ElementAt(0);
                    _regionManager.Regions["PreviewRegion"].Remove(Preview);
                }
                _regionManager.Regions["PreviewRegion"].Add(pv);
                if (showlive)
                {
                    if (!WindowExt.IsWindowOpen<Window>("LiveWindow") && ls != null)
                    {
                        livewindow = new Window();
                        ls = new LiveScreen();
                        livewindow.Name = "LiveWindow";
                        livewindow.Title = "Live";
                        livewindow.Content = ls;
                        WindowExt.MaximizeToSecondaryMonitor(livewindow);
                        livewindow.WindowStyle = WindowStyle.None;
                        livewindow.Show();
                    }
                    ls.verseContent.Text = Verselist[index].ToString();
                    ls.chapterlabel.Text = _bookselected + " " + _chapterselected;
                    ls.verseContent.FontSize = Convert.ToDouble(SliderValueBinding);
                    Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.FontColor));
                    ls.verseContent.Foreground = brush;
                    ls.chapterlabel.Foreground = brush;
                    ls.livebackgroundimage.Source = new BitmapImage(new Uri(Properties.Settings.Default.BackgroundImage, UriKind.Relative));
                    Properties.Settings.Default.VerseFontSize = Convert.ToDouble(SliderValueBinding);
                    Properties.Settings.Default.CurVerse = Verselist[index].ToString();
                    Properties.Settings.Default.Save();
                    ls.UpdateLayout();
                }
                showlive = true;
            //}
        }
        #endregion
    }
}
