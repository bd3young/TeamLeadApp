using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using TeamLeadApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChecklistPage : ContentPage
	{
		ChoreViewModel choreViewModel;
		public ChecklistPage()
		{
			InitializeComponent();
			BindingContext = choreViewModel = new ChoreViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			choreViewModel.OnAppearing();
		}
		private void IsCompleted_CheckedChanged(object sender, PropertyChangedEventArgs e)
		{
			var cb = (CheckBox)sender;

			var viewModel = (ChoreViewModel)BindingContext;
			if (viewModel.UpdateIsCompletedCommand.CanExecute(cb.BindingContext))
			{
				viewModel.UpdateIsCompletedCommand.Execute(cb.BindingContext);
			}
		}
	}
}