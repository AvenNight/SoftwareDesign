using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentApi.Graph
{
	internal enum NodeShape
	{
		Box,
		Ellipse
	}

	internal class DotGraphBuilder
	{
		public static GraphBlank DirectedGraph(string name) => new GraphBlank(name, true, false);
		public static GraphBlank NondirectedGraph(string name) => new GraphBlank(name, false, false);
	}

	internal class GraphBlank
	{
		private readonly List<GraphElement> graphElements;

		private GraphElement currentElement;

		private readonly string graphName;
		private readonly bool directed;
		private readonly bool strict;

		internal GraphBlank(string graphName, bool directed, bool strict)
		{
			this.graphName = graphName;
			this.directed = directed;
			this.strict = strict;
			graphElements = new List<GraphElement>();
		}

		internal GraphBlank AddNode(string name)
		{
			currentElement = new NodeElement(name);
			graphElements.Add(currentElement);
			return this;
		}

		internal GraphBlank AddEdge(string from, string to)
		{
			currentElement = new EdgeElement(from, to);
			graphElements.Add(currentElement);
			return this;
		}

		internal GraphBlank With(Action<GraphElement> setAttributes)
		{
			setAttributes(currentElement);
			return this;
		}

		internal string Build()
		{
			var graph = new Graph(graphName, directed, strict);

			foreach (var e in graphElements)
			{
				if (e is NodeElement node)
				{
					graph.AddNode(node.Name);
					var newNode = graph.Nodes.LastOrDefault();
					foreach (var attr in node.Attributes)
						newNode.Attributes.Add(attr.Key, attr.Value);
				}
				else if (e is EdgeElement edge)
				{
					graph.AddEdge(edge.From, edge.To);
					var newEdge = graph.Edges.LastOrDefault();
					foreach (var attr in edge.Attributes)
						newEdge.Attributes.Add(attr.Key, attr.Value);
				}
			}
			return graph.ToDotFormat();
		}
	}
	internal abstract class GraphElement
	{
		internal readonly Dictionary<string, string> Attributes = new Dictionary<string, string>();

		internal GraphElement Color(string color) => AddAttribute("color", color);
		internal GraphElement Shape(NodeShape shape) => AddAttribute("shape", shape);
		internal GraphElement FontSize(int size) => AddAttribute("fontsize", size);
		internal GraphElement Label(string label) => AddAttribute("label", label);
		internal GraphElement Weight(int weight) => AddAttribute("weight", weight);

		private GraphElement AddAttribute(string name, object value)
		{
			Attributes.Add(name, value.ToString().ToLower());
			return this;
		}
	}

	internal class NodeElement : GraphElement
	{
		internal string Name { get; }

		internal NodeElement(string name)
		{
			this.Name = name;
		}
	}

	internal class EdgeElement : GraphElement
	{
		internal string From { get; }
		internal string To { get; }

		internal EdgeElement(string from, string to)
		{
			this.From = from;
			this.To = to;
		}
	}
}