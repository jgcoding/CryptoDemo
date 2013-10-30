using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Crypto
{
    /// <summary>
    /// Overlay utility class for working with encrytped app.config settings
    /// </summary>
    public class CryptoConfiguration
	{
		#region constructor
		/// <summary>
		/// Private static constructor ensuring a singleton instance of the class.
		/// </summary>
		private static readonly CryptoConfiguration cryptoConfig = new CryptoConfiguration();

		/// <summary>
		/// Default constructor
		/// </summary>
		private CryptoConfiguration()
		{

		}

		/// <summary>
		/// Static singleton constructor
		/// </summary>
		public static CryptoConfiguration CryptoConfig
		{
			get
			{
				return cryptoConfig;
			}
		}
		#endregion

		#region private instance fields
		static String m_configCipherResult;
		static Boolean? m_configProtected;
		static Configuration m_config;
		static DateTime m_lastModified;
		static AppSettingsSection m_appSettings;
		#endregion

		#region public properties
		/// <summary>
		/// Gets the result from action taken on an app.config
		/// </summary>
		public static String ConfigCipherResult
		{
			get
			{
				return m_configCipherResult;
			}
		}

		/// <summary>
		/// Gets the current protection state of the appSettings section
		/// </summary>
		public static Boolean? ConfigProtected
		{
			get
			{
				return m_configProtected;
			}
		}

		/// <summary>
		/// Gets an instance of the app.config file
		/// </summary>
		public static Configuration GetConfig
		{
			get
			{
				if (m_config == null)
				{
					m_config = ConfigurationManager.OpenExeConfiguration("CryptoManager.exe");
					m_lastModified = File.GetLastWriteTimeUtc(m_config.FilePath);
				}
				return m_config;
			}
		}

		/// <summary>
		/// Gets or sets the date the app.config file was last updated.
		/// Used in determining whether changes have been made to the app.config
		/// in memory which need to be persisted.
		/// </summary>
		public static DateTime LastUpdated
		{
			get
			{
				if (m_config == null)
				{
					m_lastModified = File.GetLastWriteTimeUtc(GetConfig.FilePath);
				}
				return m_lastModified;
			}
			set { m_lastModified = value; }
		}
		#endregion

		#region public and private methods
		/// <summary>
		/// Gets the app.config appSettings section
		/// </summary>
		public static AppSettingsSection GetAppSettings
		{
			get
			{
				if (m_appSettings == null)
				{
					m_appSettings = GetConfig.GetSection("appSettings") as AppSettingsSection;
					if (m_appSettings.SectionInformation.IsProtected)
					{
						// Remove encryption.
						m_appSettings.SectionInformation.UnprotectSection();
					}
					m_configProtected = m_appSettings.SectionInformation.IsProtected;
				}
				return m_appSettings;
			}
		}

		/// <summary>
		/// Called whenever the program is closed or terminated
		/// </summary>
		public static void Destroy()
		{
			SaveConfig();
		}

		/// <summary>
		/// Encrypts and saves any changes made to the app.config appSettings
		/// </summary>
		public static void SaveConfig()
		{
			// if no changes are detected ignore the request
			if (File.GetLastWriteTimeUtc(GetConfig.FilePath) == m_lastModified)
			{
				return;
			}
			// Takes the executable file name without the
			// .config extension.
			try
			{
				if (!GetAppSettings.SectionInformation.IsProtected)
				{
					// Encrypt the section.
					GetAppSettings.SectionInformation.ProtectSection(
						"DataProtectionConfigurationProvider");
				}
				// Save the current configuration.
				m_config.Save();
				m_configProtected = GetAppSettings.SectionInformation.IsProtected;
				m_configCipherResult = String.Format("Protected={0}",
					GetAppSettings.SectionInformation.IsProtected);
			}
			catch (Exception ex)
			{
				m_configCipherResult = String.Format("{0}", ex.Message);
			}
		}
		#endregion
		
    }
}
