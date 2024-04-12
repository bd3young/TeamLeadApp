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
		}

	}
}
