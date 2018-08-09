using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using horus.fw.api.Source.Data;
using horus.fw.Assertion;
using horus.fw.Base.Attributes;
using horus.fw.FwUtil;
using Newtonsoft.Json;

namespace horus.fw.api.Source.Step
{
    public class Reqres_Api_Step
    {
        [TestStep]
        public void Create_User()
        {
            var jObj = new User()
            {
                name = "morpheus",
                job = "leader"
            };

            var json = JsonConvert.SerializeObject(jObj);
            var httpResponse = HttpRequest.Post("https://reqres.in/api/users", json);

            PostResponse result = JsonConvert.DeserializeObject<PostResponse>(httpResponse);
            Assert.IsTrue(result.id > 0);
            Assert.IsTrue(result.createdAt < DateTime.Now);
        }

        [TestStep]
        public void Update_User()
        {
            var jObj = new User()
            {
                name = "morpheus",
                job = "zion resident"
            };

            var json = JsonConvert.SerializeObject(jObj);
            var httpResponse = HttpRequest.Put("https://reqres.in/api/users/2", json);

            PutResponse result = JsonConvert.DeserializeObject<PutResponse>(httpResponse);
            Assert.IsTrue(result.name == null);
            Assert.IsTrue(result.job == null);
            Assert.IsTrue(result.updatedAt < DateTime.Now);
        }

        [TestStep]
        public void Get_User()
        {
            var httpResponse = HttpRequest.Get("https://reqres.in/api/users?page=2");

            GetResponse result = JsonConvert.DeserializeObject<GetResponse>(httpResponse);
            var expectedUsers = new List<User>()
            {
                new User()
                {
                    id = 4,
                    first_name="Eve",
                    last_name= "Holt",
                    avatar= "https://s3.amazonaws.com/uifaces/faces/twitter/marcoramires/128.jpg"
                },
                new User()
                {
                    id= 5,
                    first_name= "Charles",
                    last_name= "Morris",
                    avatar= "https://s3.amazonaws.com/uifaces/faces/twitter/stephenmoon/128.jpg"
                },
                new User(){
                    id= 6,
                    first_name= "Tracey",
                    last_name= "Ramos",
                    avatar= "https://s3.amazonaws.com/uifaces/faces/twitter/bigmancho/128.jpg"
                }
            };

            Assert.Equals(result.page, 2);
            Assert.Equals(result.per_page, 3);
            Assert.Equals(result.total, 12);
            Assert.Equals(result.total_pages, 4);
            for (int i = 0; i < result.data.Count; i++)
            {
                Assert.Equals(result.data[i].id, expectedUsers[i].id);
                Assert.Equals(result.data[i].first_name, expectedUsers[i].first_name);
                Assert.Equals(result.data[i].last_name, expectedUsers[i].last_name);
                Assert.Equals(result.data[i].avatar, expectedUsers[i].avatar);
            }
        }

        [TestStep]
        public void Delete_User()
        {
            var httpResponse = HttpRequest.Delete("https://reqres.in/api/users/2");
            Assert.Equals(httpResponse, string.Empty);
        }
    }
}
