﻿using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Icons;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerBase
    {
        public MatchPlayerBase(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile, ReplayMatchPlayer player)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            UserProfile = userProfile;
            Player = player;
        }

        protected MatchPlayerBase(MatchPlayerBase matchPlayerBase)
        {
            Database = matchPlayerBase.Database;
            HeroesIcons = matchPlayerBase.HeroesIcons;
            Player = matchPlayerBase.Player;

            LeaderboardPortrait = matchPlayerBase.LeaderboardPortrait;
            MvpAward = matchPlayerBase.MvpAward;
            PartyIcon = matchPlayerBase.PartyIcon;
            PlayerName = matchPlayerBase.PlayerName;
            CharacterName = matchPlayerBase.CharacterName;
            CharacterTooltip = matchPlayerBase.CharacterTooltip;
            CharacterLevel = matchPlayerBase.CharacterLevel;
            MvpAwardDescription = matchPlayerBase.MvpAwardDescription;
            Silenced = matchPlayerBase.Silenced;
            IsUserPlayer = matchPlayerBase.IsUserPlayer;
        }

        public RelayCommand ShowHotsLogsPlayerProfileCommand => new RelayCommand(ShowHotsLogsPlayerProfile);

        public BitmapImage LeaderboardPortrait { get; private set; }
        public BitmapImage MvpAward { get; private set; }
        public BitmapImage PartyIcon { get; private set; }
        public string PlayerName { get; private set; }
        public string CharacterName { get; private set; }
        public string CharacterTooltip { get; private set; }
        public string CharacterLevel { get; private set; }
        public string MvpAwardDescription { get; private set; }
        public bool Silenced { get; private set; }
        public bool IsUserPlayer { get; private set; }

        public RelayCommand HeroSearchAllMatchCommand => new RelayCommand(HeroSearchAllMatch);
        public RelayCommand HeroSearchQuickMatchCommand => new RelayCommand(HeroSearchQuickMatch);
        public RelayCommand HeroSearchUnrankedDraftCommand => new RelayCommand(HeroSearchUnrankedDraft);
        public RelayCommand HeroSearchHeroLeagueCommand => new RelayCommand(HeroSearchHeroLeague);
        public RelayCommand HeroSearchTeamLeagueCommand => new RelayCommand(HeroSearchTeamLeague);
        public RelayCommand HeroSearchBrawlCommand => new RelayCommand(HeroSearchBrawl);
        public RelayCommand HeroSearchCustomGameCommand => new RelayCommand(HeroSearchCustomGame);

        public RelayCommand PlayerSearchAllMatchCommand => new RelayCommand(PlayerSearchAllMatch);
        public RelayCommand PlayerSearchQuickMatchCommand => new RelayCommand(PlayerSearchQuickMatch);
        public RelayCommand PlayerSearchUnrankedDraftCommand => new RelayCommand(PlayerSearchUnrankedDraft);
        public RelayCommand PlayerSearchHeroLeagueCommand => new RelayCommand(PlayerSearchHeroLeague);
        public RelayCommand PlayerSearchTeamLeagueCommand => new RelayCommand(PlayerSearchTeamLeague);
        public RelayCommand PlayerSearchBrawlCommand => new RelayCommand(PlayerSearchBrawl);
        public RelayCommand PlayerSearchCustomGameCommand => new RelayCommand(PlayerSearchCustomGame);

        public RelayCommand PlayerAndHeroSearchAllMatchCommand => new RelayCommand(PlayerAndHeroSearchAllMatch);
        public RelayCommand PlayerAndHeroSearchQuickMatchCommand => new RelayCommand(PlayerAndHeroSearchQuickMatch);
        public RelayCommand PlayerAndHeroSearchUnrankedDraftCommand => new RelayCommand(PlayerAndHeroSearchUnrankedDraft);
        public RelayCommand PlayerAndHeroSearchHeroLeagueCommand => new RelayCommand(PlayerAndHeroSearchHeroLeague);
        public RelayCommand PlayerAndHeroSearchTeamLeagueCommand => new RelayCommand(PlayerAndHeroSearchTeamLeague);
        public RelayCommand PlayerAndHeroSearchBrawlCommand => new RelayCommand(PlayerAndHeroSearchBrawl);
        public RelayCommand PlayerAndHeroSearchCustomGameCommand => new RelayCommand(PlayerAndHeroSearchCustomGame);

        public RelayCommand CopyHeroNameToClipboardCommand => new RelayCommand(CopyHeroNameToClipboard);
        public RelayCommand CopyPlayerNameToClipboardCommand => new RelayCommand(CopyPlayerNameToClipboard);
        public RelayCommand CopyHeroAndPlayerNameToClipboardCommand => new RelayCommand(CopyHeroAndPlayerNameToClipboard);

        public IBrowserWindowService BrowserWindow
        {
            get { return ServiceLocator.Current.GetInstance<IBrowserWindowService>(); }
        }

        public IMainPageService MainTabs
        {
            get { return ServiceLocator.Current.GetInstance<IMainPageService>(); }
        }

        public IMatchesTabService MatchesTab
        {
            get { return ServiceLocator.Current.GetInstance<IMatchesTabService>(); }
        }

        public IMatchSummaryFlyoutService MatchSummaryFlyout
        {
            get { return ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>(); }
        }

        protected IDatabaseService Database { get; }
        protected IHeroesIconsService HeroesIcons { get; }
        protected IUserProfileService UserProfile { get; }
        protected ReplayMatchPlayer Player { get; }

        public void SetPlayerInfo(bool isAutoSelect, Dictionary<int, PartyIconColor> playerPartyIcons, Dictionary<long, string> matchAwardDictionary)
        {
            var playerInfo = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(Player.PlayerId);

            LeaderboardPortrait = Player.Character != "None" ? HeroesIcons.Heroes().GetHeroLeaderboardPortrait(Player.Character) : null;
            CharacterTooltip = $"{Player.Character}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(Player.Character)[0]}";
            Silenced = Player.IsSilenced;
            CharacterName = Player.Character;

            PlayerName = Database.SettingsDb().UserSettings.IsBattleTagHidden ? HeroesHelpers.BattleTags.GetNameFromBattleTagName(playerInfo.BattleTagName) : playerInfo.BattleTagName;
            IsUserPlayer = (playerInfo.PlayerId == UserProfile.PlayerId && playerInfo.BattleNetRegionId == UserProfile.RegionId) ? true : false;

            if (Player.Team == 4)
                CharacterLevel = "Observer";
            else
                CharacterLevel = isAutoSelect ? "Auto Select" : Player.CharacterLevel.ToString();

            if (playerPartyIcons.ContainsKey(Player.PlayerNumber))
                SetPartyIcon(playerPartyIcons[Player.PlayerNumber]);

            if (matchAwardDictionary.ContainsKey(Player.PlayerId))
                SetMVPAward(matchAwardDictionary[Player.PlayerId]);
        }

        public virtual void Dispose()
        {
            LeaderboardPortrait = null;
            PartyIcon = null;
            MvpAward = null;
            MvpAwardDescription = null;
        }

        private void SetPartyIcon(PartyIconColor icon)
        {
            PartyIcon = HeroesIcons.GetPartyIcon(icon);
        }

        private void SetMVPAward(string awardType)
        {
            string mvpAwardName = null;
            MVPScoreScreenColor teamColor;

            if (Player.Team == 0)
                teamColor = MVPScoreScreenColor.Blue;
            else
                teamColor = MVPScoreScreenColor.Red;

            MvpAward = HeroesIcons.MatchAwards().GetMVPScoreScreenAward(awardType, teamColor, out mvpAwardName);
            MvpAwardDescription = $"{mvpAwardName}{Environment.NewLine}{HeroesIcons.MatchAwards().GetMatchAwardDescription(awardType)}";
        }

        private void ShowHotsLogsPlayerProfile()
        {
            BrowserWindow.CreateBrowserWindow();
        }

        private void HeroSearchAllMatch()
        {
            HeroSearch(Core.MatchesTab.AllMatches);
        }

        private void HeroSearchQuickMatch()
        {
            HeroSearch(Core.MatchesTab.QuickMatch);
        }

        private void HeroSearchUnrankedDraft()
        {
            HeroSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void HeroSearchHeroLeague()
        {
            HeroSearch(Core.MatchesTab.HeroLeague);
        }

        private void HeroSearchTeamLeague()
        {
            HeroSearch(Core.MatchesTab.TeamLeague);
        }

        private void HeroSearchBrawl()
        {
            HeroSearch(Core.MatchesTab.Brawl);
        }

        private void HeroSearchCustomGame()
        {
            HeroSearch(Core.MatchesTab.Custom);
        }

        private void PlayerSearchAllMatch()
        {
            PlayerSearch(Core.MatchesTab.AllMatches);
        }

        private void PlayerSearchQuickMatch()
        {
            PlayerSearch(Core.MatchesTab.QuickMatch);
        }

        private void PlayerSearchUnrankedDraft()
        {
            PlayerSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void PlayerSearchHeroLeague()
        {
            PlayerSearch(Core.MatchesTab.HeroLeague);
        }

        private void PlayerSearchTeamLeague()
        {
            PlayerSearch(Core.MatchesTab.TeamLeague);
        }

        private void PlayerSearchBrawl()
        {
            PlayerSearch(Core.MatchesTab.Brawl);
        }

        private void PlayerSearchCustomGame()
        {
            PlayerSearch(Core.MatchesTab.Custom);
        }

        private void PlayerAndHeroSearchAllMatch()
        {
            PlayerAndHeroSearch(Core.MatchesTab.AllMatches);
        }

        private void PlayerAndHeroSearchQuickMatch()
        {
            PlayerAndHeroSearch(Core.MatchesTab.QuickMatch);
        }

        private void PlayerAndHeroSearchUnrankedDraft()
        {
            PlayerAndHeroSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void PlayerAndHeroSearchHeroLeague()
        {
            PlayerAndHeroSearch(Core.MatchesTab.HeroLeague);
        }

        private void PlayerAndHeroSearchTeamLeague()
        {
            PlayerAndHeroSearch(Core.MatchesTab.TeamLeague);
        }

        private void PlayerAndHeroSearchBrawl()
        {
            PlayerAndHeroSearch(Core.MatchesTab.Brawl);
        }

        private void PlayerAndHeroSearchCustomGame()
        {
            PlayerAndHeroSearch(Core.MatchesTab.Custom);
        }

        private void CopyHeroNameToClipboard()
        {
            Clipboard.SetText(CharacterName);
        }

        private void CopyPlayerNameToClipboard()
        {
            Clipboard.SetText(PlayerName);
        }

        private void CopyHeroAndPlayerNameToClipboard()
        {
            Clipboard.SetText($"{CharacterName} - {PlayerName}");
        }

        private void HeroSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToPage(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedCharacter = CharacterName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }

        private void PlayerSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToPage(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedBattleTagName = PlayerName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }

        private void PlayerAndHeroSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToPage(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedBattleTagName = PlayerName, SelectedCharacter = CharacterName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }
    }
}
