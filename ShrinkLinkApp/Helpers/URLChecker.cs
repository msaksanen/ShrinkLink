using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ShrinkLinkApp.Helpers
{
    public class URLChecker
    {
        public bool IsValidURL(string URL)
        {
            if(URL.Contains("localhost"))
                URL = URL.Replace("localhost", "local.host");
            if (URL.Contains('%'))
                URL = URL.Replace('%', 'X');
            string Pattern1 = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            string Pattern2 = @"^(?:ftp?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx1 = new Regex(Pattern1, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex Rgx2 = new Regex(Pattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx1.IsMatch(URL)|| Rgx2.IsMatch(URL);

        }
        
    }
}
