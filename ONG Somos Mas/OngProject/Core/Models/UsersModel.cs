using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace OngProject.Core.Models
{
    public class UsersModel : EntityBase
    {        
        [Required(ErrorMessage = "The FirstName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field must be completed.")]

        [MaxLength(128)]        
        [MinLength(8, ErrorMessage = "You must enter at least 8 characters.")]
        [RegularExpression(@"(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string? Photo { get; set; } 
        
        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public virtual RolModel Rol { get; set; }

        public static string ComputeSha256Hash(string rawData)
        {            
            using (SHA256 sha256Hash = SHA256.Create())
            {                 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
