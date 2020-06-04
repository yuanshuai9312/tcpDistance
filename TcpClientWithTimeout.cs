using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tcp检测远程端口
{

    /// <summary>
    /// TcpClientWithTimeout 用来设置一个带连接超时功能的类
    /// 使用者可以设置毫秒级的等待超时时间 (1000=1second)
    /// 例如:
    /// TcpClient connection = new TcpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;
        protected Exception exception;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public double Connect()
        {
            //开始计时
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // kick off the thread that tries to connect
            connected = false;
            exception = null;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // 作为后台线程处理
            // 不会占用机器太长的时间
            thread.Start();

            // 等待如下的时间
            //thread.Join(_timeout_milliseconds);
            TimeSpan ts2 = sw.Elapsed;  
            while (ts2.TotalMilliseconds < _timeout_milliseconds)
            {
                ts2 = sw.Elapsed;
                Thread.Sleep(100);
                //循环判断超时时间，连接成功则立即跳出
                if (connected)
                {
                    break;
                }
            } 
     
            if (connected == true)
            {
                // 如果成功就返回TcpClient对象
                thread.Abort();
                Console.WriteLine(_hostname + ":" + _port + " 当前速度：" + ts2.TotalMilliseconds);
                return ts2.TotalMilliseconds;
            }
            if (exception != null)
            {
                // 如果失败就抛出错误
                thread.Abort();
                return ts2.TotalMilliseconds;
            }
            else
            {
                // 同样地抛出错误
                thread.Abort();
                Console.WriteLine(_hostname + ":" + _port + " ，节点连接超时" + ts2.TotalMilliseconds);
                return ts2.TotalMilliseconds;
            }
        }
        protected void BeginConnect()
        {
            try
            {
                connection = new TcpClient(_hostname, _port);
                // 标记成功，返回调用者
                connected = true;
            }
            catch (Exception ex)
            {
                // 标记失败
                exception = ex;
            }
        }
    }
}
