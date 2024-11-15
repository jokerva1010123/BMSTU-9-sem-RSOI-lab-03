using ModelDTO.Response;
using System.Collections.Concurrent;

namespace Gateway.Services
{
    public class BackQueue
    {
        private static ConcurrentQueue<AddReq> _backBonusesQueue;

        public static ConcurrentQueue<AddReq> BackBonusesQueue
        {
            get
            {
                if (_backBonusesQueue == null)
                {
                    _backBonusesQueue = new ConcurrentQueue<AddReq>();
                }

                return _backBonusesQueue;
            }
        }
    }
}
