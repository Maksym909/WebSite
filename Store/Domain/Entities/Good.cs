using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Good
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name=" Название")]
        [Required(ErrorMessage="Введите пожалуйса название товара")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name=" Описание")]
        [Required(ErrorMessage ="Введите описание")]
        public string Description { get; set; }

        [Display(Name=" Категория")]
        [Required(ErrorMessage ="Введите категорию")]
        public string Category { get; set; }

        [Display(Name = " Цена (грн)")]
        [Required]
        [Range (0.01 , double.MaxValue, ErrorMessage="Введите корректную цену")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

    }
}
