using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
    Person sut;

    [SetUp]
    public void Setup()
    {
        this.sut = new Person("Test Pista", 54);
    }

    public class MarriageTests : PersonTests
    {
        [Test]
        public void GotMerried_First_NameShouldChange()
        {
            // Arrange
            string newName = "Test-Eleso Pista";
            double salaryBeforeMarriage = sut.Salary;
            var beforeChanges = Person.Clone(sut);

            // Act
            sut.GotMarried(newName);

            // Assert
            Assert.That(sut.Name, Is.EqualTo(newName)); // act = exp

            sut.Name.Should().Be(newName);
            sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));

            //sut.Salary.Should().Be(salaryBeforeMarriage);

            //Assert.AreEqual(newName, sut.Name); // = (exp, act)
            //Assert.AreEqual(salaryBeforeMarriage, sut.Salary);
        }

        [Test]
        public void GotMerried_Second_ShouldFail()
        {
            // Arrange
            string newName = "Test-Eleso-Felallo Pista";
            sut.GotMarried("");

            // Act
            var task = Task.Run(() => sut.GotMarried(""));
            try { task.Wait(); } catch { }

            // Assert
            Assert.IsTrue(task.IsFaulted);
        }
    }


    public class SalaryTests : PersonTests
    {
        [Test]
        public void IncreaseSalary_PositiveIncrease_ShouldIncrease()
        {
            // Arrange
            double initalSalary = sut.Salary;
            double increasePercent = 10;
            double expectedSalary = initalSalary * 1.1;

            // Act
            sut.IncreaseSalary(increasePercent);

            // Assert
            Assert.AreEqual(expectedSalary, sut.Salary);
        }

        [Test]
        public void IncreaseSalary_ZeroPercentIncrease_ShouldNotChange()
        {
            // Arrange
            double initialSalary = sut.Salary;

            // Act
            sut.IncreaseSalary(0);

            // Assert
            Assert.AreEqual(initialSalary, sut.Salary);
        }

        [Test]
        public void IncreaseSalary_NegativeIncrease_ShouldDecrease()
        {
            // Arrange
            double initialSalary = sut.Salary;
            double decreasePercent = -5; // 5% decrease
            double expectedSalary = initialSalary * 0.95;

            // Act
            sut.IncreaseSalary(decreasePercent);

            // Assert
            Assert.AreEqual(expectedSalary, sut.Salary);
        }

        [Test]
        public void IncreaseSalary_SmallerThanMinusTenPerc_ShouldFail()
        {
            // Arrange
            double decreasePercent = -11; // More than 10% decrease

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.IncreaseSalary(decreasePercent));
        }
    }
}