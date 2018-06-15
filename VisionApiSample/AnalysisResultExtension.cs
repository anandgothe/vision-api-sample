using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace VisionApiSample
{
    public static class AnalysisResultExtension
    {
        public static bool HasAFace(this AnalysisResult result)
        {
            return result.Faces.Any() && (result.Tags.Any(x => x.Name == "person") 
                || !result.Tags.Any(x => x.Name == "animal"));
        }

        public static bool IsAdultOrRacyContent(this AnalysisResult result)
        {
            return result.Adult.IsRacyContent || result.Adult.IsAdultContent;
        }

        public static bool IsYoungerThan(this AnalysisResult result, int minAge)
        {
            return result.Faces.Any(x => x.Age <= minAge);
        }

        public static string DetectCelebrity(this AnalysisResult result)
        {
            var detectedPeople = string.Empty;
            var people = result.Categories.FirstOrDefault(x => x.Name.StartsWith("people_"));
            if (people != null)
            {
                JObject detail = (JObject)people.Detail;
                if (detail != null)
                {
                    var celebrities = detail.SelectToken("celebrities");
                    if (celebrities.Any())
                    {
                        foreach (dynamic celebrity in celebrities)
                        {
                            double confidence = double.Parse(celebrity.confidence.ToString());

                            if (confidence > 0.95)
                            {
                                detectedPeople += celebrity.name + ", ";
                            }
                        }
                    }
                }
            }

            return detectedPeople;
        }
    }
}
