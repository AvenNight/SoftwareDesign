using System;
using System.Collections.Generic;
using System.Linq;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        private HashSet<TRow> rows;
        public IEnumerable<TRow> Rows => rows;
        private HashSet<TColumn> columns;
        public IEnumerable<TColumn> Columns => columns;

        private readonly Dictionary<(TRow, TColumn), TValue> values;

        public readonly TableIndexer<TRow, TColumn, TValue> Open;
        public readonly TableIndexer<TRow, TColumn, TValue> Existed;

        public Table()
        {
            rows = new HashSet<TRow>();
            columns = new HashSet<TColumn>();
            values = new Dictionary<(TRow, TColumn), TValue>();
            Open = new TableIndexer<TRow, TColumn, TValue>();
            Open.SetTable += Open_SetTable;
            Open.GetTable += Open_GetTable;
            Existed = new TableIndexer<TRow, TColumn, TValue>();
            Existed.SetTable += Existed_SetTable;
            Existed.GetTable += Existed_GetTable;
        }

        public void AddRow(TRow value) => rows.Add(value);
        public void AddColumn(TColumn value) => columns.Add(value);

        private TValue Open_GetTable(TRow r, TColumn c)
        {
            bool success = values.TryGetValue((r, c), out TValue v);
            return success ? v : default;
        }

        private void Open_SetTable(TRow r, TColumn c, TValue v)
        {
            AddRow(r);
            AddColumn(c);
            values[(r, c)] = v;
        }

        private TValue Existed_GetTable(TRow r, TColumn c)
        {
            bool success = values.TryGetValue((r, c), out TValue v);
            if (CellContains(r, c))
                return success ? v : default;
            else
                throw new ArgumentException();
        }

        private void Existed_SetTable(TRow r, TColumn c, TValue v)
        {
            if (CellContains(r, c))
                values[(r, c)] = v;
            else
                throw new ArgumentException();
        }

        private bool CellContains(TRow r, TColumn c) => Rows.Contains(r) && Columns.Contains(c);
    }

    public class TableIndexer<TRow, TColumn, TValue>
    {
        public event Action<TRow, TColumn, TValue> SetTable;
        public event Func<TRow, TColumn, TValue> GetTable;
        public TValue this[TRow r, TColumn c]
        {
            get => GetTable(r, c);
            set => SetTable?.Invoke(r, c, value);
        }
    }
}