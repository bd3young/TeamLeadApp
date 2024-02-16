using System;
using System.Collections.Generic;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
    public class AddPositionViewModel : BasePositionViewModel
    {
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public AddPositionViewModel()
		{
			SaveCommand = new Command(OnSave);
			CancelCommand = new Command(OnCancel);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();

			Position = new Position();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			var position = Position;
			await App.PositionService.AddProductAsync(position);

			await Shell.Current.GoToAsync("..");
		}
	}
}
