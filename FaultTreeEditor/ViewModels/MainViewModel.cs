﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FaultTreeEditor.Core.Models;
using FaultTreeEditor.Helpers;
using FaultTreeEditor.Services;
using Newtonsoft.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Shapes;

namespace FaultTreeEditor.ViewModels
{
    public class MainViewModel : Observable
    {
        #region Field and Properties
        readonly DataPackage dataPackage = new DataPackage();

        private Point pointerPoint = new Point(150, 150);
        public Point PointerPoint
        {
            get { return pointerPoint; }
            set { Set(ref pointerPoint, value); }
        }

        private int eventCounter = 0;
        private int basicEventCounter = 0;
        private int andGateCounter = 0;
        private int orGateCounter = 0;
        private int voteGateCounter = 0;
        private int priorityAndGateCounter = 0;
        private int priorityOrGateCounter = 0;
        private int warmSpareGateCounter = 0;
        private int functionalDependencyCounter = 0;
        private int probabilisticDependencyCounter = 0;
        private int sequenceEnforcerCounter = 0;

        private Project project = new Project();
        public Project Project
        {
            get { return project; }
            set { Set(ref project, value); }
        }

        public ObservableCollection<Element> Elements { get; set; }

        private Element selectedElement;
        public Element SelectedElement
        {
            get { return selectedElement; }
            set { Set(ref selectedElement, value); }
        }

        private Element selectedCanvasElement;
        public Element SelectedCanvasElement
        {
            get { return selectedCanvasElement; }
            set { Set(ref selectedCanvasElement, value); }
        }

        private string outputText = string.Empty;
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

        public float MinZoomFactor
        {
            get { return 1.0f; }
        }

        public float MaxZoomFactor
        {
            get { return 3.8f; }
        }

        private float zoomFactor = 1.0f;
        public float ZoomFactor
        {
            get { return zoomFactor; }
            set { Set(ref zoomFactor, value); ZoomFactorString = ZoomFactor.ToString(); }
        }

        private string zoomFactorString = "1";
        public string ZoomFactorString
        {
            //get { return String.Format("{0:N2}x", float.Parse(zoomFactorString, CultureInfo.InvariantCulture.NumberFormat)); }
            get { return String.Format("{0:N0}%", 100*float.Parse(zoomFactorString, CultureInfo.InvariantCulture.NumberFormat)); }
            set { Set(ref zoomFactorString, value); }
        }
        #endregion

        #region Commands
        public RelayCommand<Point> AddItemToCanvasCommand { get; set; }
        public RelayCommand GenerateOutputCommand { get; set; }
        public RelayCommand ListConnectionsCommand { get; set; }
        public RelayCommand ShowJSONCommand { get; set; }
        public RelayCommand RemoveConnectionsCommand { get; set; }
        public RelayCommand DeleteElementCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
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
            Elements = new ObservableCollection<Element>
            {
                new Event(),
                new BasicEvent(),
                new AndGate(),
                new OrGate(),
                new VoteGate(),
                new PriorityAndGate(),
                new PriorityOrGate(),
                new WarmSpareGate(),
                new FunctionalDependency(),
                new ProbabilisticDependency(),
                new SequenceEnforcer(),
            };
            SelectedElement = Elements[0];

            Project.FaultTree.Elements.Add(new TopLevelEvent
            {
                Title = "top_level_event",
                X = 400,
                Y = 20,
            });
            SelectedCanvasElement = Project.FaultTree.Elements[0];
        }

