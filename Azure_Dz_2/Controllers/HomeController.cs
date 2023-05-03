using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure_Dz_2.Data;
using Azure_Dz_2.Models;
using Azure_Dz_2.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Azure_Dz_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhotoContext context;
        private readonly BlobServiceClient blobServiceClient;
        private readonly IConfiguration configuration;
        private readonly string containerName;


        public HomeController(PhotoContext context, BlobServiceClient blobServiceClient,
            IConfiguration configuration)
        {
            this.context = context;
            this.blobServiceClient = blobServiceClient;
            this.configuration = configuration;
            containerName = configuration.GetSection("BlobContainerName").Value;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<Photo> photos = context.Photos;
            return View(await photos.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePhotoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            string filename = $"{Path.GetFileNameWithoutExtension(dto.Photo.FileName)}" + $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
            BlobClient client = containerClient.GetBlobClient(filename);
            await client.UploadAsync(dto.Photo.OpenReadStream());
            Photo addedPhoto = new Photo
            {
                Filename = filename,
                PhotoUrl = client.Uri.AbsoluteUri,
                Description = dto.Description
            };
            context.Photos.Add(addedPhoto);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}