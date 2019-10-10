﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FaultTreeEditor.Core.Models;
using FaultTreeEditor.Helpers;
using Windows.UI.Xaml.Shapes;

namespace FaultTreeEditor.ViewModels
{
    public class MainViewModel : Observable
    {
        public List<Element> Elements { get; set; }

        private Element selectedElement;
        public Element SelectedElement
        {
            get { return selectedElement; }
            set { Set(ref selectedElement, value); }
        }

        public ObservableCollection<Element> CanvasElements { get; set; } = new ObservableCollection<Element>();

        private Element selectedCanvasElement;
        public Element SelectedCanvasElement
        {
            get { return selectedCanvasElement; }
            set { Set(ref selectedCanvasElement, value); }
        }

        public ObservableCollection<Connection> Connections { get; set; } = new ObservableCollection<Connection>();

        public ICommand AddItemToCanvasCommand { get; set; }
        public ICommand PropertySaveCommand { get; set; }
        public ICommand PropertyDeleteCommand { get; set; }

        public MainViewModel()
        {
            Elements = new List<Element>
            {
                new TopLevelEvent
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.TopLevelEvent,
                },
                new Event
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.Event,
                },
                new BasicEvent
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.BasicEvent,
                },
                new AndGate
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.AndGate,
                },
                new OrGate
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.OrGate,
                },
                new VoteGate
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.VoteGate,
                },
            };

            SelectedElement = Elements[0];

            SelectedCanvasElement = null;

            AddItemToCanvasCommand = new RelayCommand(() =>
            {
                switch (SelectedElement.ElementType)
                {
                    case ElementType.TopLevelEvent:
                        TopLevelEvent addTopLevelEvent = new TopLevelEvent
                        {
                            Title = "Top level event",
                            ElementType = ElementType.TopLevelEvent,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addTopLevelEvent);
                        SelectedCanvasElement = addTopLevelEvent;
                        break;
                    case ElementType.Event:
                        Event addEvent = new Event
                        {
                            Title = "Event",
                            ElementType = ElementType.Event,
                            Children = new ObservableCollection<Element>(),
                            ImageSource= "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addEvent);
                        SelectedCanvasElement = addEvent;
                        break;
                    case ElementType.BasicEvent:
                        BasicEvent addLeafEvent = new BasicEvent
                        {
                            Title = "Basic event",
                            ElementType = ElementType.BasicEvent,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                            Probability = 0.0,
                        };
                        CanvasElements.Add(addLeafEvent);
                        SelectedCanvasElement = addLeafEvent;
                        break;
                    case ElementType.AndGate:
                        AndGate addAndGate = new AndGate
                        {
                            Title = "AND gate",
                            ElementType = ElementType.AndGate,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addAndGate);
                        SelectedCanvasElement = addAndGate;
                        break;
                    case ElementType.OrGate:
                        OrGate addOrGate = new OrGate
                        {
                            Title = "OR gate",
                            ElementType = ElementType.OrGate,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addOrGate);
                        SelectedCanvasElement = addOrGate;
                        break;
                    case ElementType.VoteGate:
                        VoteGate addVoteGate = new VoteGate
                        {
                            Title = "Vote gate",
                            ElementType = ElementType.VoteGate,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addVoteGate);
                        SelectedCanvasElement = addVoteGate;
                        break;
                    default:
                        break;
                }
            });

            PropertySaveCommand = new RelayCommand(() =>
            {
                if (SelectedCanvasElement != null)
                {
                    CanvasElements.Remove(SelectedCanvasElement);
                    CanvasElements.Add(SelectedCanvasElement);
                }
            });

            PropertyDeleteCommand = new RelayCommand(() =>
            {
                CanvasElements.Remove(SelectedCanvasElement);

                var toRemove = new List<Connection>();

                foreach(var v in Connections)
                {
                    if (v.From == SelectedCanvasElement || v.To == SelectedCanvasElement)
                    {
                        toRemove.Add(v);
                    }
                }

                foreach(var v in toRemove)
                {
                    Connections.Remove(v);
                }

                if (CanvasElements.Count > 0)
                {
                    SelectedCanvasElement = CanvasElements[0];
                }
                else
                {
                    SelectedCanvasElement = null;
                }
            });

            ////////////////////////

            Event addEvent1 = new Event
            {
                Title = "Event",
                ElementType = ElementType.Event,
                Children = new ObservableCollection<Element>(),
                ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                Parents = new ObservableCollection<Element>(),
                X = 150,
                Y = 150,
            };
            CanvasElements.Add(addEvent1);

            Event addEvent2 = new Event
            {
                Title = "Event",
                ElementType = ElementType.Event,
                Children = new ObservableCollection<Element>(),
                ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                Parents = new ObservableCollection<Element>(),
                X = 200,
                Y = 200,
            };
            CanvasElements.Add(addEvent2);

            Event addEvent3 = new Event
            {
                Title = "Event",
                ElementType = ElementType.Event,
                Children = new ObservableCollection<Element>(),
                ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                Parents = new ObservableCollection<Element>(),
                X = 250,
                Y = 250,
            };
            CanvasElements.Add(addEvent3);

            Connections.Add(new Connection
            {
                From = addEvent1,
                To = addEvent2
            });

            Connections.Add(new Connection
            {
                From = addEvent2,
                To = addEvent3
            });

            SelectedCanvasElement = CanvasElements[0];
        }
    }
}
