using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MapEditor.Models
{
    public enum NotificationType
    {
        Success,
        Failure,
        Warning
    }
    public class Notification
    {
        public Notification(string message, NotificationType type, int lifeTime)
        {
            Message = message;
            Type = type;
            _lifeTime = lifeTime;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        public NotificationType Type { get; set; }
        public string Message { get; set; }

        private int _lifeTime;
        private int _curSec;
        private DispatcherTimer _timer;

        public event NotificationLifeTimeEvent? LifeTimeEnded;
        public delegate void NotificationLifeTimeEvent(Notification sender);

        private void _timer_Tick(object? sender, EventArgs e)
        {
            if (_curSec < _lifeTime)
            {
                _curSec++;
                return;
            }
            LifeTimeEnded?.Invoke(this);
        }

    }
}
