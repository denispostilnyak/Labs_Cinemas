using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Staffs
    {
        public Staffs()
        {
            Buffets = new HashSet<Buffets>();
            Halls = new HashSet<Halls>();
        }

        //[Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Ім'я повинно починатися з прописної букви і містити лише букви")]
        public string Name { get; set; }
        [Display(Name = "Позиція")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Позиція повинна починатися з прописної букви і містити лише букви")]
        public string Position { get; set; }
        [Display(Name = "Вік")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"\d{2}", ErrorMessage = "Вік повинен містити лише цифри та в форматі '##'")]
        public int? Age { get; set; }
        [Display(Name = "Посада")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Посада повинна починатися з прописної букви і містити лише букви")]
        public string JobPlace { get; set; }
        [Display(Name = "Контакти")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public string Contact { get; set; }

        public virtual ICollection<Buffets> Buffets { get; set; }
        public virtual ICollection<Halls> Halls { get; set; }
    }
}
