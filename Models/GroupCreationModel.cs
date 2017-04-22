using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class GroupCreationModel
    {
        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }

        [UIHint("MultilineText")]
        public string Description { get; set; }
    }


}