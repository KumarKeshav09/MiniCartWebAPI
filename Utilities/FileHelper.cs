namespace MintCartWebApi.Utilities
{
    public static class FileHelper
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string webRootPath)
        {

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);


            string uploadsFolder = Path.Combine(webRootPath, "uploads");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string returnUrl = $"/uploads/{uniqueFileName}";
            return returnUrl;
        }
        public static async Task<List<string>> SaveMultiFilesAsync(List<IFormFile> files, string webRootPath)
        {
            List<string> fileUrls = new List<string>();

            foreach (var file in files)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string uploadsFolder = Path.Combine(webRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                string returnUrl = $"/uploads/{uniqueFileName}";
                fileUrls.Add(returnUrl);
            }

            return fileUrls;
        }
    }
}
