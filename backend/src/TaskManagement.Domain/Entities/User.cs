using System.Text.RegularExpressions;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string name, string email, string passwordHash)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidatePasswordHash(passwordHash);

        Id = Guid.NewGuid();
        Name = name;
        Email = email.ToLower();
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    private User() { }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("User name cannot be empty");
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailPattern))
            throw new DomainException("Invalid email format");
    }

    private void ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty");
    }
}

