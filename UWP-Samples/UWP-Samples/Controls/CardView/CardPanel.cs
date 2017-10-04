using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWP_Samples.Controls.CardView.LayoutManagement;

namespace UWP_Samples.Controls.CardView {
  [TemplatePart(Name = "CardViewCanvas", Type = typeof(CardViewCanvas))]
  public sealed class CardPanel : Control {

    private CardViewCanvas cardViewCanvas;
    public CardPanel() {
      this.DefaultStyleKey = typeof(CardPanel);
    }

    public static readonly DependencyProperty CardsProperty =
      DependencyProperty.Register(nameof(Cards), typeof(ObservableCollection<Card>), typeof(CardPanel), new PropertyMetadata(null, OnCardsChanged));

    private static void OnCardsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var cardPanel = d as CardPanel;
      if (cardPanel?.CardViewCanvas == null) return;
      cardPanel.cardViewCanvas.Cards = (ObservableCollection<Card>)e.NewValue;
    }

    public ObservableCollection<Card> Cards {
      get { return (ObservableCollection<Card>)GetValue(CardsProperty); }
      set { SetValue(CardsProperty, value); }
    }

    public static readonly DependencyProperty ColumnsProperty =
      DependencyProperty.Register(nameof(Columns), typeof(int), typeof(CardPanel), new PropertyMetadata(null, OnColumnsChanged));

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var cardPanel = d as CardPanel;
      CanvasGeometry.Columns = (int)e.NewValue;
      if (cardPanel?.CardViewCanvas == null) return;
      cardPanel.cardViewCanvas.Columns = (int)e.NewValue;
    }

    public int Columns {
      get { return (int)GetValue(ColumnsProperty); }
      set { SetValue(ColumnsProperty, value); }
    }

    public CardViewCanvas CardViewCanvas {
      get { return cardViewCanvas; }
      set {
        if (value == cardViewCanvas) return;
        cardViewCanvas = value;

        if (cardViewCanvas == null) return;
        cardViewCanvas.Cards = Cards;
        cardViewCanvas.Columns = Columns;
      }
    }


    protected override void OnApplyTemplate() {
      base.OnApplyTemplate();
      CardViewCanvas = GetTemplateChild("CardViewCanvas") as CardViewCanvas; ;
    }

    protected override Size MeasureOverride(Size availableSize) {
      CanvasGeometry.Resize(availableSize);
      return base.MeasureOverride(availableSize);
    }
  }
}
