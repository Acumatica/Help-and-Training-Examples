using System.Collections.Generic;

namespace PX.SmsProvider.Plivo
{
    public class PlivoSmsProviderFactory : ISmsProviderFactory
    {
        public ISmsProvider Create(IEnumerable<ISmsProviderSetting> settings)
        {
            var provider = new PlivoSmsProvider();
            provider.LoadSettings(settings);
            return provider;
        }

        public ISmsProvider Create()
        {
            var provider = new PlivoSmsProvider();
            return provider;
        }

        public string Description { get; } = Messages.ProviderName;
        public string Name { get; } = typeof(PlivoSmsProvider).FullName;
    }
}
