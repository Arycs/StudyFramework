using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using YouYouServer.Core;

namespace YouYouServer.Commmon
{
    /// <summary>
    /// 定时器管理器
    /// </summary>
    public static class TimerManager
    {
        #region 同步定时器 (模拟客户端OnUpdate)
        private static Timer m_TickTimer;

        public static event Action OnTick;

        /// <summary>
        /// 开始tick的时间
        /// </summary>
        private static double m_BeginTickTime;

        /// <summary>
        /// 服务器运行时间(秒)
        /// 
        /// </summary>
        public static float time => (float)(DateTime.UtcNow.Ticks - m_BeginTickTime) / 10000000;

        #endregion

        #region 真正的定时器

        /// <summary>
        /// 秒定时器
        /// </summary>
        private static Timer m_SecondTimer;

        /// <summary>
        /// 定时器链表
        /// </summary>
        private static LinkedList<ServerTimer> m_ServerTimers;

        #endregion
        public static void Init()
        {
            m_ServerTimers = new LinkedList<ServerTimer>();

            m_SecondTimer = new Timer();
            m_SecondTimer.Elapsed += SecondTimer_Elapsed;
            m_SecondTimer.Interval = 1000;
            m_SecondTimer.Enabled = true;

            m_BeginTickTime = DateTime.UtcNow.Ticks;
            m_TickTimer = new Timer();
            m_TickTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                OnTick?.Invoke();
            }; 
            m_TickTimer.Interval = 20; // 间隔模拟帧率, 实际根据游戏情况具体设置20-40
            m_TickTimer.Enabled = true;
        }

        /// <summary>
        /// 当前年
        /// </summary>
        private static int m_CurrYear;

        /// <summary>
        /// 当前月
        /// </summary>
        private static int m_CurrMouth;

        /// <summary>
        /// 当前天
        /// </summary>
        private static int m_CurrDay;

        /// <summary>
        /// 当前周几
        /// </summary>
        private static DayOfWeek m_CurrWeekday;

        /// <summary>
        /// 当前小时
        /// </summary>
        private static int m_CurrHour;

        /// <summary>
        /// 当前分
        /// </summary>
        private static int m_CurrMinute;

        /// <summary>
        /// 当前秒
        /// </summary>
        private static int m_CurrSecond;

        private static void SecondTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currtime = DateTime.Now;
            m_CurrYear = currtime.Year;
            m_CurrMouth = currtime.Month;
            m_CurrDay = currtime.Day;
            m_CurrWeekday = currtime.DayOfWeek;
            m_CurrHour = currtime.Hour;
            m_CurrMinute = currtime.Minute;
            m_CurrSecond = currtime.Second;

            LinkedListNode<ServerTimer> curr = m_ServerTimers.First;
            while (curr != null)
            {
                ServerTimer servertime = curr.Value;
                switch (servertime.RunType)
                {
                    case ServerTimerRunType.Once:
                        //年月日时分秒
                        if (servertime.Year == m_CurrYear && servertime.Mouth == m_CurrMouth && servertime.Day == m_CurrDay && servertime.Hour == m_CurrHour && servertime.Minute == m_CurrMinute && servertime.Second == m_CurrSecond)
                        {
                            servertime.DoAction();
                        }
                        break;
                    case ServerTimerRunType.FixedInterval:
                        servertime.ServerTimerTick();
                        break;
                    case ServerTimerRunType.EvertyDay:
                        //时分秒
                        if (servertime.Hour == m_CurrHour && servertime.Minute == m_CurrMinute && servertime.Second == m_CurrSecond)
                        {
                            servertime.DoAction();
                        }
                        break;
                    case ServerTimerRunType.EveryWeek:
                        //周几 时分秒
                        if (servertime.Weekday == m_CurrWeekday && servertime.Hour == m_CurrHour && servertime.Minute == m_CurrMinute && servertime.Second == m_CurrSecond)
                        {
                            servertime.DoAction();
                        }
                        break;
                    case ServerTimerRunType.EveryMonth:
                        //几号 时分秒
                        if (servertime.Day == m_CurrDay && servertime.Hour == m_CurrHour && servertime.Minute == m_CurrMinute && servertime.Second == m_CurrSecond)
                        {
                            servertime.DoAction();
                        }
                        break;
                    case ServerTimerRunType.EveryYear:
                        //几月几号 时分秒
                        if (servertime.Mouth == m_CurrMouth && servertime.Day == m_CurrDay && servertime.Hour == m_CurrHour && servertime.Minute == m_CurrMinute && servertime.Second == m_CurrSecond)
                        {
                            servertime.DoAction();
                        }
                        break;
                    default:
                        break;
                }
                curr = curr.Next;
            }
        }
        public static void RegisterServerTimer(ServerTimer time)
        {
            m_ServerTimers.AddLast(time);
        }

        public static void RemoveServerTimer(ServerTimer time)
        {
            m_ServerTimers.Remove(time);
        }

    }

    /// <summary>
    /// 服务器定时器
    /// </summary>
    public class ServerTimer
    {
        /// <summary>
        /// 运行类型
        /// </summary>
        public ServerTimerRunType RunType { get; private set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Mouth { get; private set; }

        /// <summary>
        /// 周几
        /// </summary>
        public DayOfWeek Weekday { get; private set; }

        /// <summary>
        /// 几号
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; private set; }

        /// <summary>
        /// 间隔(秒)
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// 当前间隔(秒)
        /// </summary>
        public int CurrIntervalSecond { get; private set; }

        /// <summary>
        /// 执行的操作
        /// </summary>
        private Action OnDoAction;

        public ServerTimer(ServerTimerRunType runtype, Action ondoaction, int year = 0, int mouth = 0, DayOfWeek weekday = 0, int day = 0, int hour = 0, int minute = 0, int second = 0, int interval = 0)
        {
            RunType = runtype;
            Year = year;
            Mouth = mouth;
            Weekday = weekday;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Interval = interval;
            CurrIntervalSecond = 0;

            OnDoAction = ondoaction;
        }

        public void ServerTimerTick()
        {
            CurrIntervalSecond++;
            if (CurrIntervalSecond >= Interval)
            {
                CurrIntervalSecond = 0;
                DoAction();
            }
        }

        public void DoAction()
        {
            OnDoAction?.Invoke();
        }

    }
}
