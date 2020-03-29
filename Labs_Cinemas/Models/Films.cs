using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Films
    {
        public Films()
        {
            Halls = new HashSet<Halls>();
        }

       // [Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }
        [Display(Name = "Рік випуску")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"\d{4}", ErrorMessage = "Рік повинен містити лише цифри та в форматі '####'")]
        public string Years { get; set; }
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }
        [Display(Name = "Назва фільму")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Назва повинна починатися з прописної букви і містити лише букви")]
        public string Name { get; set; }
        [Display(Name = "Тривалість(хв)")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Кількість повинна містити лише цифри")]
        public int Duration { get; set; }
        [Display(Name = "Вікова категорія")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"\d{2}", ErrorMessage = "Кількість повинна містити лише цифри та в форматі '##'")]
        public int? Age { get; set; }
        [Display(Name = "Країна")]
        public virtual Countries Country { get; set; }
        [Display(Name = "Жанр")]
        public virtual Genres Genre { get; set; }
        public virtual ICollection<Halls> Halls { get; set; }
    }
}
