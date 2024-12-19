using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonImageDto
    {
        public Guid PersonId { get; set; }
        [AllowedFileExtensionValidator([".jpg"])]
        [MaxFileSize(5 * 1024 * 1024)] //5mb
        public IFormFile PersonImage { get; set; }
    }
}
