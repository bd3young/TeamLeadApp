using System;
using System.Collections.Generic;
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
	public partial class AddOfficerPage : ContentPage
	{
		public Officer Officer { get; set; }
		public AddOfficerPage()
		{
			InitializeComponent();
			BindingContext = new AddOfficerViewModel();
		}

		public AddOfficerPage(Officer officer)
		{
			InitializeComponent();
			BindingContext = new AddOfficerViewModel();

			if (officer != null)
			{
				((AddOfficerViewModel)BindingContext).Officer = officer;
			}
		}
	}
}