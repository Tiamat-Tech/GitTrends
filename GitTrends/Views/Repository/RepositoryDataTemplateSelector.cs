using System.ComponentModel;
using CommunityToolkit.Maui.Markup;
using GitTrends.Common;
using GitTrends.Mobile.Common;
using GitTrends.Resources;

namespace GitTrends;

class RepositoryDataTemplateSelector(in IDeviceInfo deviceInfo, in MobileSortingService sortingService) : DataTemplateSelector
{
	readonly MobileSortingService _sortingService = sortingService;

	readonly ViewsDataTemplate _viewsDataTemplate = new(deviceInfo);
	readonly ClonesDataTemplate _clonesDataTemplate = new(deviceInfo);
	readonly IssuesForksDataTemplate _issuesForksDataTemplate = new(deviceInfo);

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
	{
		var category = MobileSortingService.GetSortingCategory(_sortingService.CurrentOption);

		return category switch
		{
			SortingCategory.Views => _viewsDataTemplate,
			SortingCategory.Clones => _clonesDataTemplate,
			SortingCategory.IssuesForks => _issuesForksDataTemplate,
			_ => throw new NotSupportedException()
		};
	}

	sealed class ClonesDataTemplate(IDeviceInfo deviceInfo) : BaseRepositoryDataTemplate(() => new CardView(deviceInfo, CreateClonesDataTemplateViews(deviceInfo)))
	{
		static IEnumerable<View> CreateClonesDataTemplateViews(IDeviceInfo deviceInfo) =>
		[
			new StatisticsSvgImage(deviceInfo, "total_clones.svg", nameof(BaseTheme.CardClonesStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji1),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardClonesStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Assign(out Label clonesLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsClonesData)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.TotalClones,
					mode: BindingMode.OneWay,
					convert: static totalClones => totalClones.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static clonesLabel => clonesLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: clonesLabel,
					convert: static isClonesLabelVisible => !isClonesLabelVisible),

			new StatisticsSvgImage(deviceInfo, "unique_clones.svg", nameof(BaseTheme.CardUniqueClonesStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji2),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardUniqueClonesStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic2)
				.Assign(out Label uniqueClonesLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsClonesData,
					mode: BindingMode.OneWay)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.TotalUniqueClones,
					mode: BindingMode.OneWay,
					convert: static totalUniqueClones => totalUniqueClones.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic2)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static uniqueClonesLabel => uniqueClonesLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: uniqueClonesLabel,
					convert: static isUniqueClonesLabelVisible => !isUniqueClonesLabelVisible),

