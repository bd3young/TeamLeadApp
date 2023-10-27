using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TeamLeadApp.Models;

namespace TeamLeadApp.ViewModels
{
	public class BaseOfficerViewModel:INotifyPropertyChanged
	{
		private Officer _officer;

		public Officer Officer 
		{ 
			get { return _officer; }
			set { _officer = value;OnPropertyChanged(); }
		}
		
		bool isBusy = false;
		public bool IsBusy 
		{
			get { return isBusy; }
			set
			{
				SetProperty(ref isBusy, value);
			}
		}

		protected bool SetProperty<T>(ref T backingStore, T value,
			[CallerMemberName] string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "") 
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
