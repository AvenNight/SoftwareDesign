using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public abstract class ValueType<T> where T : class
	{
		private PropertyInfo[] properties;
		private IEnumerable<object> values;

		public ValueType()
		{
			properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			values = properties.Select(p => p.GetValue(this, null));
		}

		public override bool Equals(object obj) => Equals(obj as T);

		public bool Equals(T obj)
		{
			var objAsT = obj as ValueType<T>;
			bool e1 = obj is ValueType<T>;
			bool e2 = false;
			bool e3 = false;
			if (e1)
			{
				e2 = properties.SequenceEqual(objAsT.properties);
				e3 = true;
				for (int i = 0; i < properties.Length; i++)
				{
					var p1 = properties[i].GetValue(this);
					var p2 = objAsT.properties[i].GetValue(objAsT);
					if (p1 == null && p2 == null)
						continue;
					else if (!p1.Equals(p2))
					{
						e3 = false;
						break;
					}
				}
			}
			return e1 && e2 && e3;
		}

		public override int GetHashCode()
		{
			int hash = -1479509188;
			unchecked
			{
				foreach (var v in values)
					hash = (hash * 187) ^ v.GetHashCode();
				
			}
			return hash;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			var endOfProp = "; ";
			stringBuilder.Append(this.GetType().Name);
			stringBuilder.Append("(");
			foreach (var p in properties.OrderBy(p=> p.Name))
			{
				stringBuilder.Append(p.Name);
				stringBuilder.Append(": ");
				stringBuilder.Append(p.GetValue(this));
				stringBuilder.Append(endOfProp);
			}
			stringBuilder.Remove(stringBuilder.Length - 2, endOfProp.Length);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}