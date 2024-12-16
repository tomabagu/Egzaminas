using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Egzaminas.Services
{
    public class ImageService
    {
        public static byte[] ResizeImage(IFormFile formFile)
        {
            using (var inStream = formFile.OpenReadStream())
            using (var outStream = new MemoryStream())
            {
                try
                {
                    // Load the image from the input stream
                    using (var image = Image.Load(inStream))
                    {
                        // Resize the image to 200x200 pixels
                        image.Mutate(x => x.Resize(200, 200));

                        // Save the resized image to the output stream
                        image.Save(outStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    }

                    return outStream.ToArray();
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error)
                    throw new InvalidOperationException("An error occurred while resizing the image.", ex);
                }
            }
        }

        public static FileContentResult GetFileContentResult(byte[] fileBytes, string contentType, string fileName)
        {
            // Return the file as a FileContentResult
            return new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
