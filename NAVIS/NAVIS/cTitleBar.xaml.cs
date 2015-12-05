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
using System.Diagnostics;
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

namespace NAVIS
{
    /// <summary>
    /// UserControl for the title bar template
    /// </summary>
    public partial class cTitleBar : UserControl
    {
        Window _Owner;
        public Window Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        static WindowState _WStateNoPop = WindowState.Normal;
        WindowState _WState = WindowState.Normal;

        public WindowState WState
        {
            get
            {
                if (_IsPop)
                    return _WState; // pop up window
                return _WStateNoPop;    // Non pop up window
            }
            set
            {
                if (_IsPop)
                    _WState = value;
                _WStateNoPop = value;
            }
        }

        #region Configuration switches
        bool _ShowMin = true;
        /// <summary>
        /// Shows minimize button
        /// </summary>
        public bool ShowMin
        {
            get { return _BtMinimize.IsVisible; }
            set
            {
                _ShowMin = value;
                if (_ShowMin == true)
                    _BtMinimize.Visibility = Visibility.Visible;
                else
                    _BtMinimize.Visibility = Visibility.Hidden;
            }
        }

        bool _ShowMax = true;
        /// <summary>
        /// Shows maximize button
        /// </summary>
        public bool ShowMax
        {
            get { return _BtMax.IsVisible; }
            set
            {
                _ShowMax = value;
                if (_ShowMax == true)
                {
                    _BtMax.Visibility = Visibility.Visible;
                }
                else
                {
                    if (_ShowMin)
                    {
                        _BtMax.IsEnabled = false;
                        _BtMax.Opacity = 0.25;
                    }
                }
            }
        }

        bool _JustCloseWindow = false;
        /// <summary>
        /// Only closes the window, not the entire application
        /// </summary>    
        public bool JustCloseWindow
        {
            get { return _JustCloseWindow; }
            set { _JustCloseWindow = value; }
        }
        bool _IsPop = false;
        /// <summary>
        /// Pop up window
        /// </summary>
        public bool IsPop
        {
            get { return _IsPop; }
            set { _IsPop = value; }
        }
        #endregion

        bool _isMainWindow = false;
        /// <summary>
        /// No CornerRadius on mainWindow
        /// </summary>    
        public bool isMainWindow
        {
            get { return _isMainWindow; }
            set { _isMainWindow = value; }
        }

        public cTitleBar()
        {
            InitializeComponent();
            _BtNormal.Opacity = 1.0;
        }

        private void _TbClose_Click(object sender, RoutedEventArgs e)
        {
            if (_JustCloseWindow)
            {
                if (this._Owner != null)
                    _Owner.Close();
            }
            else
                Application.Current.Shutdown();
        }
        void _BtMinimize_Click(object sender, RoutedEventArgs e)
        {
            _Owner.WindowState = WindowState.Minimized;
        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _Owner.DragMove();
        }
        private void _BtMax_Click(object sender, RoutedEventArgs e)
        {
            SetWState(WindowState.Maximized);
        }

        private void cTittleBar_Loaded(object sender, RoutedEventArgs e)
        {
            _Owner = Window.GetWindow(this);
            if (_Owner != null)
            {
                this.TBName.Content = _Owner.Title;
                //Set Window State
                SetWState();
                _Owner.SizeChanged += _Owner_SizeChanged;
            }

            if (isMainWindow)
            {
                RoundedBar.CornerRadius = new CornerRadius(0);
            }
        }

        void _Owner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("Window size" + e.NewSize);
        }

        private void SetWState()
        {
            if (WState == WindowState.Normal)
            {
                _BtNormal.Visibility = Visibility.Hidden;
                if (_ShowMax)
                {
                    _BtMax.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (_ShowMax)
                    _BtNormal.Visibility = Visibility.Visible;
                else if (_ShowMin)
                {
                    _BtMax.IsEnabled = false;
                    _BtMax.Opacity = 0.25;
                }
                else _BtMax.Visibility = Visibility.Hidden;
            }
            _Owner.WindowState = WState;
            Debug.WriteLine("Window state: " + _Owner.WindowState.ToString());
        }
        private void SetWState(WindowState WS)
        {
            WState = WS;
            SetWState();
        }

        private void _BtNormal_Click(object sender, RoutedEventArgs e)
        {
            SetWState(WindowState.Normal);
        }

        private void cTitleBar_Name_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_ShowMax)
            {
                if (WState == WindowState.Maximized)
                {
                    SetWState(WindowState.Normal);
                }
                else if (WState == WindowState.Normal)
                {
                    SetWState(WindowState.Maximized);
                }
            }
        }
    }
}
