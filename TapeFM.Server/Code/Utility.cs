namespace TapeFM.Server.Code
{
    public static class Utility
    {
        public static int? IntParseNullable(string text)
        {
            int result;
            if (int.TryParse(text, out result))
            {
                return result;
            }
            return null;
        }
    }
}