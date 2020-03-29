using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas
{
    public partial class Buffets
    {  // [Display(Name="Код")]
        //[Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int Id { get; set; }
        [Display(Name = "Товар")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Товар повинна починатися з прописної букви і містити лише букви")]
        public string Product { get; set; }
        [Display(Name = "Персонал")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int StaffId { get; set; }
        [Display(Name = "Графік")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        [RegularExpression(@"^[A-Z]+[a-zA-z""'\s-]*$", ErrorMessage = "Графік повинна починатися з прописної букви і містити лише букви")]
        public string Schedule { get; set; }
        [Display(Name = "Кінотеатр")]
        [Required(ErrorMessage = "Помилка, заповнити повністю")]
        public int CinemaId { get; set; }

        [Display(Name = "Кінотеатр")]
        public virtual Cinemas Cinema { get; set; }
        [Display(Name = "Персонал")]
        public virtual Staffs Staff { get; set; }
    }
}
