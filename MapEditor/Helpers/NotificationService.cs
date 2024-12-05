using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using MapEditor.Models;

namespace MapEditor.Helpers
{
    /// <summary>
    /// Класс менеджера оповещений.
    /// </summary>
    public class NotificationService:ObservableObject
    {
        /// <summary>
        /// Конструктор класса менеджера оповещений.
        /// </summary>
        /// <param name="notificationLiveTimeSeconds">Время отображения оповещения</param>
        /// <param name="maxNotificationsCount">Максимальное кол-во отображаемых оповещений в один момент</param>
        public NotificationService(int notificationLiveTimeSeconds, int maxNotificationsCount)
        {
            _notificationLiveTimeSeconds = notificationLiveTimeSeconds;
            _maxNotificationsCount = maxNotificationsCount;
        }

        private readonly int _notificationLiveTimeSeconds;
        private readonly int _maxNotificationsCount;
        public ObservableCollection<Notification> Notifications
        {
            get => GetOrCreate(new ObservableCollection<Notification>());
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Добавляет оповещение для отображения.
        /// </summary>
        /// <param name="message">Текст оповещения</param>
        /// <param name="type">Тип оповещения</param>
        public void AddNotification(string message, NotificationType type)
        {
            var notification = new Notification(message, type, _notificationLiveTimeSeconds);
            notification.LifeTimeEnded += Notification_LifeTimeEnded;
            Notifications.Add(notification);
            if (Notifications.Count > _maxNotificationsCount)
            {
                Notifications.RemoveAt(0);
            }
        }

        private void Notification_LifeTimeEnded(Notification sender)
        {
            Notifications.Remove(sender);
        }
    }
}
