namespace ForeignExchange.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Lenguages
    {
        static Lenguages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string AmountValidation
        {
            get { return Resource.AmountValidation; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string Title
        {
            get { return Resource.Title; }
        }

        public static string AmountNumericValidation
        {
            get { return Resource.AmountNumericValidation; }
        }

        public static string SourceRateValidation
        {
            get { return Resource.SourceRateValidation; }
        }

        public static string TargetRateValidation
        {
            get { return Resource.TargetRateValidation; }
        }

        public static string LabelAmount
        {
            get { return Resource.LabelAmount; }
        }

        public static string PlaceHolrderAmount
        {
            get { return Resource.PlaceHolrderAmount; }
        }

        public static string LabelSourceRate
        {
            get { return Resource.LabelSourceRate; }
        }

        public static string TitleSourceRate
        {
            get { return Resource.TitleSourceRate; }
        }

        public static string LabelTargetRate
        {
            get { return Resource.LabelTargetRate; }
        }

        public static string TitleTargetRate
        {
            get { return Resource.TitleTargetRate; }
        }

        public static string CommandConvert
        {
            get { return Resource.CommandConvert; }
        }

        public static string ComandClear
        {
            get { return Resource.ComandClear; }
        }

        public static string TitleLoadRate
        {
            get { return Resource.TitleLoadRate; }
        }

        public static string TitleReadyConvert
        {
            get { return Resource.TitleReadyConvert; }
        }

        public static string TitleTheAmount
        {
            get { return Resource.TitleTheAmount; }
        }

        public static string TitleIn
        {
            get { return Resource.TitleIn; }
        }

        public static string TitleIsEqual
        {
            get { return Resource.TitleIsEqual; }
        }

        public static string TitleSettingsInternet
        {
            get { return Resource.TitleSettingsInternet; }
        }

        public static string TitleAccessInternet
        {
            get { return Resource.TitleAccessInternet; }
        }

        public static string TitleStatusInternet
        {
            get { return Resource.TitleStatusInternet; }
        }

    }
}
