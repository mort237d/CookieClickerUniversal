using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CookieClickerUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int _points;
        private int _totalPoints;
        private int _clickMultiplier = 1;
        private int _cookiesPerSecond = 0;

        private int _counter;

        #region store
        private int _cursor;
        private int _cursorPrice = 10;

        private int _grandma;
        private int _grandmaPrice = 20;

        private int _farm;
        private int _farmPrice = 30;
        private int _farmCase = 5;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            updatePoints();
            pCursorButton1.Visibility = Visibility.Collapsed;
            pCursorButton1.Content = "Cursor" + "\nCost: " + _cursorPrice;
            pGrandmaButton2.Visibility = Visibility.Collapsed;
            pGrandmaButton2.Content = "Grandma" + "\nCost: " + _grandmaPrice;

            TimeSpan period = TimeSpan.FromSeconds(0.1);

            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {// Update the UI thread by using the UI core dispatcher.
                _counter++;
                if (_counter == 10)
                {
                    _points += _cookiesPerSecond;
                    _totalPoints += _cookiesPerSecond;
                    _counter = 0;
                }
                Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {// UI components can be accessed within this scope.
                        updatePoints();

                        if (_points < _cursorPrice) pCursorButton1.Foreground = new SolidColorBrush(Colors.Red);
                        else pCursorButton1.Foreground = new SolidColorBrush(Colors.Black);
                        if (_points < _grandmaPrice) pGrandmaButton2.Foreground = new SolidColorBrush(Colors.Red);
                        else pGrandmaButton2.Foreground = new SolidColorBrush(Colors.Black);

                        switch (_totalPoints)
                        {
                            case 10:
                                pCursorButton1.Visibility = Visibility.Visible;
                                break;
                            case 30:
                                pGrandmaButton2.Visibility = Visibility.Visible;
                                break;
                        }

                        
                    });

            }, period);
        }

        public void updatePoints()
        {
            _clickMultiplier = 1 + _cursor;
            _cookiesPerSecond = _grandma;
            pPointTextBlock.Text = _points + " Cookies" +
                                   "\n" + _cookiesPerSecond + " pr sec" +
                                   "\n" + _clickMultiplier + " pr click" +
                                   "\n\ntotal cookies: " + _totalPoints;
        }

        private void pCursorButton1_Click(object sender, RoutedEventArgs e)
        {
            if (_points >= _cursorPrice)
            {
                _points -= _cursorPrice;
                updatePoints();
                _cursor++;
                _cursorPrice += 5;
                pCursorButton1.Content = "Cursor: " + _cursor + "\nCost: " + _cursorPrice;
            }
        }

        private void pGrandmaButton2_Click(object sender, RoutedEventArgs e)
        {
            if (_points >= _grandmaPrice)
            {
                _points -= _grandmaPrice;
                updatePoints();
                _grandma++;
                _grandmaPrice += 10;
                pGrandmaButton2.Content = "Grandma: " + _grandma + "\nCost: " + _grandmaPrice;
            }
        }

        private void pCookie_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _clickMultiplier; i++)
            {
                _totalPoints++;
                _points++;
            }
            updatePoints();
        }
    }
}
