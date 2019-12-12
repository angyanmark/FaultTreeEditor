using System.Collections.ObjectModel;
using FaultTreeEditor.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FaultTreeEditorUnitTests
{
    [TestClass]
    public class OutputTests
    {
        Project project;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            TopLevelEvent topLevelEvent = new TopLevelEvent
            {
                Title = "top_level_event"
            };
            AndGate andGate1 = new AndGate
            {
                Title = "and_gate_1"
            };
            Event event1 = new Event
            {
                Title = "event_1"
            };
            Event event2 = new Event
            {
                Title = "event_2"
            };
            OrGate orGate1 = new OrGate
            {
                Title = "or_gate_1"
            };
            OrGate orGate2 = new OrGate
            {
                Title = "or_gate_2"
            };
            BasicEvent basicEvent1 = new BasicEvent
            {
                Title = "basic_event_1",
                Lambda = 0.2
            };
            BasicEvent basicEvent2 = new BasicEvent
            {
                Title = "basic_event_2",
                Lambda = 0.1
            };
            BasicEvent basicEvent3 = new BasicEvent
            {
                Title = "basic_event_3",
                Lambda = 0.3
            };
            BasicEvent basicEvent4 = new BasicEvent
            {
                Title = "basic_event_4",
                Lambda = 0.1
            };

            Connection connection1 = new Connection
            {
                From = topLevelEvent,
                To = andGate1
            };
            Connection connection2 = new Connection
            {
                From = andGate1,
                To = event1
            };
            Connection connection3 = new Connection
            {
                From = andGate1,
                To = event2
            };
            Connection connection4 = new Connection
            {
                From = event1,
                To = orGate1
            };
            Connection connection5 = new Connection
            {
                From = orGate1,
                To = basicEvent1
            };
            Connection connection6 = new Connection
            {
                From = orGate1,
                To = basicEvent2
            };
            Connection connection7 = new Connection
            {
                From = event2,
                To = orGate2
            };
            Connection connection8 = new Connection
            {
                From = orGate2,
                To = basicEvent3
            };
            Connection connection9 = new Connection
            {
                From = orGate2,
                To = basicEvent4
            };

            topLevelEvent.Children.Add(andGate1);
            andGate1.Parents.Add(topLevelEvent);
            andGate1.Children.Add(event1);
            andGate1.Children.Add(event2);

            event1.Parents.Add(andGate1);
            event1.Children.Add(orGate1);
            event2.Parents.Add(andGate1);
            event2.Children.Add(orGate2);

            orGate1.Parents.Add(event1);
            orGate1.Children.Add(basicEvent1);
            orGate1.Children.Add(basicEvent2);
            orGate2.Parents.Add(event2);
            orGate2.Children.Add(basicEvent3);
            orGate2.Children.Add(basicEvent4);

            basicEvent1.Parents.Add(orGate1);
            basicEvent2.Parents.Add(orGate1);
            basicEvent3.Parents.Add(orGate2);
            basicEvent4.Parents.Add(orGate2);

            project = new Project
            {
                Title = "TestProject",
                FaultTree = new FaultTree
                {
                    Elements = new ObservableCollection<Element>
                    {
                        topLevelEvent,
                        andGate1,
                        event1,
                        event2,
                        orGate1,
                        orGate2,
                        basicEvent1,
                        basicEvent2,
                        basicEvent3,
                        basicEvent4
                    },
                    Connections = new ObservableCollection<Connection>
                    {
                        connection1,
                        connection2,
                        connection3,
                        connection4,
                        connection5,
                        connection6,
                        connection7,
                        connection8,
                        connection9
                    }
                }
            };
        }

        [TestMethod]
        public void GalieloStringTest1()
        {
            // Arrange
            string expected =
                "toplevel top_level_event;\n" +
                "top_level_event and event_1 event_2;\n" +
                "event_1 or basic_event_1 basic_event_2;\n" +
                "event_2 or basic_event_3 basic_event_4;\n" +
                "basic_event_1 lambda=0.2 dorm=0;\n" +
                "basic_event_2 lambda=0.1 dorm=0;\n" +
                "basic_event_3 lambda=0.3 dorm=0;\n" +
                "basic_event_4 lambda=0.1 dorm=0;\n";

            // Act
            string actual = project.FaultTree.GetGalileoString();

            // Assert
            Assert.AreEqual(expected, actual, "Galileo string is not correct.");
        }

        [TestMethod]
        public void ListConnectionsTest1()
        {
            // Arrange
            string expected =
                "top_level_event -> and_gate_1\n" +
                "and_gate_1 -> event_1\n" +
                "and_gate_1 -> event_2\n" +
                "event_1 -> or_gate_1\n" +
                "or_gate_1 -> basic_event_1\n" +
                "or_gate_1 -> basic_event_2\n" +
                "event_2 -> or_gate_2\n" +
                "or_gate_2 -> basic_event_3\n" +
                "or_gate_2 -> basic_event_4\n";

            // Act
            string actual = project.FaultTree.ListConnections();

            // Assert
            Assert.AreEqual(expected, actual, "Listed connections are not correct.");
        }
    }
}
