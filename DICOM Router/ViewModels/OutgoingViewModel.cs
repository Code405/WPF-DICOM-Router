using DICOM_Router.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Threading;


namespace DICOM_Router.ViewModels
{
    public class OutgoingViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<OutgingStudy> _outgoingStudiesList;
        public ObservableCollection<OutgingStudy> OutgoingStudiesList
        {
            get => _outgoingStudiesList;
            set
            {
                _outgoingStudiesList = value;
                OnPropertyChanged(nameof(OutgoingStudiesList));
            }
        }

        private readonly DispatcherTimer _refreshTimer;

        public OutgoingViewModel()
        {
            OutgoingStudiesList = new ObservableCollection<OutgingStudy>();
            LoadData();

            // Configure timer to poll the database periodically (e.g., every 5 seconds)
            _refreshTimer = new DispatcherTimer
            {
                Interval = System.TimeSpan.FromSeconds(5)
            };
            _refreshTimer.Tick += (s, e) => LoadData();
            _refreshTimer.Start();
        }

        private void LoadData()
        {
            using (var context = new DatabaseContext())
            {
                var freshData = context.OutgingStudies.ToList();

                // Simple refresh approach: clear and re-populate
                OutgoingStudiesList.Clear();
                foreach (var study in freshData)
                {
                    OutgoingStudiesList.Add(study);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
