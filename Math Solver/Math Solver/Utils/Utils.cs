using System;
using System.Collections.Generic;
using System.Text;
using static Math_Solver.App;
using Xamarin.Forms;
using System.Linq;
using System.IO;
using FFImageLoading.Forms;

namespace Math_Solver.Utils
{
    public class Utils
    {
        public string GetBase64Image(string name, bool isToGetImgExample = false)
        {
            string result = string.Empty;

            result = (isToGetImgExample) ? base64Images.Where(x => x.Name == name).Select(y => y.ImgExample).First() :
                base64Images.Where(x => x.Name == name).Select(y => y.Img).First();

            return String.Concat("data:image/png;base64,", result);
        }

        public CachedImage CreateImage(string name)
        {
            return new CachedImage()
            {
                Aspect = Aspect.AspectFit,
                WidthRequest = 200,
                HeightRequest = 200,
                CacheDuration = TimeSpan.FromDays(1),
                DownsampleToViewSize = false,
                RetryCount = 2,
                RetryDelay = 250,
                BitmapOptimizations = false,
                LoadingPlaceholder = GetBase64Image("loading"),
                ErrorPlaceholder = GetBase64Image("not_found"),
                Source = GetBase64Image(name)
            };
        }
    }
}
