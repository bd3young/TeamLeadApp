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
	public partial class AddRotationPage : ContentPage
	{
		public new Rotation Rotation { get; set; }
		public AddRotationPage()
		{
			InitializeComponent();
			BindingContext = new AddRotationViewModel();
		}
		public AddRotationPage(Rotation rotation)
		{
			InitializeComponent();
			BindingContext = new AddRotationViewModel();

			if (rotation != null)
			{
				((AddRotationViewModel)BindingContext).Rotation = rotation;
			}
		}
	}
}