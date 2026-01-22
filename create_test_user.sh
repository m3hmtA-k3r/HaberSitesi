#!/bin/bash

# Test kullanıcısı oluşturma scripti
# BCrypt hash: password123 -> $2a$11$... (BCrypt.Net ile oluşturulacak)

echo "Test kullanıcısı oluşturuluyor..."

# API üzerinden test kullanıcısı oluştur
curl -X POST http://localhost:5100/api/yazar/InsertYazar \
  -H "Content-Type: application/json" \
  -d '{
    "ad": "Test",
    "soyad": "Yazar",
    "eposta": "test@habersitesi.com",
    "sifre": "password123",
    "telefon": "5551234567",
    "biyografi": "Test kullanıcısı",
    "aktifMi": true,
    "resim": ""
  }'

echo ""
echo ""
echo "Test kullanıcı bilgileri:"
echo "Email: test@habersitesi.com"
echo "Şifre: password123"
