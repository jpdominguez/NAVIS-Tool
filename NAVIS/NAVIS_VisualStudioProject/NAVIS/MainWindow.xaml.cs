/////////////////////////////////////////////////////////////////////////////////
//                                                                             //
//    Copyright © 2015  Juan P. Dominguez-Morales                              //
//                                                                             //        
//    This file is part of Neuromorphic Auditory Visualizer Tool (NAVIS Tool). //
//                                                                             //
//    NAVIS Tool is free software: you can redistribute it and/or modify       //
//    it under the terms of the GNU General Public License as published by     //
//    the Free Software Foundation, either version 3 of the License, or        //
//    (at your option) any later version.                                      //
//                                                                             //
//    NAVIS Tool is distributed in the hope that it will be useful,            //
//    but WITHOUT ANY WARRANTY; without even the implied warranty of           //
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the              //
//    GNU General Public License for more details.                             //
//                                                                             //
//    You should have received a copy of the GNU General Public License        //
//    along with NAVIS Tool.  If not, see <http://www.gnu.org/licenses/>.      //
//                                                                             //
/////////////////////////////////////////////////////////////////////////////////


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

using Microsoft.Win32;
using System.IO;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms.DataVisualization.Charting;
using NAVIS.Settings;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Threading;
using System.Globalization;
using System.Reflection;

namespace NAVIS
{
    /// <summary>
    /// Main application window
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String root, fileName;
        public bool isLoaded = false;
        Microsoft.Win32.OpenFileDialog selectFileDialog;

        public static cAedat32 aedatObject32;
        public static cAedat16 aedatObject16;

        public static cSettings settings;

        public static List<Button> buttonList;
        public static List<bool> buttonListCollapsed;

        public static EnumCochleaInfo cochleaInfo;
        
        private bool isOpen_ManualAedatSplitter = false;

        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            settings = new cSettings();

            // List of buttons which will be disabled untill the aedat file is loaded
            buttonList = new List<Button> { Btn_LoadAedat, Btn_Settings, Btn_About, Btn_PDF, Btn_generateCSV, Btn_histogram, Btn_sonogram, Btn_average, Btn_difference, Btn_AedatSplitterManual, Btn_AedatSplitterAuto };
            updateButtonList();
            menuItem_Reload.IsEnabled = false; menuItem_Report.IsEnabled = false; menuItem_Tools.IsEnabled = false;
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (i >= 3)
                {
                    buttonList[i].IsEnabled = false;
                    buttonList[i].Opacity = 0.6;
                }
                if (buttonListCollapsed[i] == false)
                {
                    buttonList[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    buttonList[i].Visibility = Visibility.Visible;
                }
            }

            text_ManualAedatSplitterInit.Text = "0";
            text_ManualAedatSplitterEnd.Text = "0";

