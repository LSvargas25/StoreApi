using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using StoreApi.Tools;
using System.Data;
using System.Text.RegularExpressions;

public class UserService : IUserService
{
    private readonly string _connectionString;
    private readonly ICustomPasswordService _passwordService;
    private readonly AesCrypto _crypto;

    public UserService(IConfiguration config, ICustomPasswordService passwordService, AesCrypto crypto)
    {
        _connectionString = config.GetConnectionString("StoreDb")
            ?? throw new ArgumentNullException("StoreDb", "Connection string is not configured!");
        _passwordService = passwordService;
        _crypto = crypto;
    }

    // -------------------------------------------------------------
    // VALIDATION HELPERS
    // -------------------------------------------------------------
    private void ValidateRequired(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{field} is required.");
    }

    private void ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");

        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!regex.IsMatch(email))
            throw new ArgumentException("Email format is invalid.");
    }

    private void ValidatePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return;

        if (!Regex.IsMatch(phone, @"^[0-9+\- ]+$"))
            throw new ArgumentException("Phone number contains invalid characters.");
    }

    private string EncryptIfNeeded(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input ?? "";

        // Already encrypted → do not encrypt twice
        if (_crypto.IsDataEncrypted(input))
            return input;

        return _crypto.Encrypt(input);
    }

    // -------------------------------------------------------------
    // CREATE USER
    // -------------------------------------------------------------
    public async Task<int> CreateAsync(UserAccountCreateDTO dto)
    {
        // --- Input validation ---
        ValidateRequired(dto.UserName, "UserName");
        ValidateEmail(dto.Email);
        ValidatePhone(dto.PhoneNumber);

        // --- Password hashing ---
        _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Create", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        // --- Encrypt sensitive data ---
        cmd.Parameters.AddWithValue("@UserName", EncryptIfNeeded(dto.UserName));
        cmd.Parameters.AddWithValue("@Email", EncryptIfNeeded(dto.Email));
        cmd.Parameters.AddWithValue("@PhoneNumber", dto.PhoneNumber == null ? DBNull.Value : EncryptIfNeeded(dto.PhoneNumber));
        cmd.Parameters.AddWithValue("@CardID", dto.CardId == null ? DBNull.Value : EncryptIfNeeded(dto.CardId));

        // --- Other fields ---
        cmd.Parameters.AddWithValue("@PasswordHash", hash);
        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
        cmd.Parameters.AddWithValue("@IsActive", true);
        cmd.Parameters.AddWithValue("@RoleID", dto.RoleId);
        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@UpdatedAt", DBNull.Value);

        var output = new SqlParameter("@NewUserID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        cmd.Parameters.Add(output);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)output.Value;
    }

    // -------------------------------------------------------------
    // GET ALL USERS
    // -------------------------------------------------------------
    public async Task<List<UserAccountDTO>> GetAllAsync(string? search)
    {
        var result = new List<UserAccountDTO>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_GetAll", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new UserAccountDTO
            {
                UserId = (int)reader["UserID"],
                UserName = _crypto.SafeDecrypt((string)reader["UserName"]),
                Email = _crypto.SafeDecrypt((string)reader["Email"]),
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : _crypto.SafeDecrypt((string)reader["PhoneNumber"]),
                CardId = reader["CardID"] == DBNull.Value ? null : _crypto.SafeDecrypt((string)reader["CardID"]),
                IsActive = (bool)reader["IsActive"],
                RoleId = (int)reader["RoleID"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTime?)reader["UpdatedAt"]
            });
        }

        return result;
    }

    // -------------------------------------------------------------
    // GET BY ID
    // -------------------------------------------------------------
    public async Task<UserAccountDTO?> GetByIdAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_GetById", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", id);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new UserAccountDTO
        {
            UserId = (int)reader["UserID"],
            UserName = _crypto.SafeDecrypt((string)reader["UserName"]),
            Email = _crypto.SafeDecrypt((string)reader["Email"]),
            PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : _crypto.SafeDecrypt((string)reader["PhoneNumber"]),
            CardId = reader["CardID"] == DBNull.Value ? null : _crypto.SafeDecrypt((string)reader["CardID"]),
            IsActive = (bool)reader["IsActive"],
            RoleId = (int)reader["RoleID"],
            CreatedAt = (DateTime)reader["CreatedAt"],
            UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTime?)reader["UpdatedAt"]
        };
    }

    // -------------------------------------------------------------
    // UPDATE USER
    // -------------------------------------------------------------
    public async Task<bool> UpdateAsync(int id, UserUpdateDTO dto)
    {
        ValidateRequired(dto.UserName, "UserName");
        ValidateEmail(dto.Email);
        ValidatePhone(dto.PhoneNumber);

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        // --- Encrypt sensitive data ---
        cmd.Parameters.AddWithValue("@UserName", EncryptIfNeeded(dto.UserName));
        cmd.Parameters.AddWithValue("@Email", EncryptIfNeeded(dto.Email));
        cmd.Parameters.AddWithValue("@PhoneNumber", dto.PhoneNumber == null ? DBNull.Value : EncryptIfNeeded(dto.PhoneNumber));
        cmd.Parameters.AddWithValue("@CardID", dto.CardId == null ? DBNull.Value : EncryptIfNeeded(dto.CardId));

        // --- Password update optional ---
        if (!string.IsNullOrEmpty(dto.Password))
        {
            _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);
            cmd.Parameters.AddWithValue("@PasswordHash", hash);
            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
        }
        else
        {
            cmd.Parameters.AddWithValue("@PasswordHash", DBNull.Value);
            cmd.Parameters.AddWithValue("@PasswordSalt", DBNull.Value);
        }

        cmd.Parameters.AddWithValue("@UserID", id);

        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)returnParam.Value == 1;
    }

    // -------------------------------------------------------------
    // DELETE USER
    // -------------------------------------------------------------
    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Delete", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserID", id);

        await conn.OpenAsync();
        var rows = await cmd.ExecuteScalarAsync();

        return (int)(rows ?? 0) > 0;
    }

    // -------------------------------------------------------------
    // CHANGE STATUS
    // -------------------------------------------------------------
    public async Task<bool> ChangeStatus(int id, UserActiveDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_ChangeStatus", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserId", id);
        cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)returnParam.Value == 1;
    }

    // -------------------------------------------------------------
    // CHANGE ROLE
    // -------------------------------------------------------------
    public async Task<bool> ChangeRole(int id, UserRoleDTO dto)
    {
        if (dto.RoleId < 1 || dto.RoleId > 12)
            throw new ArgumentException("RoleId is invalid.");

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_ChangeRole", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserId", id);
        cmd.Parameters.AddWithValue("@RoleId", dto.RoleId);

        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)returnParam.Value == 1;
    }
}
