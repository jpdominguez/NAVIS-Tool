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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NAVIS
{
    /// <summary>
    /// Main settings class
    /// </summary>
    public class MainS
    {
        public int dotSize;
        public EnumColor leftColor;
        public EnumColor rightColor;
        public bool showChart;
        public int eventSize;
        public int dotsToPaint;
        public int channels;
        public EnumAudio mono_stereo;
        public EnumScreenSize screenSize;

        /// <summary>
        /// Main settings empty constructor
        /// </summary>
        public MainS()
        {
            this.dotSize = 2;
            this.leftColor = EnumColor.BLUE;
            this.rightColor = EnumColor.ORANGE;
            this.showChart = true;
            this.eventSize = 32;
            this.dotsToPaint = 10;
            this.channels = 64;
            this.mono_stereo = EnumAudio.STEREO;
            this.screenSize = EnumScreenSize.s1366x768;
        }

        /// <summary>
        /// Main settings constructor with parameters
        /// </summary>
        public MainS(int dotSize, EnumColor leftColor, EnumColor rightColor, bool showChart, int eventSize, int dotsToPaint, int channels, EnumAudio mono_stereo, EnumScreenSize screenSize)
        {
            this.dotSize = dotSize;
            this.leftColor = leftColor;
            this.rightColor = rightColor;
            this.showChart = showChart;
            this.eventSize = eventSize;
            this.dotsToPaint = dotsToPaint;
            this.channels = channels;
            this.mono_stereo = mono_stereo;
            this.screenSize = screenSize;
        }

        /// <summary>
        /// Main settings export to XML
        /// </summary>
        public void toXML(XmlTextWriter textWriter)
        {
            CultureInfo ci = new CultureInfo("en-us");

            textWriter.WriteStartElement("Main");
            textWriter.WriteAttributeString("dotSize", dotSize.ToString(ci));
            textWriter.WriteAttributeString("leftColor", leftColor.ToString("G"));
            textWriter.WriteAttributeString("rightColor", rightColor.ToString("G"));
            textWriter.WriteAttributeString("showChart", showChart.ToString(ci));
            textWriter.WriteAttributeString("eventSize", eventSize.ToString(ci));
            textWriter.WriteAttributeString("dotsToPaint", dotsToPaint.ToString(ci));
            textWriter.WriteAttributeString("channels", channels.ToString(ci));
            textWriter.WriteAttributeString("mono_stereo", mono_stereo.ToString("G"));
            textWriter.WriteAttributeString("screenSize", screenSize.ToString("G"));
            textWriter.WriteEndElement();
        }

        /// <summary>
        /// Main settings read from XML
        /// </summary>
        public void fromXML(XElement root)
        {
            CultureInfo ci = new CultureInfo("en-us");
            XElement xe;
            xe = root.Element("Main");
            dotSize = Convert.ToInt16(xe.Attribute("dotSize").Value, ci);
            leftColor = (EnumColor)Enum.Parse(typeof(EnumColor), xe.Attribute("leftColor").Value);
            rightColor = (EnumColor)Enum.Parse(typeof(EnumColor), xe.Attribute("rightColor").Value);
            showChart = Convert.ToBoolean(xe.Attribute("showChart").Value, ci);
            eventSize = Convert.ToInt16(xe.Attribute("eventSize").Value, ci);
            dotsToPaint = Convert.ToInt16(xe.Attribute("dotsToPaint").Value, ci);
            channels = Convert.ToInt16(xe.Attribute("channels").Value, ci);
            mono_stereo = (EnumAudio)Enum.Parse(typeof(EnumAudio), xe.Attribute("mono_stereo").Value);
            screenSize = (EnumScreenSize)Enum.Parse(typeof(EnumScreenSize), xe.Attribute("screenSize").Value);
        }
    }

    /// <summary>
    /// Tools settings class
    /// </summary>
    public class ToolsS
    {
        public EnumSize imgSize;
        public long integrationPeriod;
        public int noiseThreshold;
        public int noiseTolerance;

        /// <summary>
        /// Tools settings empty constructor
        /// </summary>
        public ToolsS()
        {
            this.imgSize = EnumSize.LARGE;
            this.integrationPeriod = 100000;
            this.noiseThreshold = 3;
            this.noiseTolerance = 20;
        }

        /// <summary>
        /// Tools settings constructor with parameters
        /// </summary>
        public ToolsS(EnumSize imgSize, long integrationPeriod, int noiseThreshold, int noiseTolerance)
        {
            this.imgSize = imgSize;
            this.integrationPeriod = integrationPeriod;
            this.noiseThreshold = noiseThreshold;
            this.noiseTolerance = noiseThreshold;
        }

        /// <summary>
        /// Tools settings export to XML
        /// </summary>
        public void toXML(XmlTextWriter textWriter)
        {
            CultureInfo ci = new CultureInfo("en-us");

            textWriter.WriteStartElement("Tools");
            textWriter.WriteAttributeString("imgSize", imgSize.ToString("G"));
            textWriter.WriteAttributeString("integrationPeriod", integrationPeriod.ToString(ci));
            textWriter.WriteAttributeString("noiseThreshold", noiseThreshold.ToString(ci));
            textWriter.WriteAttributeString("noiseTolerance", noiseTolerance.ToString(ci));
            textWriter.WriteEndElement();
        }

        /// <summary>
        /// Tools settings read from XML
        /// </summary>
        public void fromXML(XElement root)
        {
            CultureInfo ci = new CultureInfo("en-us");
            XElement xe;
            xe = root.Element("Tools");
            imgSize = (EnumSize)Enum.Parse(typeof(EnumSize), xe.Attribute("imgSize").Value);
            integrationPeriod = Convert.ToInt64(xe.Attribute("integrationPeriod").Value, ci);
            noiseThreshold = Convert.ToInt32(xe.Attribute("noiseThreshold").Value, ci);
            noiseTolerance = Convert.ToInt32(xe.Attribute("noiseTolerance").Value, ci);
        }
    }

    /// <summary>
    /// PDF settings class
    /// </summary>
    public class PdfS
    {
        public bool showDate;
        public bool showCochleogram;
        public bool showHistogram;
        public bool showSonogram;
        public bool showDiff;
        public bool showAverage;

        /// <summary>
        /// PDF settings empty constructor
        /// </summary>
        public PdfS()
        {
            this.showDate = true;
            this.showCochleogram = true;
            this.showHistogram = true;
            this.showSonogram = true;
            this.showDiff = true;
            this.showAverage = true;
        }

        /// <summary>
        /// PDF settings constructor with parameters
        /// </summary>
        public PdfS(bool showDate, bool showCochleogram, bool showHistogram, bool showSonogram, bool showDiff, bool showAverage)
        {
            this.showDate = showDate;
            this.showCochleogram = showCochleogram;
            this.showHistogram = showHistogram;
            this.showSonogram = showSonogram;
            this.showDiff = showDiff;
            this.showAverage = showAverage;
        }

        /// <summary>
        /// PDF settings export to XML
        /// </summary>
        public void toXML(XmlTextWriter textWriter)
        {
            CultureInfo ci = new CultureInfo("en-us");

            textWriter.WriteStartElement("PDF");
            textWriter.WriteAttributeString("showDate", showDate.ToString(ci));
            textWriter.WriteAttributeString("showCochleogram", showCochleogram.ToString(ci));
            textWriter.WriteAttributeString("showHistogram", showHistogram.ToString(ci));
            textWriter.WriteAttributeString("showSonogram", showSonogram.ToString(ci));
            textWriter.WriteAttributeString("showDiff", showDiff.ToString(ci));
            textWriter.WriteAttributeString("showAverage", showAverage.ToString(ci));
            textWriter.WriteEndElement();
        }

        /// <summary>
        /// PDF settings read from XML
        /// </summary>
        public void fromXML(XElement root)
        {
            CultureInfo ci = new CultureInfo("en-us");
            XElement xe;
            xe = root.Element("PDF");
            showDate = Convert.ToBoolean(xe.Attribute("showDate").Value);
            showCochleogram = Convert.ToBoolean(xe.Attribute("showCochleogram").Value);
            showHistogram = Convert.ToBoolean(xe.Attribute("showHistogram").Value);
            showSonogram = Convert.ToBoolean(xe.Attribute("showSonogram").Value);
            showDiff = Convert.ToBoolean(xe.Attribute("showDiff").Value);
            showAverage = Convert.ToBoolean(xe.Attribute("showAverage").Value);
        }
    }

    /// <summary>
    /// Toolbar settings class
    /// </summary>
    public class ToolbarS
    {
        public bool showLoadAedat;
        public bool showSettings;
        public bool showGeneratePDF;
        public bool showGenerateCSV;
        public bool showSonogram;
        public bool showHistogram;
        public bool showAverage;
        public bool showDiff;
        public bool showManualAedatSplitter;
        public bool showAutomaticAedatSplitter;
        public bool showAbout;
        public bool showStereoToMono;
        public bool showMonoToStereo;

        /// <summary>
        /// Toolbar settings empty constructor
        /// </summary>
        public ToolbarS()
        {
            this.showLoadAedat = true;
            this.showSettings = true;
            this.showGeneratePDF = true;
            this.showGenerateCSV = true;
            this.showSonogram = true;
            this.showHistogram = true;
            this.showAverage = true;
            this.showDiff = true;
            this.showManualAedatSplitter = true;
            this.showAutomaticAedatSplitter = true;
            this.showAbout = true;
            this.showStereoToMono = true;
            this.showMonoToStereo = true;
        }

        /// <summary>
        /// Toolbar settings constructor with parameters
        /// </summary>
        public ToolbarS(bool showLoadAedat, bool showSettings, bool showAbout, bool showGeneratePDF, bool showGenerateCSV, bool showSonogram, bool showHistogram, bool showAverage, bool showDiff, bool showManualAedatSplitter, bool showAutomaticAedatSplitter, bool showStereoToMono, bool showMonoToStereo)
        {
            this.showLoadAedat = showLoadAedat;
            this.showSettings = showSettings;
            this.showAbout = showAbout;
            this.showGeneratePDF = showGeneratePDF;
            this.showGenerateCSV = showGenerateCSV;
            this.showSonogram = showSonogram;
            this.showHistogram = showHistogram;
            this.showAverage = showAverage;
            this.showDiff = showDiff;
            this.showManualAedatSplitter = showManualAedatSplitter;
            this.showAutomaticAedatSplitter = showAutomaticAedatSplitter;
            this.showMonoToStereo = showMonoToStereo;
            this.showStereoToMono = showStereoToMono;
        }

        /// <summary>
        /// Toolbar settings export to XML
        /// </summary>
        public void toXML(XmlTextWriter textWriter)
        {
            CultureInfo ci = new CultureInfo("en-us");

            textWriter.WriteStartElement("Toolbar");
            textWriter.WriteAttributeString("showLoadAedat", showLoadAedat.ToString(ci));
            textWriter.WriteAttributeString("showSettings", showSettings.ToString(ci));
            textWriter.WriteAttributeString("showAbout", showAbout.ToString(ci));
            textWriter.WriteAttributeString("showGeneratePDF", showGeneratePDF.ToString(ci));
            textWriter.WriteAttributeString("showGenerateCSV", showGenerateCSV.ToString(ci));
            textWriter.WriteAttributeString("showSonogram", showSonogram.ToString(ci));
            textWriter.WriteAttributeString("showHistogram", showHistogram.ToString(ci));
            textWriter.WriteAttributeString("showAverage", showAverage.ToString(ci));
            textWriter.WriteAttributeString("showDiff", showDiff.ToString(ci));
            textWriter.WriteAttributeString("showManualAedatSplitter", showManualAedatSplitter.ToString(ci));
            textWriter.WriteAttributeString("showAutomaticAedatSplitter", showAutomaticAedatSplitter.ToString(ci));
            textWriter.WriteAttributeString("showStereoToMono", showStereoToMono.ToString(ci));
            textWriter.WriteAttributeString("showMonoToStereo", showMonoToStereo.ToString(ci));
            textWriter.WriteEndElement();
        }

        /// <summary>
        /// Toolbar settings read from XML
        /// </summary>
        public void fromXML(XElement root)
        {
            CultureInfo ci = new CultureInfo("en-us");
            XElement xe;
            xe = root.Element("Toolbar");
            showLoadAedat = Convert.ToBoolean(xe.Attribute("showLoadAedat").Value);
            showSettings = Convert.ToBoolean(xe.Attribute("showSettings").Value);
            showAbout = Convert.ToBoolean(xe.Attribute("showAbout").Value);
            showGeneratePDF = Convert.ToBoolean(xe.Attribute("showGeneratePDF").Value);
            showGenerateCSV = Convert.ToBoolean(xe.Attribute("showGenerateCSV").Value);
            showSonogram = Convert.ToBoolean(xe.Attribute("showSonogram").Value);
            showHistogram = Convert.ToBoolean(xe.Attribute("showHistogram").Value);
            showAverage = Convert.ToBoolean(xe.Attribute("showAverage").Value);
            showDiff = Convert.ToBoolean(xe.Attribute("showDiff").Value);
            showManualAedatSplitter = Convert.ToBoolean(xe.Attribute("showManualAedatSplitter").Value);
            showAutomaticAedatSplitter = Convert.ToBoolean(xe.Attribute("showAutomaticAedatSplitter").Value);
            showStereoToMono = Convert.ToBoolean(xe.Attribute("showStereoToMono").Value);
            showMonoToStereo = Convert.ToBoolean(xe.Attribute("showMonoToStereo").Value);
        }
    }

    public enum EnumColor { BLUE = 0, ORANGE = 1, RED = 2, BLACK = 3, GREEN = 4, YELLOW = 5, BROWN = 6, MAGENTA = 7 };
    public enum EnumSize { TINY = 1, SMALL = 2, MEDIUM = 3, LARGE = 4 };
    public enum EnumAudio { MONO = 0, STEREO = 1 };
    public enum EnumCochleaInfo { MONO32 = 0, MONO64 = 1, MONO128 = 2, MONO256 = 3, STEREO32 = 4, STEREO64 = 5, STEREO128 = 6, STEREO256 = 7 };
    public enum EnumScreenSize { s800x600 = 0, s1024x768 = 1, s1280x1024 = 2, s1366x768 = 3, s1920x1080 = 4 };

    /// <summary>
    /// Settings class. Includes the other four settings classes (Tools, PDF, Toolbar and Main)
    /// </summary>
    public class cSettings
    {
        public MainS MainS;
        public ToolsS ToolsS;
        public PdfS PdfS;
        public ToolbarS ToolbarS;

        /// <summary>
        /// Settings empty constructor
        /// </summary>
        public cSettings()
        {
            MainS = new MainS();
            ToolsS = new ToolsS();
            PdfS = new PdfS();
            ToolbarS = new ToolbarS();

            if (File.Exists("./Resources/Settings.xml"))
            {
                loadSettingsFromFile();
            }
            else
            {
                if (Directory.Exists("./Resources"))
                {
                    saveSettingsToFile();
                    loadSettingsFromFile();
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory("./Resources");
                    saveSettingsToFile();
                    loadSettingsFromFile();
                }
            }
        }

        /// <summary>
        /// Save settings to file. Has a parameter to specify the output filename
        /// </summary>
        public void saveSettingsToFile(string filename)
        {
            CultureInfo ci = new CultureInfo("en-us");
            XmlTextWriter textWriter = new XmlTextWriter(filename, null);

            textWriter.WriteStartDocument();
            textWriter.Formatting = Formatting.Indented;
            textWriter.Indentation = 2;

            textWriter.WriteStartElement("Settings");
            textWriter.WriteAttributeString("SettingsVersion", "1.0");

            textWriter.WriteStartElement("Main");
            MainS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Tools");
            ToolsS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("PDF");
            PdfS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Toolbar");
            ToolbarS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        /// <summary>
        /// Save settings to the default settings file.
        /// </summary>
        public void saveSettingsToFile()
        {
            CultureInfo ci = new CultureInfo("en-us");
            XmlTextWriter textWriter = new XmlTextWriter("./Resources/Settings.xml", null);

            textWriter.WriteStartDocument();
            textWriter.Formatting = Formatting.Indented;
            textWriter.Indentation = 2;

            textWriter.WriteStartElement("Settings");
            textWriter.WriteAttributeString("SettingsVersion", "1.0");

            textWriter.WriteStartElement("Main");
            MainS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Tools");
            ToolsS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("PDF");
            PdfS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Toolbar");
            ToolbarS.toXML(textWriter);
            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        /// <summary>
        /// Load settings from the file specified with the input parameter
        /// </summary>
        public void loadSettingsFromFile(string filename)
        {
            XDocument xDocSystem;
            xDocSystem = XDocument.Load(filename);
            XElement root, xe;
            root = xDocSystem.Element("Settings");

            xe = root.Element("Main");
            MainS.fromXML(xe);
            xe = root.Element("Tools");
            ToolsS.fromXML(xe);
            xe = root.Element("PDF");
            PdfS.fromXML(xe);
            xe = root.Element("Toolbar");
            ToolbarS.fromXML(xe);
        }

        /// <summary>
        /// Load settings from the default settings path
        /// </summary>
        public void loadSettingsFromFile()
        {
            XDocument xDocSystem;
            xDocSystem = XDocument.Load("./Resources/Settings.xml");
            XElement root, xe;
            root = xDocSystem.Element("Settings");

            xe = root.Element("Main");
            MainS.fromXML(xe);
            xe = root.Element("Tools");
            ToolsS.fromXML(xe);
            xe = root.Element("PDF");
            PdfS.fromXML(xe);
            xe = root.Element("Toolbar");
            ToolbarS.fromXML(xe);
        }

        /// <summary>
        /// Settings clone
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
