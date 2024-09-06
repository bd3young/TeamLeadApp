using System;
using System.Collections.Generic;
using TeamLeadApp.ViewModels;
using TeamLeadApp.Views;
using Xamarin.Forms;

namespace TeamLeadApp
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(AddOfficerPage), typeof(AddOfficerPage));
			Routing.RegisterRoute(nameof(AddPositionPage), typeof(AddPositionPage));
			Routing.RegisterRoute(nameof(AddChorePage), typeof(AddChorePage));
			Routing.RegisterRoute(nameof(PositionsListPage), typeof(PositionsListPage));
			Routing.RegisterRoute(nameof(AddRotationPage), typeof(AddRotationPage));
			Routing.RegisterRoute(nameof(RotationPositionsPage), typeof(RotationPositionsPage));
			Routing.RegisterRoute(nameof(AddEhsTimePage), typeof(AddEhsTimePage));
			Routing.RegisterRoute(nameof(AddLvTimePage), typeof(AddLvTimePage));
		}

	}
}
