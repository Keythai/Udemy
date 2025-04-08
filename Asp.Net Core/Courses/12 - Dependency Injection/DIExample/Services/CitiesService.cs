using ServiceContracts;
namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;
        private Guid _serviceInstanceId;
        public Guid ServiceInstanceId { 
            get {
                return _serviceInstanceId;
            } }
        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid(); // to test the lifetime
            _cities = new List<string>()
            {
                "London",
                "Paris",
                "New York",
                "Tokyo",
                "Rome"
            };
        }
        public List<string> GetCities()
        {
            return _cities;
        }

        // close db connection
        public void Dispose()
        {
        }
    }
}
