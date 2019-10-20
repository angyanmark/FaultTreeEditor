using System;
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
        private int eventCounter = 0;
        private int basicEventCounter = 0;
        private int andGateCounter = 0;
        private int orGateCounter = 0;
        private int voteGateCounter = 0;

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
                /*new TopLevelEvent
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.TopLevelEvent,
                },*/
                new Event
                {
                    ImageSource = "/Assets/Images/Elements/event.png",
                    ElementType = ElementType.Event,
                },
                new BasicEvent
                {
                    ImageSource = "https://www.w3schools.com/w3css/img_lights.jpg",
                    ElementType = ElementType.BasicEvent,
                },
                new AndGate
                {
                    ImageSource = "/Assets/Images/Elements/andgate.png",
                    ElementType = ElementType.AndGate,
                },
                new OrGate
                {
                    ImageSource = "/Assets/Images/Elements/orgate.png",
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
                    /*case ElementType.TopLevelEvent:
                        TopLevelEvent addTopLevelEvent = new TopLevelEvent
                        {
                            Title = "top_level_event",
                            ElementType = ElementType.TopLevelEvent,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "/Assets/Images/Elements/event.png",
                            Parents = new ObservableCollection<Element>(),
                            X = 150,
                            Y = 150,
                        };
                        CanvasElements.Add(addTopLevelEvent);
                        SelectedCanvasElement = addTopLevelEvent;
                        break;*/
                    case ElementType.Event:
                        Event addEvent = new Event
                        {
                            Title = "event_" + ++eventCounter,
                            ElementType = ElementType.Event,
                            Children = new ObservableCollection<Element>(),
                            ImageSource= "/Assets/Images/Elements/event.png",
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
                            Title = "basic_event_" + ++basicEventCounter,
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
                            Title = "and_gate_" + ++andGateCounter,
                            ElementType = ElementType.AndGate,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "/Assets/Images/Elements/andgate.png",
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
                            Title = "or_gate_" + ++orGateCounter,
                            ElementType = ElementType.OrGate,
                            Children = new ObservableCollection<Element>(),
                            ImageSource = "/Assets/Images/Elements/orgate.png",
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
                            Title = "vote_gate_" + ++voteGateCounter,
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

            TopLevelEvent initialTopLevelEvent = new TopLevelEvent
            {
                Title = "top_level_event",
                ElementType = ElementType.TopLevelEvent,
                Children = new ObservableCollection<Element>(),
                ImageSource = "/Assets/Images/Elements/event.png",
                Parents = new ObservableCollection<Element>(),
                X = 600,
                Y = 0,
            };
            CanvasElements.Add(initialTopLevelEvent);

            SelectedCanvasElement = CanvasElements[0];
        }

        public void ResetCounters()
        {
            eventCounter = 0;
            basicEventCounter = 0;
            andGateCounter = 0;
            orGateCounter = 0;
            voteGateCounter = 0;
        }
    }
}
