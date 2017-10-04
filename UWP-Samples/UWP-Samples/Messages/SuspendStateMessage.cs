using Windows.ApplicationModel;

namespace UWP_Samples.Messages
{
  public class SuspendStateMessage
  {
    public SuspendStateMessage(SuspendingOperation operation)
    {
      Operation = operation;
    }

    public SuspendingOperation Operation { get; }
  }
}
