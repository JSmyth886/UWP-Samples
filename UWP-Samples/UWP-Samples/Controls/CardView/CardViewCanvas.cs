using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using UWP_Samples.Controls.CardView.LayoutManagement;

namespace UWP_Samples.Controls.CardView {
  public class CardViewCanvas : Canvas {
    public CardViewCanvas() {
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
    }

    private double[] columnOffsetX;
    private double[] columnOffsetY;

    private void Initialize() {
      //Initialize happens when Columns is set
      Children.Clear();
      InitializeColumnOffsets();
      UpdateCards();
    }

    private void InitializeColumnOffsets() {
      columnOffsetX = new double[Columns];
      columnOffsetY = new double[Columns];

      for (int i = 0; i < Columns; i++) {
        columnOffsetY[i] = CanvasGeometry.ColumnPadding;
        SetColumnOffsetX(i);
      }
    }

    private void UpdateColumnOffsets() {
      for (int i = 0; i < Columns; i++) {
        //Remember to take into account items with ColumnSpan larger than 1
        columnOffsetY[i] = CanvasGeometry.ColumnPadding;
        //This needs addressed, should try and use SetColumnOffsetY here
        //SetColumnOffsetY(i);
        SetColumnOffsetX(i);
      }
    }

    private void SetColumnOffsetX(int column) {
      if (column == 0) {
        columnOffsetX[column] = CanvasGeometry.ColumnPadding;
        return;
      }

      columnOffsetX[column] = CanvasGeometry.ColumnPadding * (column + 1) + CanvasGeometry.ColumnWidth * column;
    }

    private void SetColumnOffsetY(int column, int? columnSpan = null, double? itemHeight = null) {
      if (columnOffsetY[column].Equals(0))
        columnOffsetY[column] = CanvasGeometry.ColumnPadding;

      if (columnSpan == null || itemHeight == null)
        return;

      columnOffsetY[column] = columnOffsetY[column] + (double)itemHeight + CanvasGeometry.ColumnPadding;

      if (!(columnSpan > 1)) return;
      columnOffsetY[column + (int)columnSpan -1] = columnOffsetY[column + (int)columnSpan -1] + (double)itemHeight + CanvasGeometry.ColumnPadding;
      if (columnOffsetY[column] < columnOffsetY[column + (int) columnSpan - 1])
        columnOffsetY[column] = columnOffsetY[column + (int) columnSpan - 1];
      else
        columnOffsetY[column + (int) columnSpan - 1] = columnOffsetY[column];
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {

    }

    private void OnUnloaded(object sender, RoutedEventArgs e) {
      RemoveAllCardViews();
    }

    //Method to create and resize Columns
    //Set each initial column padding for Top and Left
    //As Items get added to the view update the Top and Left values
    //Ensure ColumnSpan is taken into account

    protected override Size ArrangeOverride(Size finalSize) {
      foreach (var child in Children) {
        var resizable = child as IArrangeable;
        if (resizable == null)
          continue;
       
        var useColumnOffset =(columnOffsetY[resizable.Column] < columnOffsetY[resizable.Column + resizable.ColumnSpan - 1] ? columnOffsetY[resizable.Column + resizable.ColumnSpan - 1] : columnOffsetY[resizable.Column]);
        //May need to update original columns offset
        var itemHeight = resizable.Arrange(columnOffsetX[resizable.Column], useColumnOffset);
        SetColumnOffsetY(resizable.Column, resizable.ColumnSpan, itemHeight);
      }
      return finalSize;
    }

    private double GetMaxOffset {
      get { return columnOffsetY.Max(); }
    }
    
    protected override Size MeasureOverride(Size availableSize) {
      var desiredSize = LimitUnboundedSize(availableSize);
      //var desiredSize = new Size(availableSize.Width, availableSize.Height);
      UpdateColumnOffsets();
      foreach (var child in Children) {
        child.Measure(desiredSize);
      }
      return desiredSize;
    }

    //Calculate the Height needed for the Canvas (Window.Current.Bounds.Height > GetMaxOffset ? Window.Current.Bounds.Height : GetMaxOffset)
    private Size LimitUnboundedSize(Size input) {
      if (Double.IsInfinity(input.Height))
      {
        var scrollViewer = this.FindAscendant<ScrollViewer>();
        var contentHeight = scrollViewer.ViewportHeight.Equals(0)
          ?  Window.Current.Bounds.Height : scrollViewer.ViewportHeight;
        input.Height = (contentHeight >= GetMaxOffset ? contentHeight : GetMaxOffset);
      }
      return input;
    }

    private ObservableCollection<Card> cards;
    public ObservableCollection<Card> Cards {
      get { return cards; }
      set {
        if (cards != null) {
          cards.CollectionChanged -= OnCardsCollectionChanged;
          foreach (var card in cards) {
            card.PropertyChanged -= OnCardChanged;
          }
        }

        cards = value;

        UpdateCards();

        if (cards == null) return;
        cards.CollectionChanged += OnCardsCollectionChanged;
        foreach (var card in cards) {
          card.PropertyChanged += OnCardChanged;
        }
      }
    }

    private int columns = 1;

    public int Columns {
      get { return columns; }
      set {
        if (columns.Equals(value)) return;
        columns = value == 0 ? 1 : value;
        Initialize();
      }
    }

    private void OnCardChanged(object sender, PropertyChangedEventArgs e) {
      var card = sender as Card;
      if (card == null) return;
      //Do Something
    }

    private void OnCardsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
      switch (notifyCollectionChangedEventArgs.Action) {
        case NotifyCollectionChangedAction.Add:
          foreach (Card card in notifyCollectionChangedEventArgs.NewItems) {
            AddCardView(card);
            card.PropertyChanged += OnCardChanged;
          }
          RefreshCardViews();
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (Card card in notifyCollectionChangedEventArgs.OldItems) {
            card.PropertyChanged -= OnCardChanged;
            RemoveCardView(card);
          }
          RefreshCardViews();
          break;
        case NotifyCollectionChangedAction.Reset:
          if (notifyCollectionChangedEventArgs.OldItems == null) return;
          foreach (Card card in notifyCollectionChangedEventArgs.OldItems) {
            card.PropertyChanged -= OnCardChanged;
          }

          UpdateCards();
          break;
      }
    }

    private void AddCardView(Card card) {
      var cardView = new CardView();
      cardView.Title = card.Title;
      cardView.Description = card.Description;
      cardView.Column = card.Column;
      cardView.ColumnSpan = card.ColumnSpan;
      Children.Add(cardView);
    }

    private void RemoveCardView(Card card) {
    }

    private void AddAllCardViews() {
      if (Cards == null) return;
      foreach (var card in Cards) {
        AddCardView(card);
      }
    }

    private void RemoveAllCardViews() {
      if (Cards == null) return;
      if (Cards.Count.Equals(0)) return;
      Children.Clear();
      Cards.Clear();
    }

    private void RefreshCardViews() {
    }

    private void UpdateCards() {
      AddAllCardViews();
    }
  }
}
