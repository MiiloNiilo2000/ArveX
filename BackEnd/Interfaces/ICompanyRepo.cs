using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface ICompanyRepo
    {
        Task<List<Company>> GetUserCompanies(Profile profile);
    }
}