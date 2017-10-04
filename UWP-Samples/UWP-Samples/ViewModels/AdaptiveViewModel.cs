using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using UWP_Samples.Controls.CardView;

namespace UWP_Samples.ViewModels {
  public class AdaptiveViewModel : Screen {
    private string Lorem =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc blandit turpis ullamcorper elementum feugiat." +
        " Nulla eu lacus vitae sapien convallis molestie sed vel odio. Morbi tempor velit consectetur, pellentesque est ut, semper velit." +
        " Nam id aliquam mi, eu accumsan erat. Etiam facilisis ante arcu, at volutpat velit sodales id. Proin id sollicitudin nulla, non cursus erat. " +
        "Suspendisse potenti. Vestibulum pretium convallis odio. Curabitur sed placerat lorem." +
        " Nam eget ipsum dictum, gravida sapien finibus, viverra odio. Sed eget odio elit. Nulla facilisi.";

    public AdaptiveViewModel() {
      Items = new ObservableCollection<Card>();

      //var card1 = new Card("Hello", Lorem) {
      //  Column = 0,
      //  ColumnSpan = 2
      //};

      //var card2 = new Card("World", Lorem) {
      //  Column = 2,
      //  ColumnSpan = 1
      //};
      //var card3 = new Card("NEW CARD", "ALL CAPS DESCRIPTION") {
      //  Column = 0,
      //  ColumnSpan = 1
      //};
      //var card4 = new Card("NEW CARD", Lorem) {
      //  Column = 1,
      //  ColumnSpan = 2
      //};
      //var card5 = new Card("NEW CARD", Lorem) {
      //  Column = 0,
      //  ColumnSpan = 2
      //};
      //var card6 = new Card("FAR RIGHT", Lorem) {
      //  Column = 2,
      //  ColumnSpan = 1
      //};

      //Items.Add(card1);
      //Items.Add(card2);
      //Items.Add(card3);
      //Items.Add(card4);
      //Items.Add(card5);
      //Items.Add(card6);

      var random = new Random();
      for (int i = 0; i < 1000; i++) {
        var column = random.Next(3);
        var columnSpan = random.Next(1, 3);
        var card = new Card(Lorem, Lorem) {
          Column = column,
          ColumnSpan = columnSpan == 2 ? (column == 2 ? 1 : 2) : columnSpan
        };
        Items.Add(card);
      }
    }

    public ObservableCollection<Card> Items { get; set; }
  }
}

