using MISA.Core.Dtos;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CustomerService : BaseService<Customer, FilterCustomerDto, PageResultDto<Customer>>, ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomerService(ICustomerRepo customerRepo) : base(customerRepo)
        {
            _customerRepo = customerRepo;
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng thêm mới</param>
        /// <returns>Thông tin khách hàng</returns>
        /// CreatedBy: (NDH 22/10/2025)
        /// <exception cref="ValidateException"></exception>
        public override Customer Insert(Customer entity)
        {
            // Thực hiện validate dữ liệu
            // 1. Các thông tin bắt buộc nhập
            if (string.IsNullOrEmpty(entity.CustomerCode))
            {
                throw new ValidateException("Mã khách hàng k đc để trống");
            }
            if (string.IsNullOrEmpty(entity.CustomerName))
            {
                throw new ValidateException("Tên khách hàng k đc để trống");
            }

            // 2.Check mã khách hàng không được trùng
            var isExits = _customerRepo.CheckCustomerCode(entity.CustomerCode);

            if (isExits)
            {
                throw new ValidateException("Mã khách hàng k đc để trùng");
            }
            // Gọi repo để lưu dữ liệu vào DB
            return base.Insert(entity);
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="id">id khách hàng</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Thông tin khách hàng</returns>
        /// <exception cref="ValidateException"></exception>
        /// CreatedBy: (NDH 22/10/2025)
        public override int Update(Customer entity)
        {   
            // Kiểm tra tồn tài customer
            var exitCustomer = _customerRepo.FindById(entity.CustomerId);

            if (exitCustomer == null)
                throw new ValidateException("Không tồn tại khách hàng");

            // Thực hiện validate dữ liệu
            // 1. Các thông tin bắt buộc nhập
            if (string.IsNullOrEmpty(entity.CustomerCode))
            {
                throw new ValidateException("Mã khách hàng k đc để trống");
            }
            if (string.IsNullOrEmpty(entity.CustomerName))
            {
                throw new ValidateException("Tên khách hàng k đc để trống");
            }

            // 2.Check mã khách hàng không được trùng
            var isExits = _customerRepo.CheckCustomerCode(entity.CustomerCode);

            if (isExits)
            {
                throw new ValidateException("Mã khách hàng k đc để trùng");
            }

            exitCustomer.CustomerCode = entity.CustomerCode;
            exitCustomer.CustomerName = entity.CustomerName;
            exitCustomer.CustomerAddr = entity.CustomerAddr;

            return base.Update(exitCustomer);
        }

    }
}
