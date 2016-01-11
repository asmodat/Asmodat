using Asmodat.Extensions.Objects;
using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Asmodat.Abbreviate
{
    public static class Network
    {

        public static string SaveFileRequest(string url, string path, bool AddTimestamp = false)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            /*request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705;)";
            request.Method = "GET";
            request.Proxy = null;
            request.Credentials = CredentialCache.DefaultCredentials;*/
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            if(AddTimestamp)
            {
                var list = path.SplitSafe('.');
                path = list[0] + TickTime.NowTicks + "." + list[1];
            }

            return Streams.ToFile(stream, path);
        }

        public static string SaveFileWebClient(string url, string path, bool AddTimestamp = false)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);

            if (AddTimestamp)
            {
                var list = path.SplitSafe('.');
                path = list[0] + TickTime.NowTicks + "." + list[1];
            }

            return Streams.ToFile(stream, path);
        }


        public static string Redirection(string url)
        {
            string redirect = null;
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                redirect = request.GetResponse().ResponseUri.AbsoluteUri.ToString();
            }
            catch { }
            return redirect;
        }

        public static bool IsSubnetValid(IPAddress IPA)
        {
            byte[] bValidOctets = new byte[] { 255, 254, 252, 248, 240, 224, 192, 128, 0 };
            byte[] bIPOctets = IPA.GetAddressBytes();
            bool bLeadingZeros = false;

            for (int i = 0; i < 4; i++)
            {
                if (!bValidOctets.Contains(bIPOctets[i])) return false;
                if (bLeadingZeros && bIPOctets[i] != 0) return false;
                if (bIPOctets[i] < 255) bLeadingZeros = true;
            }


            return true;
        }


        public static IPAddress GetNetworkAddress(IPAddress adress, IPAddress mask)
        {
            byte[] baIPABytes = adress.GetAddressBytes();
            byte[] baIPMBytes = mask.GetAddressBytes();

            if (baIPABytes.Length != baIPMBytes.Length) return null;

            byte[] baBroadcast = new byte[baIPABytes.Length];
            for (int i = 0; i < baBroadcast.Length; i++)
                baBroadcast[i] = (byte)(baIPABytes[i] & baIPMBytes[i]);

            return new IPAddress(baBroadcast);
        }

        public static bool IsSubnetMatching(IPAddress adress1, IPAddress adress2, IPAddress mask)
        {
            IPAddress IPANetwork1 = GetNetworkAddress(adress1, mask);
            IPAddress IPANetwork2 = GetNetworkAddress(adress2, mask);

            if (IPANetwork1 == null || IPANetwork2 == null) return false;

            return IPANetwork1.Equals(IPANetwork2);
        }



    }
}


//private bool IsValidSubnet(IPAddress ip) {
//    byte[] validOctets = new byte[] { 255, 254, 252, 248, 240, 224, 192, 128, 0 };
//    byte[] ipOctets = ip.GetAddressBytes();
//    bool restAreZeros = false;
//    for (int i = 0; i < 4; i++) {
//        if (!validOctets.Contains(ipOctets[i]))
//            return false;
//        if (restAreZeros && ipOctets[i] != 0)
//            return false;
//        if (ipOctets[i] < 255)
//            restAreZeros = true;
//    }
//    return true;
//}

//// checks if the address is all leading ones followed by only zeroes
//private bool IsValidSubnet2(IPAddress ip) {
//    byte[] ipOctets = ip.GetAddressBytes();
//    bool restAreOnes = false;
//    for (int i = 3; i >= 0; i--) {
//        for (int j = 0; j < 8; j++) {
//            bool bitValue = (ipOctets[i] >> j & 1) == 1;
//            if (restAreOnes && !bitValue)
//                return false;
//            restAreOnes = bitValue;
//        }
//    }
//    return true;
//}