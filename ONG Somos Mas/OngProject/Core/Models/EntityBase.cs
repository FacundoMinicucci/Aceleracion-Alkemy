using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OngProject.Core.Models
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
