using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Countries
    {
        public Countries()
        {
            Films = new HashSet<Films>();
        }

        //[Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Назва країни")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Назва повинна починатися з прописної букви і містити лише букви")]
        public string Name { get; set; }
        [Display(Name = "Рік незалежності")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"\d{4}", ErrorMessage = "Рік повинен містити лише цифри та в форматі '####'")]
        public string YearOfIndependency { get; set; }
        [Display(Name = "Кількість фільмів")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Кількість повинна містити лише цифри")]
        public string CountOfFilms { get; set; }

        public virtual ICollection<Films> Films { get; set; }
    }
}
