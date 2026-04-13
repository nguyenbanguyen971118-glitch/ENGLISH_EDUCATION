# QuanLyTrungTam

Hướng dẫn để pull về và chạy ổn định:

## 1) Yêu cầu môi trường

- Backend: .NET 8 SDK + .NET 8 Runtime (x64)
- Frontend: Node.js LTS 20.x (kèm npm)
- Database: MySQL (theo chuỗi kết nối trong `backend/appsettings.json`)

## 2) Chạy Frontend trên VS Code

1. Tạo file `QuanLyTrungTam/frontend/.env` từ `QuanLyTrungTam/frontend/.env.example` nếu chưa có.
2. Sửa `VITE_API_BASE_URL` cho đúng backend đang chạy, ví dụ `http://localhost:5000`.

```bash
cd QuanLyTrungTam/frontend
npm install
npm run dev
```

Frontend mặc định chạy ở cổng `3000` (theo `vite.config.js`).
REST API và SignalR sẽ tự lấy theo `VITE_API_BASE_URL`, nên chỉ cần đổi env khi chuyển sang môi trường khác.

## 4) Triển khai / Production

- Tạo `QuanLyTrungTam/frontend/.env.production` hoặc set biến môi trường trên host deploy với `VITE_API_BASE_URL` trỏ đúng backend production.
- Chạy `npm run build` sau mỗi lần đổi `VITE_API_BASE_URL`; Vite sẽ bake giá trị env vào bundle trong `dist`.
- Không commit `dist`; đây là output build theo từng môi trường và phải được tạo lại khi backend host/cổng thay đổi.

## 5) Chạy Backend trên Visual Studio

1. Mở file solution `QuanLyTrungTam.sln` bằng Visual Studio 2022.
2. Chọn Startup Project là `backend`.
3. Chạy bằng `F5` hoặc `Ctrl + F5`.

Backend đọc profile chạy trong `backend/Properties/launchSettings.json`.

## 6) Lưu ý sau khi pull

- Nếu lỗi thiếu .NET runtime 8.0: cài thêm .NET 8 Runtime/SDK.
- Nếu lỗi npm/node không nhận lệnh: cài Node.js LTS và mở lại terminal.
- Không commit các thư mục build/cache (`bin`, `obj`, `node_modules`, `dist`, `.vs`) vì đã được ignore.
