namespace DatesAndStuff.Mobile;

public partial class CounterPage : ContentPage
{
    int count = 0;

    public CounterPage()
	{
        InitializeComponent();
	}

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        CurrentCountLabel.Text = $"Current count: {count}";

        SemanticScreenReader.Announce(CurrentCountLabel.Text);
    }
}