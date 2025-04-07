using FluentAssertions;

namespace DatesAndStuff.Tests
{
    public sealed class SimulationTimeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetupStuff()
        {
            //
        }

        [SetUp]
        public void Setup()
        {
            // minden teszt felteheti, hogy előtte lefutott ez
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        public class ConstructorTests
        {
            [Test]
            // Default time is not current time.
            public void DefaultConstructor_CreatesTimeNotEqualToCurrentTime()
            {
                // Arrange
                var sut = new SimulationTime();
                // Act
                var result = sut.ToAbsoluteDateTime();
                // Assert
                result.Should().NotBe(DateTime.Now);
            }
        }

        public class ComparisonTests
        {
            [Test]
            // equal
            // not equal
            // <
            // >
            // <= different
            // >= different
            // <= same
            // >= same
            // max
            // min
            public void SimulationTime_Should_SupportComparisonOperators()
            {
                // Arrange
                var time1 = new SimulationTime(new DateTime(2023, 1, 1, 12, 0, 0));
                var time2 = new SimulationTime(new DateTime(2023, 1, 1, 12, 0, 0));
                var time3 = new SimulationTime(new DateTime(2023, 1, 1, 13, 0, 0));
                var time4 = new SimulationTime(new DateTime(2023, 1, 1, 11, 0, 0));

                // Act & Assert
                Assert.AreEqual(time1, time2, "Expected time1 to be equal to time2");
                Assert.AreNotEqual(time1, time3, "Expected time1 to not be equal to time3");
                Assert.IsTrue(time1 < time3, "Expected time1 to be less than time3");
                Assert.IsTrue(time3 > time1, "Expected time3 to be greater than time1");
                Assert.IsTrue(time4 <= time1, "Expected time4 to be less than or equal to time1");
                Assert.IsTrue(time3 >= time1, "Expected time3 to be greater than or equal to time1");
                Assert.IsTrue(time1 <= time2, "Expected time1 to be less than or equal to time2");
                Assert.IsTrue(time1 >= time2, "Expected time1 to be greater than or equal to time2");
                Assert.AreEqual(SimulationTime.MaxValue, SimulationTime.MaxValue, "Expected MaxValue to be equal to itself");
                Assert.AreEqual(SimulationTime.MinValue, SimulationTime.MinValue, "Expected MinValue to be equal to itself");
            }
        }

        public class TimeSpanArithmeticTests
        {
            [Test]
            // TimeSpanArithmetic
            // add
            // substract
            // Given_When_Then
            // UserSignedIn_OrderSent_OrderIsRegistered
            // DBB, specflow, cucumber, gherkin
            public void BaseDateTime_AddingTimeSpan_ShiftsSimulationTime()
            {
                // Arrange
                DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                SimulationTime sut = new SimulationTime(baseDate);
                var ts = TimeSpan.FromMilliseconds(4544313);
                // Act
                var result = sut + ts;
                // Assert
                var expectedDateTime = baseDate + ts;
                result.ToAbsoluteDateTime().Should().Be(expectedDateTime);
            }

            [Test]
            // Method_Should_Then
            // code kozelibb
            // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
            public void SimulationTime_Should_AllowSubtraction()
            {
                // Arrange
                var simulationTime1 = SimulationTime.MinValue.AddMilliseconds(500);
                var simulationTime2 = SimulationTime.MinValue.AddMilliseconds(1000);

                // Act
                var resultTimeSpan = simulationTime2 - simulationTime1;

                // Assert
                resultTimeSpan.Should().Be(TimeSpan.FromMilliseconds(500));
            }
        }

        public class SubtractionTests
        {
            [Test]
            // simulation difference timespane and datetimetimespan is the same
            public void TwoSimulationTimes_Subtracting_ProduceCorrectTimespan()
            {
                // Arrange
                var simulationTime1 = SimulationTime.MinValue.AddMilliseconds(500);
                var simulationTime2 = SimulationTime.MinValue.AddMilliseconds(1000);

                // Act
                var resultTimeSpan = simulationTime2 - simulationTime1;

                // Assert
                resultTimeSpan.Should().Be(TimeSpan.FromMilliseconds(500));
            }
        }

        public class MillisecondTests
        {
            [Test]
            // millisecond representation works
            public void SimulationTime_Should_SupportMillisecondRepresentation()
            {
                // Arrange
                var t1 = SimulationTime.MinValue.AddMilliseconds(10);

                // Act
                var expectedTime = SimulationTime.MinValue + TimeSpan.FromMilliseconds(10);

                // Assert
                t1.Should().Be(expectedTime);
            }

            [Test]
            // next millisec calculation works
            public void SimulationTime_IncrementingByMillisecond_UpdatesCorrectly()
            {
                // Arrange
                var t1 = SimulationTime.MinValue;

                // Act
                var t2 = t1.NextMillisec;

                // Assert
                t2.TotalMilliseconds.Should().Be(t1.TotalMilliseconds + 1);
            }

            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void SimulationTime_Should_AllowAddingMilliseconds()
            {
                // Arrange
                var dateTime = DateTime.UtcNow;
                var simulationTime = new SimulationTime(dateTime);
                int millisecondsToAdd = 500;

                // Act
                var newDateTime = dateTime.AddMilliseconds(millisecondsToAdd);
                var newSimulationTime = simulationTime.AddMilliseconds(millisecondsToAdd);

                // Assert
                newSimulationTime.ToAbsoluteDateTime().Should().BeCloseTo(newDateTime, TimeSpan.FromMilliseconds(1));
            }

            [Test]
            public void SimulationTime_PreviousMillisec_DecreasesTotalMillisecondsByOne()
            {
                // Arrange
                var dateTime = DateTime.UtcNow;
                var originalTime = new SimulationTime(dateTime); // Assuming TotalMilliseconds is set to 1000

                // Act
                var previousTime = originalTime.PreviousMillisec;

                // Assert
                previousTime.TotalMilliseconds.Should().Be(originalTime.TotalMilliseconds - 1);
            }

        }

        public class LogicalTickTests
        {
            [Test]
            public void PreviousLogicalTick_ShouldDescrease_LogicalTickByOne()
            {
                // Arrange
                var dateTime = DateTime.UtcNow;
                var originalTime = new SimulationTime(dateTime); // Assuming TotalMilliseconds is set to 1000

                // Act
                var previousLogicalTick = originalTime.PreviousLogicalTick;

                // Assert
                previousLogicalTick.TotalMilliseconds.Should().Be(originalTime.TotalMilliseconds - 1);
            }

            [Test]
            public void NextLogicalTick_ShouldIncrease_LogicalTickByOne()
            {
                // Arrange
                var dateTime = DateTime.UtcNow;
                var originalTime = new SimulationTime(dateTime); // Assuming TotalMilliseconds is set to 1000

                // Act
                var nextLogicalTick = originalTime.NextLogicalTick;

                // Assert
                nextLogicalTick.TotalMilliseconds.Should().Be(originalTime.TotalMilliseconds + 1);
            }

            [Test]
            public void LogicalTick_ShouldBeEqual_WhenSameTime()
            {
                // Arrange
                long rawTicks = 9876543210;
                var time = SimulationTime.FromLogicalTicks(rawTicks);

                // Act & Assert
                time.LogicalTicks.Should().Be(rawTicks);
            }
        }

        public class TimeManipulationTests
        {
            [Test]
            // the same as before just with seconds
            public void SimulationTime_AddingSeconds_ShiftsTimeCorrectly()
            {
                // Arrange
                var simulationTime = SimulationTime.MinValue;
                int secondsToAdd = 5;

                // Act
                var newSimulationTime = simulationTime.AddSeconds(secondsToAdd);

                // Assert
                newSimulationTime.TotalMilliseconds.Should().Be(simulationTime.TotalMilliseconds + (secondsToAdd * 1000));
            }

            [Test]
            // same as before just with timespan
            public void SimulationTime_Should_SupportTimeSpanAddition()
            {
                // Arrange
                var simulationTime = SimulationTime.MinValue;
                var timeSpanToAdd = TimeSpan.FromMinutes(10);

                // Act
                var newSimulationTime = simulationTime.AddTimeSpan(timeSpanToAdd);

                // Assert
                newSimulationTime.TotalMilliseconds.Should().Be((long)(simulationTime.TotalMilliseconds + timeSpanToAdd.TotalMilliseconds));
            }

            [Test]
            public void AddMinutes_ShouldIncreaseSimulationTimeByMinutes()
            {
                // Arrange
                var initialTime = new SimulationTime(new DateTime(2025, 1, 1, 12, 0, 0)); // Jan 1, 2025 12:00:00
                var expectedTime = initialTime.ToAbsoluteDateTime().AddMinutes(15);

                // Act
                var result = initialTime.AddMinutes(15);

                // Assert
                result.ToAbsoluteDateTime().Should().Be(expectedTime);
            }

            [Test]
            public void AddHours_ShouldIncreaseSimulationTimeByHours()
            {
                // Arrange
                var initialTime = new SimulationTime(new DateTime(2025, 1, 1, 12, 0, 0)); // Jan 1, 2025 12:00:00
                var expectedTime = initialTime.ToAbsoluteDateTime().AddHours(5);

                // Act
                var result = initialTime.AddHours(5);

                // Assert
                result.ToAbsoluteDateTime().Should().Be(expectedTime);
            }

            [Test]
            public void AddDays_ShouldIncreaseSimulationTimeByDays()
            {
                // Arrange
                var initialTime = new SimulationTime(new DateTime(2025, 1, 1, 12, 0, 0)); // Jan 1, 2025 12:00:00
                var expectedTime = initialTime.ToAbsoluteDateTime().AddDays(10);

                // Act
                var result = initialTime.AddDays(10);

                // Assert
                result.ToAbsoluteDateTime().Should().Be(expectedTime);
            }

            [Test]
            public void AddTimeSpan_ShouldIncreaseSimulationTimeByTimeSpan()
            {
                // Arrange
                var original = new SimulationTime(new DateTime(2025, 12, 25, 14, 45, 30)); // Dec 25, 2025 14:45:30
                var expected = new DateTime(2025, 12, 25, 0, 0, 0); // Midnight

                // Act
                var result = original.LastMidnight();

                // Assert
                result.ToAbsoluteDateTime().Should().Be(expected);
            }

            [Test]
            public void LastMidnight_Of_Already_Midnight_Time_Should_Not_Change()
            {
                // Arrange
                var midnight = new SimulationTime(new DateTime(2025, 10, 5, 0, 0, 0)); // Already midnight

                // Act
                var result = midnight.LastMidnight();

                // Assert
                result.Should().Be(midnight);
            }
        }

        public class StringRepresentationTests
        {
            [Test]
            // check string representation given by ToString
            public void SimulationTime_ConvertingToString_ProducesCorrectRepresentation()
            {
                // Arrange
                var dateTime = DateTime.Now;
                var simulationTime = new SimulationTime(dateTime);

                // Act
                var stringRepresentation = simulationTime.ToString();

                // Assert
                stringRepresentation.Should().Be(dateTime.ToIsoStringFast());
            }
        }
    }
}