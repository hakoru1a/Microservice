namespace Basket.API.Services.Interfaces
{
    public interface IEmailTemplateService
    {
       public string GenerateReminderCheckoutOrderEmail(string usernname);


    }
}
