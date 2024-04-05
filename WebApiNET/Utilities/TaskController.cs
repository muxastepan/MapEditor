namespace WebApiNET.Utilities
{
    public class TaskController
    {
        public bool IsCanceled { get; private set; }

        public void Cancel()
        {
            IsCanceled = true;
        }
    }
}
