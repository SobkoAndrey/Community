using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class UserProfileEditModel
    {
        [Display(Name = "Пол")]
        public Gender Gender { get; set; }

        [Display(Name = "Населенный пункт")]
        [StringLength(30, ErrorMessage = "Название населенного пункта не должено превышать 30 символов")]
        public string Location { get; set; }

        [Display(Name = "Информация о вас")]
        [StringLength(200, ErrorMessage = "Информация о вас не должна превышать 200 символов")]
        public string Description { get; set; }

        [Display(Name = "День рождения в формате гггг-мм-дд")]
        public DateTime? Birthday { get; set; }
    }

    public class GroupCreationModel
    {
        [Required]
        [Display(Name = "Название группы")]
        [StringLength(50, ErrorMessage = "Название группы не должно превышать 50 символов")]
        public string Name { get; set; }

        [Display(Name = "Информация о группе")]
        [StringLength(200, ErrorMessage = "Информация о группе не должна превышать 200 символов")]
        public string Description { get; set; }
    }

}