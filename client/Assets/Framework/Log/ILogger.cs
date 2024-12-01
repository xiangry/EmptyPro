namespace Framework.Log
{
    public interface ILogger
    {
        public void Debug(string info);

        public void Debug(string tag, string info);

        public void Error(string info);

        public void Error(string tag, string info);
    }
}