namespace UWP_Samples.Controls.CardView {
  interface IArrangeable {
    double Arrange(double columnOffsetX, double columnOffsetY);
    int ColumnSpan { get; set; }
    int Column { get; set; }
  }
}
