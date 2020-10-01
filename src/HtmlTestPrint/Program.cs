using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeAnnotation;
using CodeAnnotation.Tests;
using static System.Environment;
using static System.Net.WebUtility;

namespace HtmlTestPrint
{
	public static class Program
	{
		private const string OutputFileName = "annotated_source_code.html";

		private static readonly SourceCode[] Sources =
		{
			SourceCode.NewCSharp(CSharpAnnotatorTestData.csharpCode),
			SourceCode.NewFSharp(FSharpAnnotatorTestData.fsharpCode),
		};

		private static readonly Dictionary<CodeBuildingBlock, string> ColorByBlock =
			new Dictionary<CodeBuildingBlock, string>
		{
			{CodeBuildingBlock.Comment, "#57A64A"},
			{CodeBuildingBlock.Constant, "#D182D1" },
			{CodeBuildingBlock.Function, "#FFFFFF" },
			{CodeBuildingBlock.Keyword, "#569CD6" },
			{CodeBuildingBlock.Literal, "#B5CEA8" },
			{CodeBuildingBlock.String, "#D69D85" },
			{CodeBuildingBlock.Type, "#4EC9B0" },
			{CodeBuildingBlock.Interface, "#A8DB87" },
			{CodeBuildingBlock.Namespace, "#579B81" },
			{CodeBuildingBlock.TypeCase, "#FFC8D1" },
			{CodeBuildingBlock.Attribute, "#BBB529" },
			{CodeBuildingBlock.Property, "#9EDCDC"},
			{CodeBuildingBlock.Event, "#8264F5"}
		};

		public static void Main()
		{
			var annotatedSourceHtml = Sources
				.Select(Annotator.annotate)
				.Aggregate("", (current, annotatedSource) => current + (annotatedSource.ConvertToHtml() + "<br><hr><br>"));
			annotatedSourceHtml.PrintAsHtml();
		}

		private static void PrintAsHtml(this string annotatedSourceHtml)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), OutputFileName);
			using var stream = new StreamWriter(filePath, false);
			stream.Write(annotatedSourceHtml);

			Console.WriteLine($"Test html successfully written to \"{filePath}\"");
		}

		private static string ConvertToHtml(this string txt)
		{
			txt = HtmlEncode(txt);
			foreach (var block in ColorByBlock.Keys)
			{
				var pattern = Annotator.generateSearchPatternFor(block);
				var replacement = @$"<span class=""{block}""><b>$1</b></span>";

				txt = Regex.Replace(txt, pattern, replacement);
			}

			txt = $"<pre>{txt}</pre>";

			var html =
				$"<!DOCTYPE html>{NewLine}" +
				$"<html>{NewLine}" +
				$"{BuildHeadHtml()}{NewLine}" +
				$"{txt.BuildBodyHtml()}{NewLine}" +
				$"</html>{NewLine}";

			return html;
		}

		private static string BuildHeadHtml()
		{
			var styles = ColorByBlock.Aggregate(
				"",
				(state, curr) => state + $".{curr.Key} {{color:{curr.Value};}}{NewLine}");

			return
				$"<head>{NewLine}" +
				$"<style>{NewLine}" +
				$"body {{background-color:#101010; color:#AAAAAA}}{NewLine}" +
				$"{styles}{NewLine}" +
				$"</style>{NewLine}" +
				$"</head>{NewLine}";
		}

		private static string BuildBodyHtml(this string content)
		{
			return
				$"<body>{NewLine}" +
				$"<p>{content}</p>{NewLine}" +
				$"</body>{NewLine}";
		}
	}
}
