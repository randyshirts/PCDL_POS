using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class User : IdentityUser, IUser, IEntity<string>
    {
        //public override Id { get; set; }
        //{
        //    get { return Int32.Parse(base.Id); }
        //    set { base.Id = value.ToString(); }
        //}
        //public int IdInt { get; set; }
        


        public string EmailConfirmationCode { get; set; }
        public string PasswordResetCode { get; set; }
        public virtual Person PersonUser { get; set; }
        public bool IsTransient()
        {
            return EqualityComparer<string>.Default.Equals(Id, default(string));
        }

        //int IEntity<int>.Id
        //{
        //    get { return Int32.Parse(Id); }
        //    set { Id = value.ToString(); }
        //}
    }
}
