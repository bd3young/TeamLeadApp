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
	public partial class AddLvTimePage : ContentPage
	{
		public AddLvTimePage (Officer officer)
		{
			InitializeComponent();
			BindingContext = new AddLvTimeViewModel();

			((AddLvTimeViewModel)BindingContext).Officer = officer;
		}
	}
}