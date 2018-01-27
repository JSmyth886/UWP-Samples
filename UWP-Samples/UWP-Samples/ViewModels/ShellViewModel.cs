using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using UWP_Samples.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace UWP_Samples.ViewModels
{
  public class ShellViewModel : Screen, IHandle<ResumeStateMessage>, IHandle<SuspendStateMessage>
  {
    private readonly WinRTContainer _container;
    private readonly IEventAggregator _eventAggregator;
    private INavigationService _navigationService;
    private bool _resume;

    public ShellViewModel(WinRTContainer container, IEventAggregator eventAggregator)
    {
      _container = container;
      _eventAggregator = eventAggregator;
    }

    protected override void OnActivate()
    {
      _eventAggregator.SubscribeOnPublishedThread(this);
    }

    protected override void OnDeactivate(bool close)
    {
      _eventAggregator.Unsubscribe(this);
    }

    public void SetupNavigationService(Frame frame)
    {
      _navigationService = _container.RegisterNavigationService(frame);

      if (_resume)
        _navigationService.ResumeState();
    }

    public void ShowEmpty()
    {
      _navigationService.For<EmptyViewModel>().Navigate();
    }

    public void ShowAdaptive() {
      _navigationService.For<AdaptiveViewModel>().Navigate();
    }

    public void ShowJSON()
    {
      _navigationService.For<JSONViewModel>().Navigate();
    }

        public Task HandleAsync(ResumeStateMessage message, CancellationToken cancellationToken)
    {
      _resume = true;
      return Task.CompletedTask;
    }

    public Task HandleAsync(SuspendStateMessage message, CancellationToken cancellationToken)
    {
      _navigationService.SuspendState();
      return Task.CompletedTask;
    }
  }
}
