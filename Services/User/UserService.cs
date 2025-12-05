using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using StoreApi.Tools;
using System.Data;

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

    public async Task<int> CreateAsync(UserAccountCreateDTO dto)
    {
        _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Create", conn);
        cmd.CommandType = CommandType.StoredProcedure; 

        cmd.Parameters.AddWithValue("@UserName", _crypto.Encrypt(dto.UserName));
        cmd.Parameters.AddWithValue("@Email", _crypto.Encrypt(dto.Email));
        cmd.Parameters.AddWithValue("@PasswordHash", hash);
        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
        cmd.Parameters.AddWithValue("@PhoneNumber", dto.PhoneNumber == null ? (object)DBNull.Value : _crypto.Encrypt(dto.PhoneNumber));
        cmd.Parameters.AddWithValue("@CardID", dto.CardId == null ? (object)DBNull.Value : _crypto.Encrypt(dto.CardId));
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

    public async Task<bool> UpdateAsync(int id, UserUpdateDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Update", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        byte[] passwordHash = null!;
        byte[] passwordSalt = null!;

        if (!string.IsNullOrEmpty(dto.Password))
        {
            var passwordService = new CustomPasswordService();
            passwordService.CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);
        }

        cmd.Parameters.AddWithValue("@UserID", dto.UserId);
        cmd.Parameters.AddWithValue("@UserName", dto.UserName ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Email", dto.Email ?? (object)DBNull.Value);
        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 512).Value =
            string.IsNullOrEmpty(dto.Password) ? DBNull.Value : (object)passwordHash;
        cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, 128).Value =
            string.IsNullOrEmpty(dto.Password) ? DBNull.Value : (object)passwordSalt;
        cmd.Parameters.AddWithValue("@PhoneNumber", (object?)dto.PhoneNumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@CardID", (object?)dto.CardId ?? DBNull.Value);

        // Add return value parameter
        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        int result = (int)returnParam.Value;

        return result == 1;
    }



    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Delete", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserID", id);

        await conn.OpenAsync();

        // Execute and check rows affected
        var rowsAffected = await cmd.ExecuteScalarAsync();  
        return (int)(rowsAffected ?? 0) > 0;
    }

 

    public Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeStatus(int id, UserActiveDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_ChangeStatus", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@UserId", dto.UserId);
        cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);

        // Capture the RETURN value from SQL
        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParameter.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        int result = (int)returnParameter.Value;

        return result == 1;
    }

    public async Task<bool> ChangeRole(int id, UserRoleDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_ChangeRole", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@UserId", dto.UserId);
        cmd.Parameters.AddWithValue("@RoleId", dto.RoleId);

        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParameter.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        int result = (int)returnParameter.Value;

        return result == 1;
    }



}

