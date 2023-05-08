namespace ProfileSample.Models
{
    public class ImageModel
    {
        public string Name { get; set; }

        public byte[] Data { get; set; } 

        public string ImgSource { 
            get { return string.Format($"data:image/jpg;base64,{System.Convert.ToBase64String(this.Data)}");}
            }
    }
}