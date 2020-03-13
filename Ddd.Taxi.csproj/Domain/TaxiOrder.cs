using Ddd.Infrastructure;
using System;
using System.Globalization;
using System.Linq;

namespace Ddd.Taxi.Domain
{
	// In real aplication it would be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public Driver GetDriverFromBase(int driverId)
		{
			if (driverId == 15)
				return new Driver(driverId, 
					new PersonName("Drive", "Driverson"),
					new Car("Lada sedan", "Baklazhan", "A123BT 66"));
			else
				throw new Exception("Unknown driver id " + driverId);
		}

		public bool CheckDriverInBase(Driver driver)
		{
			return driver.Id == 15 ? true : throw new InvalidOperationException("WaitingForDriver");
		}
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private readonly DriversRepository driversRepo;
		private readonly Func<DateTime> currentTime;
		private int idCounter;

		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
		{
			this.driversRepo = driversRepo;
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
		{
			return new TaxiOrder(idCounter++, 
				new PersonName(firstName, lastName),
				new Address(street, building),
				currentTime());
		}

		public void UpdateDestination(TaxiOrder order, string street, string building)
		{
			order.UpdateDestination(new Address(street, building));
		}

		public void AssignDriver(TaxiOrder order, int driverId)
		{
			order.AssignDriver(driversRepo.GetDriverFromBase(driverId), currentTime());
		}

		public void UnassignDriver(TaxiOrder order)
		{
			if (driversRepo.CheckDriverInBase(order.Driver))
				order.UnassignDriver();
		}

		public string GetDriverFullInfo(TaxiOrder order) => order.GetDriverFullInfo();

		public string GetShortOrderInfo(TaxiOrder order) => order.GetShortOrderInfo();

		public void Cancel(TaxiOrder order) => order.Cancel(currentTime());

		public void StartRide(TaxiOrder order) => order.StartRide(currentTime());

		public void FinishRide(TaxiOrder order) => order.FinishRide(currentTime());
	}

	public class TaxiOrder : Entity<int>
	{
		public PersonName ClientName { get; }
		public Address Start { get; }
		public Address Destination { get; private set; }
		public Driver Driver { get; private set; }

		private TaxiOrderStatus status;
		private DateTime creationTime;
		private DateTime driverAssignmentTime;
		private DateTime cancelTime;
		private DateTime startRideTime;
		private DateTime finishRideTime;

		public TaxiOrder(int id, PersonName clientName, Address startAddress, DateTime creationTime) : base(id)
		{
			ClientName = clientName;
			Start = startAddress;
			Destination = new Address(null, null);
			Driver = new Driver(-1, new PersonName(null, null), new Car());
			this.creationTime = creationTime;
			status = TaxiOrderStatus.WaitingForDriver;
		}

		public void AssignDriver(Driver driver, DateTime curTime)
		{
			if (Driver.Id != -1 || status != TaxiOrderStatus.WaitingForDriver) throw new InvalidOperationException();
			Driver = driver;
			driverAssignmentTime = curTime;
			this.status = TaxiOrderStatus.WaitingCarArrival;
		}

		public void UnassignDriver()
		{
			if (Driver.Id == -1 || status == TaxiOrderStatus.InProgress) throw new InvalidOperationException();
			Driver = new Driver(-1, new PersonName(null, null), new Car());
			status = TaxiOrderStatus.WaitingForDriver;
		}

		public void UpdateDestination(Address address) => Destination = address;

		public void Cancel(DateTime time)
		{
			if (status == TaxiOrderStatus.InProgress) throw new InvalidOperationException();
			status = TaxiOrderStatus.Canceled;
			cancelTime = time;
		}

		public void StartRide(DateTime time)
		{
			if (Driver.Id == -1 || status != TaxiOrderStatus.WaitingCarArrival) throw new InvalidOperationException();
			status = TaxiOrderStatus.InProgress;
			startRideTime = time;
		}

		public void FinishRide(DateTime time)
		{
			if (Driver.Id == -1 || status != TaxiOrderStatus.InProgress) throw new InvalidOperationException();
			status = TaxiOrderStatus.Finished;
			finishRideTime = time;
		}

		public string GetDriverFullInfo()
		{
			if (status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + Driver.Id,
				"DriverName: " + FormatName(Driver.Name.FirstName, Driver.Name.LastName),
				"Color: " + Driver.Car.Color,
				"CarModel: " + Driver.Car.Model,
				"PlateNumber: " + Driver.Car.PlateNumber);
		}

		public string GetShortOrderInfo()
		{
			return string.Join(" ",
				"OrderId: " + Id,
				"Status: " + status,
				"Client: " + FormatName(ClientName.FirstName, ClientName.LastName),
				"Driver: " + FormatName(Driver.Name.FirstName, Driver.Name.LastName),
				"From: " + FormatAddress(Start.Street, Start.Building),
				"To: " + FormatAddress(Destination.Street, Destination.Building),
				"LastProgressTime: " + GetLastProgressTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		public DateTime GetLastProgressTime()
		{
			if (status == TaxiOrderStatus.WaitingForDriver) return creationTime;
			if (status == TaxiOrderStatus.WaitingCarArrival) return driverAssignmentTime;
			if (status == TaxiOrderStatus.InProgress) return startRideTime;
			if (status == TaxiOrderStatus.Finished) return finishRideTime;
			if (status == TaxiOrderStatus.Canceled) return cancelTime;
			throw new NotSupportedException(status.ToString());
		}

		public string FormatName(string firstName, string lastName)
		{
			return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
		}

		public string FormatAddress(string street, string building)
		{
			return string.Join(" ", new[] { street, building }.Where(n => n != null));
		}
	}

	public class Driver : Entity<int>
	{
		public PersonName Name { get; }
		public Car Car { get; }

		public Driver(int id, PersonName name, Car car) : base(id)
		{
			Name = name;
			Car = car;
		}
	}

	public class Car : ValueType<Car>
	{
		public string Model { get; set; }
		public string Color { get; set; }
		public string PlateNumber { get; set; }

		public Car() { }

		public Car(string model, string color, string plateNumber)
		{
			Color = color;
			Model = model;
			PlateNumber = plateNumber;
		}
	}
}