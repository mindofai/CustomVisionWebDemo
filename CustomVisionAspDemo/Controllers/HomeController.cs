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
                    ApiKey = "4a6f15ad57c941b1b72e215f6f69c410",
                    Endpoint = "https://southeastasia.api.cognitive.microsoft.com/"
                };

                var result = await endpoint.ClassifyImageAsync(new Guid("6e8a2de4-a37d-4649-b9f6-d30f9a53052d"),
                    "Iteration1", image);

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
