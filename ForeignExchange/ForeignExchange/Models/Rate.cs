namespace ForeignExchange.Models
{

    public class Rate
    {
        public int RateId
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public float TaxRate
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
