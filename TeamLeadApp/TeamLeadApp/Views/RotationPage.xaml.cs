﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RotationPage : ContentPage
	{
		RotationViewModel rotationViewModel;
		public RotationPage ()
		{
			InitializeComponent ();
			BindingContext = rotationViewModel = new RotationViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			rotationViewModel.OnAppearing();
		}
		private void BreakOne_CheckedChanged(object sender, PropertyChangedEventArgs e)
		{
			var cb = (CheckBox)sender;

			var viewModel = (RotationViewModel)BindingContext;
			if (viewModel.UpdateBreakOneCommand.CanExecute(null))
			{
				viewModel.UpdateBreakOneCommand.Execute(cb.BindingContext);
			}
		}
		private void BreakTwo_CheckedChanged(object sender, PropertyChangedEventArgs e)
		{
			var cb = (CheckBox)sender;

			var viewModel = (RotationViewModel)BindingContext;
			if (viewModel.UpdateBreakTwoCommand.CanExecute(cb.BindingContext))
			{
				viewModel.UpdateBreakTwoCommand.Execute(cb.BindingContext);
			}
		}
		private void Lunch_CheckedChanged(object sender, PropertyChangedEventArgs e)
		{
			var cb = (CheckBox)sender;

			var viewModel = (RotationViewModel)BindingContext;
			if (viewModel.UpdateLunchCommand.CanExecute(cb.BindingContext)) {
				viewModel.UpdateLunchCommand.Execute(cb.BindingContext);
			}
		}

	}
}