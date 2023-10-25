using System;
using System.IO;
using TeamLeadApp.Models;
using TeamLeadApp.Services;
using TeamLeadApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp
{
	public partial class App : Application
	{
		static OfficerService _officerService;
		public static OfficerService OfficerService
		{
			get 
			{
				if (_officerService == null) 
				{
					_officerService = new OfficerService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Officer.db3"));
				}
				return _officerService;
			}
		}

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
