using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;
using System.IO;

namespace _12_16_2018_PetStore
{

    public class PetStore
    {
       
        private static Logger log = LogManager.GetCurrentClassLogger();
        public const string path = "https://petstore.swagger.io";
        public string name= "'Doggie'";
              
        public string AddPet()
        {
            log.Debug($"ADDING A PET");
            string photoUrls = "'http://image.com/image.jpg'";
            string status = "'available'";
            string json = "{"+$"'name':{name},'photoUrls':[{photoUrls}],'status':{status}"+"}";
            json = json.Replace("'","\"");
            RestAssured rest = new RestAssured();
            var given = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").Body(json);
            var when = given.When().Post(path + "/v2/pet");
            var then = when.Then();
            string response = then.Retrieve(x => x.ToString()).ToString();            
            string id = then.Retrieve(x => x.id).ToString();
            log.Debug($"Sending request {json}");
            log.Debug($"Response body: {response}");
            log.Debug($"Response id: {id}");
            return id;
            
        }
        [Test]
        [Order(1)]
        [Description("Get a pet")]
        public void GetPet()
        {
            string id = AddPet();
            log.Debug($"GETTING A PET WITH {id}");
            RestAssured rest = new RestAssured();
            string response = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").When().Get(path + "/v2/pet/" + id).
                Then().Retrieve(x => x.name).ToString();
            string nameofdog = name.Replace("'"," ").Trim();
            log.Debug($"Pet is received. Request name {nameofdog}, response name {response}");
            Assert.That(response, Is.EqualTo(nameofdog));
           

        }
        [Test]
        [Order(2)]
        [Description("Update a pet")]
        public void UpdatePet()
        {
            string id = AddPet();
            log.Debug($"UPDATING A PET WITH {id}");
            RestAssured rest = new RestAssured();
            string updatename = "'Bobik'";
            string previuosname = name.Replace("'", " ").Trim(); 
            string response = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").When().Get(path + "/v2/pet/"+id).
                Then().Retrieve(x => x.name).ToString();
            string json = "{" + $"'id':{id},'name':{updatename}" + "}";
            json = json.Replace("'", "\"");
            string updatedresponse = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").Body(json).When().Put(path + "/v2/pet/").
                Then().Retrieve(x => x.name).ToString();
            string nameofdog = updatename.Replace("'", " ").Trim();
            log.Debug($"Pet is updated. Old name was{previuosname}, response updated name {updatedresponse}");
            Assert.That(response, Is.EqualTo(previuosname));
            Assert.That(response, Is.Not.EqualTo(updatename));
            
        }
        [Test]
        [Order(3)]
        [Description("Delete a pet")]
        public void DeletePet()
        {
            string id = AddPet();
            log.Debug($"DELETING A PET WITH {id}");
            RestAssured rest = new RestAssured();
            rest.Given().Header("api_key", "special-key").
                Header("accept", "application/json").When().Delete(path + "/v2/pet/" + id).
                Then().Debug();
            var givenwhen = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").When().Get(path + "/v2/pet/" + id);
            string response = givenwhen.
                Then().Retrieve(x => x.ToString()).ToString();
            string message = givenwhen.
                Then().Retrieve(x => x.message).ToString(); ;
            log.Debug($"Response body : {response}");
            Assert.That("Pet not found", Is.EqualTo(message));
            
        }
       [Test]
       [Order(4)]
       [Description("Attach file json to add a pet")]
       public void ReadFromJSONFile()
        {
            log.Debug("SENDING INFO FROM JSON");
            Pet pet = new Pet();
            string line;
            var encoding = Encoding.GetEncoding(1251);
            using (StreamReader r = new StreamReader(@"E:\Epam_training\12_16_2018_PetStore\12_16_2018_PetStore\Resourses\AddPet.json", encoding))
            {
                string json = r.ReadToEnd();
                pet = JsonConvert.DeserializeObject<Pet>(json);

            }
            line = "{"+$"'id':{pet.id},'name':'{pet.name}','photoUrls':['{pet.photoUrls.ToList()[0]}'],'status':'{pet.status}'"+"}";
            string body = line.Replace("'","\"");
            RestAssured rest = new RestAssured();
            var given = rest.Given().Header("Content-Type", "application/json").
                Header("accept", "application/json").Body(body);
            var when = given.When().Post(path + "/v2/pet");
            var then = when.Then();
            string responseid = then.Retrieve(x => x.id).ToString();
            string response = then.Retrieve(x => x.ToString()).ToString();
            string petid = pet.id.ToString();
            log.Debug($"Response body {response}");
            log.Debug($"We sent pet id {petid} and received pet id {responseid}");
            Assert.That(responseid, Is.EqualTo(petid));
            

        }
        [Test]
        [Order(5)]
        [Description("Sending a json file")]
        public void SendAFile()
        {

        }

        //[Test]
        //[Order(6)]
        //[Description("Sen")]
    }
}
