using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using MahApps.Metro.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VocabularyTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ICollectionView view;

        public MainWindow()
        {
            InitializeComponent();
            Helper.MainWindow = this;
            Config.Load();

            Config.Instance.SaveDataInAppData = false;
            Config.Save();

            VocabularyDatabase.Load();


            foreach (string s in Config.Instance.categories)
            {
                AddVocabulary.comboCategory.Items.Add(s);
                Options.comboCategory.Items.Add(s);
                System.Windows.Controls.CheckBox cc = new System.Windows.Controls.CheckBox();
                cc.Checked += (o, e) => { view.Refresh(); };
                cc.Unchecked += (o, e) => { view.Refresh(); };
                cc.Content = s;
                menuKategorie.Items.Add(cc);
            }

            foreach (string s in Config.Instance.lections)
            {
                AddVocabulary.comboLection.Items.Add(s);
                Options.comboLection.Items.Add(s);
                System.Windows.Controls.CheckBox cc = new System.Windows.Controls.CheckBox();
                cc.Checked += (o, e) => { view.Refresh(); };
                cc.Unchecked += (o, e) => { view.Refresh(); };
                cc.Content = s;
                menuLektionen.Items.Add(cc);
            }

            ObservableCollection<Vocabulary> custdata = VocabularyDatabase.Instance.vocs;

            var itemSourceView = new CollectionViewSource() { Source = VocabularyDatabase.Instance.vocs };
            view = itemSourceView.View;

            view.Filter = new Predicate<object>( 
                            item => 
                                {
                                    Vocabulary voc = (Vocabulary)item;
                                    if (!this.searchText.Text.Equals(""))
                                    {
                                        
                                        if (voc.german.Contains(this.searchText.Text) || voc.japanese.Contains(this.searchText.Text) || voc.romaji.Contains(this.searchText.Text))
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }

                                    Predicate<object> stdPred = new Predicate<object>(x => { return true; });

                                    menuLektionen.Items.Filter = new Predicate<object>(x => { return ((System.Windows.Controls.CheckBox)x).IsChecked.Value; });
                                    menuKategorie.Items.Filter = new Predicate<object>(x => { return ((System.Windows.Controls.CheckBox)x).IsChecked.Value; });

                                    List<string> cLektionen = new List<string>();
                                    foreach (System.Windows.Controls.CheckBox c in menuLektionen.Items)
                                        cLektionen.Add(c.Content.ToString());
                                    List<string> cKategorien = new List<string>();
                                    foreach (System.Windows.Controls.CheckBox c in menuKategorie.Items)
                                        cKategorien.Add(c.Content.ToString());

                                    menuLektionen.Items.Filter = stdPred;
                                    menuKategorie.Items.Filter = stdPred;

                                    //Filter sind aktiv
                                    if ( (cLektionen.Count == 0 || cLektionen.Contains(voc.lection, null) ) && 
                                            (cKategorien.Count == 0 || cKategorien.Contains(voc.cat)) )
                                    {
                                        return true;
                                    }

                                    //Fällt in keine Kategorie oder Lektion
                                    return false;
                                }
            );
            dataGrid.ItemsSource = view;
        }

        void cc_Checked(object sender, RoutedEventArgs e)
        {
            view.Refresh();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            FlyoutOptions.IsOpen = true;
        }

        private void ButtonAddVoc_Click(object sender, RoutedEventArgs e)
        {
            //FlyoutAddVocabulary.Height = 0.2* this.ActualHeight;
            FlyoutAddVocabulary.IsOpen = true;
            //Logger.WriteLine(this.ActualHeight.ToString(), "ActualHeight");
            //Logger.WriteLine(this.FlyoutAddVocabulary.Height.ToString(), "Flyout Height");
            
        }

        internal class WindowAspectRatio
        {
            private double _ratio;

            private WindowAspectRatio(Window window)
            {
                _ratio = window.Width / window.Height;
                ((HwndSource)HwndSource.FromVisual(window)).AddHook(DragHook);
            }

            public static void Register(Window window)
            {
                new WindowAspectRatio(window);
            }

            internal enum WM
            {
                WINDOWPOSCHANGING = 0x0046,
            }

            [Flags()]
            public enum SWP
            {
                NoMove = 0x2,
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct WINDOWPOS
            {
                public IntPtr hwnd;
                public IntPtr hwndInsertAfter;
                public int x;
                public int y;
                public int cx;
                public int cy;
                public int flags;
            }

            private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handeled)
            {
                if ((WM)msg == WM.WINDOWPOSCHANGING)
                {
                    WINDOWPOS position = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                    if ((position.flags & (int)SWP.NoMove) != 0 ||
                        HwndSource.FromHwnd(hwnd).RootVisual == null) return IntPtr.Zero;

                    position.cx = (int)(position.cy * _ratio);

                    Marshal.StructureToPtr(position, lParam, true);
                    handeled = true;
                }

                return IntPtr.Zero;
            }
        }

        private void MetroWindow_SourceInitialized(object sender, EventArgs e)
        {
            //WindowAspectRatio.Register((Window)sender);
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Config.Save();
            VocabularyDatabase.Save();
        }

        private void contextRemove_Click(object sender, RoutedEventArgs e)
        {
            Vocabulary voc = (Vocabulary)dataGrid.SelectedItem;
            VocabularyDatabase.Instance.vocs.Remove(voc);
        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Refresh();
        }

        private void lectionCeckBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            view.Refresh();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<Vocabulary> vocList = new List<Vocabulary>();
            foreach (Vocabulary voc in view)
            {
                vocList.Add(voc);
            }

            // TODO: Permutate list
            Dictionary<string, string> dicVoc = new Dictionary<string, string>();

            // Build dictionary
            foreach (Vocabulary voc in vocList)
            {
                if (germanToRomaji.IsChecked.Value)
                {
                    dicVoc.Add(voc.german, voc.romaji);
                }
                else if (germanToJapanese.IsChecked.Value)
                {
                    dicVoc.Add(voc.german, voc.japanese);
                }
                else if (japaneseToGerman.IsChecked.Value)
                {
                    dicVoc.Add(voc.japanese, voc.german);
                }
                else if (romajiToGerman.IsChecked.Value)
                {
                    dicVoc.Add(voc.romaji, voc.german);
                }
            }
        }
    }
}
