using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace PrismDemo.Images
{
    public class SampleImage
    {
        private static string _imagePathRegex = @"((?'assembly'.*)(;component)\/(?'path'.*)|\/*(?'path2'.*))";  
        
        public string ImagePath { get; private set; }

        public string ImageFullPath => GetImageFullPath();

        public SampleImage(string imagePath)
        {
            ImagePath = imagePath;
        }

        public string GetImageFullPath()
        {
            var assemblyName = GetImageAssemblyName();
            var localPath = GetImageLocalPath();
            if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(localPath)) return null;
            var fullPath = $"/{assemblyName};component/{localPath}";
            return fullPath;
        }
        public string GetImageAssemblyName()
        {
            if (string.IsNullOrEmpty(ImagePath)) return null;
            var match = Regex.Match(ImagePath, _imagePathRegex);
            var assemblyName = match.Groups["assembly"]?.Value;
            return string.IsNullOrEmpty(assemblyName) ? "PrismDemo" : assemblyName;
        }
        public string GetImageLocalPath()
        {
            if (string.IsNullOrEmpty(ImagePath)) return null;
            var match = Regex.Match(ImagePath, _imagePathRegex);
            var path = match.Groups["path"]?.Value.ToLower();
            return string.IsNullOrEmpty(path) ? match.Groups["path2"]?.Value.ToLower() : path;
        }


        public static bool ResourceExists(string resourcePath)
        {
            return GetResourcePaths(Assembly.GetExecutingAssembly())
                 .Select(s => new Uri(s.ToString(), UriKind.Relative).ToString().ToLowerInvariant())
                 .Contains(resourcePath.ToLowerInvariant());
        }

        public static IEnumerable<object> GetResourcePaths(Assembly assembly)
        {
            var resourceManager = new ResourceManager($"{assembly.GetName().Name}.g", assembly);
            try
            {
                foreach (DictionaryEntry resource in resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true))
                    yield return resource.Key;
            }
            finally
            {
                resourceManager.ReleaseAllResources();
            }
        }

        public static Uri Get(string resourcePath)
        {
            var uri = string.Format(
                "pack://application:,,,/{0};component/{1}"
                , Assembly.GetExecutingAssembly().GetName().Name
                , resourcePath
            );

            return new Uri(uri);
        }

        public static BitmapImage CreateImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            var si = new SampleImage(imagePath); 
            var assemblyName = si.GetImageAssemblyName();
            var localPath = si.GetImageLocalPath();
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(s => s.GetName().Name == assemblyName);
            
            if (string.IsNullOrEmpty(localPath) || !Uri.TryCreate(si.ImageFullPath, UriKind.Relative, out var uri) || assembly == null ||
                GetResourcePaths(assembly).Select(s => new Uri(s.ToString(), UriKind.Relative).ToString().ToLowerInvariant()).Contains($"/{localPath}"))
                return null;

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.DecodePixelWidth = 24;
            bitmap.DecodePixelHeight = 24;
            bitmap.EndInit();

            if (bitmap.StreamSource?.Length == 0)
            {
                bitmap = null;
            }

            return bitmap;
        }
    }
}
