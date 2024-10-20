using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class TermService
    {
        private TermDAL _termDAL;

        public TermService()
        {
            _termDAL = new TermDAL();
        }

        // Lấy tất cả kỳ học dựa vào User ID
        public List<TermDTO> GetAllTermsByUserID(int userID)
        {
            return _termDAL.GetAllTermsByUserID(userID);
        }
        // Lấy TermID dựa vào TermName
        public int GetTermIDByTermName(string termName)
        {
            return _termDAL.GetTermIDByTermName(termName);
        }
    }
}
