using System;
using System.Collections.Generic;
using FaultTreeEditor.Core.Models;
using FaultTreeEditor.ViewModels;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FaultTreeEditor.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        private readonly UISettings uiSettings = new UISettings();

        private Color lineColor = Colors.Black;
        private readonly int lineEdgeOffset = 40;

        public MainPage()
        {
            InitializeComponent();
            uiSettings.ColorValuesChanged += ColorValuesChanged;
            InitializeLineColor();
        }

        private void InitializeLineColor()
        {
            var defaultTheme = new UISettings();
            var uiThemeColor = defaultTheme.GetColorValue(UIColorType.Background);
            SetLineColor(uiThemeColor);
        }

        private void ColorValuesChanged(UISettings sender, object args)
        {
            //Color accentColor = sender.GetColorValue(UIColorType.Accent);
            //or
            //Color accentColor = (Color)Resources["SystemAccentColor"];
            var uiThemeColor = sender.GetColorValue(UIColorType.Background);
            SetLineColor(uiThemeColor);

            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    DrawLines();
                }
            );
        }

        private void SetLineColor(Color color)
        {
            if (color == Colors.Black)
            {
                lineColor = Colors.White;
            }
            else if (color == Colors.White)
            {
                lineColor = Colors.Black;
            }
            else // some new theme color
            {
                lineColor = Colors.Black;
            }
        }

        void SP_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Opacity = 0.4;
            Element element = sp.DataContext as Element;
            ViewModel.SelectedCanvasElement = element;
        }

        void SP_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            Element element = sp.DataContext as Element;
            float zf = MyScrollViewer.ZoomFactor;
            element.X += e.Delta.Translation.X / zf;
            element.Y += e.Delta.Translation.Y / zf;
            DrawLines();
        }

        void SP_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Opacity = 1;
            DrawLines();
        }

        void DrawLines()
        {
            Canvas1.Children.Clear();
            foreach(var v in ViewModel.Connections)
            {
                Line line = new Line
                {
                    X1 = lineEdgeOffset + v.From.X,
                    X2 = lineEdgeOffset + v.To.X,
                    Y1 = lineEdgeOffset + v.From.Y,
                    Y2 = lineEdgeOffset + v.To.Y,
                    Stroke = new SolidColorBrush(lineColor),
                    StrokeThickness = 2
                };
                Canvas.SetZIndex(line, 1);
                Canvas1.Children.Add(line);
            }
        }

        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Element myListBoxSelectedItem = MyListBox.SelectedItem as Element;
            if (MyListBox.SelectedIndex == -1)
                return;

            if (ViewModel.IsAddConnectionToggled)
            {
                if(ViewModel.SelectedCanvasElement != myListBoxSelectedItem){
                    ViewModel.Connections.Add(new Connection
                    {
                        From = ViewModel.SelectedCanvasElement,
                        To = myListBoxSelectedItem
                    });
                    ViewModel.SelectedCanvasElement.Children.Add(myListBoxSelectedItem);
                    myListBoxSelectedItem.Parents.Add(ViewModel.SelectedCanvasElement);

                    ViewModel.IsAddConnectionToggled = false;
                    DrawLines();
                }
            }
            else if (ViewModel.IsRemoveConnectionToggled)
            {
                var toRemove = new List<Connection>();
                foreach (var v in ViewModel.Connections)
                {
                    if((v.From == ViewModel.SelectedCanvasElement && v.To == myListBoxSelectedItem) || (v.From == myListBoxSelectedItem && v.To == ViewModel.SelectedCanvasElement))
                    {
                        toRemove.Add(v);
                    }
                }
                foreach (var v in toRemove)
                {
                    ViewModel.Connections.Remove(v);
                }

                ViewModel.SelectedCanvasElement.Children.Remove(myListBoxSelectedItem);
                ViewModel.SelectedCanvasElement.Parents.Remove(myListBoxSelectedItem);
                myListBoxSelectedItem.Children.Remove(ViewModel.SelectedCanvasElement);
                myListBoxSelectedItem.Parents.Remove(ViewModel.SelectedCanvasElement);

                ViewModel.IsRemoveConnectionToggled = false;
                DrawLines();
            }
            else
            {
                ViewModel.SelectedCanvasElement = myListBoxSelectedItem;
            }
            MyListBox.SelectedIndex = -1;
        }

        private void Delete_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.DeleteElementCommand.Execute(null);
            DrawLines();
        }

        private void Clear_Canvas_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Connections.Clear();
            var toRemove = new List<Element>();
            foreach (var v in ViewModel.CanvasElements)
            {
                if(v.DisplayTitle != "Top level event")
                {
                    toRemove.Add(v);
                }
                else
                {
                    v.Parents.Clear();
                    v.Children.Clear();
                }
            }
            foreach(var v in toRemove)
            {
                ViewModel.CanvasElements.Remove(v);
            }
            if (ViewModel.CanvasElements.Count > 0)
            {
                ViewModel.SelectedCanvasElement = ViewModel.CanvasElements[0];
            }
            else
            {
                ViewModel.SelectedCanvasElement = null;
            }
            ViewModel.ResetCounters();
            DrawLines();
        }

        private void Add_Connection_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element element = (sender as MenuFlyoutItem).DataContext as Element;
            ViewModel.SelectedCanvasElement = element;
            ViewModel.IsAddConnectionToggled = true;
        }

        private void Remove_Connection_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element element = (sender as MenuFlyoutItem).DataContext as Element;
            ViewModel.SelectedCanvasElement = element;
            ViewModel.IsRemoveConnectionToggled = true;
        }

        private void Remove_All_Connections_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element element = (sender as MenuFlyoutItem).DataContext as Element;
            ViewModel.SelectedCanvasElement = element;
            ViewModel.RemoveConnectionsCommand.Execute(null);
            DrawLines();
        }

        private void Delete_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element element = (sender as MenuFlyoutItem).DataContext as Element;
            ViewModel.SelectedCanvasElement = element;
            ViewModel.DeleteElementCommand.Execute(null);
            DrawLines();
        }

        private void MyListBox_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Point p = e.GetCurrentPoint(sender as ListBox).Position;
            ViewModel.PointerPoint = p;
        }
    }
}
