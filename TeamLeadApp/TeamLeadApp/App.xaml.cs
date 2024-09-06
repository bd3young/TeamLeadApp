using System;
using System.IO;
using TeamLeadApp.Models;
using TeamLeadApp.Services;
using TeamLeadApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace TeamLeadApp
{
	public partial class App : Application
	{
		static OfficerService _officerService;
		static PositionService _positionService;
		static ChoreService _choreService;
		static DateService _dateService;
		static RotationService _rotationService;
		static RotationPositionService _rotationPositionService;
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
		public static RotationPositionService RotationPositionService
		{
			get
			{
				if (_rotationPositionService == null)
				{
					_rotationPositionService = new RotationPositionService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RotationPosition.db3"));
				}
				return _rotationPositionService;
			}
		}

		public App()
		{
			InitializeComponent();
			MainPage = new AppShell();
		}

		protected override async void OnStart()
		{
			//App.OfficerService.AddInitOfficers();
			App.DateService.AddInitDates();

			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}
		}

		protected override void OnSleep()
		{
		}

		protected override async void OnResume()
		{
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				MainPage = new AppShell();
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}
		}
	}
}
