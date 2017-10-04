using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;

namespace UWP_Samples.Controls.CardView.LayoutManagement {
  public static class DeviceInfo {
    public static bool IsMobile() {
      return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
    }
  }
}
