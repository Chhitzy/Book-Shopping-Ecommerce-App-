using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Ecommerce_App.Utility
{
    public class TwilioService
    {
        private readonly string _accountsid;
        private readonly string _authtoken;
        private readonly string _phone;

        public TwilioService(IConfiguration configuration)
        {
            _accountsid = configuration["Twilio:AccountSID"];
            _authtoken = configuration["Twilio:AuthToken"];
            _phone = configuration["Twilio:FromPhone"];
        }

        public void sendsms(string phone, string message)
        {
            TwilioClient.Init(_accountsid, _authtoken);

            var sms = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(_phone),
                to: new Twilio.Types.PhoneNumber(phone));
            
        }

    }
}
