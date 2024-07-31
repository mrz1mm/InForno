namespace InForno.Services
{
    public class ImageSvc
    {
        private readonly string _imagePath;

        public ImageSvc(string imagePath)
        {
            _imagePath = imagePath;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("File immagine non valido");
            }

            var filePath = Path.Combine(_imagePath, imageFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{imageFile.FileName}";
        }
    }
}