			new StatisticsSvgImage(deviceInfo, "star.svg", nameof(BaseTheme.CardStarsStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji3),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardStarsStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic3)
				.Assign(out Label starsLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsStarsData,
					mode: BindingMode.OneWay)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.StarCount,
					mode: BindingMode.OneWay,
					convert: static (long? starCount) => starCount.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic3)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static starsLabel => starsLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: starsLabel,
					convert: static isStarsLabelVisible => !isStarsLabelVisible),
		];
	}

	sealed class ViewsDataTemplate(IDeviceInfo deviceInfo) : BaseRepositoryDataTemplate(() => new CardView(deviceInfo, CreateViewsDataTemplateViews(deviceInfo)))
	{
		static IEnumerable<View> CreateViewsDataTemplateViews(IDeviceInfo deviceInfo) =>
		[
			new StatisticsSvgImage(deviceInfo, "total_views.svg", nameof(BaseTheme.CardViewsStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji1),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardViewsStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Assign(out Label viewsLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsViewsData,
					mode: BindingMode.OneWay)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.TotalViews,
					mode: BindingMode.OneWay,
					convert: static totalViews => totalViews.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static viewsLabel => viewsLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: viewsLabel,
					convert: static isViewsLabelVisible => !isViewsLabelVisible),

			new StatisticsSvgImage(deviceInfo, "unique_views.svg", nameof(BaseTheme.CardUniqueViewsStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji2),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardUniqueViewsStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic2)
				.Assign(out Label uniqueViewsLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsViewsData,
					mode: BindingMode.OneWay)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.TotalUniqueViews,
					mode: BindingMode.OneWay,
					convert: static totalUniqueViews => totalUniqueViews.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic2)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static uniqueViewsLabel => uniqueViewsLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: uniqueViewsLabel,
					convert: static isUniqueViewsLabelVisible => !isUniqueViewsLabelVisible),

			new StatisticsSvgImage(deviceInfo, "star.svg", nameof(BaseTheme.CardStarsStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji3),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardStarsStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic3)
				.Assign(out Label starsLabel)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsStarsData,
					mode: BindingMode.OneWay)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.StarCount,
					mode: BindingMode.OneWay,
					convert: static (long? starCount) => starCount.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic3)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static starsLabel => starsLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: starsLabel,
					convert: static isStarsLabelVisible => !isStarsLabelVisible),
		];
	}

	sealed class IssuesForksDataTemplate(IDeviceInfo deviceInfo) : BaseRepositoryDataTemplate(() => new CardView(deviceInfo, CreateIssuesForksDataTemplateViews(deviceInfo)))
	{
		static IEnumerable<View> CreateIssuesForksDataTemplateViews(IDeviceInfo deviceInfo) =>
		[
			new StatisticsSvgImage(deviceInfo, "star.svg", nameof(BaseTheme.CardStarsStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji1),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardStarsStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Bind(Label.IsVisibleProperty,
					getter: static (Repository repository) => repository.ContainsStarsData,
					mode: BindingMode.OneTime)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.StarCount,
					mode: BindingMode.OneWay,
					convert: static (long? starCount) => starCount.ToAbbreviatedText()),

			//Display an activity indicator while the Data is loading
			new StatisticsActivityIndicator()
				.Row(Row.Statistics).Column(Column.Statistic1)
				.Bind(ActivityIndicator.IsVisibleProperty,
					getter: static starsLabel => starsLabel.IsVisible,
					mode: BindingMode.OneWay,
					source: starsLabel,
					convert: static isStarsLabelVisible => !isStarsLabelVisible),

			new StatisticsSvgImage(deviceInfo, "repo_forked.svg", nameof(BaseTheme.CardForksStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji2),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardForksStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic2)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.ForkCount,
					mode: BindingMode.OneWay,
					convert: static (long? forkCount) => forkCount.ToAbbreviatedText()),

			new StatisticsSvgImage(deviceInfo, "issue_opened.svg", nameof(BaseTheme.CardIssuesStatsIconColor))
				.Row(Row.Statistics).Column(Column.Emoji3),

			//Only display the value when the Repository Data finishes loading. This avoids showing '0' while the data is loading.
			new StatisticsLabel(nameof(BaseTheme.CardIssuesStatsTextColor))
				.Row(Row.Statistics).Column(Column.Statistic3)
				.Bind(Label.TextProperty,
					getter: static (Repository repository) => repository.IssuesCount,
					mode: BindingMode.OneWay,
					convert: static (long? issuesCount) => issuesCount.ToAbbreviatedText()),
		];
	}

	sealed class StatisticsSvgImage : SvgImage
	{
		public StatisticsSvgImage(IDeviceInfo deviceInfo, string fileName, string baseThemeColor)
			: base(deviceInfo, fileName, () => AppResources.GetResource<Color>(baseThemeColor), 14, 14)
		{
			VerticalOptions = LayoutOptions.Center;
			HorizontalOptions = LayoutOptions.Center;
		}
	}

	sealed class StatisticsActivityIndicator : ActivityIndicator
	{
		public StatisticsActivityIndicator()
		{
#if IOS
			Scale = 2.0/3.0;
#endif

			Margin = 2;

			IsRunning = true;
			IsVisible = true;

			HorizontalOptions = LayoutOptions.Start;
			VerticalOptions = LayoutOptions.Center;

			SetDynamicResource(ColorProperty, nameof(BaseTheme.PrimaryTextColor));

			PropertyChanged += HandlePropertyChanged;
		}

		void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == IsVisibleProperty.PropertyName)
			{
				var temp = IsVisible;
			}
		}
	}
}