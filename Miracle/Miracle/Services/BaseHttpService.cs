using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Miracle.Services
{
    public class BaseHttpService
    {
        private static readonly BaseHttpService instance = new BaseHttpService();


        public static BaseHttpService Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<string> SendRequestAsync(string date)
        {
            string result = string.Empty;

            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append("https://opm.kepco.co.kr:11080/OpenAPI/getDayLpData.do");
            sbUrl.Append(string.Format("?custNo={0}", "0340191964"));
            sbUrl.Append(string.Format("&date={0}", date));
            sbUrl.Append(string.Format("&serviceKey={0}", "9l5l704dihn2fbz4p2i4"));
            sbUrl.Append(string.Format("&returnType={0}", "02"));


            //if (!Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
            //{
            //    // Active Wi-Fi connection.
            //    return "ERROR-Wifi 연결 오류\n다시 처리해 주세요.";
            //}

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(sbUrl.ToString())))
                {
                    using (var handler = new HttpClientHandler())
                    {
                        using (var client = new HttpClient(handler))
                        {
                            Console.WriteLine(client.Timeout.ToString());
                            //client.Timeout = new TimeSpan(0, 3, 0); //3분
                            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                            {
                                //var content = response.Content == null
                                //    ? null
                                //    : Encoding.UTF8.GetString(await response.Content.ReadAsStringAsync());

                                if (response.IsSuccessStatusCode)
                                {
                                    result = await response.Content.ReadAsStringAsync();
                                }
                            }

                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //var properties = new Dictionary<string, string>
                //{
                //    {"BaseHttpService", "SendRequestAsync" },
                //    {"UserID", Settings.Userid}
                //};

                //Crashes.TrackError(ex, properties);

                return "ERROR " + ex.Message.ToString();
            }
        }

        public async Task<string> SendRequestAsync2(string date)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append("https://opm.kepco.co.kr:11080/OpenAPI/getDayLpData.do");
            sbUrl.Append(string.Format("?custNo={0}", "0340191964"));
            sbUrl.Append(string.Format("&date={0}", date));
            sbUrl.Append(string.Format("&serviceKey={0}", "9l5l704dihn2fbz4p2i4"));
            sbUrl.Append(string.Format("&returnType={0}", "02"));

            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

            string result = string.Empty;

            try
            {
                //1. 서비스 데이터 조회
                result = await wc.DownloadStringTaskAsync(sbUrl.ToString());
                //result = wc.DownloadString(sbUrl.ToString());
            }
            catch (Exception e)
            {

            }
            finally
            {
                wc.Dispose();
            }

            return result;
        }

            //public async Task<string> LoginAsync(string jsonString)
            //{
            //    HttpResponseMessage response = null;
            //    try
            //    {
            //        using (var client = new HttpClient())
            //        {
            //            //client.BaseAddress = new Uri(LOGIN_CONNECT_URL.ToString());
            //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(LOGIN_CONNECT_URL.ToString())))
            //            {
            //                //request.Headers.Add("jwt", Settings.Token);
            //                request.Headers.Add("abc", "");
            //                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString)));
            //                response = await client.SendAsync(request);
            //            }

            //            if (response.IsSuccessStatusCode)
            //            {
            //                return Encoding.UTF8.GetString(Convert.FromBase64String(await response.Content.ReadAsStringAsync()));
            //            }
            //            else
            //            {
            //                return string.Empty;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return "ERROR " + ex.Message.ToString();
            //    }
            //    finally
            //    {
            //        if (response != null) response.Dispose();
            //    }
            //}

        }
}
