using System.ComponentModel.DataAnnotations.Schema;

namespace Azure_Dz_2.Models.DTOs
{
    public class CreatePhotoDTO
    {
        [NotMapped]
        public IFormFile Photo { get; set; } = default!;

        public string? Description { get; set; }
    }
}
