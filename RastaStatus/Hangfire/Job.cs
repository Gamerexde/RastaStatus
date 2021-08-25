using System;

namespace RastaStatus.Hangfire
{
    public abstract class Job
    {
        public Job()
        {
            Start();
        }

        public abstract void Start();
    }
}