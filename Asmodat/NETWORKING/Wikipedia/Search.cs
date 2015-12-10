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
using System.Net;

namespace Asmodat.Wikipedia
{

    public class Search
    {
        //private YouTubeService YTService;

        public static string RandomArticle()
        {
            return Asmodat.Abbreviate.Network.Redirection(@"http://en.wikipedia.org/wiki/Special:Random");
        }
    }
}
