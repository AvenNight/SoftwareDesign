using System;

namespace SRP.ControlDigit
{
	public static class ControlDigitAlgo
	{
		public static int Upc(long number)
		{
			int checkSum = number.GetChecksum(3, f => 4 - f);
			int result = checkSum % 10;
			if (result != 0)
				result = 10 - result;
			return result;
		}

		public static char Isbn10(long number)
		{
			int checkSum = number.GetChecksum(2, f => ++f);
			int result = checkSum % 11;
			if (result != 0)
				result = 11 - result;
			return (result - 10) == 0 ? 'X' : result.ToString()[0];
		}

		public static int Isbn13(long number)
		{
			int checkSum = number.GetChecksum(1, f => 4 - f);
			int result = checkSum % 10;
			if (result != 0)
				result = 10 - result;
			return result;
		}
	}

	public static class Extensions
	{
		public static int LastDigit(this long number) => (int)(number % 10);
		public static int GetChecksum(this long number, int factor, Func<int, int> changeFactor)
		{
			int sum = 0;
			while (number > 0)
			{
				sum += factor * number.LastDigit();
				factor = changeFactor(factor);
				number /= 10;
			}
			return sum;
		}
	}
}