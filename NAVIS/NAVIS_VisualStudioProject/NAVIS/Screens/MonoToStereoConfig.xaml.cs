using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NAVIS
{
    /// <summary>
    /// Lógica de interacción para monoToStereoConfig.xaml
    /// </summary>
    public partial class MonoToStereoConfig : Window
    {
        public int delayValue {get; set;}
        public string filePath { get; set; }

        public MonoToStereoConfig()
        {
            InitializeComponent();
        }

        private void cb_DelayBool_Checked(object sender, RoutedEventArgs e)
        {
            tb_DelayValue.Visibility = Visibility.Visible;
            tb_muSymbol.Visibility = Visibility.Visible;
        }

        private void cb_DelayBool_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_DelayValue.Visibility = Visibility.Collapsed;
            tb_muSymbol.Visibility = Visibility.Collapsed;
        }

        private void tb_DelayValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals("-") && tb_DelayValue.Text.Length.Equals(0))
            {
                return;
            }

            int i;
            if (!int.TryParse(e.Text, out i))
            {
                e.Handled = true;
            }
        }

        private void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                 e.Command == ApplicationCommands.Copy ||
                 e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Aedat file (.aedat)|*.aedat";

            sfd.Title = "Select path to save the aedat file";
            sfd.FileName = MainWindow.fileName.Split('.')[0] + "_stereo";

            if (sfd.ShowDialog() == true && sfd.FileName != "")
            {
                delayValue = Convert.ToInt32(tb_DelayValue.Text);
                filePath = sfd.FileName;
                this.DialogResult = true;
            }
        }
    }
}
