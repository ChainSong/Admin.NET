
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SpliteToBox.Common
{
    public class RedisCacheHelper
    {
        private static readonly PooledRedisClientManager pool = null;
        private static readonly string[] redisHosts = null;
        public static int RedisMaxReadPool = 12;
        public static int RedisMaxWritePool = 8;
        public static int sessionExpireMinutes = 180;

        static RedisCacheHelper()
        {
            var redisHostStr = "127.0.0.1:6379";

            if (!string.IsNullOrEmpty(redisHostStr))
            {
                redisHosts = redisHostStr.Split(',');

                if (redisHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(redisHosts, redisHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,
                            AutoStart = true
                        });
                }
            }
        }
        public static void Add<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }

        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            r.Set(key, value, slidingExpiration);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }
        public static void Add<T>(string key, T value)
        {
            return;
            if (value == null)
            {
                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            r.Set(key, value, System.DateTime.Now.AddYears(10));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }


        public static T Get<T>(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
            }


            return obj;
        }

        public static void Remove(string key)
        {

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
            }

        }

        public static bool Exists(string key)
        {

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = sessionExpireMinutes;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }

            return false;
        }

        /// <summary>
        /// 根据指定的Key，将值减1(仅整型有效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="NewNum">用于获取更新后的值,失败返回 -1 </param>
        /// <returns></returns>
        public static bool DecrementValue(string key, out long NewNum)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {

                            NewNum = r.DecrementValue(key);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }
            NewNum = -1;
            return false;
        }

        /// <summary>
        /// 根据指定的Key，将值+1(仅整型有效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="NewNum">用于获取更新后的值,失败返回 -1 </param>
        /// <returns></returns>
        public static bool IncrementValue(string key, out long NewNum)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {

                            NewNum = r.IncrementValue(key);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }
            NewNum = -1;
            return false;
        }


        /// <summary>
        /// 根据指定的Key，将值加上指定值(仅整型有效)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">需要添加的数量</param>
        /// <returns></returns>
        public static bool IncrementValueBy(string key, int count)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {

                            r.IncrementValueBy(key, count);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }
            return false;
        }


        /// <summary>
        /// 根据指定的Key，将值减去指定值(仅整型有效)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">需要添加的数量</param>
        /// <returns></returns>
        public static bool DecrementValueBy(string key, int count)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {

                            r.DecrementValueBy(key, count);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }
            return false;
        }
    }
}
