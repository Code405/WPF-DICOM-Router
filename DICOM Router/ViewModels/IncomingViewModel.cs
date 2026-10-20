using DICOM_Router.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Threading;

namespace DICOM_Router.ViewModels
{
    public class IncomingViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<IncomingStudy> _incomingStudiesList;
        public ObservableCollection<IncomingStudy> IncomingStudiesList
        {
            get => _incomingStudiesList;
            set
            {
                _incomingStudiesList = value;
                OnPropertyChanged(nameof(IncomingStudiesList));
            }
        }

        private readonly DispatcherTimer _refreshTimer;

        public IncomingViewModel()
        {
            IncomingStudiesList = new ObservableCollection<IncomingStudy>();
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
                var freshData = context.IncomingStudies.ToList();

                // Simple refresh approach: clear and re-populate
                IncomingStudiesList.Clear();
                foreach (var study in freshData)
                {
                    IncomingStudiesList.Add(study);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
