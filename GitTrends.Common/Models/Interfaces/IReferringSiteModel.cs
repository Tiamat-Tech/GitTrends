namespace GitTrends.Common;

public interface IReferringSiteModel
{
	DateTimeOffset DownloadedAt { get; }
	string Referrer { get; }
	bool IsReferrerUriValid { get; }
	Uri? ReferrerUri { get; }
	long TotalCount { get; }
	long TotalUniqueCount { get; }
}