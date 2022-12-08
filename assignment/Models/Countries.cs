namespace assignment.Models
{
    public class CountryDetails {
        public string Country { get; set; }
        public string Region { get; set; }
    }

    public class Countries
    {
        public List<CountryDetails> data { get; set; }
    }
}
