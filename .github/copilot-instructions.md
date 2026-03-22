# Project Guidelines

## Code Style
- Keep existing naming conventions used by the domain and database mapping: Vietnamese business terms in entities/fields (for example, MaNguoiDung, HocSinh, GiaoVien).
- Do not rename role literals unless all backend and frontend usages are updated together. Current role values used in routing/authorization include Admin, Giao_Vien, Hoc_Sinh, and Phu_Huynh.
- In frontend React code, keep component/file naming in PascalCase and route paths in lowercase with role-prefixed segments (for example, /admin/users, /teacher/schedule).
- Prefer minimal, focused edits. Avoid broad refactors in generated EF model files unless explicitly requested.

## Architecture
- Monorepo-style structure under QuanLyTrungTam:
  - Backend: ASP.NET Core Web API in QuanLyTrungTam/backend.
  - Frontend: React + Vite app in QuanLyTrungTam/frontend.
- Backend layers exist as Controllers, Services, Repositories, and Data/AppDbContext. Follow this separation for new features, even if some existing endpoints are still controller-centric.
- Frontend uses AuthContext + PrivateRoute + MainLayout with role-based route groups in src/App.jsx.

## Build And Run
- Frontend:
  - cd QuanLyTrungTam/frontend
  - npm install
  - npm run dev
  - npm run build
- Backend:
  - cd QuanLyTrungTam/backend
  - dotnet restore
  - dotnet build
  - dotnet run
- Full solution can also be run from QuanLyTrungTam.sln in Visual Studio 2022.
- No formal automated test scripts are currently configured in frontend package.json, and no established backend test project is present.

## Environment And Data
- Backend is configured for MySQL via DefaultConnection in QuanLyTrungTam/backend/appsettings.json.
- SQL schema files are present at repository root (Qlttta.sql and HeThongHocOnline_Final.sql). Keep schema-related assumptions aligned with these files and AppDbContext.
- CORS currently allows frontend origin http://localhost:3000 in backend Program.cs. If changing frontend port/origin, update CORS accordingly.

## Conventions And Pitfalls
- Keep API base URL/port usage consistent across frontend services and pages. Check both src/services/api.js and direct fetch calls in pages before changing endpoints.
- When editing routing, verify redirects and target paths remain aligned (for example, HomeRedirect in App.jsx and role-specific login redirects).
- Preserve middleware order in backend Program.cs unless there is a clear reason to change it.
- Do not commit build output or dependency directories (bin, obj, node_modules, dist, .vs).

## Key Files
- QuanLyTrungTam/backend/Program.cs
- QuanLyTrungTam/backend/Data/AppDbContext.cs
- QuanLyTrungTam/backend/appsettings.json
- QuanLyTrungTam/frontend/src/App.jsx
- QuanLyTrungTam/frontend/src/context/AuthContext.jsx
- QuanLyTrungTam/frontend/src/routes/PrivateRoute.jsx
- QuanLyTrungTam/frontend/src/services/api.js
- QuanLyTrungTam/README.md
