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

    public UserService(
        IConfiguration config,
        ICustomPasswordService passwordService,
        AesCrypto crypto)
    {
        _connectionString = config.GetConnectionString("StoreDb")
            ?? throw new ArgumentNullException("StoreDb");
        _passwordService = passwordService;
        _crypto = crypto;
    }

    // -------------------------------------------------------------
    // VALIDATIONS
    // -------------------------------------------------------------
    private void ValidateRequired(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{field} is required.");
    }

    private void ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.");
    }

    private void ValidatePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return;

        if (!Regex.IsMatch(phone, @"^\d+$"))
            throw new ArgumentException("Phone number must contain only numbers.");
    }

    private void ValidateCardId(string? cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId)) return;

        if (!Regex.IsMatch(cardId, @"^\d+$"))
            throw new ArgumentException("Card ID must contain only numbers.");
    }

    private string EncryptCardId(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input ?? "";

        if (_crypto.IsDataEncrypted(input))
            return input;

        return _crypto.Encrypt(input);
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------
    public async Task<int> CreateAsync(UserAccountCreateDTO dto)
    {
        ValidateRequired(dto.UserName, "UserName");
        ValidateEmail(dto.Email);
        ValidatePhone(dto.PhoneNumber);
        ValidateCardId(dto.CardId);

        _passwordService.CreatePasswordHash(
            dto.Password,
            out byte[] hash,
            out byte[] salt
        );

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Create", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserName", dto.UserName);
        cmd.Parameters.AddWithValue("@Email", dto.Email);
        cmd.Parameters.AddWithValue("@PhoneNumber",
            dto.PhoneNumber == null ? DBNull.Value : dto.PhoneNumber);

        cmd.Parameters.AddWithValue("@CardID",
            dto.CardId == null ? DBNull.Value : EncryptCardId(dto.CardId));

        cmd.Parameters.AddWithValue("@PasswordHash", hash);
        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
        cmd.Parameters.AddWithValue("@IsActive", true);
        cmd.Parameters.AddWithValue("@RoleID", dto.RoleId);
        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@UpdatedAt", DBNull.Value);
        cmd.Parameters.AddWithValue("@Url", dto.url);

        var output = new SqlParameter("@NewUserID", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(output);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)output.Value;
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------
    public async Task<List<UserAccountDTO>> GetAllAsync(string? search)
    {
        var result = new List<UserAccountDTO>();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_GetAll", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Search",
            (object?)search ?? DBNull.Value);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new UserAccountDTO
            {
                UserId = (int)reader["UserID"],
                UserName = (string)reader["UserName"],
                Email = (string)reader["Email"],
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value
                    ? null
                    : (string)reader["PhoneNumber"],
                CardId = reader["CardID"] == DBNull.Value
                    ? null
                    : _crypto.SafeDecrypt((string)reader["CardID"]),
                IsActive = (bool)reader["IsActive"],
                RoleId = (int)reader["RoleID"],
                RoleName = (string)reader["RoleName"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                    ? null
                    : (DateTime?)reader["UpdatedAt"]
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

        if (!await reader.ReadAsync())
            return null;

        return new UserAccountDTO
        {
            UserId = (int)reader["UserID"],
            UserName = (string)reader["UserName"],
            Email = (string)reader["Email"],
            PhoneNumber = reader["PhoneNumber"] == DBNull.Value
                ? null
                : (string)reader["PhoneNumber"],
            CardId = reader["CardID"] == DBNull.Value
                ? null
                : _crypto.SafeDecrypt((string)reader["CardID"]),
            IsActive = (bool)reader["IsActive"],
            RoleId = (int)reader["RoleID"],
            CreatedAt = (DateTime)reader["CreatedAt"],
            UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                ? null
                : (DateTime?)reader["UpdatedAt"]
        };
    }

    // -------------------------------------------------------------
    // UPDATE (PERMITE CAMBIAR ROL)
    // -------------------------------------------------------------
    public async Task<bool> UpdateAsync(int id, UserUpdateDTO dto)
    {
        ValidateRequired(dto.UserName, "UserName");
        ValidateEmail(dto.Email);
        ValidatePhone(dto.PhoneNumber);
        ValidateCardId(dto.CardId);

        if (dto.RoleId.HasValue && dto.RoleId.Value <= 0)
            throw new ArgumentException("Invalid RoleId.");

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserID", id);
        cmd.Parameters.AddWithValue("@UserName", dto.UserName);
        cmd.Parameters.AddWithValue("@Email", dto.Email);
        cmd.Parameters.AddWithValue("@PhoneNumber",
            dto.PhoneNumber == null ? DBNull.Value : dto.PhoneNumber);

        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@Url", DBNull.Value);

        cmd.Parameters.AddWithValue("@CardID",
            dto.CardId == null ? DBNull.Value : EncryptCardId(dto.CardId));

        cmd.Parameters.AddWithValue("@RoleID",
            dto.RoleId.HasValue ? dto.RoleId.Value : DBNull.Value);

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            _passwordService.CreatePasswordHash(
                dto.Password,
                out byte[] hash,
                out byte[] salt);

            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = hash;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary).Value = salt;
        }
        else
        {
            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = DBNull.Value;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary).Value = DBNull.Value;
        }

        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)returnParam.Value == 1;
    }

    // -------------------------------------------------------------
    // DELETE
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

    public async Task<bool> EditPhoto(int id, UserImageDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.url))
            throw new ArgumentException("Image URL is required.");

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("Users.sp_User_EditPhoto", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@UserID", id);
        cmd.Parameters.AddWithValue("@Url", dto.url);

        var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
        returnParam.Direction = ParameterDirection.ReturnValue;

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return (int)returnParam.Value == 1;
    }

}
