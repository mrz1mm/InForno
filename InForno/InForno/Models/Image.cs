namespace InForno.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
