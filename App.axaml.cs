using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using AvaloniaRaspbianLiteDrm.Views;

namespace AvaloniaRaspbianLiteDrm
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
               desktop.MainWindow = new MainWindow();
          
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
                singleView.MainView = new MainSingleView();

            base.OnFrameworkInitializationCompleted();
        }
        /*public override void RegisterServices()
        {
            AvaloniaLocator.CurrentMutable.Bind<IFontManagerImpl>().ToConstant(new CustomFontManagerImpl());
            base.RegisterServices();
        }*/
    }
    

}
