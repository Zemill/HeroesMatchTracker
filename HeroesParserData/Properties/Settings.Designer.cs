﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeroesParserData.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ReplaysLocation {
            get {
                return ((string)(this["ReplaysLocation"]));
            }
            set {
                this["ReplaysLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://github.com/koliva8245/HeroesParserData")]
        public string UpdateUrl {
            get {
                return ((string)(this["UpdateUrl"]));
            }
            set {
                this["UpdateUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ReplayWatchCheckBox {
            get {
                return ((bool)(this["ReplayWatchCheckBox"]));
            }
            set {
                this["ReplayWatchCheckBox"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ReplayAutoScanCheckBox {
            get {
                return ((bool)(this["ReplayAutoScanCheckBox"]));
            }
            set {
                this["ReplayAutoScanCheckBox"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ParsedDateTimeCheckBox {
            get {
                return ((bool)(this["ParsedDateTimeCheckBox"]));
            }
            set {
                this["ParsedDateTimeCheckBox"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime ReplaysLatestSaved {
            get {
                return ((global::System.DateTime)(this["ReplaysLatestSaved"]));
            }
            set {
                this["ReplaysLatestSaved"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime ReplaysLastSaved {
            get {
                return ((global::System.DateTime)(this["ReplaysLastSaved"]));
            }
            set {
                this["ReplaysLastSaved"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Season 2")]
        public string SelectedSeason {
            get {
                return ((string)(this["SelectedSeason"]));
            }
            set {
                this["SelectedSeason"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsAutoUpdates {
            get {
                return ((bool)(this["IsAutoUpdates"]));
            }
            set {
                this["IsAutoUpdates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IsMinimizeToTray {
            get {
                return ((bool)(this["IsMinimizeToTray"]));
            }
            set {
                this["IsMinimizeToTray"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsBattleTagHidden {
            get {
                return ((bool)(this["IsBattleTagHidden"]));
            }
            set {
                this["IsBattleTagHidden"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsIncludeSubDirectories {
            get {
                return ((bool)(this["IsIncludeSubDirectories"]));
            }
            set {
                this["IsIncludeSubDirectories"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string UserBattleTagName {
            get {
                return ((string)(this["UserBattleTagName"]));
            }
            set {
                this["UserBattleTagName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string UserPlayerId {
            get {
                return ((string)(this["UserPlayerId"]));
            }
            set {
                this["UserPlayerId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HeroesParserData.db")]
        public string DatabaseFile {
            get {
                return ((string)(this["DatabaseFile"]));
            }
            set {
                this["DatabaseFile"] = value;
            }
        }
    }
}
