using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ViewModels
{
    public class PrivatePersonViewModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string gender { get; set; }
        public string seriShChar { get; set; }
        public string seriSh { get; set; }
        public string serial { get; set; }
        public string shNumber { get; set; }
        public string birthDate { get; set; }
        public string placeOfIssue { get; set; }
        public string placeOfBirth { get; set; }
        public string signitureFile { get; set; }

        public static implicit operator PrivatePersonViewModel(JToken jsonData)
        {
            return new PrivatePersonViewModel()
            {
                firstName = (string)jsonData["firstName"],
                lastName = (string)jsonData["lastName"],
                fatherName = (string)jsonData["fatherName"],
                gender = (string)jsonData["gender"],
                seriShChar = (string)jsonData["seriShChar"],
                seriSh = (string)jsonData["seriSh"],
                serial = (string)jsonData["serial"],
                shNumber = (string)jsonData["shNumber"],
                birthDate = (string)jsonData["birthDate"],
                placeOfIssue = (string)jsonData["placeOfIssue"],
                placeOfBirth = (string)jsonData["placeOfBirth"],
                signitureFile = (string)jsonData["signitureFile"]
            };
        }
    }
}
