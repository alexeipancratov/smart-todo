using System;

namespace SmartTodo.Business.Infrastructure
{
    public interface ITimeProvider
    {
        DateTime GetCurrentServerTime();
    }
}