using Infrastructure.Security;

namespace MASKER.Tests.Infrastructure;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher;

    public BCryptPasswordHasherTests()
    {
        _hasher = new BCryptPasswordHasher();
    }

    [Fact]
    public void HashPassword_GecerliSifre_HashDondurur()
    {
        // Act
        var hash = _hasher.HashPassword("Test123!");

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual("Test123!", hash);
        Assert.StartsWith("$2", hash); // BCrypt hash her zaman $2 ile baslar
    }

    [Fact]
    public void HashPassword_AyniSifre_FarkliHashUretir()
    {
        // Act
        var hash1 = _hasher.HashPassword("AyniSifre");
        var hash2 = _hasher.HashPassword("AyniSifre");

        // Assert - BCrypt her seferinde farkli salt kullanir
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void VerifyPassword_DogruSifre_TrueDondurur()
    {
        // Arrange
        var sifre = "GuvenliSifre123!";
        var hash = _hasher.HashPassword(sifre);

        // Act
        var result = _hasher.VerifyPassword(sifre, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_YanlisSifre_FalseDondurur()
    {
        // Arrange
        var hash = _hasher.HashPassword("DogruSifre");

        // Act
        var result = _hasher.VerifyPassword("YanlisSifre", hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_GecersizHash_FalseDondurur()
    {
        // Act
        var result = _hasher.VerifyPassword("sifre", "gecersiz_hash");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_BosHash_FalseDondurur()
    {
        // Act
        var result = _hasher.VerifyPassword("sifre", "");

        // Assert
        Assert.False(result);
    }
}
