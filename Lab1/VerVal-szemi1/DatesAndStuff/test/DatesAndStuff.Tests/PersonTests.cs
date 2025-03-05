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

    [Test]
    public void IncreaseSalary_PositiveIncrease_ShouldIncrease()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void IncreaseSalary_ZeroPercentIncrease_ShouldNotChange()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void IncreaseSalary_NegativeIncrease_ShouldDecrease()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void IncreaseSalary_SmallerThanMinusTenPerc_ShouldFail()
    {
        throw new NotImplementedException();
    }
}