        private void InitializeCommands()
        {
            AddItemToCanvasCommand = new RelayCommand<Point>((Point p) =>
            {
                switch (SelectedElement.DisplayTitle)
                {
                    case "Event":
                        Event addEvent = new Event
                        {
                            Title = "event_" + ++eventCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addEvent);
                        SelectedCanvasElement = addEvent;
                        break;
                    case "Basic event":
                        BasicEvent addLeafEvent = new BasicEvent
                        {
                            Title = "basic_event_" + ++basicEventCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addLeafEvent);
                        SelectedCanvasElement = addLeafEvent;
                        break;
                    case "AND gate":
                        AndGate addAndGate = new AndGate
                        {
                            Title = "and_gate_" + ++andGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addAndGate);
                        SelectedCanvasElement = addAndGate;
                        break;
                    case "OR gate":
                        OrGate addOrGate = new OrGate
                        {
                            Title = "or_gate_" + ++orGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addOrGate);
                        SelectedCanvasElement = addOrGate;
                        break;
                    case "Vote gate":
                        VoteGate addVoteGate = new VoteGate
                        {
                            Title = "vote_gate_" + ++voteGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addVoteGate);
                        SelectedCanvasElement = addVoteGate;
                        break;
                    case "Priority AND gate":
                        PriorityAndGate addPriorityAndGate = new PriorityAndGate
                        {
                            Title = "priority_and_gate_" + ++priorityAndGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addPriorityAndGate);
                        SelectedCanvasElement = addPriorityAndGate;
                        break;
                    case "Priority OR gate":
                        PriorityOrGate addPriorityOrGate = new PriorityOrGate
                        {
                            Title = "priority_or_gate_" + ++priorityOrGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addPriorityOrGate);
                        SelectedCanvasElement = addPriorityOrGate;
                        break;
                    case "Warm spare gate":
                        WarmSpareGate addWarmSpareGate = new WarmSpareGate
                        {
                            Title = "warm_spare_gate_" + ++warmSpareGateCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addWarmSpareGate);
                        SelectedCanvasElement = addWarmSpareGate;
                        break;
                    case "Functional dependency":
                        FunctionalDependency addFunctionalDependency = new FunctionalDependency
                        {
                            Title = "functional_dependency_" + ++functionalDependencyCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addFunctionalDependency);
                        SelectedCanvasElement = addFunctionalDependency;
                        break;
                    case "Probabilistic dependency":
                        ProbabilisticDependency addProbabilisticDependency = new ProbabilisticDependency
                        {
                            Title = "probabilistic_dependency_" + ++probabilisticDependencyCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addProbabilisticDependency);
                        SelectedCanvasElement = addProbabilisticDependency;
                        break;
                    case "Sequence enforcer":
                        SequenceEnforcer addSequenceEnforcer = new SequenceEnforcer
                        {
                            Title = "sequence_enforcer_" + ++sequenceEnforcerCounter,
                            X = p.X,
                            Y = p.Y,
                        };
                        Project.FaultTree.Elements.Add(addSequenceEnforcer);
                        SelectedCanvasElement = addSequenceEnforcer;
                        break;
                    default:
                        break;
                }
            });

            GenerateOutputCommand = new RelayCommand(() =>
            {
                OutputText = Project.FaultTree.GetGalileoString();
            });

            ShowJSONCommand = new RelayCommand(() =>
            {
                OutputText = GetJsonString();
            });

            ListConnectionsCommand = new RelayCommand(() =>
            {
                OutputText = Project.FaultTree.ListConnections();
            });

            DeleteElementCommand = new RelayCommand(() =>
            {
                var tempElement = SelectedCanvasElement;

                if (tempElement.DisplayTitle == "Top level event")
                {
                    return;
                }

                Project.FaultTree.RemoveElement(tempElement);

                if (Project.FaultTree.Elements.Count > 0)
                {
                    SelectedCanvasElement = Project.FaultTree.Elements[0];
                }
                else
                {
                    SelectedCanvasElement = null;
                }
            });

            RemoveConnectionsCommand = new RelayCommand(() =>
            {
                Project.FaultTree.RemoveConnections(SelectedCanvasElement);
            });

            CopyCommand = new RelayCommand(() =>
            {
                dataPackage.SetText(OutputText);
                Clipboard.SetContent(dataPackage);
            });

            ClearCommand = new RelayCommand(() =>
            {
                OutputText = string.Empty;
            });

            ToGalileoCommand = new RelayCommand(async () =>
            {
                await SaveToFileAsync(Project.FaultTree.GetGalileoString());
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
            priorityAndGateCounter = 0;
            priorityOrGateCounter = 0;
            warmSpareGateCounter = 0;
            functionalDependencyCounter = 0;
            probabilisticDependencyCounter = 0;
            sequenceEnforcerCounter = 0;
        }

        private string GetJsonString()
        {
            return JsonConvert.SerializeObject(Project, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }

        private async Task SaveToFileAsync(string content)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Galileo", new List<string>() { ".dft" });
            savePicker.FileTypeChoices.Add("JSON Document", new List<string>() { ".json" });
            savePicker.FileTypeChoices.Add("Text Document", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = Project.Title;

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
                FileInfo fileInfo = new FileInfo(file.Path);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    // File saved
                    NotificationService.FileSavedNotification(fileInfo);
                }
                else
                {
                    // File couldn't be saved
                    NotificationService.FileNotSavedNotification(fileInfo);
                }
            }
            else
            {
                // Operation cancelled.
            }
        }

        public async Task LoadFromFileAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".json");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);

                Project project = Newtonsoft.Json.JsonConvert.DeserializeObject<Project>(text, new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                });

                Project = project;
            }
            else
            {
                // Operation cancelled.
            }
        }
    }
}
