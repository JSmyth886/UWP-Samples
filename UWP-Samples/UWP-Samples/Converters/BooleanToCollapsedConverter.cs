using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWP_Samples.Converters
{
  public class BooleanToCollapsedConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      throw new NotImplementedException();
    }
  }
}
