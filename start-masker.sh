#!/bin/bash

# ============================================
# MASKER - Hizli Baslatma Script'i
# ============================================
# Bu script tum servisleri tek komutla baslatir
# Kullanim: ./start-masker.sh
# ============================================

set -e

# Renkli cikti
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Proje dizini
PROJECT_DIR="/home/ubuntu_user/projects/MASKER"

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   MASKER - Servis Baslatma Script'i${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# .NET PATH ayari
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"

# 1. Docker kontrolu
echo -e "${YELLOW}[1/5] Docker kontrol ediliyor...${NC}"
if ! command -v docker &> /dev/null; then
    echo -e "${RED}HATA: Docker bulunamadi!${NC}"
    echo -e "${YELLOW}Docker Desktop'i acin ve WSL Integration'i aktif edin:${NC}"
    echo "  1. Docker Desktop -> Settings -> Resources -> WSL Integration"
    echo "  2. Ubuntu icin toggle'i acin"
    echo "  3. Apply & Restart"
    exit 1
fi

# 2. Docker servislerini baslat
echo -e "${YELLOW}[2/5] Docker servisleri baslatiliyor (PostgreSQL + pgAdmin)...${NC}"
cd "$PROJECT_DIR"
docker-compose up -d

# Docker'in hazir olmasini bekle
sleep 3

# 3. Eski processleri temizle
echo -e "${YELLOW}[3/5] Eski processler temizleniyor...${NC}"
pkill -f "dotnet.*ApiUI" 2>/dev/null || true
pkill -f "dotnet.*AdminUI" 2>/dev/null || true
sleep 2

# 4. ApiUI baslat
echo -e "${YELLOW}[4/5] ApiUI baslatiliyor (Port: 5100)...${NC}"
cd "$PROJECT_DIR/ApiUI"
nohup dotnet run --launch-profile http > /tmp/apiui.log 2>&1 &
API_PID=$!
echo "  PID: $API_PID"

# 5. AdminUI baslat
echo -e "${YELLOW}[5/5] AdminUI baslatiliyor (Port: 5251)...${NC}"
cd "$PROJECT_DIR/AdminUI"
nohup dotnet run --launch-profile http > /tmp/adminui.log 2>&1 &
ADMIN_PID=$!
echo "  PID: $ADMIN_PID"

# Servislerin hazir olmasini bekle
echo ""
echo -e "${YELLOW}Servisler baslatiliyor, lutfen bekleyin...${NC}"
sleep 15

# Durum kontrolu
echo ""
echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Servis Durumu${NC}"
echo -e "${BLUE}============================================${NC}"

check_service() {
    if nc -zv localhost $1 2>&1 | grep -q "succeeded"; then
        echo -e "  $2: ${GREEN}CALISIYOR${NC}"
        return 0
    else
        echo -e "  $2: ${RED}BASARISIZ${NC}"
        return 1
    fi
}

check_service 5432 "PostgreSQL (5432)"
check_service 5050 "pgAdmin    (5050)"
check_service 5100 "ApiUI      (5100)"
check_service 5251 "AdminUI    (5251)"

echo ""
echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Erisim Adresleri${NC}"
echo -e "${BLUE}============================================${NC}"
echo -e "  Admin Panel : ${GREEN}http://localhost:5251${NC}"
echo -e "  API Swagger : ${GREEN}http://localhost:5100/swagger${NC}"
echo -e "  pgAdmin     : ${GREEN}http://localhost:5050${NC}"
echo ""
echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   Giris Bilgileri${NC}"
echo -e "${BLUE}============================================${NC}"
echo -e "  ${YELLOW}AdminUI:${NC}"
echo -e "    E-posta : admin@masker.com"
echo -e "    Sifre   : Admin123"
echo ""
echo -e "  ${YELLOW}pgAdmin:${NC}"
echo -e "    E-posta : admin@masker.com"
echo -e "    Sifre   : MaskerAdmin2026!"
echo ""
echo -e "${BLUE}============================================${NC}"
echo -e "${GREEN}Tum servisler basariyla baslatildi!${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""
echo -e "${YELLOW}Log dosyalari:${NC}"
echo "  ApiUI   : tail -f /tmp/apiui.log"
echo "  AdminUI : tail -f /tmp/adminui.log"
echo ""
echo -e "${YELLOW}Servisleri durdurmak icin:${NC}"
echo "  ./stop-masker.sh"
