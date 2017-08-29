namespace ForeignExchange.Models
{
    using SQLite.Net.Attributes;

    public class Rate
    {
        //  Notacion para definr la clave primaria del SQLite   \\
        [PrimaryKey]
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

        //  Este metodo (Metodo sobre escritura) se coloca para que retorne la clave principal de la clase  \\
        public override int GetHashCode()
        {
            return RateId;
        }
    }
}
