namespace MVC.SliderFrontToBack.Helpers
{
    public class Helper
    {
        public static string GetPathCombine(string rootPath, string folderName , IFormFile imageFile)
        {
            string fileName = imageFile.FileName.Substring(imageFile.FileName.Length - 64, 64) ? Guid.NewGuid().ToString() + fileName : fileName;

            return "";
        }
    }
}
