using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Mvc;

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
            _currentMember = DB.QuerySingle<Member>(MemberScripts.GetMember, new { LoginID = identity });


            var claimsIdentity = User.Identity as ClaimsIdentity;


            if (_currentMember != null && (!_currentMember.UpdatedDate.HasValue || _currentMember.UpdatedDate.Value.AddDays(7) < DateTime.Now))
            {
                var token = Request.Headers["Authorization"].ToString();

                string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;
                string aud = User.Claims.FirstOrDefault(c => c.Type == "aud").Value;

                if (!string.IsNullOrEmpty(token))
                {
                    var client = new HttpClient()
                        {
                            BaseAddress = new Uri(_settings.Auth0_Domain)
                        };
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var request = new HttpRequestMessage(HttpMethod.Get, "tokeninfo")
                        {
                            Content = new StringContent("{\"id_token\":\"" + accessToken + "\"}",
                                Encoding.UTF8,
                                "application/json")
                        };
                    //CONTENT-TYPE header

                    var res = await client.SendAsync(request);
                    if (res.IsSuccessStatusCode)
                    {
                        var userInfo = res.Content.ReadAsStringAsync();
                    }

                    //var apiClient = new AuthenticationApiClient(new Uri("https://spd.auth0.com/"));//_settings.Auth0_Domain);

                    //var userInfo = await apiClient.GetUserInfoAsync(accessToken);
                }
            }

            return _currentMember;
        }
    }
}
