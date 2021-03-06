﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly int lineEdgeOffsetX = 40;
        private readonly int lineEdgeOffsetY = 26;

        double CanvasWidth;
        double CanvasHeight;
        double ElementWidth;
        double ElementHeight;

        public MainPage()
        {
            InitializeComponent();
            uiSettings.ColorValuesChanged += ColorValuesChangedAsync;
            InitializeLineColor();
        }

        private void InitializeLineColor()
        {
            var defaultTheme = new UISettings();
            var uiThemeColor = defaultTheme.GetColorValue(UIColorType.Background);
            SetLineColor(uiThemeColor);
        }

        private async void ColorValuesChangedAsync(UISettings sender, object args)
        {
            //Color accentColor = sender.GetColorValue(UIColorType.Accent);
            //or
            //Color accentColor = (Color)Resources["SystemAccentColor"];
            var uiThemeColor = sender.GetColorValue(UIColorType.Background);
            
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    SetLineColor(uiThemeColor);
                    DrawLines();
                }
            );
        }

        private void SetLineColor(Color color)
        {
            if (color == Colors.Black)
            {
                Element.ThemeColor = "Dark";
                lineColor = Colors.White;
            }
            else if (color == Colors.White)
            {
                Element.ThemeColor = "Light";
                lineColor = Colors.Black;
            }
            else // some new theme color
            {
                lineColor = Colors.Black;
            }
            SetElementImageSources();
        }

        private void SetElementImageSources()
        {
            foreach (var e in ViewModel.Elements.Union(ViewModel.Project.FaultTree.Elements))
            {
                e.ImageSource = "/Assets/Images/Elements/" + Element.ThemeColor + "/" + e.Source;
            }
        }

        void SP_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Opacity = 0.4;
            Element element = sp.DataContext as Element;
            ViewModel.SelectedCanvasElement = element;

            CanvasWidth = MyListBox.ActualWidth;
            CanvasHeight = MyListBox.ActualHeight;
            ElementWidth = sp.ActualWidth;
            ElementHeight = sp.ActualHeight;
        }

        void SP_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            Element element = sp.DataContext as Element;
            float zf = CanvasScrollViewer.ZoomFactor;

            double newX = element.X + e.Delta.Translation.X / zf;
            double newY = element.Y + e.Delta.Translation.Y / zf;

            if (newX > 0 && newX < (CanvasWidth - ElementWidth))
            {
                element.X = newX;
            }
            if (newY > 0 && newY < (CanvasHeight - ElementHeight))
            {
                element.Y = newY;
            }
            
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
            MainCanvas.Children.Clear();
            foreach(var v in ViewModel.Project.FaultTree.Connections)
            {
                Line line = new Line
                {
                    X1 = lineEdgeOffsetX + v.From.X,
                    X2 = lineEdgeOffsetX + v.To.X,
                    Y1 = lineEdgeOffsetY + v.From.Y,
                    Y2 = lineEdgeOffsetY + v.To.Y,
                    Stroke = new SolidColorBrush(lineColor),
                    StrokeThickness = 1.6,
                    Opacity = 0.4
                };
                Canvas.SetZIndex(line, 1);
                MainCanvas.Children.Add(line);
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
                    ViewModel.Project.FaultTree.Connections.Add(new Connection
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
                foreach (var v in ViewModel.Project.FaultTree.Connections)
                {
                    if((v.From == ViewModel.SelectedCanvasElement && v.To == myListBoxSelectedItem) || (v.From == myListBoxSelectedItem && v.To == ViewModel.SelectedCanvasElement))
                    {
                        toRemove.Add(v);
                    }
                }
                foreach (var v in toRemove)
                {
                    ViewModel.Project.FaultTree.Connections.Remove(v);
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
            ViewModel.Project.FaultTree.Connections.Clear();
            var toRemove = new List<Element>();
            foreach (var v in ViewModel.Project.FaultTree.Elements)
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
                ViewModel.Project.FaultTree.Elements.Remove(v);
            }
            if (ViewModel.Project.FaultTree.Elements.Count > 0)
            {
                ViewModel.SelectedCanvasElement = ViewModel.Project.FaultTree.Elements[0];
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

        private void List_Element_Connections_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element element = (sender as MenuFlyoutItem).DataContext as Element;
            string builder = "";
            foreach(var v in element.Parents)
            {
                builder += $"{v.Title} -> {element.Title}\n";
            }
            foreach (var v in element.Children)
            {
                builder += $"{element.Title} -> {v.Title}\n";
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                ViewModel.OutputText = $"{element.Title} has no connections...";
            }
            else
            {
                ViewModel.OutputText = builder;
            }
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

        private async void From_JSON_MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadFromFileAsync();
            if(ViewModel.Project.FaultTree.Elements.Count > 0)
            {
                ViewModel.SelectedCanvasElement = ViewModel.Project.FaultTree.Elements[0];
            }
            DrawLines();
        }

        private void Element_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.AddItemToCanvasCommand.Execute(ViewModel.PointerPoint);
        }

        private void CanvasScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            ViewModel.ZoomFactor = scrollViewer.ZoomFactor;
        }
    }
}
