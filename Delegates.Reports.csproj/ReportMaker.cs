using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Reports
{
	public abstract class ReportMaker
	{
		protected abstract IStatisticMaker StatisticMaker { get; }
		protected abstract string Caption { get; }
		protected abstract string BeginList { get; }
		protected abstract string EndList { get; }
		protected abstract string MakeItem(string valueType, string entry);
		public string MakeReport(IEnumerable<Measurement> measurements)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(Caption);
			result.Append(BeginList);
			result.Append(MakeItem("Temperature", StatisticMaker.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", StatisticMaker.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(EndList);
			return result.ToString();
		}
	}

	public class HtmlReportMaker : ReportMaker
	{
		protected override IStatisticMaker StatisticMaker { get; }
		protected override string BeginList => "<ul>";
		protected override string EndList => "</ul>";
		protected override string Caption => $"<h1>{StatisticMaker.Caption}</h1>";

		public HtmlReportMaker(IStatisticMaker statisticMaker)
		{
			StatisticMaker = statisticMaker;
		}

		protected override string MakeItem(string valueType, string entry) => $"<li><b>{valueType}</b>: {entry}";
	}

	public class MarkdownReportMaker : ReportMaker
	{
		protected override IStatisticMaker StatisticMaker { get; }

		protected override string BeginList => "";
		protected override string EndList => "";
		protected override string Caption => $"## {StatisticMaker.Caption}\n\n";

		public MarkdownReportMaker(IStatisticMaker statisticMaker)
		{
			StatisticMaker = statisticMaker;
		}

		protected override string MakeItem(string valueType, string entry) => $" * **{valueType}**: {entry}\n\n";
	}

	public interface IStatisticMaker
	{
		string Caption { get; }
		object MakeStatistics(IEnumerable<double> _data);
	}

	public class MeanAndStdStatisticMaker : IStatisticMaker
	{
		public string Caption => "Mean and Std";
		public object MakeStatistics(IEnumerable<double> _data)
		{
			var data = _data.ToList();
			var mean = data.Average();
			var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

			return new MeanAndStd
			{
				Mean = mean,
				Std = std
			};
		}
	}
	public class MedianMarkdownStatisticMaker : IStatisticMaker
	{
		public string Caption => "Median";
		public object MakeStatistics(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

			return list[list.Count / 2];
		}
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return new HtmlReportMaker(new MeanAndStdStatisticMaker()).MakeReport(data);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return new MarkdownReportMaker(new MedianMarkdownStatisticMaker()).MakeReport(data);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return new MarkdownReportMaker(new MeanAndStdStatisticMaker()).MakeReport(measurements);
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			return new HtmlReportMaker(new MedianMarkdownStatisticMaker()).MakeReport(measurements);
		}
	}
}