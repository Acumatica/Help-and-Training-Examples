using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PX.Data;

namespace PX.SmsProvider.Plivo
{
    public class PlivoSmsProvider : ISmsProvider
    {
        #region DetailIDs const
        private const string AuthID_DetailID = "AUTH_ID";
        private const string AuthToken_DetailID = "AUTH_TOKEN";
        private const string FromPhoneNbr_DetailID = "FROM_PHONE_NBR";
        #endregion

        private string m_AuthID;
        public string AuthID { get { return m_AuthID; } }

        private string m_AuthToken;
        public string AuthToken { get { return m_AuthToken; } }

        private string m_FromPhoneNbr;
        public string FromPhoneNbr { get { return m_FromPhoneNbr; } }

        public IEnumerable<PXFieldState> ExportSettings
        {
            get
            {
                var settings = new List<PXFieldState>();

                var authID = (PXStringState)PXStringState.CreateInstance(
                    m_AuthID,
                    null,
                    false,
                    AuthID_DetailID,
                    null,
                    1,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                authID.DisplayName = Messages.AuthID_DetailID_Display;
                settings.Add(authID);
                var authToken = (PXStringState)PXStringState.CreateInstance(
                    m_AuthToken,
                    null,
                    false,
                    AuthToken_DetailID,
                    null,
                    1,
                    "*",
                    null,
                    null,
                    null,
                    null
                );
                authToken.DisplayName = Messages.AuthToken_DetailID_Display;
                settings.Add(authToken);

                var fromPhoneNbr = (PXStringState)PXStringState.CreateInstance(
                    m_FromPhoneNbr,
                    null,
                    false,
                    FromPhoneNbr_DetailID,
                    null,
                    1,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                fromPhoneNbr.DisplayName = Messages.FromPhoneNbr_DetailID_Display;
                settings.Add(fromPhoneNbr);

                return settings;
            }
        }

        public void LoadSettings(IEnumerable<ISmsProviderSetting> settings)
        {
            foreach (ISmsProviderSetting detail in settings)
            {
                switch (detail.Name.ToUpper())
                {
                    case AuthID_DetailID: m_AuthID = detail.Value; break;
                    case AuthToken_DetailID: m_AuthToken = detail.Value; break;
                    case FromPhoneNbr_DetailID: m_FromPhoneNbr = detail.Value; break;
                }
            }
        }

        public async Task SendMessageAsync(SendMessageRequest request, CancellationToken cancellation)
        {
            PlivoService rs = new PlivoService(m_AuthID, m_AuthToken);
            PlivoResponse twResonse = await rs
                .PostAsync(BuildMessageParameters(request), cancellation).ConfigureAwait(false);
        }

        private List<KeyValuePair<string, string>> BuildMessageParameters(SendMessageRequest request)
        {
            List<KeyValuePair<string, string>> list = CreateMessageParameters;
            list.Add(new KeyValuePair<string, string>("src", m_FromPhoneNbr));
            list.Add(new KeyValuePair<string, string>("dst", request.RecepientPhoneNbr));
            list.Add(new KeyValuePair<string, string>("text", request.RecepientSMSMessage));
            return list;
        }

        private List<KeyValuePair<string, string>> CreateMessageParameters
        {
            get
            {
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                return list;
            }
        }
    }
}
