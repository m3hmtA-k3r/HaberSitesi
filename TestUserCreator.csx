#!/usr/bin/env dotnet-script

// Test kullanıcısı oluşturma scripti
// Kullanım: dotnet script TestUserCreator.csx

#r "nuget: BCrypt.Net-Next, 4.0.3"

using BCrypt.Net;

string password = "Test123!";
string hashedPassword = BCrypt.HashPassword(password);

Console.WriteLine("=== TEST KULLANICI BİLGİLERİ ===");
Console.WriteLine($"Email: test@habersitesi.com");
Console.WriteLine($"Şifre: {password}");
Console.WriteLine($"BCrypt Hash: {hashedPassword}");
Console.WriteLine();
Console.WriteLine("SQL Sorgusu:");
Console.WriteLine($"INSERT INTO YAZARLAR (AD, SOYAD, EPOSTA, SIFRE, TELEFON, BIYOGRAFI, AKTIF_MI, RESIM)");
Console.WriteLine($"VALUES ('Test', 'Kullanıcı', 'test@habersitesi.com', '{hashedPassword}', '5551234567', 'Test kullanıcısı', 1, 'default.jpg');");
