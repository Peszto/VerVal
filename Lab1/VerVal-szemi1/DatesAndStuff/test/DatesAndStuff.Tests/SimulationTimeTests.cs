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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
            }

            [Test]
            // Method_Should_Then
            // code kozelibb
            // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
            public void SimulationTime_Should_AllowSubtraction()
            {
                throw new NotImplementedException();
            }
        }
        
        public class SubtractionTests
        {
            [Test]
            // simulation difference timespane and datetimetimespan is the same
            public void TwoSimulationTimes_Subtracting_ProduceCorrectTimespan()
            {
                throw new NotImplementedException();
            }
        }
        
        public class MillisecondTests
        {
            [Test]
            // millisecond representation works
            public void SimulationTime_Should_SupportMillisecondRepresentation()
            {
                //var t1 = SimulationTime.MinValue.AddMilliseconds(10);
                throw new NotImplementedException();
            }

            [Test]
            // next millisec calculation works
            public void SimulationTime_IncrementingByMillisecond_UpdatesCorrectly()
            {
                //Assert.AreEqual(t1.TotalMilliseconds + 1, t1.NextMillisec.TotalMilliseconds);
                throw new NotImplementedException();
            }

            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void SimulationTime_Should_AllowAddingMilliseconds()
            {
                throw new NotImplementedException();
            }
        }
        
        public class TimeManipulationTests
        {
            [Test]
            // the same as before just with seconds
            public void SimulationTime_AddingSeconds_ShiftsTimeCorrectly()
            {
                throw new NotImplementedException();
            }

            [Test]
            // same as before just with timespan
            public void SimulationTime_Should_SupportTimeSpanAddition()
            {
                throw new NotImplementedException();
            }
        }
        
        public class StringRepresentationTests
        {
            [Test]
            // check string representation given by ToString
            public void SimulationTime_ConvertingToString_ProducesCorrectRepresentation()
            {
                throw new NotImplementedException();
            }
        }
    }
}