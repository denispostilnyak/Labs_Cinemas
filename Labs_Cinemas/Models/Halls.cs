using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Halls
    {
       // [Display(Name = "Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Назва зали")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Назва повинна починатися з прописної букви і містити лише букви")]
        public string Name { get; set; }
        [Display(Name = "Кількість")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Кількість повинна містити лише цифри")]
        public int PeopleAmount { get; set; }
        [Display(Name = "Контролер")]
        public int ControlerId { get; set; }
        [Display(Name = "Кінотеатр")]
        public int CinemaId { get; set; }
        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public DateTime DateTime { get; set; }
        [Display(Name = "Ціна")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Кількість повинна містити лише цифри")]
        public string Price { get; set; }
        [Display(Name = "Фільм")]
        public int FilmId { get; set; }

        [Display(Name = "Кінотеатр")]
        public virtual Cinemas Cinema { get; set; }
        [Display(Name = "Контролер")]
        public virtual Staffs Controler { get; set; }
        [Display(Name = "Фільм")]
        public virtual Films Film { get; set; }
    }
}
