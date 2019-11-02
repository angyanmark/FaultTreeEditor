﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FaultTreeEditor.Core.Models;
using FaultTreeEditor.Helpers;
using Newtonsoft.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Shapes;

namespace FaultTreeEditor.ViewModels
{
    public class MainViewModel : Observable
    {
        #region Field and Properties
        readonly DataPackage dataPackage = new DataPackage();

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

        private ObservableCollection<Element> canvasElements = new ObservableCollection<Element>();
        public ObservableCollection<Element> CanvasElements
        {
            get { return canvasElements; }
            set { Set(ref canvasElements, value); }
        }

        private Element selectedCanvasElement;
        public Element SelectedCanvasElement
        {
            get { return selectedCanvasElement; }
            set { Set(ref selectedCanvasElement, value); }
        }

        public ObservableCollection<Connection> Connections { get; set; } = new ObservableCollection<Connection>();

        private string outputText = "";
        public string OutputText
        {
            get { return outputText; }
            set { Set(ref outputText, value); }
        }

        private bool isAddConnectionToggled;
        public bool IsAddConnectionToggled
        {
            get { return isAddConnectionToggled; }
            set
            {
                if (value)
                {
                    IsRemoveConnectionToggled = false;
                }
                Set(ref isAddConnectionToggled, value);
            }
        }

        private bool isRemoveConnectionToggled;
        public bool IsRemoveConnectionToggled
        {
            get { return isRemoveConnectionToggled; }
            set
            {
                if (value)
                {
                    IsAddConnectionToggled = false;
                }
                Set(ref isRemoveConnectionToggled, value);
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddItemToCanvasCommand { get; set; }
        public RelayCommand SaveElementCommand { get; set; }
        public RelayCommand GenerateOutputCommand { get; set; }
        public RelayCommand ListConnectionsCommand { get; set; }
        public RelayCommand RemoveConnectionsCommand { get; set; }
        public RelayCommand DeleteElementCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand ToGalileoCommand { get; set; }
        public RelayCommand ToJsonCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            InitializeCanvas();
            InitializeCommands();
        }

        private void InitializeCanvas()
        {
            Elements = new List<Element>
            {
                new Event(),
                new BasicEvent(),
                new AndGate(),
                new OrGate(),
                new VoteGate(),
            };
            SelectedElement = Elements[0];

            CanvasElements.Add(new TopLevelEvent
            {
                Title = "top_level_event",
                X = 600,
                Y = 0,
            });
            SelectedCanvasElement = CanvasElements[0];
        }

        private void InitializeCommands()
        {
            AddItemToCanvasCommand = new RelayCommand(() =>
            {
                switch (SelectedElement.DisplayTitle)
                {
                    case "Event":
                        Event addEvent = new Event
                        {
                            Title = "event_" + ++eventCounter,
                        };
                        CanvasElements.Add(addEvent);
                        SelectedCanvasElement = addEvent;
                        break;
                    case "Basic event":
                        BasicEvent addLeafEvent = new BasicEvent
                        {
                            Title = "basic_event_" + ++basicEventCounter,
                        };
                        CanvasElements.Add(addLeafEvent);
                        SelectedCanvasElement = addLeafEvent;
                        break;
                    case "AND gate":
                        AndGate addAndGate = new AndGate
                        {
                            Title = "and_gate_" + ++andGateCounter,
                        };
                        CanvasElements.Add(addAndGate);
                        SelectedCanvasElement = addAndGate;
                        break;
                    case "OR gate":
                        OrGate addOrGate = new OrGate
                        {
                            Title = "or_gate_" + ++orGateCounter,
                        };
                        CanvasElements.Add(addOrGate);
                        SelectedCanvasElement = addOrGate;
                        break;
                    case "Vote gate":
                        VoteGate addVoteGate = new VoteGate
                        {
                            Title = "vote_gate_" + ++voteGateCounter,
                        };
                        CanvasElements.Add(addVoteGate);
                        SelectedCanvasElement = addVoteGate;
                        break;
                    default:
                        break;
                }
            });

            GenerateOutputCommand = new RelayCommand(() =>
            {
                OutputText = GetGalileoString();
            });

            ListConnectionsCommand = new RelayCommand(() =>
            {
                string builder = "";
                foreach (var v in Connections)
                {
                    builder += $"{v.From.Title} -> {v.To.Title}\n";
                }
                if (String.IsNullOrWhiteSpace(builder))
                {
                    OutputText = "No connections...";
                }
                else
                {
                    OutputText = builder;
                }
            });

            DeleteElementCommand = new RelayCommand(() =>
            {
                var tempElement = SelectedCanvasElement;

                if (tempElement.DisplayTitle == "Top level event")
                {
                    return;
                }

                CanvasElements.Remove(tempElement);

                RemoveConnections(tempElement);

                if (CanvasElements.Count > 0)
                {
                    SelectedCanvasElement = CanvasElements[0];
                }
                else
                {
                    SelectedCanvasElement = null;
                }
            });

            RemoveConnectionsCommand = new RelayCommand(() =>
            {
                RemoveConnections(SelectedCanvasElement);
            });

            CopyCommand = new RelayCommand(() =>
            {
                dataPackage.SetText(OutputText);
                Clipboard.SetContent(dataPackage);
            });

            LoadCommand = new RelayCommand(() =>
            {
                
            });

            ToGalileoCommand = new RelayCommand(async () =>
            {
                await SaveToFileAsync(GetGalileoString());
            });

            ToJsonCommand = new RelayCommand(async () =>
            {
                await SaveToFileAsync(GetJsonString());
            });
        }

        public void ResetCounters()
        {
            eventCounter = 0;
            basicEventCounter = 0;
            andGateCounter = 0;
            orGateCounter = 0;
            voteGateCounter = 0;
        }

        private string GetGalileoString()
        {
            string builder = "";
            foreach (var v in CanvasElements)
            {
                builder += v.ToGalileo();
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                return "No output...";
            }
            else
            {
                return builder;
            }
        }

        private string GetJsonString()
        {
            Graph graph = new Graph()
            {
                Elements = new List<Element>(CanvasElements),
                Connections = new List<Connection>(Connections)
            };

            string output = JsonConvert.SerializeObject(graph, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            return output;
        }

        private void RemoveConnections(Element element)
        {
            foreach (var v in CanvasElements)
            {
                v.Children.Remove(element);
                v.Parents.Remove(element);
            }

            var toRemove = new List<Connection>();
            foreach (var v in Connections)
            {
                if (v.From == element || v.To == element)
                {
                    toRemove.Add(v);
                }
            }
            foreach (var v in toRemove)
            {
                Connections.Remove(v);
            }
        }

        private async Task SaveToFileAsync(string content)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Galileo", new List<string>() { ".dft" });
            savePicker.FileTypeChoices.Add("JSON Text", new List<string>() { ".json" });
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "NewFaultTreeDocument";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    // File saved
                }
                else
                {
                    // File couldn't be saved
                }
            }
            else
            {
                // Operation cancelled.
            }
        }
    }
}
