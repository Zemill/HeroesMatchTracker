﻿using HeroesIcons;
using HeroesParserData.Properties;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesParserData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HeroesInfo HeroesInfo { get; set; }
        public static string NewLatestDirectory { get; set; }
        public static bool IsProcessingReplays { get; set; }
        public static System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public static bool NewReleaseApplied { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            NewReleaseApplied = false;

            // set default location
            if (string.IsNullOrEmpty(Settings.Default.ReplaysLocation))
                Settings.Default.ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");

            // add custom accent and theme resource dictionaries
            //ThemeManager.AddAccent("Theme", new Uri("pack://application:,,,/Resources/Theme.xaml"));

            //// get the theme from the current application
            //var theme = ThemeManager.DetectAppStyle(Application.Current);

            //// now use the custom accent
            //ThemeManager.ChangeAppStyle(Application.Current,
            //                        ThemeManager.GetAccent("Theme"),
            //                        theme.Item1);

            Task.Run(async () =>
            {
                await PeriodicallySaveSettings();
            });

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Default.Save();

            if (NotifyIcon != null)
            {
                NotifyIcon.Visible = false;
            }

            // this should only trigger if the update is applied through the settings menu
            if (NewReleaseApplied)
                AutoUpdater.CopyDatabaseToLatestRelease();

            base.OnExit(e);
        }

        private async Task PeriodicallySaveSettings()
        {
            while (true)
            {
                await Task.Delay(300000); // 5 minutes
                Settings.Default.Save();
            }
        }
    }
}
