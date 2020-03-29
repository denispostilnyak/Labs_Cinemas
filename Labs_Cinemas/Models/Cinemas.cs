using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Labs_Cinemas
{
    public partial class Cinemas
    {
        public Cinemas()
        {
            Buffets = new HashSet<Buffets>();
            Halls = new HashSet<Halls>();
        }

        //[Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name="Назва кінотеатру")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Назва повинна починатися з прописної букви і містити лише букви")]
        public string Name { get; set; }
        [Display(Name = "Місто")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Місто повинно починатися з прописної букви і містити лише букви")]
        public string City { get; set; }
        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public string Adress { get; set; }
        [Display(Name = "Контакти")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public string Contact { get; set; }

        public virtual ICollection<Buffets> Buffets { get; set; }
        public virtual ICollection<Halls> Halls { get; set; }
    }
}
