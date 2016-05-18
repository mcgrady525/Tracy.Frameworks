namespace Tracy.Frameworks.Redis
{
    /// <summary>
    /// 发布订阅channel
    /// </summary>
    public class RedisSubscribeChannels
    {
        public static readonly string ABSClearCacheChannel = "ABS.Channels.ClearCache";//ABS清理基础缓存
        public static readonly string ABSClearOutPutCacheChannel = "ABS.Channels.ClearOutPutCache";//ABS清理线上OutPutCache缓存
        public static readonly string ABSFillCacheChannel = "ABS.Channels.FillCache";//ABS填充基础缓存
        public static readonly string ABSRqQueueChannel = "ABS.Channels.RqQueue";//ABS队列处理RQ

        public static readonly string HBSClearCacheChannel = "HBS.Channels.ClearCache";//HBS清理基础缓存
        public static readonly string HBSClearOutPutCacheChannel = "HBS.Channels.ClearOutPutCache";//HBS清理线上OutPutCache缓存
        public static readonly string HBSFillCacheChannel = "HBS.Channels.FillCache";//HBS填充基础缓存
    }
}
