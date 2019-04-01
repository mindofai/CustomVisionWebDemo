using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomVisionAspDemo.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CustomVisionAspDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new IndexModel());
        }

        public async Task<IActionResult> Predict(IFormFile file)
        {
            var image = file.OpenReadStream();

            if (image != null)
            {
                CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
                {
                    ApiKey = "{set api key here}",
                    Endpoint = "{set endpoint here}"
                };

                var result = await endpoint.ClassifyImageAsync(new Guid("{set project id here}"),
                    "{set iteration here}", image);

                return View("Index", new IndexModel() { Image = convertFileToString(file), Prediction = result.Predictions[0].TagName });
            }

            return null;
        }

        private string convertFileToString(IFormFile file)
        {
            var imgSrc = "";

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    imgSrc = String.Format("data:image/jpg;base64,{0}", s);
                }
            }

            return imgSrc;
        }

    }
}
