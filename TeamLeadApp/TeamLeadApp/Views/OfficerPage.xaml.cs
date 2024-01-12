﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using TeamLeadApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OfficerPage : ContentPage
	{
		OfficerViewModel officerViewModel;
		public OfficerPage()
		{

			App.OfficerService.AddInitOfficers();

			
			InitializeComponent();
			BindingContext = officerViewModel = new OfficerViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			officerViewModel.OnAppearing();
		}
	}
}