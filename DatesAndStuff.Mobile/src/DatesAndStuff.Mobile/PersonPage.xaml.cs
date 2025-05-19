namespace DatesAndStuff.Mobile;

public partial class PersonPage : ContentPage
{
    public PersonPage()
    {
        InitializeComponent();

        this.BindingContext = new PersonViewModel();
    }
}