using Gateway.IServices;
using ModelDTO.Bonus;
using ModelDTO.Response;
using System.Net;

namespace Gateway.Services
{
    public class PrivilegeServices : IPrivilegeServices
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://privilegeservice:8080")
            // BaseAddress = new Uri("http://localhost:8060")
            //  BaseAddress = new Uri("http://host.docker.internal:8060")
            //BaseAddress = new Uri("http://localhost:5006")
        };

        public async Task Start()
        {
            Console.WriteLine("0 "+ BackQueue.BackBonusesQueue.Count.ToString());
            if (BackQueue.BackBonusesQueue.Count == 0)
                return;

            int status = 503;
            int n = 0;

            while (status != 200 && n < 1)
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, "manage/health");
                try
                {
                    using var res = await _httpClient.SendAsync(req);
                    status = (int)res.StatusCode;
                }
                catch (Exception ex)
                {
                    n++;
                }
                if (status == 200)
                    while (BackQueue.BackBonusesQueue.Count > 0 && status == 200)
                    {
                        AddReq item;
                        status = 0;
                        if (BackQueue.BackBonusesQueue.TryDequeue(out item))
                        {
                            try
                            {
                                using var request = new HttpRequestMessage(HttpMethod.Post, $"/back_bonus");
                                request.Content = JsonContent.Create(item);
                                using var response = await _httpClient.SendAsync(request);
                                Console.WriteLine(item.username + " " + item.ticketUid.ToString() +" " + (int)response.StatusCode);
                                status = (int)response.StatusCode;
                            }
                            catch (Exception ex)
                            {
                                status = 0;
                            }
                            if (status != 200)
                                BackQueue.BackBonusesQueue.Enqueue(item);
                        }
                    }
            }
        }

        public void Add(string username, Guid ticketUid)
        {
            BackQueue.BackBonusesQueue.Enqueue(new AddReq
            {
                username = username,
                ticketUid = ticketUid
            });
            Console.WriteLine("1 " + BackQueue.BackBonusesQueue.Count.ToString());
        }

        public async Task<bool> HealthCheck()
        {
            Console.WriteLine("3 " + BackQueue.BackBonusesQueue.Count.ToString());
            using var req = new HttpRequestMessage(HttpMethod.Get, "manage/health");
            try
            {
                using var res = await _httpClient.SendAsync(req);
                await Start();
                //_circuitBreaker.ResetFailureCount();
                return res.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<PrivilegeDTO> GetByUsername(string username)
        {
            //if (_circuitBreaker.IsOpened())
            //    return null;
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/privilege/{username}");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            var response = await res.Content.ReadFromJsonAsync<PrivilegeDTO>();
            return response;
        }
        public async Task UpdatePrivilege(int id, PrivilegeDTO privilege)
        {
            using HttpRequestMessage req = new(HttpMethod.Patch, $"api/v1/privilege/{id}");
            req.Content = JsonContent.Create(privilege);
            await _httpClient.SendAsync(req);
        }
        public async Task<List<PrivilegeHistoryDTO>> GetAll(int page, int size)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/privilegeHistory?page={page}&size={size}");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            var response = await res.Content.ReadFromJsonAsync<List<PrivilegeHistoryDTO>>();
            return response;
        }
        public async Task AddNew(PrivilegeHistoryDTO history)
        {
            using HttpRequestMessage req = new(HttpMethod.Post, $"api/v1/privilegeHistory");
            req.Content = JsonContent.Create(history);
            using var res = await _httpClient.SendAsync(req);
        }
    }
}
