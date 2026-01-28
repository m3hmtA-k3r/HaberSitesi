#!/bin/bash

# ============================================
# MASKER - Servisleri Durdurma Script'i
# ============================================

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

PROJECT_DIR="/home/ubuntu_user/projects/MASKER"

echo -e "${BLUE}============================================${NC}"
echo -e "${BLUE}   MASKER - Servisleri Durdurma${NC}"
echo -e "${BLUE}============================================${NC}"
echo ""

# .NET uygulamalarini durdur
echo -e "${YELLOW}[1/2] .NET uygulamalari durduruluyor...${NC}"
pkill -f "dotnet.*ApiUI" 2>/dev/null && echo -e "  ApiUI: ${GREEN}Durduruldu${NC}" || echo -e "  ApiUI: ${YELLOW}Zaten calismiyordu${NC}"
pkill -f "dotnet.*AdminUI" 2>/dev/null && echo -e "  AdminUI: ${GREEN}Durduruldu${NC}" || echo -e "  AdminUI: ${YELLOW}Zaten calismiyordu${NC}"
pkill -f "dotnet.*WebUI" 2>/dev/null && echo -e "  WebUI: ${GREEN}Durduruldu${NC}" || echo -e "  WebUI: ${YELLOW}Zaten calismiyordu${NC}"

# Docker servislerini durdur (opsiyonel)
echo ""
read -p "Docker servislerini de durdurmak istiyor musunuz? (e/h): " choice
if [[ "$choice" == "e" || "$choice" == "E" ]]; then
    echo -e "${YELLOW}[2/2] Docker servisleri durduruluyor...${NC}"
    cd "$PROJECT_DIR"
    docker-compose down
    echo -e "  PostgreSQL: ${GREEN}Durduruldu${NC}"
    echo -e "  pgAdmin: ${GREEN}Durduruldu${NC}"
else
    echo -e "${YELLOW}[2/2] Docker servisleri calismaya devam ediyor.${NC}"
fi

echo ""
echo -e "${GREEN}Islem tamamlandi!${NC}"
