using System;

namespace SmartTodo.Business.Infrastructure
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetCurrentServerTime()
        {
            return DateTime.Now;
        }
    }
}