using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Api.Models;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IDatabase db, IAppSettings settings)
        {
            DB = db;
            _settings = settings;
        }

        protected readonly IDatabase DB;

        private Member _currentMember;
        private readonly IAppSettings _settings;

        public async Task<Member> GetCurrentMember()
        {
            if (_currentMember != null || User.Claims == null)
                return _currentMember;
            
            var identity = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identity))
                return null;

            _currentMember = DB.QuerySingle<Member>(MemberScripts.GetMember, new { LoginID = identity }) ?? new Member();

            if (_currentMember != null && (!_currentMember.UpdatedDate.HasValue || _currentMember.UpdatedDate.Value.AddDays(7) < DateTime.Now))
            {
                var accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;

                if (!string.IsNullOrEmpty(accessToken))
                {
                    var client = new HttpClient() { BaseAddress = new Uri(_settings.Auth0_Domain) };

                    var request = new HttpRequestMessage(HttpMethod.Get, "tokeninfo")
                        {
                            Content = new StringContent("{\"id_token\":\"" + accessToken + "\"}",Encoding.UTF8,"application/json")
                        };

                    var res = await client.SendAsync(request);
                    if (res.IsSuccessStatusCode)
                    {
                        var userInfo = JsonConvert.DeserializeObject<Auth0User>(await res.Content.ReadAsStringAsync());
                        _currentMember.Map(userInfo);
                        DB.Execute(_currentMember.MemberID < 1 ? MemberScripts.Insert : MemberScripts.Update, _currentMember);
                    }
                }
            }

            return _currentMember;
        }
    }
}
