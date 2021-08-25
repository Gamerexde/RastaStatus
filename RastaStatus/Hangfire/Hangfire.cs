using Hangfire;
using RastaStatus.Datasource;

namespace RastaStatus.Hangfire
{
    public class Hangfire
    {
        private IRecurringJobManager _recurringJobManager;
        
        public Hangfire(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }
    }
}