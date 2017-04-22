using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class UserProfileEditModel
    {
        public Gender Gender { get; set; }

        [StringLength(3, ErrorMessage = "fdgdfgfdgdg")]
        public string Location { get; set; }

        [UIHint("MultilineText")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [RegularExpression(@"\d{1,2}/.\d{1,2}/.\d{2,4}", ErrorMessage = "Некорректная дата")]
        public DateTime? Birthday { get; set; }
    }
}