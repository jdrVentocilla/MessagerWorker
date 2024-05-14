namespace MessagerWorker.Queue
{
    public interface IMessageQueueSender<T> : IDisposable where T : class
    {
        void Execute(T mensaje);
       
    }
}