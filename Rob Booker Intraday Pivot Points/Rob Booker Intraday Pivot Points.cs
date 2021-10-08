using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RobBookerIntradayPivotPoints : Indicator
    {
        private Bars _shortTermBars, _mediumTermBars, _longTermBars;

        private Color _shotTermColor, _mediumTermColor, _longTermColor;

        [Parameter("TimeFrame", DefaultValue = "Hour", Group = "Short Term")]
        public TimeFrame ShortTermTimeFrame { get; set; }

        [Parameter("Color", DefaultValue = "Red", Group = "Short Term")]
        public string ShortTermColor { get; set; }

        [Parameter("Thickness", DefaultValue = 1, Group = "Short Term")]
        public int ShortTermThickness { get; set; }

        [Parameter("Style", DefaultValue = LineStyle.Solid, Group = "Short Term")]
        public LineStyle ShortTermStyle { get; set; }

        [Parameter("TimeFrame", DefaultValue = "Hour4", Group = "Medium Term")]
        public TimeFrame MediumTermTimeFrame { get; set; }

        [Parameter("Color", DefaultValue = "Yellow", Group = "Medium Term")]
        public string MediumTermColor { get; set; }

        [Parameter("Thickness", DefaultValue = 1, Group = "Medium Term")]
        public int MediumTermThickness { get; set; }

        [Parameter("Style", DefaultValue = LineStyle.Solid, Group = "Medium Term")]
        public LineStyle MediumTermStyle { get; set; }

        [Parameter("TimeFrame", DefaultValue = "Hour8", Group = "Long Term")]
        public TimeFrame LongTermTimeFrame { get; set; }

        [Parameter("Color", DefaultValue = "Blue", Group = "Long Term")]
        public string LongTermColor { get; set; }

        [Parameter("Thickness", DefaultValue = 1, Group = "Long Term")]
        public int LongTermThickness { get; set; }

        [Parameter("Style", DefaultValue = LineStyle.Solid, Group = "Long Term")]
        public LineStyle LongTermStyle { get; set; }

        protected override void Initialize()
        {
            _shortTermBars = MarketData.GetBars(ShortTermTimeFrame);
            _mediumTermBars = MarketData.GetBars(MediumTermTimeFrame);
            _longTermBars = MarketData.GetBars(LongTermTimeFrame);

            _shotTermColor = GetColor(ShortTermColor);
            _mediumTermColor = GetColor(MediumTermColor);
            _longTermColor = GetColor(LongTermColor);
        }

        public override void Calculate(int index)
        {
            DrawPivotPoint(ShortTermTimeFrame, _shortTermBars, index, _shotTermColor, ShortTermThickness, ShortTermStyle);
            DrawPivotPoint(MediumTermTimeFrame, _mediumTermBars, index, _mediumTermColor, MediumTermThickness, MediumTermStyle);
            DrawPivotPoint(LongTermTimeFrame, _longTermBars, index, _longTermColor, LongTermThickness, LongTermStyle);
        }

        private void DrawPivotPoint(TimeFrame timeFrame, Bars bars, int index, Color color, int thickness, LineStyle lineStyle)
        {
            var barsIndex = bars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]) - 1;

            var shortTermPivotPoints = GetPivotPoint(bars, barsIndex);

            Chart.DrawTrendLine(GetLineName(barsIndex, timeFrame), bars.OpenTimes[barsIndex + 1], shortTermPivotPoints, Bars.OpenTimes[index], shortTermPivotPoints, color, thickness, lineStyle);
        }

        private double GetPivotPoint(Bars bars, int index)
        {
            return (bars.HighPrices[index] + bars.LowPrices[index] + bars.ClosePrices[index]) / 3;
        }

        private string GetLineName(int index, TimeFrame timeFrame)
        {
            return string.Format("RobBookerIntradayPivotPoints_{0}_{1}", index, timeFrame);
        }

        private Color GetColor(string colorString, int alpha = 255)
        {
            var color = colorString[0] == '#' ? Color.FromHex(colorString) : Color.FromName(colorString);

            return Color.FromArgb(alpha, color);
        }
    }
}