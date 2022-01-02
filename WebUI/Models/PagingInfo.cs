using System;

namespace WebUI.Models
{
    public class PagingInfo
    {
        // количество товаров
        public int TotalItems { get; set; }

        // количество товаров на одной странице
        public int ItemsPerPage { get; set; }

        // номер текущей страницы
        public int CurrentPage { get; set; }

        // общее количество страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}