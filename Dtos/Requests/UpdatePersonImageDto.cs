using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonImageDto
    {
        public Guid PersonId { get; set; }
        [AllowedFileExtensionValidator([".jpg"])]
        public IFormFile PersonImage { get; set; }
    }
}
