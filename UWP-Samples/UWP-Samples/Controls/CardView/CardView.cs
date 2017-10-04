using System;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWP_Samples.Controls.CardView.LayoutManagement;

namespace UWP_Samples.Controls.CardView {
  public sealed class CardView : Control, IArrangeable {

    public CardView() {
      this.DefaultStyleKey = typeof(CardView);
    }

    public static readonly DependencyProperty ItemActionCommandProperty =
      DependencyProperty.Register(nameof(ItemActionCommand), typeof(ICommand), typeof(Card), new PropertyMetadata(null));

    public ICommand ItemActionCommand {
      get { return (ICommand)GetValue(ItemActionCommandProperty); }
      set { SetValue(ItemActionCommandProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
      DependencyProperty.Register(nameof(Title), typeof(string), typeof(Card), new PropertyMetadata(null));

    public string Title {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty DescriptionProperty =
      DependencyProperty.Register(nameof(Description), typeof(string), typeof(Card), new PropertyMetadata(null));

    public string Description {
      get { return (string)GetValue(DescriptionProperty); }
      set { SetValue(DescriptionProperty, value); }
    }

    protected override Size MeasureOverride(Size availableSize) {
      double columnSpan = ColumnSpan;
      var size = base.MeasureOverride(new Size(
        columnSpan * CanvasGeometry.ColumnWidth + (columnSpan > 1
          ? columnSpan * CanvasGeometry.ColumnPadding
          : 0), availableSize.Height));
      return new Size(
        columnSpan * CanvasGeometry.ColumnWidth + (columnSpan > 1 
          ? (columnSpan -1) * CanvasGeometry.ColumnPadding
          : 0), size.Height);
    }

    public double Arrange(double columnOffsetX, double columnOffsetY) {
      var anchorPoint = new Point(columnOffsetX, columnOffsetY);
      Arrange(new Rect(anchorPoint, DesiredSize));

      //update the canvas
      Canvas.SetLeft(this, anchorPoint.X);
      Canvas.SetTop(this, anchorPoint.Y);

      return ActualHeight;
    }

    public int ColumnSpan { get; set; }
    public int Column { get; set; }
  }

}
