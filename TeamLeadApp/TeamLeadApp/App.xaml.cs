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
		static PositionService _positionService;
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
		public static PositionService PositionService
		{
			get
			{
				if (_positionService == null)
				{
					_positionService = new PositionService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Position.db3"));
				}
				return _positionService;
			}
		}

		public App()
		{
			InitializeComponent();
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
