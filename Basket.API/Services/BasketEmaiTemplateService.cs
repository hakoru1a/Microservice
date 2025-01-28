using Basket.API.Services.Interfaces;
using Shared.Configurations;

namespace Basket.API.Services
{
    public class BasketEmaiTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public BasketEmaiTemplateService(BackgroundJobSettings backgroundJobSetting) : base(backgroundJobSetting)
        {
        }

        public string GenerateReminderCheckoutOrderEmail(string username)
        {
            var _checkoutUrl = $"{this._backgroundJobSetting.CheckourUrl}/{_backgroundJobSetting.BasketUrl}/{username}";
            var emailText = ReadEmailTemplateContent("reminder-checkout-order");
            var emailReplacedText = emailText.Replace(oldValue: "[username]", newValue: username)
                .Replace(oldValue: "[checkoutUrl]", newValue: _checkoutUrl);

            return emailReplacedText;
        }


    }
}
