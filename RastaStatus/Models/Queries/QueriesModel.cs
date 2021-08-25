namespace RastaStatus.Models.Queries
{
    public class QueriesModel
    {
        public string id { get; set; }
        public string serviceId { get; set; }
        public bool state { get; set; }
        public string date { get; set; }
        public int latency { get; set; }
    }
}