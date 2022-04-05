using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AdminDashBoard.UploadImages
{
    public class Images
    {

        [Obsolete]
        public Images() 
        {
            
        }

        [Obsolete]
        public static async Task<string> uploadImage(IFormFile file, IHostingEnvironment _environment)
        {
            string result;
            string date = DateTime.Now.Date.ToShortDateString();
            string time = DateTime.Now.TimeOfDay.ToString();
            if (file != null && file.Length > 0)
                try
                {
                    string newFileName = Path.Combine(Path.GetDirectoryName(file.FileName)
                               , string.Concat(Path.GetFileNameWithoutExtension(file.FileName)
                               , DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss")
                               , Path.GetExtension(file.FileName)
                               )
                );
                    string path = Path.Combine(_environment.WebRootPath + ("/images"), newFileName);
                    path = path.Trim();
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                       await file.CopyToAsync(fileStream);
                    }
                    result = path;//"images/" + newFileName;
                    //string path = Path.Combine(Server.MapPath("~/images"), newFileName);
                    //path = path.Trim();
                    //file.SaveAs(path);
                    //result = "http://furnish-001-site1.btempurl.com/images/" + newFileName;
                    //ViewBag.s = "scusses";
                }
                catch (Exception ex)
                {
                    result = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                result = "You have not specified a file.";
            }
            return result;
        }
        [Obsolete]
        public static void DeleteImage(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
