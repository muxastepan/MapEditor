using System;
using System.Windows;
using EventTrigger = Microsoft.Xaml.Behaviors.EventTrigger;

namespace MapEditor.Utilities
{
    public class DisableRoutingEventTrigger : EventTrigger
    {
        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs is RoutedEventArgs routedEventArgs)
                routedEventArgs.Handled = true;
            base.OnEvent(eventArgs);
        }
    }
}
