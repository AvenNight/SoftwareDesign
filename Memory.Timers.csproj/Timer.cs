using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memory.Timers
{
    // сделано на скорую руку, можно переделать лучше =)
    public static class Timer
    {
        public static string Report => CreateReport();

        private static List<Record> records;
        private static List<DisposibleTimer> timers;

        private static int lastIndent;

        private const string indent = " ";
        private const int indentCount = 20;
        private const int indentTabCount = 4;

        static Timer()
        {
            ResetData();
        }

        public static void ResetData()
        {
            timers = new List<DisposibleTimer>();
            records = new List<Record>();
            lastIndent = 0;
        }

        public static void EndRecord(string timerName, int timeMS)
        {
            timers.Remove(timers.Find(t => t.Name == timerName));
            var curRec = records.Find(r => r.Name == timerName);
            curRec.TimeMS = timeMS;
            if (lastIndent > curRec.Indent)
            {
                var rest = new Record("Rest", lastIndent);
                var lastRecs = records.Where(r => r.Indent == lastIndent);
                rest.TimeMS = curRec.TimeMS - lastRecs.Sum(r => r.TimeMS);
                records.Add(rest);
                lastIndent = curRec.Indent;
            }
        }

        private static string CreateReport()
        {
            var report = new StringBuilder();
            foreach (var r in records)
            {
                report.AppendRepeat(indent, r.Indent);
                report.Append(r.Name);
                report.AppendRepeat(indent, indentCount - r.Name.Length - r.Indent);
                report.Append($": {r.TimeMS}\n");
            }
            ResetData();
            return report.ToString();
        }

        public static IDisposable Start() => Start("*");

        public static IDisposable Start(string name)
        {
            records.Add(new Record(name, lastIndent = timers.Count * indentTabCount));
            var timer = new DisposibleTimer(name);
            timers.Add(timer);
            return timer;
        }
    }

    public class Record
    {
        public string Name { get; }
        public int Indent { get; }
        public int TimeMS;

        public Record(string name, int indent)
        {
            Name = name;
            TimeMS = 0;
            Indent = indent;
        }
    }

    public class DisposibleTimer : IDisposable
    {
        public readonly string Name;
        private DateTime start;

        private bool disposedValue = false;

        public DisposibleTimer(string name)
        {
            Name = name;
            start = DateTime.Now;
        }

        ~DisposibleTimer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // ...
                }
                Timer.EndRecord(Name, (int)(DateTime.Now - start).TotalMilliseconds);
                disposedValue = true;
            }
        }
    }

    public static class StringBuilderExtensions
    {
        public static void AppendRepeat(this StringBuilder sb, string s, int count)
        {
            for (int i = 0; i < count; i++)
                sb.Append(s);
        }
    }
}