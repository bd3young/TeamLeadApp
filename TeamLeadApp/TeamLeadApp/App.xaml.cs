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
		static ChoreService _choreService;
		static DateService _dateService;
		static RotationService _rotationService;
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
		public static ChoreService ChoreService
		{
			get
			{
				if (_choreService == null)
				{
					_choreService = new ChoreService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Chore.db3"));
				}
				return _choreService;
			}
		}
		public static DateService DateService
		{
			get
			{
				if (_dateService == null)
				{
					_dateService = new DateService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Date.db3"));
				}
				return _dateService;
			}
		}
		public static RotationService RotationService
		{
			get
			{
				if (_rotationService == null)
				{
					_rotationService = new RotationService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Rotation.db3"));
				}
				return _rotationService;
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
