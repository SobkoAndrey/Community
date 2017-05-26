using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class Audio
    {
        [Required]
        public int AudioId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Name { get; set; }

        public string Label { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

    }

    public class Image
    {
        [Required]
        public int ImageId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Name { get; set; }

        public string Label { get; set; }

        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

    }
}