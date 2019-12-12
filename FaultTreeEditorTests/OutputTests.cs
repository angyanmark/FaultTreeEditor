using FaultTreeEditor.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace FaultTreeEditorTests
{
    [TestClass]
    public class OutputTests
    {
        [TestMethod]
        public void GalieloStringTest1()
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

            Project project = new Project
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
                        basicEvent1,
                        basicEvent2,
                        orGate2,
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

            string expected = "toplevel top_level_event;" +
                "top_level_event and event_1 event_2;" +
                "basic_event_1 lambda = 0.2 dorm = 0;" +
                "basic_event_2 lambda = 0.1 dorm = 0;" +
                "basic_event_3 lambda = 0.3 dorm = 0;" +
                "basic_event_4 lambda = 0.1 dorm = 0;" +
                "event_1 or basic_event_1 basic_event_2;" +
                "event_2 or basic_event_3 basic_event_4;";

            // Act
            string actual = project.FaultTree.GetGalileoString();

            // Assert
            Assert.AreEqual(expected, actual, "Galileo string is ot correct.");
        }

        /*[TestMethod]
        public void ListConnectionsTest1()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }*/
    }
}
