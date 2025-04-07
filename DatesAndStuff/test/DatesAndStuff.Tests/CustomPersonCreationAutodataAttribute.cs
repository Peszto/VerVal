using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Moq;

namespace DatesAndStuff.Tests
{

    internal class SufficientBalancePersonCreationAutodataAttribute : AutoDataAttribute
    {
        public SufficientBalancePersonCreationAutodataAttribute()
            : base(() =>
            {
                var fixture = new Fixture();

                fixture.Customize(new AutoMoqCustomization());
                var paymentSequence = new MockSequence();
                var paymentService = new Mock<IPaymentService>();
                paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
                paymentService.Setup(m => m.GetBalance()).Returns(Person.SubscriptionFee + 100); // > subscription fee
                paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
                paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());
                fixture.Inject(paymentService);

                return fixture;
            })
        { }
    }

    internal class InsufficientBalancePersonCreationAutodataAttribute : AutoDataAttribute
    {
        public InsufficientBalancePersonCreationAutodataAttribute()
            : base(() =>
            {
                var fixture = new Fixture();

                fixture.Customize(new AutoMoqCustomization());
                var paymentSequence = new MockSequence();
                var paymentService = new Mock<IPaymentService>();
                paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
                paymentService.Setup(m => m.GetBalance()).Returns(Person.SubscriptionFee - 100); // < subscription fee
                paymentService.InSequence(paymentSequence).Setup(m => m.Cancel());
                fixture.Inject(paymentService);

                return fixture;
            })
        { }
    }
}
