# 🧺 Carpet Cleaning System

A professional management system for carpet cleaning businesses, featuring a robust .NET backend and a modern React frontend with Role-Based Access Control (RBAC).

## 📂 Project Structure

This repository is organized into two main sections:

### 🖥️ [Backend](file:///Backend)
Built with **.NET 8** and a clean **CQRS** architecture.
- **CarpetCleaningSystem.API**: RESTful API endpoints and configuration.
- **CarpetCleaningSystem.Application**: Business logic, CQRS Handlers, and Abstractions.
- **CarpetCleaningSystem.Domain**: Core entities, Enums, and Domain logic.
- **CarpetCleaningSystem.Infrastructure**: Persistence (EF Core), Repository implementations, and external services.

### 🌐 [Frontend](file:///Frontend)
Built with **React**, **Vite**, and **Vanilla CSS**.
- **Admin Dashboard**: Real-time order monitoring for Admins.
- **Order Management**: Detailed order processing flow for Employees.
- **Customer Lookup**: Quick search functionality by phone number.

## 🚀 Getting Started

1. **Backend**:
   - Navigate to `Backend/`
   - Run `dotnet restore`
   - Run `dotnet run --project CarpetCleaningSystem.API`

2. **Frontend**:
   - Navigate to `Frontend/`
   - Run `npm install`
   - Run `npm run dev`

## ✨ Key Features
- **RBAC**: Secure authentication for Admins and Employees.
- **Order Flow**: Track orders from Draft to Completion.
- **Price Calculation**: Automated pricing based on carpet material and dimensions.
- **UI/UX**: Responsive design with vibrant role indicators and intuitive dashboard.

---
*Maintained with all commit history preserved.*
