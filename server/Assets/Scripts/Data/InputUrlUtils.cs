using System.Collections.Generic;

namespace Data
{
    public static class InputUrlUtils
    {
        public static Dictionary<string, string> ParseGetUrlParams(string url)
        {
            var inputParam = new Dictionary<string, string>();
            var list = url.Split('?');
            if (list.Length < 2)
                return inputParam;

            var paramList = list[1].Split('&');
            foreach (var one in paramList)
            {
                var subList = one.Split('=');
                if (subList.Length == 2)
                {
                    inputParam[subList[0]] = subList[1];
                }
            }

            return inputParam;
        }
    }
}