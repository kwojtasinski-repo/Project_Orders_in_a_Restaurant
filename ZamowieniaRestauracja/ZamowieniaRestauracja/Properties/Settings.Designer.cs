﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZamowieniaRestauracja.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=D:\\PROJEKTY\\DOTNET\\ZAMOWIENIAR" +
            "ESTAURACJA\\ZAMOWIENIARESTAURACJA\\DATABASE_OF_RESTAURANTS.MDF;Integrated Security" +
            "=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False")]
        public string D__PROJEKTY_DOTNET_ZAMOWIENIARESTAURACJA_ZAMOWIENIARESTAURACJA_DATABASE_OF_RESTAURANTS_MDFConnectionString {
            get {
                return ((string)(this["D__PROJEKTY_DOTNET_ZAMOWIENIARESTAURACJA_ZAMOWIENIARESTAURACJA_DATABASE_OF_RESTAU" +
                    "RANTS_MDFConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_of_R" +
            "estaurants.mdf;Integrated Security=True")]
        public string Database_of_RestaurantsConnectionString {
            get {
                return ((string)(this["Database_of_RestaurantsConnectionString"]));
            }
        }
    }
}
