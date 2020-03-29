using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Genres
    {
        public Genres()
        {
            Films = new HashSet<Films>();
        }

        //[Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Назва жанру")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Назва повинна починатися з прописної букви і містити лише букви")]
        public string Names { get; set; }
        [Display(Name = "Рік створення")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"\d{4}", ErrorMessage = "Рік повинен містити лише цифри та в форматі '####'")]
        public string CreateYear { get; set; }
        [Display(Name = "Творець")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Творець повинен починатися з прописної букви і містити лише букви")]
        public string WhoCreate { get; set; }

        public virtual ICollection<Films> Films { get; set; }
    }
}
