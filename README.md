# 🏡 Society Management System  

🚀 **A full-stack society management platform** built using **ASP.NET Core Web API** for the backend and **.NET** for the frontend.  
This system helps manage society visitor records, maintenance schedules, and upcoming events while providing a responsive user interface with secure authentication.

![Image](https://github.com/user-attachments/assets/dba9ad0f-dd5e-4610-b98f-f5482c366d42)
![Image](https://github.com/user-attachments/assets/9999dca2-bd67-4524-88fa-7079d272071d)

## 🌟 Features  

### 👤 **User Functionalities**  
✅ Register/Login (JWT Authentication)  
🔑 Secure user authentication using JWT  
📜 View and manage **visitor history**  
🛠️ **Maintenance tracking** for society issues  
📅 **Event Management** – View upcoming events  

### 🔐 **Admin Panel Functionalities**  
🏠 Manage **Society Members & Visitors**  
📅 **Schedule & Update Events**  
🛠️ **Maintenance Requests & Status Updates**  
📊 Dashboard for **real-time society updates**  

## 🛠️ **Tech Stack**  
🎨 **Frontend**: .NET (Blazor/MVC)  
🖥️ **Backend**: ASP.NET Core Web API  
🗄️ **Database**: SQL Server  
🔐 **Authentication**: JWT Token  

## 🚀 **How to Run the Project?**  

### Backend (ASP.NET Core Web API) 🏗️  
```sh
cd Backend/SocietyManagementAPI
dotnet restore
dotnet run
👉 API runs on: http://localhost:5000/api/

Frontend (.NET) 💻
sh
Copy code
cd Frontend/SocietyManagementUI
dotnet run
👉 App runs on: http://localhost:3000/

🔗 API Endpoints
Method	Endpoint	Description
GET	/api/Visitor/GetAll	Fetch all visitor records
POST	/api/User/Register	Register a new user
POST	/api/User/Login	User login (JWT)
GET	/api/Event/GetUpcoming	Get upcoming events
POST	/api/Maintenance/Request	Submit a maintenance request
📩 Contact Me
📧 Email: abhayviradiya6236@gmail.com
🔗 GitHub Profile: Abhay Viradiya
