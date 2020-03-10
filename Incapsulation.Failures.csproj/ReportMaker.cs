using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Failure
    {
        public Device Device { get; }
        public FailureType Type { get; }
        public DateTime Date { get; }

        public bool IsSerious =>
            Type == FailureType.HardwareFailures || Type == FailureType.UnexpectedShutdown;

        public Failure(Device device, FailureType failure, DateTime dateTime)
        {
            Device = device;
            Type = failure;
            Date = dateTime;
        }
    }

    public class Device
    {
        public string Name { get; }
        public int Id { get; }

        public Device(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day, int month, int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var dateTime = new DateTime(year, month, day);
            var failures = failureTypes
                .Select(f => (FailureType)f)
                .ToArray();
            var failDates = times
                .Select(t => new DateTime((int)t[2], (int)t[1], (int)t[0]))
                .ToArray();
            var devicesArr = devices
                .Select(d => new Device((string)d["Name"], (int)d["DeviceId"]))
                .ToArray();

            Failure[] deviceFailures = new Failure[devicesArr.Length];

            for (int i = 0; i < deviceFailures.Length; i++)
                deviceFailures[i] = new Failure(
                    devicesArr[i],
                    failures[i],
                    failDates[i]);

            return FindDevicesFailedBeforeDate(deviceFailures, dateTime);
        }

        public static List<string> FindDevicesFailedBeforeDate(Failure[] failures, DateTime dateTime)
        {
            var result = new List<string>();

            foreach (var e in failures)
                if (e.IsSerious && e.Date < dateTime)
                    result.Add(e.Device.Name);

            return result;
        }
    }
}