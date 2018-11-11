namespace AstWeb.Common.Logging
{
    /// <summary>
    /// 日志处理工厂
    /// </summary>
    public class LoggingFactory
    {
        private static ILogger _logger;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="logger"></param>
        public static void InitializeLogFactory(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 获取日志处理对象
        /// </summary>
        /// <returns></returns>
        public static ILogger GetLogger()
        {
            return _logger;
        }
    }
}
