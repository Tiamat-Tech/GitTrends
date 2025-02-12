namespace GitTrends.Common;

public interface IDailyClonesModel
{
	DateTime LocalDay { get; }

	DateTimeOffset Day { get; }

	long TotalClones { get; }

	long TotalUniqueClones { get; }
}