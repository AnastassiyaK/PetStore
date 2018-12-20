using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_16_2018_PetStore
{
    class Pet
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string name{ get; set; }
        [JsonProperty(PropertyName = "photoUrls")]
        public ICollection<string> photoUrls { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }
        
            
    }
}
