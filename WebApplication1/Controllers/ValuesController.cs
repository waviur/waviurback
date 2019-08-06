using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class member
    {
        public string name;
        public string position;
        public contacts contacts;
        public string photo;
    }
    public class contacts
    {
        public string skype;
        public string mail;
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<member> Get()
        {
            List<member> lst = new List<member>() {
                new member()
                {
                    name = "Землянова Кристина",
                    position = "Скрам-мастер",
                    photo = "землякова.png",
                    contacts = new contacts()
                    {
                        mail = "zemlyanova@moedelo.org",
                        skype = "zemlyanova92"
                    }
                },
                new member()
                {
                     name = "Федорова Ксения",
                    position = "Продуктолог",
                    photo = "федорова.png",
                    contacts = new contacts()
                    {
                        mail = "fedorova@moedelo.org",
                        skype = "kseniay199228"
                    }
                },
                new member()
                {
                     name = "Козлов Роман",
                    position = "Разработчик",
                    photo = "козлов.png",
                    contacts = new contacts()
                    {
                        mail = "kozlov@moedelo.org",
                        skype = "from_kuzneburg"
                    }
                },
                 new member()
                {
                     name = "Постников Максим",
                    position = "Разработчик",
                    photo = "постников.png",
                    contacts = new contacts()
                    {
                        mail = "postnikov@moedelo.org",
                        skype = "webert1990"
                    }
                },
                     new member()
                {
                     name = "Федосеев Николай",
                    position = "Разработчик",
                    photo = "федосеев.png",
                    contacts = new contacts()
                    {
                        mail = "n.fedoseev@moedelo.org",
                        skype = "xneek"
                    }
                },
                      new member()
                {
                     name = "Лебедев Александр",
                    position = "Разработчик",
                    photo = "лебедев а.png",
                    contacts = new contacts()
                    {
                        mail = "a.lebedev@moedelo.org",
                        skype = "imofka"
                    }
                },


        };

        return lst;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
