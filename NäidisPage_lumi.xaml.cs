using Microsoft.Maui.Layouts;

namespace TARgv24;

public partial class NäidisPage_lumi : ContentPage
{
    Random random = new Random();
    AbsoluteLayout taust;
    Grid tahvel;

    public NäidisPage_lumi()
    {
        taust = new AbsoluteLayout
        {
            BackgroundColor = Color.FromRgb(10, 100, 100)
        };

        tahvel = new Grid
        {
            BackgroundColor = Colors.Transparent,
            HeightRequest = (int)DeviceDisplay.MainDisplayInfo.Height,
            WidthRequest = (int)DeviceDisplay.MainDisplayInfo.Width
        };

        // Tap (topeltklõps)
        var tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
        tapGesture.Tapped += TapGesture_Tapped;
        tahvel.GestureRecognizers.Add(tapGesture);

        // Pan (lohista ja lase lahti)
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += PanGesture_PanUpdated;
        tahvel.GestureRecognizers.Add(panGesture);

        taust.Children.Add(tahvel);


        //var test = new Image
        //{
        //    Source = "snow.png",
        //    WidthRequest = 100,
        //    HeightRequest = 100
        //};
        //AbsoluteLayout.SetLayoutBounds(test, new Rect(50, 50, 100, 100));
        //AbsoluteLayout.SetLayoutFlags(test, AbsoluteLayoutFlags.None);
        //taust.Children.Add(test);


        Content = taust;
    }

    private void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Completed)
        {
            // PanUpdatedEventArgs does not have GetPosition, so use TotalX and TotalY as relative movement
            // You may want to use the last known position or calculate from tahvel's bounds
            // Here, we use TotalX and TotalY as the drop location relative to tahvel's top-left
            LisaLumi(e.TotalX, e.TotalY);
        }
    }

    private void TapGesture_Tapped(object? sender, TappedEventArgs e)
    {
        var point = e.GetPosition(tahvel);
        if (point == null)
            return;

        LisaLumi(point.Value.X, point.Value.Y);
    }

    private async void LisaLumi(double x, double y)
    {
        var lumi = new Image
        {
            Source = "snow.png",
            HeightRequest = random.Next(20, 100),
            WidthRequest = random.Next(20, 100),
        };

        // Asetame algselt punktile x,y
        AbsoluteLayout.SetLayoutBounds(lumi, new Rect(x, y, lumi.WidthRequest, lumi.HeightRequest));
        AbsoluteLayout.SetLayoutFlags(lumi, AbsoluteLayoutFlags.None);

        taust.Children.Add(lumi);

        // Anname juhusliku "tuule" (x nihke) ja kiiruse
        double targetX = x + random.Next(-50, 50);  // veidi külgsuunas
        double targetY = taust.Height;              // ekraani alaossa
        uint duration = (uint)random.Next(3000, 7000); // kestus millisekundites

        // Anima lumesadu (liigub alla ja küljele)
        await lumi.TranslateTo(targetX - x, targetY - y, duration, Easing.Linear);

        // Kui lumi jõuab alla, eemaldame taustast
        taust.Children.Remove(lumi);
    }
}
