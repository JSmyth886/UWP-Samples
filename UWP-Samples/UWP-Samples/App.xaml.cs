using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Caliburn.Micro;
using UWP_Samples.Messages;
using UWP_Samples.ViewModels;

namespace UWP_Samples
{
  public sealed partial class App
  {
    private WinRTContainer _container;
    private IEventAggregator _eventAggregator;

    public App()
    {
      InitializeComponent();
    }

    protected override void Configure()
    {
      _container = new WinRTContainer();
      _container.RegisterWinRTServices();

      _container
        .PerRequest<ShellViewModel>()
        .PerRequest<EmptyViewModel>()
        .PerRequest<AdaptiveViewModel>();

      _eventAggregator = _container.GetInstance<IEventAggregator>();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
      DisplayRootViewFor<ShellViewModel>();

      if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
      {
        _eventAggregator.PublishOnUIThread(new ResumeStateMessage());
      }
    }

    protected override void OnSuspending(object sender, SuspendingEventArgs e)
    {
      _eventAggregator.PublishOnUIThread(new SuspendStateMessage(e.SuspendingOperation));
    }

    protected override object GetInstance(Type service, string key)
    {
      return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
      return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
      _container.BuildUp(instance);
    }
  }
}
