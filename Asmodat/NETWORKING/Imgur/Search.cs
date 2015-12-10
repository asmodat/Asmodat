using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using AsmodatMath;
using Asmodat.Abbreviate;

namespace Asmodat.Imgur
{
    /// <summary>
    /// public access key: https://console.developers.google.com/project/asmodat-1015/apiui/credential#
    /// API key: AIzaSyBqnfCksDxdpakFZFApNvkgru0iS2NzHTU
    /// sample: https://developers.google.com/youtube/v3/code_samples/dotnet
    /// console - config services: https://code.google.com/apis/console/b/0/?noredirect#project:532786056208
    /// </summary>
    public class Search
    {
        //private YouTubeService YTService;

        public static string RandomVideo(int exceptions_countout = 3, string base_url = "www.youtube.com/watch?v=")
        {
            if (exceptions_countout < 0)
                return null;

            YouTubeService YTService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBpscnC6P4cL7S6_W_3zxQX3NHeU5v6ypA",
                ApplicationName = "Asmodat"
            });
            SearchListResponse YTSearchResponse;

           // YTService.a

            var YTSearchRequest = YTService.Search.List("snippet");
            YTSearchRequest.Q = "v=" + Randomizer.GetString(Randomizer.Seed_NumbersLowerUpperCaseLetters, 3, 4);
            YTSearchRequest.MaxResults = AMath.Random(25, 50);

            try
            {
                YTSearchResponse = YTSearchRequest.Execute();
            }
            catch
            {
                return RandomVideo(--exceptions_countout, base_url);
            }

            int count = 0;

            if (YTSearchResponse == null || YTSearchResponse.Items == null || (count = YTSearchResponse.Items.Count) <= 0)
                return null;

            int select = AMath.Random(0, count);
            return base_url + YTSearchResponse.Items[select].Id.VideoId;
        }


        public static string RandomImageOffline(string base_url = "www.youtube.com/watch?v=")
        {
            return base_url + Randomizer.GetString(Randomizer.Seed_NumbersLowerUpperCaseLetters, 11);
        }
    }
}