            System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = String.Format(lblVersion.Text, version.Major, version.Minor, version.Build, version.Revision);

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            System.GC.Collect();
        }

        public static void updateButtonList()
        {
            buttonListCollapsed = new List<bool> { settings.ToolbarS.showLoadAedat, settings.ToolbarS.showSettings, settings.ToolbarS.showAbout, settings.ToolbarS.showGeneratePDF, settings.ToolbarS.showGenerateCSV, settings.ToolbarS.showHistogram, settings.ToolbarS.showSonogram, settings.ToolbarS.showAverage, settings.ToolbarS.showDiff, settings.ToolbarS.showManualAedatSplitter, settings.ToolbarS.showAutomaticAedatSplitter };
        }

        /// <summary>
        /// If the option is enabled, clears the cochleogram chart and displays the last aedat file that the user loaded.
        /// </summary>
        public void displayCharts()
        {
            if (settings.MainS.showChart == false)
            {
                chart_Cochleogram.Visible = false;
            }
            else
            {
                clearCharts();
                displayCochleogram();
                chart_Cochleogram.Visible = true;
            }
        }

        /// <summary>
        /// Clear the content of the chart in the application's main window
        /// </summary>
        public void clearCharts()
        {
            chart_Cochleogram.Series.SuspendUpdates();
            for (int i = 0; i < chart_Cochleogram.Series.Count; i++)
            {
                while (chart_Cochleogram.Series[i].Points.Count > 0)
                {
                    chart_Cochleogram.Series[i].Points.RemoveAt(chart_Cochleogram.Series[i].Points.Count - 1);
                }
            }
            chart_Cochleogram.Series.ResumeUpdates();
        }

        /// <summary>
        /// Display the cochleogram of the loaded aedat file
        /// </summary>
        public void displayCochleogram()
        {
            int addresses = 256;
            switch (MainWindow.cochleaInfo)
            {
                case EnumCochleaInfo.MONO32: addresses = 64; break;
                case EnumCochleaInfo.MONO64: addresses = 128; break;
                case EnumCochleaInfo.STEREO32: addresses = 128; break;
                case EnumCochleaInfo.STEREO64: addresses = 256; break;
                default: addresses = 256; break;
            }

            #region chart settings
            chart_Cochleogram.ChartAreas["ChartArea"].AxisX.Minimum = 0;
            chart_Cochleogram.ChartAreas["ChartArea"].AxisY.Minimum = 0;
            chart_Cochleogram.ChartAreas["ChartArea"].AxisY.Maximum = addresses;
            chart_Cochleogram.ChartAreas["ChartArea"].AxisY.Interval = addresses / 8;

            chart_Cochleogram.Series["Left"].MarkerSize = 2;
            chart_Cochleogram.Series["Right"].MarkerSize = 2;

            chart_Cochleogram.ChartAreas["ChartArea"].Axes[0].Title = "Timestamp (us)";
            chart_Cochleogram.ChartAreas["ChartArea"].Axes[1].Title = "Address";

            chart_Cochleogram.ChartAreas["ChartArea"].Axes[0].TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11);
            chart_Cochleogram.ChartAreas["ChartArea"].Axes[1].TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11);
            #endregion

            int cont = 0;

            chart_Cochleogram.Series.SuspendUpdates(); //Don't update the chart untill every dot is added to the chart

            if (settings.MainS.eventSize == 16)
            {
                foreach (cAedatRow16 fila in aedatObject16.getValues())
                {
                    if (cont == 0)
                    {
                        if (settings.MainS.mono_stereo == EnumAudio.STEREO)  // If the aedat corresponds to a stereo sound
                        {
                            if (fila.evt < addresses / 2 - 1 && fila.evt < addresses / 2 - 2)
                            {
                                chart_Cochleogram.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                            else if (fila.evt >= addresses / 2 && fila.evt < addresses - 2)
                            {
                                chart_Cochleogram.Series["Right"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                        }
                        else if (settings.MainS.mono_stereo == EnumAudio.MONO) // If the aedat file corresponds to a moono sound
                        {
                            if (fila.evt < addresses - 2)
                            {
                                chart_Cochleogram.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                        }
                    }
                    cont++;
                    if (cont == settings.MainS.dotsToPaint)
                    {
                        cont = 0;
                    }
                }
            }
            else if (settings.MainS.eventSize == 32)
            {

                foreach (cAedatRow32 fila in aedatObject32.getValues())
                {
                    if (cont == 0)
                    {
                        if (settings.MainS.mono_stereo == EnumAudio.STEREO) // If the aedat corresponds to a stereo sound
                        {
                            if (fila.evt < addresses / 2 - 1 && fila.evt < addresses / 2 - 2)
                            {
                                chart_Cochleogram.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                            else if (fila.evt >= addresses / 2 && fila.evt < addresses - 2)
                            {
                                chart_Cochleogram.Series["Right"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                        }
                        else if (settings.MainS.mono_stereo == EnumAudio.MONO) // If the aedat file corresponds to a moono sound
                        {
                            if (fila.evt < addresses - 2)
                            {
                                chart_Cochleogram.Series["Left"].Points.AddXY(fila.timestamp, (double)fila.evt);
                            }
                        }
                    }
                    cont++;
                    if (cont == settings.MainS.dotsToPaint)
                    {
                        cont = 0;
                    }
                }
            }
            chart_Cochleogram.Series.ResumeUpdates();
            chart_Cochleogram.Series["Left"].MarkerSize = settings.MainS.dotSize;
            chart_Cochleogram.Series["Right"].MarkerSize = settings.MainS.dotSize;

            Color[] colores = colorFromSettings();
            chart_Cochleogram.Series["Left"].Color = System.Drawing.Color.FromArgb(colores[0].A, colores[0].R, colores[0].G, colores[0].B);
            chart_Cochleogram.Series["Right"].Color = System.Drawing.Color.FromArgb(colores[1].A, colores[1].R, colores[1].G, colores[1].B);

            System.GC.Collect();
        }

        /// <summary>
        /// Return an array with two values representing the left and the right color based on the settings
        /// </summary>
        public static Color[] colorFromSettings()
        {
            Color[] colors = new Color[2];
            switch (settings.MainS.leftColor)
            {
                case EnumColor.BLACK: colors[0] = Colors.Black; break;
                case EnumColor.BLUE: colors[0] = Colors.DodgerBlue; break;
                case EnumColor.BROWN: colors[0] = Colors.Brown; break;
                case EnumColor.GREEN: colors[0] = Colors.Green; break;
                case EnumColor.MAGENTA: colors[0] = Colors.Magenta; break;
                case EnumColor.ORANGE: colors[0] = Colors.Orange; break;
                case EnumColor.RED: colors[0] = Colors.Red; break;
                case EnumColor.YELLOW: colors[0] = Colors.Yellow; break;
            }
            switch (settings.MainS.rightColor)
            {
                case EnumColor.BLACK: colors[1] = Colors.Black; break;
                case EnumColor.BLUE: colors[1] = Colors.DodgerBlue; break;
                case EnumColor.BROWN: colors[1] = Colors.Brown; break;
                case EnumColor.GREEN: colors[1] = Colors.Green; break;
                case EnumColor.MAGENTA: colors[1] = Colors.Magenta; break;
                case EnumColor.ORANGE: colors[1] = Colors.Orange; break;
                case EnumColor.RED: colors[1] = Colors.Red; break;
                case EnumColor.YELLOW: colors[1] = Colors.Yellow; break;
            }
            return colors;
        }

        /// <summary>
        /// Load aedat click event
        /// </summary>
        private void loadAedatToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selectFileDialog = new Microsoft.Win32.OpenFileDialog();
            selectFileDialog.Title = "Select .aedat file";
            selectFileDialog.Filter = "aedat files|*.aedat";
            loadFile();
        }
        /// <summary>
        /// Load CSV click event
        /// </summary>
        private void loadCSVToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selectFileDialog = new Microsoft.Win32.OpenFileDialog();
            selectFileDialog.Title = "Select .CSV file";
            selectFileDialog.Filter = "csv files|*.csv";
            loadFile();
        }

        /// <summary>
        /// Load the aedat or CSV file selected by the user
        /// </summary>
        public void loadFile()
        {
            if (selectFileDialog.ShowDialog() == true)
            {
                root = selectFileDialog.FileName;
                this.Cursor = Cursors.Wait;
                fileName = root.Split('\\')[root.Split('\\').Length - 1];
                TB_aedatName.Text = fileName;

                tab_fileLoaded.Visibility = Visibility.Visible;

                if (settings.MainS.eventSize == 16)
                {
                    aedatObject16 = new cAedat16(root);
                    aedatObject16.adaptAedat();
                }
                else if (settings.MainS.eventSize == 32)
                {
                    aedatObject32 = new cAedat32(root);
                    aedatObject32.adaptAedat();
                }

                isLoaded = true;

                initState();
                displayCharts();

                for (int i = 2; i < buttonList.Count; i++)  //Enable tools buttons after loading the file
                {
                    buttonList[i].IsEnabled = true;
                    buttonList[i].Opacity = 1;
                }
                if (cochleaInfo == EnumCochleaInfo.MONO32 || cochleaInfo == EnumCochleaInfo.MONO64) // If the file corresponds to a mono sound, there's no point on enabling the "Disparity between cochleae" function.
                {
                    Btn_difference.IsEnabled = false;
                    Btn_difference.Opacity = 0.6;
                }
                menuItem_Reload.IsEnabled = true; menuItem_Report.IsEnabled = true; menuItem_Tools.IsEnabled = true;

                this.Cursor = Cursors.Arrow;

                InfoWindow iw = new InfoWindow("Success!", "The Aedat file was loaded correctly"); // Show the user a message saying that everything went OK
                iw.ShowDialog();
            }
        }

        /// <summary>
        /// Initialize cochlea type after loading the file
        /// </summary>
        public void initState()
        {
            if (MainWindow.settings.MainS.mono_stereo == EnumAudio.MONO)
            {
                if (MainWindow.settings.MainS.channels == 32)
                {
                    cochleaInfo = EnumCochleaInfo.MONO32;
                }
                else if (MainWindow.settings.MainS.channels == 64)
                {
                    cochleaInfo = EnumCochleaInfo.MONO64;
                }
            }
            else if (MainWindow.settings.MainS.mono_stereo == EnumAudio.STEREO)
            {
                if (MainWindow.settings.MainS.channels == 32)
                {
                    cochleaInfo = EnumCochleaInfo.STEREO32;
                }
                else if (MainWindow.settings.MainS.channels == 64)
                {
                    cochleaInfo = EnumCochleaInfo.STEREO64;
                }
            }
        }

        /// <summary>
        /// Reload the cochleogram chart in the main window. Used to refresh the graph after changing settings like dots to paint or chart color.
        /// </summary>
        public void reloadCochcleogram()
        {
            if (isLoaded)
            {
                if (settings.MainS.eventSize == 16)
                {
                    aedatObject16.closeAedat();
                    aedatObject16 = new cAedat16(root);
                    aedatObject16.adaptAedat();
                }

                if (settings.MainS.eventSize == 32)
                {
                    aedatObject32.closeAedat();
                    aedatObject32 = new cAedat32(root);
                    aedatObject32.adaptAedat();
                }

                displayCharts();

                InfoWindow iw = new InfoWindow("Success!", "The Aedat file was loaded correctly");
                iw.ShowDialog();
            }
        }

        /// <summary>
        /// Settings click event. Open the settings window
        /// </summary>
        private void Btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Settings set = new Settings.Settings(this);
            set.Show();
        }

        /// <summary>
        /// PDF click event. Generates a PDF with the cochleogram, histogram, sonogram, disparity between cochleae and average activity of the loaded aedat file.
        /// </summary>
        private void Btn_PDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savePDFDialog = new SaveFileDialog();
            savePDFDialog.Title = "Select a name and a path for the .pdf file";
            savePDFDialog.Filter = "pdf files|*.pdf";
            if (savePDFDialog.ShowDialog() == true)
            {
                this.Cursor = Cursors.Wait;
                Document doc = new Document();
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(savePDFDialog.FileName, FileMode.Create));
                doc.SetPageSize(iTextSharp.text.PageSize.A4);
                doc.SetMargins(50, 50, 50, 50);
                doc.Open();

                #region Fonts
                iTextSharp.text.Font fTitle = FontFactory.GetFont("Arial", 30, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fHead = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fText = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                #endregion

                #region Main page
                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("\n\n\n\n\nData Report for " + fileName, fTitle);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                if (settings.PdfS.showDate)
                {
                    iTextSharp.text.Paragraph date = new iTextSharp.text.Paragraph("\n\n" + DateTime.Now);
                    date.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(date);
                }
                #endregion
                if (settings.PdfS.showCochleogram || settings.PdfS.showHistogram)
                {
                    doc.SetMargins(0, 30, 50, 30);
                    doc.NewPage();
                }
                iTextSharp.text.Paragraph paragraph;
                #region Cochleogram
                if (settings.PdfS.showCochleogram)
                {
                    paragraph = new iTextSharp.text.Paragraph("           1. Cochleogram\n\n", fHead);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                     The following charts represents the cochleogram for both cochleae.\n", fText);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                         - " + settings.MainS.leftColor + " - Left Cochlea.", fText);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                         - " + settings.MainS.rightColor + " - Right Cochlea.\n\n\n", fText);
                    doc.Add(paragraph);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        chart_Cochleogram.SaveImage(stream, ImageFormat.Png);
                        iTextSharp.text.Image chartImage2 = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                        chartImage2.ScaleToFit(iTextSharp.text.PageSize.A4);// ScalePercent(75f);
                        doc.Add(chartImage2);
                    }
                }
                #endregion
                #region Histogram
                if (settings.PdfS.showHistogram)
                {
                    paragraph = new iTextSharp.text.Paragraph("\n           2. Histogram\n\n", fHead);
                    doc.Add(paragraph);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Histogram spd = new Histogram();
                        spd.chart_Histogram.Width = 773;
                        spd.chart_Histogram.Height = 350;
                        spd.chart_Histogram.SaveImage(stream, ChartImageFormat.Png);
                        iTextSharp.text.Image chartImage3 = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                        chartImage3.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 100, iTextSharp.text.PageSize.A4.Height - 200);
                        doc.Add(chartImage3);
                    }
                }
                #endregion
                if (settings.PdfS.showSonogram || settings.PdfS.showDiff)
                {
                    doc.SetMargins(50, 30, 50, 30);
                    doc.NewPage();
                }
                #region Sonogram
                if (settings.PdfS.showSonogram)
                {
                    paragraph = new iTextSharp.text.Paragraph("           3. Sonogram\n\n", fHead);
                    doc.Add(paragraph);
                    using (MemoryStream stream = new MemoryStream())
                    {

                        if (settings.MainS.eventSize == 16)
                        {
                            aedatObject16.generateBitmap("archivobmp", aedatObject16.maxValueSonogram());
                        }
                        else if (settings.MainS.eventSize == 32)
                        {
                            aedatObject32.generateBitmap("archivobmp", aedatObject32.maxValueSonogram());
                        }
                        Sonogram sd = new Sonogram();
                        iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ImageToBitmap(sd.Img_Sonogram), ImageFormat.Bmp);
                        image1.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 100, iTextSharp.text.PageSize.A4.Height - 200);
                        doc.Add(image1);
                    }
                }
                #endregion
                #region Disparity
                if (settings.PdfS.showDiff && (cochleaInfo == EnumCochleaInfo.STEREO32 || cochleaInfo == EnumCochleaInfo.STEREO64))
                {
                    paragraph = new iTextSharp.text.Paragraph("\n\n           4. Disparity between cochleae\n\n", fHead);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                     Difference between both cochleae.\n", fText);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                         - Red: left cochlea predominance.", fText);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                         - Green: right cochlea predominance.\n\n\n", fText);
                    doc.Add(paragraph);
                    using (MemoryStream stream = new MemoryStream())
                    {

                        if (settings.MainS.eventSize == 16)
                        {
                            aedatObject16.diff2("archivobmpDiff", aedatObject16.maxValueSonogram());
                        }

                        else if (settings.MainS.eventSize == 32)
                        {
                            aedatObject32.diff2("archivobmpDiff", aedatObject32.maxValueSonogram());
                        }
                        Difference sd = new Difference();
                        iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ImageToBitmap(sd.Img_Difference), ImageFormat.Bmp);
                        image1.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 100, iTextSharp.text.PageSize.A4.Height - 200);
                        doc.Add(image1);
                    }
                }
                #endregion
                #region Average activity
                if (settings.PdfS.showAverage)
                {
                    doc.SetMargins(0, 30, 50, 30);
                    doc.NewPage();
                    paragraph = new iTextSharp.text.Paragraph("\n\n           5. Average activity of both cochleae\n\n", fHead);
                    doc.Add(paragraph);
                    paragraph = new iTextSharp.text.Paragraph("                     Each dot represents the average of events of a certain time period (Integration Period = " + settings.ToolsS.integrationPeriod.ToString() + " us ).\n", fText);
                    doc.Add(paragraph);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Average m = new Average();
                        m.chart_Average.Width = 846;
                        m.chart_Average.Height = 322;
                        m.chart_Average.SaveImage(stream, ChartImageFormat.Png);
                        iTextSharp.text.Image chartImage4 = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                        chartImage4.ScaleToFit(iTextSharp.text.PageSize.A4);// ScalePercent(75f);
                        doc.Add(chartImage4);
                    }
                }
                #endregion
                doc.Close();

                InfoWindow iw = new InfoWindow("PDF generated", "The PDF file was generated succesfully and it's available at the path you chose");  // Tell the user that the process went OK
                iw.ShowDialog();

                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Save chart image to Bitmap
        /// </summary>
        private System.Drawing.Bitmap ImageToBitmap(System.Windows.Controls.Image img)
        {
            RenderTargetBitmap rtBmp = new RenderTargetBitmap((int)img.Width, (int)img.Height,
                96.0, 96.0, PixelFormats.Pbgra32);

            img.Measure(new System.Windows.Size((int)img.Width, (int)img.Height));
            img.Arrange(new Rect(new System.Windows.Size((int)img.Width, (int)img.Height)));

            rtBmp.Render(img);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(rtBmp));

            // Save to memory stream and create Bitmap from stream
            encoder.Save(stream);
            return new System.Drawing.Bitmap(stream);
        }

        /// <summary>
        /// Sonogram click event. Generates the sonogram for the loaded aedat file and open a window to show it
        /// </summary>
        private void Btn_sonogram_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool res = false;

            if (settings.MainS.eventSize == 16)
            {
                res = aedatObject16.generateBitmap("archivobmp", aedatObject16.maxValueSonogram());
            }
            else if (settings.MainS.eventSize == 32)
            {
                res = aedatObject32.generateBitmap("archivobmp", aedatObject32.maxValueSonogram());
            }

            if (res)
            {
                Sonogram sd = new Sonogram();
                sd.Show();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// CSV click event. Generates a CSV file with the event information (address and timestamp for each event) of the loaded aedat file 
        /// </summary>
        private void Btn_CSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveCSVDialog = new SaveFileDialog();
            saveCSVDialog.Title = "Select a name and a path for the .csv file";
            saveCSVDialog.Filter = "csv files|*.csv";
            if (saveCSVDialog.ShowDialog() == true)
            {
                this.Cursor = Cursors.Wait;
                File.WriteAllText(saveCSVDialog.FileName, "Timestamp;Address\n", Encoding.UTF8);
                StringBuilder sb = new StringBuilder();

                if (settings.MainS.eventSize == 16)
                {
                    foreach (cAedatRow16 row in aedatObject16.getValues())
                    {
                        sb.Append(row.timestamp + ";" + row.evt + "\n");
                    }
                }
                else if (settings.MainS.eventSize == 32)
                {
                    foreach (cAedatRow32 row in aedatObject32.getValues())
                    {
                        sb.Append(row.timestamp + ";" + row.evt + "\n");
                    }
                }

                File.AppendAllText(saveCSVDialog.FileName, sb.ToString());
                this.Cursor = Cursors.Arrow;

                InfoWindow iw = new InfoWindow("Success!", "CSV file generated correctly");
                iw.ShowDialog();
            }
        }

        /// <summary>
        /// Histogram click event. Opens a new window with the histogram of the loaded aedat file
        /// </summary>
        private void Btn_histogram_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Histogram h = new Histogram();
            h.Show();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Disparity between cochleae click event. Generates the graph containing the disparity information and opens a window to show it.
        /// </summary>
        private void Btn_difference_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool res = false;

            if (settings.MainS.eventSize == 16)
            {
                res = aedatObject16.diff2("archivobmpDiff", aedatObject16.maxValueSonogram());
            }
            else if (settings.MainS.eventSize == 32)
            {
                res = aedatObject32.diff2("archivobmpDiff", aedatObject32.maxValueSonogram());
            }

            if (res)
            {
                Difference d = new Difference();
                d.Show();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Reload click event. Initialize cochlea settings and reloads the cochleogram chart
        /// </summary>
        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            initState();
            reloadCochcleogram();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Reload click event. Initialize cochlea settings and reloads the cochleogram chart
        /// </summary>
        private void Btn_AedatSplitterManual_Click(object sender, RoutedEventArgs e)
        {
            if (isOpen_ManualAedatSplitter)
            {
                closeManualAedatSplitter();
                isOpen_ManualAedatSplitter = false;
            }
            else
            {
                openManualAedatSplitter();
                isOpen_ManualAedatSplitter = true;
            }
        }

        /// <summary>
        /// Average activity click event. Opens a windows containing a graph with the average activity (megaevents per second) for both cochleae
        /// </summary>
        private void Btn_average_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Average a = new Average();
            a.Show();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Manual aedat splitter cut click event. Creates a new Aedat file witht the events contained in the range [text_ManualAedatSplitterInit, text_ManualAedatSplitterEnd]
        /// </summary>
        private void Btn_cutManualAedatSplitter_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(text_ManualAedatSplitterInit.Text) > Convert.ToInt32(text_ManualAedatSplitterEnd.Text))
            {
                InfoWindow iw = new InfoWindow("Parameters not valid", "The end of the range can't be lesser than the init");
                iw.ShowDialog();
            }
            else if (Convert.ToInt32(text_ManualAedatSplitterInit.Text) == Convert.ToInt32(text_ManualAedatSplitterEnd.Text))
            {
                InfoWindow iw = new InfoWindow("Parameters not valid", "The current selected range is zero");
                iw.ShowDialog();
            }
            else
            {
                SaveFileDialog saveSplit = new SaveFileDialog();
                saveSplit.Title = "Select a name and a path for the .aedat file";
                saveSplit.Filter = "aedat files|*.aedat";
                if (saveSplit.ShowDialog() == true)
                {
                    this.Cursor = Cursors.Wait;
                    if (settings.MainS.eventSize == 16)
                    {
                        aedatObject16.AERSplitterManual(Convert.ToInt32(text_ManualAedatSplitterInit.Text), Convert.ToInt32(text_ManualAedatSplitterEnd.Text), saveSplit.FileName);
                    }
                    else if (settings.MainS.eventSize == 32)
                    {
                        aedatObject32.AedatSplitterManual(Convert.ToInt32(text_ManualAedatSplitterInit.Text), Convert.ToInt32(text_ManualAedatSplitterEnd.Text), saveSplit.FileName);
                    }
                    this.Cursor = Cursors.Arrow;

                    InfoWindow iw = new InfoWindow("Split operation succeded", "The aedat file was saved correctly");
                    iw.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Cochleogram double click event. Sets the text_ManualAedatSplitterInit value depending on the timestamp that the double click event was raised
        /// </summary>
        private void chart_Cochleogram_DoubleClick(object sender, EventArgs e)
        {
            if (isLoaded)
            {
                System.Windows.Forms.MouseEventArgs me = (System.Windows.Forms.MouseEventArgs)e;
                System.Drawing.Point point = me.Location;

                var results = chart_Cochleogram.HitTest(point.X, point.Y, false, ChartElementType.PlottingArea);

                if (results[0].ChartElementType == ChartElementType.PlottingArea)
                {
                    if (me.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        text_ManualAedatSplitterInit.Text = ((int)results[0].ChartArea.AxisX.PixelPositionToValue(point.X)).ToString();
                    }
                    else if (me.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        text_ManualAedatSplitterEnd.Text = ((int)results[0].ChartArea.AxisX.PixelPositionToValue(point.X)).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Opens the Manual aedat splitter menu
        /// </summary>
        private void openManualAedatSplitter()
        {
            tab_ManualAedatSplitter.Visibility = Visibility.Visible;
            tab_fileLoaded.Margin = new Thickness(351, 0, 0, 0);
        }

        /// <summary>
        /// Closes the Manual aedat splitter menu
        /// </summary>
        private void closeManualAedatSplitter()
        {
            tab_ManualAedatSplitter.Visibility = Visibility.Collapsed;
            tab_fileLoaded.Margin = new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// Limits for the user input on the manual aedat splitter (Saturates values greater than the maximum timestamp found on the file and prevents from using values that are not integers)
        /// </summary>
        #region Manual Aedat Splitter controls
        private void text_ManualAedatSplitterInit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (Convert.ToInt32(text_ManualAedatSplitterInit.Text.Length) == 0)
                {
                    text_ManualAedatSplitterInit.Text = 0.ToString();
                }
                else
                {
                    if (settings.MainS.eventSize == 16)
                    {
                        if (Convert.ToInt32(text_ManualAedatSplitterEnd.Text) > aedatObject16.maxTimestamp)
                        {
                            text_ManualAedatSplitterInit.Text = aedatObject16.maxTimestamp.ToString();
                        }
                    }
                    else if (settings.MainS.eventSize == 32)
                    {
                        if (Convert.ToInt32(text_ManualAedatSplitterInit.Text) > aedatObject32.maxTimestamp)
                        {
                            text_ManualAedatSplitterInit.Text = aedatObject32.maxTimestamp.ToString();
                        }
                    }
                }
            }
        }
        private void text_ManualAedatSplitterEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (Convert.ToInt32(text_ManualAedatSplitterEnd.Text.Length) == 0)
                {
                    text_ManualAedatSplitterEnd.Text = 0.ToString();
                }
                else
                {
                    if (settings.MainS.eventSize == 16)
                    {
                        if (Convert.ToInt32(text_ManualAedatSplitterEnd.Text) > aedatObject16.maxTimestamp)
                        {
                            text_ManualAedatSplitterEnd.Text = aedatObject16.maxTimestamp.ToString();
                        }
                    }
                    else if (settings.MainS.eventSize == 32)
                    {
                        if (Convert.ToInt32(text_ManualAedatSplitterEnd.Text) > aedatObject32.maxTimestamp)
                        {
                            text_ManualAedatSplitterEnd.Text = aedatObject32.maxTimestamp.ToString();
                        }
                    }
                }
            }
        }
        private void text_ManualAedatSplitterInit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void text_ManualAedatSplitterEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        #endregion

        /// <summary>
        /// Opens the Automatic aedat splitter window to select the values of noise tolerance and noise threshold before splitting the aedat file
        /// </summary>
        private void Btn_AedatSplitterAuto_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            NAVIS.Screens.AedatSplitterAutoWindow asw = new NAVIS.Screens.AedatSplitterAutoWindow();
            asw.Show();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Generates a mono aedat file from a stereo aedat with the information from the left cochlea.
        /// </summary>
        private void Btn_StereoToMono_Click(object sender, RoutedEventArgs e)
        {
            if (settings.MainS.eventSize == 16)
            {
                aedatObject16.saveStereoToMono(MainWindow.fileName.Split('.')[0] + "_mono" + ".aedat", aedatObject16.getValues());
            }
            else if (settings.MainS.eventSize == 32)
            {
                aedatObject32.saveStereoToMono(MainWindow.fileName.Split('.')[0] + "_mono" + ".aedat", aedatObject32.getValues());
            }
        }

        private void Btn_About_Click(object sender, RoutedEventArgs e)
        {
            NAVIS.About ni = new NAVIS.About();
            ni.Show();
        }
    }
}