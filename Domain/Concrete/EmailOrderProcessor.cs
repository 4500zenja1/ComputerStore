using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "zhenya.kurmysa@mail.ru";
        public string MailFromAddress = "example@mail.ru";
        public bool UseSsl = true;
        public string Username = "4500zenja";
        public string Password = "#5)qcbIYhO0835&#";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"d:\Emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using var smtpClient = new SmtpClient();
            smtpClient.EnableSsl = emailSettings.UseSsl;
            smtpClient.Host = emailSettings.ServerName;
            smtpClient.Port = emailSettings.ServerPort;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = 
                new NetworkCredential(emailSettings.Username, emailSettings.Password);

            if (emailSettings.WriteAsFile)
            {
                smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                smtpClient.EnableSsl = false;
            }

            StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары:");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                body.AppendFormat("{0} x {1} (итого: {2} Br.)\n",
                    line.Quantity, line.Product.Name, subtotal);
            }

            body.AppendFormat("Общая стоимость: {0} Br.\n", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Доставка:")
                .AppendLine("Имя: " + shippingInfo.Name)
                .AppendLine("Адрес № 1: " + shippingInfo.Line1)
                .AppendLine("Адрес № 2: " + (shippingInfo.Line2 ?? "не указан"))
                .AppendLine("Адрес № 3: " + (shippingInfo.Line3 ?? "не указан"))
                .AppendLine("Город: " + shippingInfo.City)
                .AppendLine("Страна: " + shippingInfo.Country)
                .AppendLine("---")
                .AppendFormat("Подарочная упаковка: {0}",
                    shippingInfo.GiftWrap ? "Да" : "Нет");

            MailMessage mailMessage = new(
                                   emailSettings.MailFromAddress,   // От кого
                                   emailSettings.MailToAddress,     // Кому
                                   "Новый заказ отправлен!",        // Тема
                                   body.ToString());                // Тело письма

            if (emailSettings.WriteAsFile)
            {
                mailMessage.BodyEncoding = Encoding.UTF8;
            }

            smtpClient.Send(mailMessage);
        }
    }
}
