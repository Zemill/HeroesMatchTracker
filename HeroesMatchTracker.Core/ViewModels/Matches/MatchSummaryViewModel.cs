﻿using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.GraphSummaryModels;
using HeroesMatchTracker.Core.Models.MatchModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data.Models.Replays;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class MatchSummaryViewModel : HmtViewModel, IMatchSummaryReplayService
    {
        private int? _teamBlueKills;
        private int? _teamRedKills;
        private int? _teamBlueLevel;
        private int? _teamRedLevel;
        private bool _isLeftChangeButtonVisible;
        private bool _isRightChangeButtonVisible;
        private bool _isLeftChangeButtonEnabled;
        private bool _isRightChangeButtonEnabled;
        private bool _hasBans;
        private bool _hasObservers;
        private bool _hasChat;
        private bool _isFlyoutLoadingOverlayVisible;
        private string _teamBlueIsWinner;
        private string _teamRedIsWinner;
        private string _matchTitle;
        private string _teamBlueName;
        private string _teamRedName;
        private string _matchLength;
        private Color _matchTitleGlowColor;

        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchPlayerAdvancedStats> _matchAdvancedStatsTeam1Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
        private ObservableCollection<MatchPlayerAdvancedStats> _matchAdvancedStatsTeam2Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
        private ObservableCollection<MatchChat> _matchChatCollection = new ObservableCollection<MatchChat>();
        private ObservableCollection<MatchObserver> _matchObserversCollection = new ObservableCollection<MatchObserver>();

        private IWebsiteService Website;
        private ILoadingOverlayWindowService LoadingOverlayWindow;
        private Collection<MatchPlayerStats> MatchPlayerStatsTeam1Temp = new Collection<MatchPlayerStats>();
        private Collection<MatchPlayerStats> MatchPlayerStatsTeam2Temp = new Collection<MatchPlayerStats>();

        public MatchSummaryViewModel(IInternalService internalService, IWebsiteService website, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            Website = website;
            LoadingOverlayWindow = loadingOverlayWindow;

            IsFlyoutLoadingOverlayVisible = false;
            IsLeftChangeButtonVisible = true;
            IsRightChangeButtonVisible = true;
            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;

            HasBans = false;
            HasObservers = false;
            HasChat = false;

            TeamLevelTimeGraph = new TeamLevelTimeGraph();
            TeamExperienceGraph = new TeamExperienceGraph(Database);

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));

            SimpleIoc.Default.Register<IMatchSummaryReplayService>(() => this);
        }

        public MatchBans MatchHeroBans { get; private set; } = new MatchBans();

        public TeamLevelTimeGraph TeamLevelTimeGraph { get; private set; }
        public TeamExperienceGraph TeamExperienceGraph { get; private set; }

        public int? TeamBlueKills
        {
            get => _teamBlueKills;
            set
            {
                _teamBlueKills = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamRedKills
        {
            get => _teamRedKills;
            set
            {
                _teamRedKills = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamBlueLevel
        {
            get => _teamBlueLevel;
            set
            {
                _teamBlueLevel = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamRedLevel
        {
            get => _teamRedLevel;
            set
            {
                _teamRedLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonVisible
        {
            get => _isLeftChangeButtonVisible;
            set
            {
                _isLeftChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonVisible
        {
            get => _isRightChangeButtonVisible;
            set
            {
                _isRightChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonEnabled
        {
            get => _isLeftChangeButtonEnabled;
            set
            {
                _isLeftChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonEnabled
        {
            get => _isRightChangeButtonEnabled;
            set
            {
                _isRightChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool HasBans
        {
            get => _hasBans;
            set
            {
                _hasBans = value;
                RaisePropertyChanged();
            }
        }

        public bool HasObservers
        {
            get => _hasObservers;
            set
            {
                _hasObservers = value;
                RaisePropertyChanged();
            }
        }

        public bool HasChat
        {
            get => _hasChat;
            set
            {
                _hasChat = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFlyoutLoadingOverlayVisible
        {
            get => _isFlyoutLoadingOverlayVisible;
            set
            {
                _isFlyoutLoadingOverlayVisible = value;
                RaisePropertyChanged();
            }
        }

        public string MatchTitle
        {
            get => _matchTitle;
            set
            {
                _matchTitle = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueName
        {
            get => _teamBlueName;
            set
            {
                _teamBlueName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedName
        {
            get => _teamRedName;
            set
            {
                _teamRedName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueIsWinner
        {
            get => _teamBlueIsWinner;
            set
            {
                _teamBlueIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedIsWinner
        {
            get => _teamRedIsWinner;
            set
            {
                _teamRedIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string MatchLength
        {
            get => _matchLength;
            set
            {
                _matchLength = value;
                RaisePropertyChanged();
            }
        }

        public Color MatchTitleGlowColor
        {
            get => _matchTitleGlowColor;
            set
            {
                _matchTitleGlowColor = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam1Collection
        {
            get => _matchTalentsTeam1Collection;
            set
            {
                _matchTalentsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam2Collection
        {
            get => _matchTalentsTeam2Collection;
            set
            {
                _matchTalentsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam1Collection
        {
            get => _matchStatsTeam1Collection;
            set
            {
                _matchStatsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam2Collection
        {
            get => _matchStatsTeam2Collection;
            set
            {
                _matchStatsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerAdvancedStats> MatchAdvancedStatsTeam1Collection
        {
            get => _matchAdvancedStatsTeam1Collection;
            set
            {
                _matchAdvancedStatsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerAdvancedStats> MatchAdvancedStatsTeam2Collection
        {
            get => _matchAdvancedStatsTeam2Collection;
            set
            {
                _matchAdvancedStatsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchChat> MatchChatCollection
        {
            get => _matchChatCollection;
            set
            {
                _matchChatCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchObserver> MatchObserversCollection
        {
            get => _matchObserversCollection;

            set
            {
                _matchObserversCollection = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand MatchSummaryLeftChangeButtonCommand => new RelayCommand(async () => await ChangeCurrentMatchSummaryAsync(-1));
        public RelayCommand MatchSummaryRightChangeButtonCommand => new RelayCommand(async () => await ChangeCurrentMatchSummaryAsync(1));

        public async Task LoadMatchSummaryAsync(ReplayMatch replayMatch, List<ReplayMatch> matchList)
        {
            if (replayMatch == null)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await LoadMatchSummaryDataAsync(replayMatch);

                    if (matchList == null)
                    {
                        IsLeftChangeButtonEnabled = false;
                        IsLeftChangeButtonVisible = false;
                        IsRightChangeButtonEnabled = false;
                        IsRightChangeButtonVisible = false;
                    }
                    else if (matchList.Count <= 0)
                    {
                        IsLeftChangeButtonEnabled = false;
                        IsLeftChangeButtonVisible = true;
                        IsRightChangeButtonEnabled = false;
                        IsRightChangeButtonVisible = true;
                    }
                    else
                    {
                        IsLeftChangeButtonVisible = true;
                        IsLeftChangeButtonEnabled = replayMatch.ReplayId == matchList[0].ReplayId ? false : true;

                        IsRightChangeButtonVisible = true;
                        IsRightChangeButtonEnabled = replayMatch.ReplayId == matchList[matchList.Count - 1].ReplayId ? false : true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            IsFlyoutLoadingOverlayVisible = false;
            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task LoadMatchSummaryDataAsync(ReplayMatch replayMatch)
        {
            DisposeMatchSummary();

            replayMatch = Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);

            HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);
            SetBackgroundImage(replayMatch.MapName);
            MatchTitleGlowColor = HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor(replayMatch.MapName);
            MatchTitle = $"{replayMatch.MapName} - {HeroesHelpers.GameModes.GetStringFromGameMode(replayMatch.GameMode)} [{replayMatch.TimeStamp}] [{replayMatch.ReplayLength}]";
            MatchLength = $"{replayMatch.ReplayLength.ToString(@"mm\:ss")}";

            // get players info
            var playersList = replayMatch.ReplayMatchPlayers.ToList();
            var playerTalentsList = replayMatch.ReplayMatchPlayerTalents.ToList();
            var playerScoresList = replayMatch.ReplayMatchPlayerScoreResults.ToList();
            var matchMessagesList = replayMatch.ReplayMatchMessage.ToList();
            var matchAwardDictionary = replayMatch.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);
            var matchTeamLevelsList = replayMatch.ReplayMatchTeamLevels.ToList();
            var matchTeamExperienceList = replayMatch.ReplayMatchTeamExperiences.ToList();

            // graphs
            await TeamLevelTimeGraph.SetTeamLevelGraphsAsync(matchTeamLevelsList, playersList[0].IsWinner);
            await TeamExperienceGraph.SetTeamExperienceGraphsAsync(matchTeamExperienceList, playersList[0].IsWinner);

            var playerParties = PlayerParties.FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                MatchPlayerBase matchPlayerBase;
                MatchPlayerTalents matchPlayerTalents;
                MatchPlayerStats matchPlayerStats;
                MatchPlayerAdvancedStats matchPlayerAdvancedStats;

                matchPlayerBase = new MatchPlayerBase(InternalService, Website, player);
                matchPlayerBase.SetPlayerInfo(player.IsAutoSelect, playerParties, matchAwardDictionary);

                if (player.Character != "None")
                {
                    matchPlayerTalents = new MatchPlayerTalents(matchPlayerBase);
                    matchPlayerTalents.SetTalents(playerTalentsList[player.PlayerNumber]);

                    matchPlayerStats = new MatchPlayerStats(matchPlayerBase);
                    matchPlayerStats.SetStats(playerScoresList[player.PlayerNumber], player);

                    matchPlayerAdvancedStats = new MatchPlayerAdvancedStats(matchPlayerStats);
                    matchPlayerAdvancedStats.SetAdvancedStats(playerScoresList[player.PlayerNumber]);

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            await Application.Current.Dispatcher.InvokeAsync(
                                () =>
                            {
                                MatchPlayerStatsTeam1Temp.Add(matchPlayerStats);
                                MatchTalentsTeam1Collection.Add(matchPlayerTalents);
                                MatchAdvancedStatsTeam1Collection.Add(matchPlayerAdvancedStats);
                            }, DispatcherPriority.Render);
                        }
                        else
                        {
                            await Application.Current.Dispatcher.InvokeAsync(
                                () =>
                            {
                                MatchPlayerStatsTeam2Temp.Add(matchPlayerStats);
                                MatchTalentsTeam2Collection.Add(matchPlayerTalents);
                                MatchAdvancedStatsTeam2Collection.Add(matchPlayerAdvancedStats);
                            }, DispatcherPriority.Render);
                        }
                    }
                }

                if (player.Team == 4)
                {
                    MatchObserversCollection.Add(new MatchObserver(matchPlayerBase));
                    HasObservers = true;
                }
            }

            // set the highest values and then add it to the ObservableCollection
            SetHighestTeamStatValues();

            await Application.Current.Dispatcher.InvokeAsync(
                () =>
            {
                foreach (var item in MatchPlayerStatsTeam1Temp)
                    MatchStatsTeam1Collection.Add(item);

                foreach (var item in MatchPlayerStatsTeam2Temp)
                    MatchStatsTeam2Collection.Add(item);
            }, DispatcherPriority.Render);

            // match bans
            if (replayMatch.ReplayMatchTeamBan != null)
            {
                string ban1 = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban0);
                string ban2 = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban1);
                string ban3 = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban0);
                string ban4 = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban1);

                MatchHeroBans.Team0Ban0HeroName = $"{ban1}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(ban1)[0]}";
                MatchHeroBans.Team0Ban1HeroName = $"{ban2}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(ban2)[0]}";
                MatchHeroBans.Team1Ban0HeroName = $"{ban3}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(ban3)[0]}";
                MatchHeroBans.Team1Ban1HeroName = $"{ban4}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(ban4)[0]}";
                MatchHeroBans.Team0Ban0 = HeroesIcons.Heroes().GetHeroPortrait(ban1);
                MatchHeroBans.Team0Ban1 = HeroesIcons.Heroes().GetHeroPortrait(ban2);
                MatchHeroBans.Team1Ban0 = HeroesIcons.Heroes().GetHeroPortrait(ban3);
                MatchHeroBans.Team1Ban1 = HeroesIcons.Heroes().GetHeroPortrait(ban4);

                HasBans = true;
            }

            // match chat
            if (matchMessagesList != null && matchMessagesList.Count > 0)
            {
                foreach (var message in matchMessagesList)
                {
                    if (message.MessageEventType == "SChatMessage")
                    {
                        MatchChat matchChat = new MatchChat();
                        matchChat.SetChatMessages(message);

                        await Application.Current.Dispatcher.InvokeAsync(() => MatchChatCollection.Add(matchChat));
                    }
                }

                if (MatchChatCollection.Count > 0)
                    HasChat = true;
            }

            // Set the match results: total kills, team levels, game time
            MatchResult matchResult = new MatchResult(Database);
            matchResult.SetResult(MatchStatsTeam1Collection.ToList(), MatchStatsTeam2Collection.ToList(), matchTeamLevelsList.ToList(), playersList.ToList());
            SetMatchResults(matchResult);

            await Task.CompletedTask;
        }

        private void SetMatchResults(MatchResult matchResult)
        {
            TeamBlueKills = matchResult.TeamBlueKills;
            TeamRedKills = matchResult.TeamRedKills;

            TeamBlueLevel = matchResult.TeamBlueLevel;
            TeamRedLevel = matchResult.TeamRedLevel;

            TeamBlueName = matchResult.TeamBlue;
            TeamRedName = matchResult.TeamRed;

            TeamBlueIsWinner = matchResult.TeamBlueIsWinner;
            TeamRedIsWinner = matchResult.TeamRedIsWinner;
        }

        private void SetHighestTeamStatValues()
        {
            int? highestSiege1 = MatchPlayerStatsTeam1Temp.Max(x => x.SiegeDamage);
            int? highestSiege2 = MatchPlayerStatsTeam2Temp.Max(x => x.SiegeDamage);

            int? highestHero1 = MatchPlayerStatsTeam1Temp.Max(x => x.HeroDamage);
            int? highestHero2 = MatchPlayerStatsTeam2Temp.Max(x => x.HeroDamage);

            int? highestExp1 = MatchPlayerStatsTeam1Temp.Max(x => x.ExperienceContribution);
            int? highestExp2 = MatchPlayerStatsTeam2Temp.Max(x => x.ExperienceContribution);

            int? highestDamageTaken1 = MatchPlayerStatsTeam1Temp.Max(x => x.DamageTakenRole);
            int? highestDamageTaken2 = MatchPlayerStatsTeam2Temp.Max(x => x.DamageTakenRole);

            int? highestHealing1 = MatchPlayerStatsTeam1Temp.Max(x => x.HealingRole);
            int? highestHealing2 = MatchPlayerStatsTeam2Temp.Max(x => x.HealingRole);

            foreach (var item in MatchPlayerStatsTeam1Temp)
            {
                if (item.SiegeDamage == highestSiege1)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero1)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp1)
                    item.HighestExperience = true;

                if (item.DamageTakenRole == highestDamageTaken1)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing1)
                    item.HighestHealing = true;
            }

            foreach (var item in MatchPlayerStatsTeam2Temp)
            {
                if (item.SiegeDamage == highestSiege2)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero2)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp2)
                    item.HighestExperience = true;

                if (item.DamageTakenRole == highestDamageTaken2)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing2)
                    item.HighestHealing = true;
            }
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.MatchSummaryClosed)
            {
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ReEnableMatchSummaryButton));
            }
        }

        private async Task ChangeCurrentMatchSummaryAsync(int value)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { IsFlyoutLoadingOverlayVisible = true; });

            if (value < 0)
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchLeft));
            else
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchRight));
        }

        private void DisposeMatchSummary()
        {
            foreach (var player in MatchTalentsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchTalentsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchStatsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchStatsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchPlayerStatsTeam1Temp)
                player.Dispose();

            foreach (var player in MatchPlayerStatsTeam2Temp)
                player.Dispose();

            foreach (var player in MatchAdvancedStatsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchAdvancedStatsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchObserversCollection)
                player.Dispose();

            // bans
            MatchHeroBans.Team0Ban0 = null;
            MatchHeroBans.Team0Ban1 = null;
            MatchHeroBans.Team1Ban0 = null;
            MatchHeroBans.Team1Ban1 = null;
            MatchHeroBans.Team0Ban0HeroName = null;
            MatchHeroBans.Team0Ban1HeroName = null;
            MatchHeroBans.Team1Ban0HeroName = null;
            MatchHeroBans.Team1Ban1HeroName = null;

            // chat
            MatchChatCollection = null;

            BackgroundImage = null;

            HasBans = false;
            HasChat = false;
            HasObservers = false;

            MatchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
            MatchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();
            MatchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>();
            MatchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>();
            MatchPlayerStatsTeam1Temp = new Collection<MatchPlayerStats>();
            MatchPlayerStatsTeam2Temp = new Collection<MatchPlayerStats>();
            MatchAdvancedStatsTeam1Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
            MatchAdvancedStatsTeam2Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
            MatchChatCollection = new ObservableCollection<MatchChat>();
            MatchObserversCollection = new ObservableCollection<MatchObserver>();
        }
    }
}
