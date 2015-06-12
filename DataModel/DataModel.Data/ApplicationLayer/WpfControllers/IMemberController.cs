using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IMemberController
    {
        int AddNewMember(Member consignor);
        void DeleteMemberById(int id);
        void UpdateMember(Member updatedMember);
        Member GetMemberByName(string firstName, string lastName);
        List<string> GetMemberNames();
        List<string> GetMemberNamesLastnameFirst();
        Member GetMemberByFullName(string fullName);
        double GetMemberCreditBalance(string name);
        IEnumerable<Member> GetAllMembers();
    }
}
