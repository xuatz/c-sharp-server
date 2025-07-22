#!/bin/bash

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}1. Starting the authentication test...${NC}"
echo ""

# Step 1: Register a new user
echo -e "${GREEN}Step 1: Registering a new user${NC}"
REGISTER_RESPONSE=$(curl -s -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "demo@example.com",
    "username": "demouser",
    "password": "Demo123!"
  }')

echo "Register Response: $REGISTER_RESPONSE"
echo ""

# Extract token from register response
TOKEN=$(echo $REGISTER_RESPONSE | grep -o '"token":"[^"]*' | grep -o '[^"]*$')

if [ -z "$TOKEN" ]; then
  echo -e "${GREEN}Step 2: User already exists, logging in...${NC}"
  # Step 2: Login if registration failed (user already exists)
  LOGIN_RESPONSE=$(curl -s -X POST http://localhost:5000/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{
      "usernameOrEmail": "demouser",
      "password": "Demo123!"
    }')
  
  echo "Login Response: $LOGIN_RESPONSE"
  echo ""
  
  # Extract token from login response
  TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*' | grep -o '[^"]*$')
fi

if [ -z "$TOKEN" ]; then
  echo "Error: Could not obtain token"
  exit 1
fi

echo -e "${BLUE}JWT Token obtained:${NC}"
echo "$TOKEN"
echo ""

# Step 3: Use the token to access protected endpoint
echo -e "${GREEN}Step 3: Accessing protected /api/auth/me endpoint${NC}"
ME_RESPONSE=$(curl -s http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer $TOKEN")

echo "Me Response: $ME_RESPONSE"
echo ""

# Pretty print the response if jq is available
if command -v jq &> /dev/null; then
  echo -e "${BLUE}Formatted Response:${NC}"
  echo $ME_RESPONSE | jq '.'
fi