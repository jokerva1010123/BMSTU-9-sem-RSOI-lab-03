namespace Gateway.Services
{
    public class CircuitBreaker
    {
        private static volatile CircuitBreaker instance = null;
        private static object syncRoot = new object();
        private static int failureCount = 0;
        private static int N = 5;
        private static bool IsStateOpened = false;

        public static CircuitBreaker Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CircuitBreaker();
                        }
                    }
                }
                return instance;
            }
        }

        private CircuitBreaker() { }

        public void IncrementFailureCount()
        {
            failureCount++;
            IsStateOpened = failureCount >= N ? true : false;
        }

        public void ResetFailureCount()
        {
            failureCount = 0;
            IsStateOpened = false;
        }

        public bool IsOpened()
        {
            return IsStateOpened;
        }
    }
}
