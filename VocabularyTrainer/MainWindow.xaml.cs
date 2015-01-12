using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace VocabularyTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Helper.MainWindow = this;
            Config.Load();
            VocabularyDatabase.Load();
            

            Config.Instance.SaveDataInAppData = false;
            Config.Save();

            foreach (string s in Config.Instance.categories)
            {
                AddVocabulary.comboCategory.Items.Add(s);
                Options.comboCategory.Items.Add(s);
            }

            foreach (string s in Config.Instance.lections)
            {
                AddVocabulary.comboLection.Items.Add(s);
                Options.comboLection.Items.Add(s);
            }

            ObservableCollection<Vocabulary> custdata = VocabularyDatabase.Instance.vocs;

            dataGrid.DataContext = custdata;
            foreach(Vocabulary voc in VocabularyDatabase.Instance.vocs)
            {
                Logger.WriteLine(voc.german, "Load Test");
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            FlyoutOptions.IsOpen = true;
        }

        private void ButtonAddVoc_Click(object sender, RoutedEventArgs e)
        {
            FlyoutAddVocabulary.IsOpen = true;
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
    }
}
