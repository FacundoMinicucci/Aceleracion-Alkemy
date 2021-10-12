namespace OngProject.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class UserDTO
    {
        
        [Required(ErrorMessage = "The FirstName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string FirstName { get; set; }

       /* public UserDTO(string firstName, string lastName, string email, string photo, string rolName)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Photo = photo;
            RolName = rolName;
        }*/

        [Required(ErrorMessage = "The LastName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Email { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Photo { get; set; }

        public string RolName { get; set; }

        public string LongName {
            get
            {
                return $"{FirstName} {LastName}";
            } 
            
        }
    }
}
