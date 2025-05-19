
namespace DatesAndStuff.Mobile;

public partial class SimulationTimePage : ContentPage
{
	public SimulationTimePage()
	{
		InitializeComponent();

		TimeLabel.Text = $"Time: {new SimulationTime(DateTime.Now)}";
	}
}