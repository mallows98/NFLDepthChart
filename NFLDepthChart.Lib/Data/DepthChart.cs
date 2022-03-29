using System;
using System.Collections.Generic;

namespace NFLDepthChart.Lib
{
    public class DepthChart
    {
        public string Coach { get; set; }
        public DateTime UpdatedOn { get; set; }

        public IList<KeyValuePair<int, DepthChartItem>> Contents { get; set; } =
            new List<KeyValuePair<int, DepthChartItem>>();
    }
}