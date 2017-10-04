using System.ComponentModel;
using System.Runtime.CompilerServices;
using UWP_Samples.Annotations;

namespace UWP_Samples.Controls.CardView {
  public class Card : INotifyPropertyChanged {

    private string title;
    private string description;
    private int column;

    public Card(string title, string desc) {
      Title = title;
      Description = desc;
    }

    public string Description {
      get { return description; }
      set {
        if (value.Equals(description)) return;
        description = value;
        OnCardChanged(nameof(Description));
      }
    }

    public string Title {
      get { return title; }
      set {
        if (value.Equals(title)) return;
        title = value;
        OnCardChanged(nameof(Title));
      }
    }

    public int Column {
      get { return column; }
      set {
        if (value.Equals(column)) return;
        column = value;
        OnCardChanged(nameof(Column));
      }
    }

    public int ColumnSpan { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnCardChanged([CallerMemberName] string propertyName = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
