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
	public partial class AddChorePage : ContentPage
	{
		public Chore Chore { get; set; }
		public AddChorePage()
		{
			InitializeComponent();
			BindingContext = new AddChoreViewModel();
		}
		public AddChorePage(Chore chore)
		{
			InitializeComponent();
			BindingContext = new AddChoreViewModel();

			if (chore != null)
			{
				((AddChoreViewModel)BindingContext).Chore = chore;
			}
		}
	}
}