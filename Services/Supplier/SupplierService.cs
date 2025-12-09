using StoreApi.Interface.Supplier;
using StoreApi.ModelsDTO.Supplier;
using StoreApi.Repository.Supplier;
using StoreApi.Repositorys.Supplier;
using System.Text.RegularExpressions;

namespace StoreApi.Services.Supplier
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(CreateSupplier dto)
        {
            ValidateName(dto.Name);
            ValidateEmail(dto.Email);
            ValidatePhone(dto.PhoneNumber);

  
            return await _repo.CreateAsync(dto);
        }

        public async Task<bool> UpdateAsync(int id, SupplierUpdate dto)
        {
            if (id != dto.SupplierId)
                throw new ArgumentException("Supplier ID in URL does not match Supplier ID in body.");

            ValidateName(dto.Name);
            ValidateEmail(dto.Email);
            ValidatePhone(dto.PhoneNumber);

            return await _repo.UpdateAsync(id, dto);
        }

        public async Task<List<SupplierDTO>> GetAllAsync(string? search)
        {
            return await _repo.GetAllAsync(search);
        }

        public async Task<SupplierDTO?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<bool> ChangeStatus(int id, SupplierStatus dto)
        {
            if (id != dto.SupplierId)
                throw new ArgumentException("Supplier ID in URL does not match Supplier ID in body.");

            return await _repo.ChangeStatusAsync(id, dto.IsActive);
        }

        public async Task<bool> ChangeRole(int id, SupplierRole dto)
        {
            if (id != dto.SupplierId)
                throw new ArgumentException("Supplier ID in URL does not match Supplier ID in body.");

            if (dto.SupplierTypeId.HasValue && dto.SupplierTypeId <= 0)
                throw new ArgumentException("SupplierTypeId must be greater than zero.");

            return await _repo.ChangeRoleAsync(id, dto.SupplierTypeId);
        }

        // ------------ VALIDACIONES --------------

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (!Regex.IsMatch(name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("Name only can contain letters and spaces.");
        }

        private void ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Email format is invalid.");
        }

        private void ValidatePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return;

            if (!phone.All(char.IsDigit))
                throw new ArgumentException("Phone number must contain only digits.");

            if (phone.Length < 8 || phone.Length > 20)
                throw new ArgumentException("Phone number length must be between 8 and 20 digits.");
        }
    }
}
