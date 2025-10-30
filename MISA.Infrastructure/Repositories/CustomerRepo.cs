using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Dtos;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class CustomerRepo : BaseRepo<Customer, FilterCustomerDto, PageResultDto<Customer>>, ICustomerRepo
    {

        public CustomerRepo(IConfiguration config) : base(config)
        {
        }
        /// <summary>
        /// Hàm kiểm tra trùng mã khác hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Không trùng</returns>
        /// CreatedBy: (NDH 22/10/2025)
        public bool CheckCustomerCode(string customerCode)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo các tham số vào câu lệnh
                var param = new DynamicParameters();
                param.Add("@code", customerCode);

                // 2. Mã khách hàng không được phép trùng
                var checkCustomerCode = @"Select count(*) from customer where customer_code = @code";
                int count = mySqlConnection.Query<int>(checkCustomerCode, param).FirstOrDefault();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
