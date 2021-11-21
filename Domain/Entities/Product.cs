using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue=false)]
        public int ProductId { get; set; }

        [Display(Name="Наименование")]
        [Required(ErrorMessage = "Пожалуйста, введите наименование товара")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание товара")]
        public string Description { get; set; }

        [Display(Name="Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для товара")]
        public string Category { get; set; }

        [Display(Name="Цена (в Br)")]
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Пожалуйста, введите сторого положительное значение для цены (не менее 0.01 Br)")]
        public decimal Price { get; set; }
    }
}
