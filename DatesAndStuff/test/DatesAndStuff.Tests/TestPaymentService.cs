// Updated TestPaymentService implementation
using DatesAndStuff;

internal class TestPaymentService : IPaymentService
{
    uint startCallCount = 0;
    uint specifyCallCount = 0;
    uint confirmCallCount = 0;
    uint cancelCallCount = 0;
    private double balance = 1000; 

    public void StartPayment()
    {
        if (startCallCount != 0 || specifyCallCount > 0 || confirmCallCount > 0 || cancelCallCount > 0)
            throw new Exception();
        startCallCount++;
    }

    public void SpecifyAmount(double amount)
    {
        if (startCallCount != 1 || specifyCallCount > 0 || confirmCallCount > 0 || cancelCallCount > 0)
            throw new Exception();
        specifyCallCount++;
        balance -= amount;
    }

    public void ConfirmPayment()
    {
        if (startCallCount != 1 || specifyCallCount != 1 || confirmCallCount > 0 || cancelCallCount > 0)
            throw new Exception();
        confirmCallCount++;
    }

    public void Cancel()
    {
        if (startCallCount != 1 || cancelCallCount > 0 || confirmCallCount > 0)
            throw new Exception();
        cancelCallCount++;
    }

    public double GetBalance()
    {
        return balance;
    }

    public bool SuccessFul()
    {
        return (startCallCount == 1 && specifyCallCount == 1 && confirmCallCount == 1) ||
               (startCallCount == 1 && cancelCallCount == 1);
    }
}