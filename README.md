# Smart Travel System

Hệ thống quản lý du lịch với WPF và Entity Framework Core

## Công nghệ
- .NET 6.0
- WPF (Windows Presentation Foundation)
- Entity Framework Core 7.0
- SQL Server

## Cấu trúc Database

```sql
DBTravelCenter
├── Trip (TripID, Code, Destination, Price, Status)
├── Customer (CustomerID, Code, FullName, Email, Age, Password)
└── Booking (BookingID, TripID, CustomerID, BookingDate, Status)
```

## Chức năng
1. ✅ Customer Login
2. ✅ Trip Management (List, Add, Edit)
3. ✅ Booking Management (View, Create, Update Status, Filter)
4. ✅ Logout

## Setup

### 1. Tạo Database
Chạy script SQL trong `CreateDatabase.sql`

### 2. Cài đặt
```bash
dotnet restore
dotnet build
```

### 3. Cấu hình Connection String
Sửa `TravelDataAccess/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "TravelDb": "Server=localhost;Database=DBTravelCenter;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 4. Chạy ứng dụng
```bash
dotnet run --project TravelManagementApp
```

## Test Account
- Code: `CUS-001`
- Password: `123456`

## Screenshots
(Thêm screenshots nếu có)

## License
MIT
