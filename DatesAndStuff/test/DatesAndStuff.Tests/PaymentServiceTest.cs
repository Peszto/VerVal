using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DatesAndStuff.Tests
{
    internal class PaymentServiceTest
    {
        [Test]
        public void TestPaymentService_ManualMock_SufficientBalance()
        {
            int balance= 600;
            // Arrange
            var testPaymentService = new TestPaymentService(600); //balance > 500
            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                testPaymentService,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeTrue();
            testPaymentService.StartCalled.Should().BeTrue();
            testPaymentService.SpecifyCalled.Should().BeTrue();
            testPaymentService.ConfirmCalled.Should().BeTrue();
            testPaymentService.CancelCalled.Should().BeFalse();
        }


        [Test]
        public void TestPaymentService_ManualMock_InsufficientBalance()
        {
            // Arrange
            var testPaymentService = new TestPaymentService(400); //balance < 500
            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                testPaymentService,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeFalse();
            testPaymentService.StartCalled.Should().BeTrue();
            testPaymentService.SpecifyCalled.Should().BeFalse();
            testPaymentService.ConfirmCalled.Should().BeFalse();
            testPaymentService.CancelCalled.Should().BeTrue();
        }

        [Test]
        public void TestPaymentService_Mock_SufficientBalance()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.Setup(m => m.GetBalance()).Returns(600);
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                 paymentServiceMock
             ,
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
            paymentService.Verify(m => m.Cancel(), Times.Never);
        }

        [Test]
        public void TestPaymentService_Mock_InsufficientBalance()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.Setup(m => m.GetBalance()).Returns(400);
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                 paymentServiceMock
             ,
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeFalse();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(It.IsAny<double>()), Times.Never);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Never);
            paymentService.Verify(m => m.Cancel(), Times.Once);
        }

        [Test]
        [SufficientBalancePersonCreationAutodataAttribute]
        public void TestPaymentService_MockWithAutodata_SufficientBalance(Person sut, Mock<IPaymentService> paymentService)
        {
            // Arrange
            
            // Act
            bool result = sut.PerformSubscriptionPayment();
            
            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
            paymentService.Verify(m => m.Cancel(), Times.Never);
        }
        
        [Test]
        [InsufficientBalancePersonCreationAutodataAttribute]
        public void TestPaymentService_MockWithAutodata_InsufficientBalance(Person sut, Mock<IPaymentService> paymentService)
        {
            // Arrange
            
            // Act
            bool result = sut.PerformSubscriptionPayment();
            
            // Assert
            result.Should().BeFalse();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(It.IsAny<double>()), Times.Never);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Never);
            paymentService.Verify(m => m.Cancel(), Times.Once);
        }
    }
}
