﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesMatchTracker.Core.Services;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using System.Collections.ObjectModel;
using HeroesMatchTracker.Core.Models.StatisticsModels;
using HeroesMatchTracker.Core.ViewServices;
using NLog;
using Heroes.ReplayParser;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsOverviewViewModel : HmtViewModel
    {
        private readonly string InitialSeasonListOption = "- Select Season -";

        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
        private bool _isHeroLeagueSelected;
        private bool _isTeamLeagueSelected;
        private bool _isCustomGameSelected;
        private bool _isBrawlSelected;
        private bool _isHeroStatPercentageDataGridVisible;
        private bool _isHeroStatDataGridVisible;
        private string _selectedSeason;
        private string _selectedHeroStat;

        private ILoadingOverlayWindowService LoadingOverlayWindow;

        private ObservableCollection<StatsOverviewHeroes> _heroStatsCollection = new ObservableCollection<StatsOverviewHeroes>();
        private ObservableCollection<StatsOverviewHeroes> _heroStatsPercentageCollection = new ObservableCollection<StatsOverviewHeroes>();
        private ObservableCollection<StatsOverviewMaps> _mapsStatsCollection = new ObservableCollection<StatsOverviewMaps>();

        public StatsOverviewViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroStatsList.AddRange(HeroesHelpers.OverviewHeroStatOptions.GetOverviewHeroStatOptionList());
            SelectedHeroStat = HeroStatsList[0];

            IsHeroStatPercentageDataGridVisible = false;
            IsHeroStatDataGridVisible = false;
        }

        public RelayCommand QueryOverviewStatsCommand => new RelayCommand(async () => await QueryOverviewStatsAsyncCommand());
        public RelayCommand QuerySelectedHeroStatCommand => new RelayCommand(async () => await QuerySelectedHeroStatAsyncCommand());

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroStatsList { get; private set; } = new List<string>();

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedHeroStat
        {
            get => _selectedHeroStat;
            set
            {
                _selectedHeroStat = value;
                RaisePropertyChanged();
            }
        }

        public bool IsQuickMatchSelected
        {
            get => _isQuickMatchSelected;
            set
            {
                _isQuickMatchSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsUnrankedDraftSelected
        {
            get => _isUnrankedDraftSelected;
            set
            {
                _isUnrankedDraftSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroLeagueSelected
        {
            get => _isHeroLeagueSelected;
            set
            {
                _isHeroLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTeamLeagueSelected
        {
            get => _isTeamLeagueSelected;
            set
            {
                _isTeamLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCustomGameSelected
        {
            get => _isCustomGameSelected;
            set
            {
                _isCustomGameSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBrawlSelected
        {
            get => _isBrawlSelected;
            set
            {
                _isBrawlSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroStatPercentageDataGridVisible
        {
            get => _isHeroStatPercentageDataGridVisible;
            set
            {
                _isHeroStatPercentageDataGridVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroStatDataGridVisible
        {
            get => _isHeroStatDataGridVisible;
            set
            {
                _isHeroStatDataGridVisible = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsOverviewHeroes> HeroStatsCollection
        {
            get => _heroStatsCollection;
            set
            {
                _heroStatsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsOverviewMaps> MapsStatsCollection
        {
            get => _mapsStatsCollection;
            set
            {
                _mapsStatsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsOverviewHeroes> HeroStatsPercentageCollection
        {
            get => _heroStatsPercentageCollection;
            set
            {
                _heroStatsPercentageCollection = value;
                RaisePropertyChanged();
            }
        }

        // for query button
        private async Task QueryOverviewStatsAsyncCommand()
        {
            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await QueryOverviewStatsAsync();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        // just for the Hero Stats datagrid
        private async Task QuerySelectedHeroStatAsyncCommand()
        {
            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await QuerySelectedHeroStatAsync();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task QueryOverviewStatsAsync()
        {
            Clear();

            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            HeroesIcons.LoadLatestHeroesBuild();

            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeason);
            OverviewHeroStatOption selectedStatOption = HeroesHelpers.EnumParser.ConvertHeroStatOptionToEnum(SelectedHeroStat);

            GameMode selectedGameModes = SetSelectedGameModes();

            await SetHeroStats(selectedSeason, selectedGameModes, selectedStatOption);
            await SetMapStats(selectedSeason, selectedGameModes);
        }

        // for hero stats only when changed the combobox
        private async Task QuerySelectedHeroStatAsync()
        {
            ClearHeroStatGridOnly();

            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            HeroesIcons.LoadLatestHeroesBuild();

            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeason);
            OverviewHeroStatOption selectedStatOption = HeroesHelpers.EnumParser.ConvertHeroStatOptionToEnum(SelectedHeroStat);

            GameMode selectedGameModes = SetSelectedGameModes();

            await SetHeroStats(selectedSeason, selectedGameModes, selectedStatOption);
        }

        private async Task SetHeroStats(Season season, GameMode gameModes, OverviewHeroStatOption statOption)
        {
            var heroesList = HeroesIcons.Heroes().GetListOfHeroes();
            var heroStatPercentageCollection = new Collection<StatsOverviewHeroes>();
            var heroStatCollection = new Collection<StatsOverviewHeroes>();

            foreach (var hero in heroesList)
            {
                if (statOption == OverviewHeroStatOption.HighestWinRate)
                {
                    int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, true);
                    int losses = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, false);
                    int total = wins + losses;

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = total != 0 ? wins / (double)total : 0,
                    };

                    IsHeroStatPercentageDataGridVisible = true;
                    heroStatPercentageCollection.Add(statsOverviewHeroes);
                }
                else if (statOption == OverviewHeroStatOption.MostWins)
                {
                    int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, true);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = wins,
                    };

                    IsHeroStatDataGridVisible = true;
                    heroStatCollection.Add(statsOverviewHeroes);
                }
                else if (statOption == OverviewHeroStatOption.MostDeaths || statOption == OverviewHeroStatOption.MostKills || statOption == OverviewHeroStatOption.MostAssists)
                {
                    int value = Database.ReplaysDb().Statistics.ReadStatValue(hero, season, gameModes, statOption);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = value,
                    };

                    IsHeroStatDataGridVisible = true;
                    heroStatCollection.Add(statsOverviewHeroes);
                }
            }

            // sort it, then add to ObservableCollection
            if (IsHeroStatPercentageDataGridVisible)
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsPercentageCollection = new ObservableCollection<StatsOverviewHeroes>(heroStatPercentageCollection.OrderByDescending(x => x.Value)); });

            if (IsHeroStatDataGridVisible)
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(heroStatCollection.OrderByDescending(x => x.Value)); });
        }

        private async Task SetMapStats(Season season, GameMode gameModes)
        {
            var mapList = HeroesIcons.MapBackgrounds().GetMapsList();
            var mapStatTempCollection = new Collection<StatsOverviewMaps>();

            foreach (var map in mapList)
            {
                int wins = Database.ReplaysDb().Statistics.ReadMapResults(season, gameModes, true, map);
                int losses = Database.ReplaysDb().Statistics.ReadMapResults(season, gameModes, false, map);
                int total = wins + losses;

                StatsOverviewMaps statsOverviewMaps = new StatsOverviewMaps()
                {
                    MapName = map,
                    Wins = wins,
                    Losses = losses,
                    Winrate = total != 0 ? wins / (double)total : 0,
                };

                mapStatTempCollection.Add(statsOverviewMaps);
            }

            await Application.Current.Dispatcher.InvokeAsync(() => { MapsStatsCollection = new ObservableCollection<StatsOverviewMaps>(mapStatTempCollection.OrderByDescending(x => x.Winrate)); });
        }

        private GameMode SetSelectedGameModes()
        {
            GameMode gameModes = GameMode.Unknown;

            if (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected)
            {
                gameModes = GameMode.QuickMatch | GameMode.UnrankedDraft | GameMode.HeroLeague | GameMode.TeamLeague;
            }
            else
            {
                if (IsQuickMatchSelected)
                    gameModes |= GameMode.QuickMatch;
                if (IsUnrankedDraftSelected)
                    gameModes |= GameMode.UnrankedDraft;
                if (IsHeroLeagueSelected)
                    gameModes |= GameMode.HeroLeague;
                if (IsTeamLeagueSelected)
                    gameModes |= GameMode.TeamLeague;
                if (IsCustomGameSelected)
                    gameModes |= GameMode.Custom;
                if (IsBrawlSelected)
                    gameModes |= GameMode.Brawl;
            }

            return gameModes;
        }

        private void Clear()
        {
            ClearHeroStatGridOnly();

            MapsStatsCollection = null;
            MapsStatsCollection = null;
        }

        private void ClearHeroStatGridOnly()
        {
            IsHeroStatPercentageDataGridVisible = false;
            IsHeroStatDataGridVisible = false;

            HeroStatsCollection = null;
            HeroStatsPercentageCollection = null;
        }
    }
}
