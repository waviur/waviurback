using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Data.SqlClient;
using System.Web.Configuration;

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
        

    }
    public class contacts
    {
        public string skype;
        public string mail;
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        string connectionString = "";

        public ValuesController()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["memberdata"].ConnectionString;
        }

        // GET api/values
        public IEnumerable<member> Get()
        {
            List<member> sqlLst = new List<member>();
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var sqlCommand = new SqlCommand("select * from members join contacts on members.ID = contacts.memberID", connection);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var name = reader.GetValue(0).ToString();
                var position = reader.GetValue(1).ToString();
                var tag = reader.GetValue(2).ToString();
                var photo = reader.GetValue(4).ToString();
                var ID = Convert.ToInt32(reader.GetValue(3));
                var mail = reader.GetValue(5).ToString();
                var skype = reader.GetValue(6).ToString();

                var member = new member
                {
                    name = name,
                    position = position,
                    tag = tag,
                    photo = photo,
                    ID = ID,
                    contacts = new contacts
                    {
                        mail = mail,
                        skype = skype
                    }

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
            var filestring = new string(file.FileName.Reverse().ToArray());
            var numberelements = filestring.IndexOf('.');
            var formatphoto = new string(filestring.Substring(0, numberelements).Reverse().ToArray());


            file.SaveAs(path + $"{id}.{formatphoto}");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var sqlCommand = new SqlCommand($"update members set photo = '{id}.{formatphoto}' where ID = {id}", connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();

            return "/pics/" + file.FileName;
        }
        [Route("api/values/deletefile/{id:int}")]
        [HttpPost]
        public void deletefile(int id)
        {
            
            var connection = new SqlConnection(connectionString);
            var path = AppDomain.CurrentDomain.BaseDirectory + "pics\\";
            connection.Open();
            var worker = getmemberbyid(id);
            System.IO.File.Delete(path + $"{worker.photo}");
            var sqlCommand = new SqlCommand($"update members set photo = '' where ID = {id}", connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }

        private member getmemberbyid(int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var sqlCommand = new SqlCommand($"select * from members join contacts on members.ID = contacts.memberID where members.Id = {id} ", connection);
            var reader = sqlCommand.ExecuteReader();
            var member = new member();
            while (reader.Read())
            {
                member.name = reader.GetValue(0).ToString();
                member.position = reader.GetValue(1).ToString();
                member.tag = reader.GetValue(2).ToString();
                member.photo = reader.GetValue(4).ToString();
                member.ID = Convert.ToInt32(reader.GetValue(3));
                member.contacts = new contacts
                {
                    mail = reader.GetValue(5).ToString(),
                    skype = reader.GetValue(6).ToString()
                };
            }
            connection.Close();
            return member;

        }

        // GET api/values/5
        public member Get(int id)
        {
            return getmemberbyid(id);

        }

        // POST api/values
        public int Post(memberget value)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var sqlCommand = new SqlCommand($"insert into members (name,position,tag) values ('{value.lastname}','{value.position}','{value.tag}'); select scope_identity()", connection);
            var createdid = sqlCommand.ExecuteScalar();
            sqlCommand.CommandText = $"insert into contacts (mail,skype,memberID) values ('{value.mail}','{value.skype}',{Convert.ToInt32(createdid)})";
            sqlCommand.ExecuteNonQuery();
            connection.Close();
            return Convert.ToInt32(createdid);
        }

        // PUT api/values/5
        public int Put(int id, memberget value)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var oldMember = getmemberbyid(id);

            var elemforuploads = new member();
            var elemforcont = new contacts();
            
            elemforuploads.ID = oldMember.ID;
            if (string.IsNullOrEmpty(value.lastname))
            {
                elemforuploads.name = oldMember.name;
            }
            else
            {
                elemforuploads.name = value.lastname;
            }
            if (string.IsNullOrEmpty(value.tag))
            {
                elemforuploads.tag = oldMember.tag;
            }
            else
            {
                elemforuploads.tag = value.tag;
            }

            if (string.IsNullOrEmpty(value.position))
            {
                elemforuploads.position = oldMember.position;
            }
            else
            {
                elemforuploads.position = value.position;
            }
            elemforuploads.contacts = elemforcont;

            if (string.IsNullOrEmpty(value.mail))
            {
                elemforcont.mail = oldMember.contacts.mail;
            }
            else
            {
                elemforcont.mail = value.mail;
            }

            if (string.IsNullOrEmpty(value.skype))
            {
                elemforcont.skype = oldMember.contacts.skype;
            }
            else
            {
                elemforcont.skype = value.skype;
            }
            var sqlcommand = new SqlCommand($"update members set name = '{elemforuploads.name}', position = '{elemforuploads.position}', tag = '{elemforuploads.tag}' where ID = {elemforuploads.ID}", connection);
            sqlcommand.ExecuteNonQuery();
            sqlcommand.CommandText = $"update contacts set mail = '{elemforcont.mail}', skype = '{elemforcont.skype}' where memberID = {id}";
            sqlcommand.ExecuteNonQuery();
            connection.Close();
            return elemforuploads.ID;
        }
        // DELETE api/values/5
        public void Delete(int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var OldMember = getmemberbyid(id);

            var sqlCommand = new SqlCommand ($"delete contacts where memberID = {id}", connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.CommandText = $"delete members where ID = {id}";
            sqlCommand.ExecuteNonQuery();
            connection.Close();
            var path = AppDomain.CurrentDomain.BaseDirectory + "pics\\";
            if (File.Exists(path + $"{OldMember.photo}"))
            {
                System.IO.File.Delete(path + $"{OldMember.photo}");
            }
        }
    }
}
