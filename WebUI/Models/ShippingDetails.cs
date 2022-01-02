using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите Ваше имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите хотя бы первый адрес доставки")]
        [Display(Name = "Адрес №1")]
        public string Line1 { get; set; }

        [Display(Name = "Адрес №2")]
        public string Line2 { get; set; }

        [Display(Name = "Адрес №3")]
        public string Line3 { get; set; }

        [Display(Name = "Город проживания")]
        [Required(ErrorMessage = "Укажите город проживания")]
        public string City { get; set; }

        [Display(Name = "Страна проживания")]
        [Required(ErrorMessage = "Укажите страну проживания")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
