using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZ_Walk.Models.Domain;
using NZ_Walk.Models.DTO;
using NZ_Walk.Repositories;

namespace NZ_Walk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //Convert Dto to Domain
                var ImageDomain = new Image
                {
                    File = request.File,
                    FileExtenstion = Path.GetExtension(request.File.FileName).ToLower(),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };

                //User repository to upload image
                await imageRepository.Upload(ImageDomain);

                return Ok(ImageDomain);
                
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowsExtentions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowsExtentions.Contains(Path.GetExtension(request.File.FileName)?.ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size more than 10MB, please upload smaller file size.");
            }
        }
    }
}
