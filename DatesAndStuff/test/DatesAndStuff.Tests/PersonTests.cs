using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
    Person sut;

    [SetUp]
    public void Setup()
    {
        this.sut = PersonFactory.CreateTestPerson();
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
            task.IsFaulted.Should().BeTrue();
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
            sut.Salary.Should().Be(expectedSalary);
        }

        [Test]
        public void IncreaseSalary_SmallerThanMinusTenPerc_ShouldFail()
        {
            // Arrange
            double decreasePercent = -11; // More than 10% decrease

            // Act & Assert
            Action act = () => sut.IncreaseSalary(decreasePercent);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        // [Test]
        // [CustomPersonCreationAutodataAttribute]
        // public void IncreaseSalary_ReasonableValue_ShouldModifySalary(Person sut, double salaryIncreasePercentage)
        // {
        //     // Arrange
        //     double initialSalary = sut.Salary;

        //     // Act
        //     sut.IncreaseSalary(salaryIncreasePercentage);

        //     // Assert
        //     sut.Salary.Should().BeApproximately(initialSalary * (100 + salaryIncreasePercentage) / 100, Math.Pow(10, -8), because: "numerical salary calculation might be rounded to conform legal stuff");
        // }

        [TestCase(0)]
        [TestCase(10)] 
        [TestCase(20)] 
        [TestCase(50)]
        [TestCase(-9)] 
        public void IncreaseSalary_ReasonableValue_ShouldModifySalary(double salaryIncreasePercentage)
        {
            // Arrange
            double initialSalary = sut.Salary;
            double expectedSalary = initialSalary * (1 + salaryIncreasePercentage / 100);

            // Act
            sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            sut.Salary.Should().BeApproximately(expectedSalary, 0.00000001, "Salary increase calculation failed");
        }

        
        [TestCase(-10)]
        [TestCase(-50)]
        public void IncreaseSalary_InvalidValues_ShouldThrowException(double salaryIncreasePercentage)
        {
            // Arrange & Act
            Action act = () => sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>(nameof(salaryIncreasePercentage));
        }
    }


    public class ConstructorTest : PersonTests
    {
        [Test]
        public void Constructor_DefaultParams_ShouldBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson();

            // Assert
            sut.CanEatChocolate.Should().BeTrue();
        }

        [Test]
        public void Constructor_DontLikeChocolate_ShouldNotBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson(fp => fp.CanEatChocolate = false);

            // Assert
            sut.CanEatChocolate.Should().BeFalse();
        }
    }


}
