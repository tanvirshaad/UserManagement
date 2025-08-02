# User Management System

A professional ASP.NET Core web application for user management with authentication, authorization, and user administration features.

## Features

### Authentication & Authorization
- **User Registration**: Users can create accounts with name, email, and password
- **User Login**: Secure authentication with password hashing using BCrypt
- **Session Management**: Secure session-based authentication
- **Access Control**: Non-authenticated users can only access login/registration pages
- **Account Status**: Users can be blocked or deleted with appropriate access restrictions

### User Management Dashboard
- **User List**: Displays all users with their information
- **Multiple Selection**: Checkbox-based selection with select all/deselect all functionality
- **Toolbar Actions**: 
  - Block users (button with text)
  - Unblock users (icon button)
  - Delete users (icon button)
- **No Row-Level Buttons**: Actions are performed through the toolbar, not individual row buttons
- **Sorting**: Sortable columns for Name, Email, and Last Login
- **Filtering**: Real-time search/filter functionality
- **Responsive Design**: Works on desktop and mobile devices

### Database Features
- **Unique Email Index**: Database-level unique constraint on email for non-deleted users
- **Soft Delete**: Users are marked as deleted rather than physically removed
- **Status Tracking**: Active/Blocked status management
- **Last Login Tracking**: Automatic tracking of user login times

### UI/UX Features
- **Professional Design**: Business-oriented, clean interface using Bootstrap
- **Tooltips**: Helpful tooltips for better user experience
- **Status Messages**: Success/error notifications for user actions
- **Responsive Layout**: Adapts to different screen sizes
- **No Animations**: Clean, professional appearance without distracting animations
- **No Wallpapers**: Clean background design

## Technical Requirements Met

✅ **Unique Database Index**: Created unique index on email for non-deleted users  
✅ **Professional Table Design**: Clean table layout with proper alignment  
✅ **Data Sorting**: Sortable columns with visual indicators  
✅ **Multiple Selection**: Checkbox-based selection with select all functionality  
✅ **Authentication Checks**: Server validates user status before each request  
✅ **Bootstrap Framework**: Professional styling using Bootstrap  
✅ **No Row Buttons**: Actions performed through toolbar only  
✅ **Email Uniqueness**: Database-level constraint ensures email uniqueness  
✅ **Password Flexibility**: Accepts any non-empty password (minimum 1 character)  
✅ **No Email Confirmation**: Direct registration without email verification  
✅ **Blocked User Handling**: Blocked users cannot login, deleted users can re-register  
✅ **Cross-Browser Compatibility**: Works on different browsers and resolutions  

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Bootstrap 5, JavaScript, HTML5, CSS3
- **Authentication**: Session-based with BCrypt password hashing
- **Icons**: Bootstrap Icons
- **Styling**: Custom CSS with Bootstrap framework

## Database Schema

### Users Table
- `Id` (Primary Key)
- `Name` (nvarchar(100))
- `Email` (nvarchar(255)) - Unique index for non-deleted users
- `Password` (nvarchar(255)) - BCrypt hashed
- `Status` (nvarchar(20)) - "Active" or "Blocked"
- `LastLogin` (datetime2, nullable)
- `RegistrationTime` (datetime2)
- `IsDeleted` (bit) - Soft delete flag

### Unique Index
- **Name**: `IX_Users_Email_Unique`
- **Columns**: `Email`
- **Filter**: `[IsDeleted] = 0`
- **Purpose**: Ensures email uniqueness for active users only

## Installation & Setup

1. **Prerequisites**
   - .NET 8.0 SDK
   - SQL Server (LocalDB or Express)
   - Visual Studio 2022 or VS Code

2. **Database Setup**
   ```bash
   # Install Entity Framework tools (if not already installed)
   dotnet tool install --global dotnet-ef
   
   # Update database
   dotnet ef database update
   ```

3. **Run Application**
   ```bash
   dotnet run
   ```

4. **Access Application**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`
   - Register a new account or login with existing credentials

## Usage

### For Administrators
1. **Login** to the system with your credentials
2. **View Users** in the dashboard table
3. **Select Users** using checkboxes (individual or select all)
4. **Perform Actions** using the toolbar:
   - Block selected users
   - Unblock selected users  
   - Delete selected users
5. **Filter/Sort** data using the search box and column headers

### For New Users
1. **Register** a new account with your details
2. **Login** with your email and password
3. **Access** the user management dashboard

## Security Features

- **Password Hashing**: BCrypt algorithm for secure password storage
- **Session Management**: Secure session-based authentication
- **Input Validation**: Server-side validation for all inputs
- **SQL Injection Protection**: Entity Framework parameterized queries
- **XSS Protection**: ASP.NET Core built-in protection
- **CSRF Protection**: Anti-forgery token validation

## File Structure

```
UserManagement/
├── Controllers/
│   ├── AccountController.cs    # Authentication controller
│   └── HomeController.cs       # User management controller
├── Data/
│   └── ApplicationDbContext.cs # Database context
├── DTOs/
│   ├── LoginDTO.cs            # Login data transfer object
│   ├── RegisterDTO.cs         # Registration data transfer object
│   └── UserDTO.cs             # User data transfer object
├── Interfaces/
│   ├── IAuthService.cs        # Authentication service interface
│   └── IUserService.cs        # User service interface
├── Middleware/
│   └── AuthenticationMiddleware.cs # Authentication middleware
├── Models/
│   └── Entities/
│       └── User.cs            # User entity model
├── Services/
│   ├── AuthService.cs         # Authentication service
│   └── UserService.cs         # User management service
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml       # Login page
│   │   └── Register.cshtml    # Registration page
│   ├── Home/
│   │   └── Index.cshtml       # User management dashboard
│   └── Shared/
│       └── _Layout.cshtml     # Main layout
└── wwwroot/
    ├── css/
    │   └── site.css           # Custom styles
    └── js/
        └── dashboard.js       # Dashboard functionality
```

## API Endpoints

### Authentication
- `GET /Account/Login` - Login page
- `POST /Account/Login` - Login action
- `GET /Account/Register` - Registration page
- `POST /Account/Register` - Registration action
- `POST /Account/Logout` - Logout action

### User Management
- `GET /Home/Index` - User dashboard
- `POST /Home/BlockUsers` - Block selected users
- `POST /Home/UnblockUsers` - Unblock selected users
- `POST /Home/DeleteUsers` - Delete selected users

## Browser Compatibility

- Chrome (recommended)
- Firefox
- Safari
- Edge
- Mobile browsers (responsive design)

## Deployment

The application can be deployed to:
- Azure App Service
- IIS
- Docker containers
- Any hosting platform supporting .NET Core

## License

This project is created for educational and demonstration purposes. 