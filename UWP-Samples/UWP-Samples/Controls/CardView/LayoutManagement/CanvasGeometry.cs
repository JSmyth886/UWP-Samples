using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace UWP_Samples.Controls.CardView.LayoutManagement {
  public static class CanvasGeometry {
    public static readonly double ColumnPadding = 10;
    public static int Columns = 3;
    public static double ColumnWidth;
    public static void Resize(Size finalSize) {
      ColumnWidth = (finalSize.Width - ColumnPadding * (Columns + 1)) / Columns;
    }
  }
}
