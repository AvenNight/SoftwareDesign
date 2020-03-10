using System;
using System.Collections.Generic;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string ProductName { get; set; }
        public MessageType Type { get; set; }
        public MessageTopic Topic { get; set; }

        public Category(string productName, MessageType type, MessageTopic topic)
        {
            ProductName = productName;
            Type = type;
            Topic = topic;
        }

        public int CompareTo(object obj)
        {
            if (obj is null || this is null) return 0;
            var cat = obj as Category;
            if (ProductName != null && cat.ProductName != null && !ProductName.Equals(cat.ProductName))
                return ProductName.CompareTo(cat.ProductName);
            if (!Type.Equals(cat.Type))
                return Type.CompareTo(cat.Type);
            else
                return Topic.CompareTo(cat.Topic);
        }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   ProductName == category.ProductName &&
                   Type == category.Type &&
                   Topic == category.Topic;
        }

        public override int GetHashCode()
        {
            var hashCode = 1643138706;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductName);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Topic.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{ProductName}.{Type}.{Topic}";

        public static bool operator ==(Category left, Category right) =>
            ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
        public static bool operator !=(Category left, Category right) => !(left == right);
        public static bool operator <(Category left, Category right) => left.CompareTo(right) < 0;
        public static bool operator <=(Category left, Category right) => left == right || left.CompareTo(right) <= 0;
        public static bool operator >(Category left, Category right) => left.CompareTo(right) > 0;
        public static bool operator >=(Category left, Category right) => left == right || left.CompareTo(right) >= 0;
    }
}