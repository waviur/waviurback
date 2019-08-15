using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class member
    {
        public string name;
        public int ID;
        public string tag;
        public string position;
        public contacts contacts;
        public string photo;
    }
    public class memberget
    {
        public string lastname;
        public string position;
        public string tag;
        public string skype;
        public string mail;
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
        string connectionString = @"Data Source=CHERNOV\SQL2016;Initial Catalog=members;Integrated Security=True";

        static List<member> lst = new List<member>() {
                new member()
                {
                    name = "Землянова Кристина",
                    ID = 1,
                    tag = "QA",
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
                     tag = "PO",
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
                     tag = ".NetCore   MYSQL",
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
                     tag = "JS  .NetCore  MYSQl" ,
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
                     tag = "JS .NETCore MYSQL",
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
                    tag = ".NetCore MySQL",
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
            List<member> sqlLst = new List<member>();
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var sqlCommand = new SqlCommand("select * from members", connection);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var name = reader.GetValue(0).ToString();
                var position = reader.GetValue(1).ToString();
                var tag = reader.GetValue(2).ToString();
                var photo = reader.GetValue(4).ToString();
                var ID = Convert.ToInt32(reader.GetValue(3));

                var member = new member
                {
                    name = name,
                    position = position,
                    tag = tag,
                    photo = photo,
                    ID = ID
                };

                sqlLst.Add(member);
            }
            connection.Close();
            return sqlLst;
        }

        [Route("api/values/uploadfile/{id:int}")]
        [HttpPost]
        public string uploadfile(int id)
        {
            var file = HttpContext.Current.Request.Files["photo"];
            var path = AppDomain.CurrentDomain.BaseDirectory + "pics\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            file.SaveAs(path+$"{id}.png");
            return "/pics/"+file.FileName;
        }

        // GET api/values/5
        public member Get(int id)
        {
            return lst.First(elem => elem.ID == id);
        }

        // POST api/values
        public int Post(memberget value)
        {   
            
            var elemforadd = new member();
            var elemforcont = new contacts();
            elemforadd.ID = lst.Max(elem => elem.ID + 1);
            elemforadd.photo = value.photo;
            elemforadd.name =   value.lastname;
            elemforadd.tag = value.tag;
            elemforadd.position = value.position;
            elemforadd.contacts = elemforcont;
            elemforcont.skype = value.skype;
            elemforcont.mail = value.mail;
            lst.Add(elemforadd);
            return elemforadd.ID;
        }

        // PUT api/values/5
        public int Put(int id, memberget value)
        {
            var elemforuploads = new member();
            var elemforcont = new contacts();
            var elemlast = lst.First(elem => elem.ID == id);
            elemforuploads.ID = elemlast.ID;
            if (string.IsNullOrEmpty(value.lastname))
            {
                elemforuploads.name = elemlast.name;
            }
            else
            {
                elemforuploads.name = value.lastname;
            }
            if (string.IsNullOrEmpty(value.tag))
            {
                elemforuploads.tag = elemlast.tag;
            }
            else
            {
                elemforuploads.tag = value.tag;
            }

            if (string.IsNullOrEmpty(value.position))
            {
                elemforuploads.position = elemlast.position;
            }
            else
            {
                elemforuploads.position = value.position;
            }
            elemforuploads.contacts = elemforcont;

            if (string.IsNullOrEmpty(value.mail))
            {
                elemforcont.mail = elemlast.contacts.mail;
            }
            else
            {
                elemforcont.mail = value.mail;
            }

            if (string.IsNullOrEmpty(value.skype))
            {
                elemforcont.skype = elemlast.contacts.skype;
            }
            else
            {
                elemforcont.skype = value.skype;
            }
            lst.Remove(elemlast);
            lst.Add(elemforuploads);
            return elemforuploads.ID;



        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            var elemfordelete = lst.First(elem => elem.ID == id);
            lst.Remove(elemfordelete);
        }
    }
}
