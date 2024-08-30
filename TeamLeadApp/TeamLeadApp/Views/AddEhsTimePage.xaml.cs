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
	public partial class AddEhsTimePage : ContentPage
	{
		public AddEhsTimePage (Officer officer)
		{
			InitializeComponent ();
			BindingContext = new AddTimeViewModel();

			((AddTimeViewModel)BindingContext).Officer = officer;
		}
	}
}