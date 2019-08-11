using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class member
    {
        public string name;
        public int ID;
        public string position;
        public contacts contacts;
        public string photo;
    }
    public class memberget
    {
        public string lastname;
        public string position;
        public string skype;
        public string mail;
        

    }
    public class contacts
    {
        public string skype;
        public string mail;
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        static List<member> lst = new List<member>() {
                new member()
                {
                    name = "Землянова Кристина",
                    ID = 1,
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
                     ID = 2,
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
                     ID = 3,
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
                     ID = 4,
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
                     ID = 5,
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
                     ID = 6,
                    position = "Разработчик",
                    photo = "лебедев а.png",
                    contacts = new contacts()
                    {
                        mail = "a.lebedev@moedelo.org",
                        skype = "imofka"
                    }
                },
        };
        // GET api/values
        public IEnumerable<member> Get()
        {
            return lst;
        }

        // GET api/values/5
        public member Get(int id)
        {
            return lst.First(elem => elem.ID == id);
        }

        // POST api/values
        public void Post([FromBody]memberget value)
        {   
            
            var elemforadd = new member();
            var elemforcont = new contacts();
            elemforadd.ID = lst.Max(elem => elem.ID + 1);

            elemforadd.name =   value.lastname;
            elemforadd.position = value.position;
            elemforadd.contacts = elemforcont;
            elemforcont.skype = value.skype;
            elemforcont.mail = value.mail;
            lst.Add(elemforadd);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]memberget value)
        {
            var elemforuploads = new member();
            var elemforcont = new contacts();
            var elemlast = lst.First(elem => elem.ID == id);
            elemforuploads.ID = elemlast.ID;
            if (string.IsNullOrWhiteSpace(value.lastname))
            {
                elemforuploads.name = elemlast.name;
            }
            else
            {
                elemforuploads.name = value.lastname;
            }

            if (string.IsNullOrWhiteSpace(value.position))
            {
                elemforuploads.position = elemlast.position;
            }
            else
            {
                elemforuploads.position = value.position;
            }
            elemforuploads.contacts = elemforcont;

            elemforcont.mail = value.mail;
            elemforcont.skype = value.skype;
            lst.Remove(elemlast);
                lst.Add(elemforuploads);



        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            var elemfordelete = lst.First(elem => elem.ID == id);
            lst.Remove(elemfordelete);
        }
    }
}
