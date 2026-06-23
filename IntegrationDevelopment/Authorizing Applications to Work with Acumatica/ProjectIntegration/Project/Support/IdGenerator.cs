using System;
using System.Globalization;

namespace IntegrationDiagnostics.Runner
{
	internal static class IdGenerator
	{
		private static readonly object SyncRoot = new object();
		private static readonly Random Random = new Random();
		private static int Counter;

		public static string ProjectId()
		{
			return ShortId("DG", 10);
		}

		public static string TaskId()
		{
			return ShortId("DT", 10);
		}

		public static string TemplateId()
		{
			return ShortId("DGT", 10);
		}

		public static string TemplateTaskId()
		{
			return ShortId("DTT", 10);
		}

		public static string AccountGroupId()
		{
			return ShortId("DGAG", 10);
		}

		public static string ChangeOrderClassId()
		{
			return ShortId("DGC", 10);
		}

		public static string BusinessAccountId()
		{
			return ShortId("DGBA", 10);
		}

		public static string CostCodeId()
		{
			lock (SyncRoot)
			{
				return Random.Next(9000, 9999).ToString(CultureInfo.InvariantCulture);
			}
		}

		private static string ShortId(string prefix, int maxLength)
		{
			lock (SyncRoot)
			{
				Counter = (Counter + 1) % 100;
				var suffix = DateTime.UtcNow.ToString("mmssff", CultureInfo.InvariantCulture)
					+ Counter.ToString("00", CultureInfo.InvariantCulture);
				var value = prefix + suffix;
				return value.Length <= maxLength ? value : value.Substring(0, maxLength);
			}
		}
	}
}
