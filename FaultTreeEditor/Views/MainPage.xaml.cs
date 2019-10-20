using System;
using System.Collections.Generic;
using FaultTreeEditor.Core.Models;
using FaultTreeEditor.ViewModels;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FaultTreeEditor.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        void SP_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Opacity = 0.4;
        }

        [Obsolete]
        void SP_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;

            // Set the center point of the transforms.
            sp.RenderTransformOrigin = new Point(ViewModel.SelectedCanvasElement.X, ViewModel.SelectedCanvasElement.Y);

            TranslateTransform myTranslateTransform = new TranslateTransform();

            myTranslateTransform.X = e.Position.X;
            myTranslateTransform.Y = e.Position.Y;

            //myTranslateTransform.X = e.Delta.Translation.X;
            //myTranslateTransform.Y = e.Delta.Translation.Y;

            ViewModel.SelectedCanvasElement.X = e.Position.X;
            ViewModel.SelectedCanvasElement.Y = e.Position.Y;

            sp.RenderTransform = myTranslateTransform;

            xValue.Text = ViewModel.SelectedCanvasElement.X.ToString();
            yValue.Text = ViewModel.SelectedCanvasElement.Y.ToString();

            DarwLines();
        }

        void SP_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Opacity = 1;

            DarwLines();
        }

        void DarwLines()
        {
            Canvas1.Children.Clear();

            int offset = 40;

            foreach(var v in ViewModel.Connections)
            {
                Line line = new Line();
                line.X1 = offset + v.From.X;
                line.X2 = offset + v.To.X;
                line.Y1 = offset + v.From.Y;
                line.Y2 = offset + v.To.Y;
                line.Stroke = new SolidColorBrush(Colors.White);
                line.StrokeThickness = 2;
                Canvas.SetZIndex(line, 1);
                Canvas1.Children.Add(line);
            }
        }

        private void myListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Element myListBoxSelectedItem = myListBox.SelectedItem as Element;
            if (myListBox.SelectedIndex == -1)
                return;

            if ((bool)Add_Connection_Toggle_Button.IsChecked)
            {
                if(ViewModel.SelectedCanvasElement != myListBoxSelectedItem){
                    ViewModel.Connections.Add(new Connection
                    {
                        From = ViewModel.SelectedCanvasElement,
                        To = myListBoxSelectedItem
                    });
                    ViewModel.SelectedCanvasElement.Children.Add(myListBoxSelectedItem);
                    myListBoxSelectedItem.Parents.Add(ViewModel.SelectedCanvasElement);

                    Add_Connection_Toggle_Button.IsChecked = false;
                    DarwLines();
                }
            }
            else if ((bool)Remove_Connection_Toggle_Button.IsChecked)
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

                Remove_Connection_Toggle_Button.IsChecked = false;
                DarwLines();
            }
            else
            {
                ViewModel.SelectedCanvasElement = myListBoxSelectedItem;
            }
            myListBox.SelectedIndex = -1;
        }

        private void Delete_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.SelectedCanvasElement.ElementType == ElementType.TopLevelEvent)
            {
                return;
            }

            ViewModel.CanvasElements.Remove(ViewModel.SelectedCanvasElement);

            foreach (var v in ViewModel.CanvasElements)
            {
                v.Children.Remove(ViewModel.SelectedCanvasElement);
                v.Parents.Remove(ViewModel.SelectedCanvasElement);
            }

            var toRemove = new List<Connection>();
            foreach (var v in ViewModel.Connections)
            {
                if (v.From == ViewModel.SelectedCanvasElement || v.To == ViewModel.SelectedCanvasElement)
                {
                    toRemove.Add(v);
                }
            }
            foreach (var v in toRemove)
            {
                ViewModel.Connections.Remove(v);
            }

            if (ViewModel.CanvasElements.Count > 0)
            {
                ViewModel.SelectedCanvasElement = ViewModel.CanvasElements[0];
            }
            else
            {
                ViewModel.SelectedCanvasElement = null;
            }

            DarwLines();
        }

        private void Remove_Connection_Toggle_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((bool)Remove_Connection_Toggle_Button.IsChecked)
            {
                Add_Connection_Toggle_Button.IsChecked = false;
            }
        }

        private void Add_Connection_Toggle_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((bool)Add_Connection_Toggle_Button.IsChecked)
            {
                Remove_Connection_Toggle_Button.IsChecked = false;
            }
        }

        private void Generate_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string builder = "";
            foreach (var v in ViewModel.CanvasElements)
            {
                builder += v;
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                Output_TextBox.Text = "No output...";
            }
            else
            {
                Output_TextBox.Text = builder;
            }
        }

        private void List_Connections_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string builder = "";
            foreach (var v in ViewModel.Connections)
            {
                builder += v.From.Title + " -> " + v.To.Title + "\n";
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                Output_TextBox.Text = "No connections...";
            }
            else
            {
                Output_TextBox.Text = builder;
            }
        }

        private void Clear_Canvas_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Connections.Clear();
            var toRemove = new List<Element>();
            foreach (var v in ViewModel.CanvasElements)
            {
                if(v.ElementType != ElementType.TopLevelEvent)
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
            DarwLines();
        }

        private void Load_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Save_Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
