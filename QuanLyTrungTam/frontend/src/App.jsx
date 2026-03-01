import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext'; 
import PrivateRoute from './routes/PrivateRoute';
import MainLayout from './components/MainLayout'; 

// Import Pages
import Login from './pages/Login';
import AdminDashboard from './pages/admin/AdminDashboard';
import TeacherDashboard from './pages/teacher/TeacherDashboard';
import TeacherSchedule from './pages/teacher/TeacherSchedule'; 
import StudentDashboard from './pages/student/Dashboard';
import ParentDashboard from './pages/parent/ParentDashboard'; 
import ParentSchedule from './pages/parent/ParentSchedule';
import SchedulePage from './pages/student/SchedulePage'; 

// Component Placeholder để tránh lỗi khi chưa có file trang cụ thể
const Placeholder = ({ title }) => (
  <div className="p-4 animate__animated animate__fadeIn">
    <div className="card border-0 shadow-sm rounded-5 p-5 text-center">
      <i className="bi bi-cone-striped fs-1 text-warning mb-3"></i>
      <h3 className="fw-bold">{title}</h3>
      <p className="text-muted">Tính năng này đang được phát triển và sẽ sớm ra mắt.</p>
    </div>
  </div>
);

const HomeRedirect = () => {
  const { user } = useAuth();
  if (!user) return <Navigate to="/login" replace />;

  switch (user.role) {
    case 'Admin': return <Navigate to="/admin" replace />;
    case 'Giao_Vien': return <Navigate to="/teacher" replace />;
    case 'Hoc_Sinh': return <Navigate to="/student" replace />;
    case 'Phu_Huynh': return <Navigate to="/parent" replace />;
    default: return <Navigate to="/login" replace />;
  }
};

function App() {
  return (
    <AuthProvider>
      <Router future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<HomeRedirect />} />
          
          <Route element={<MainLayout />}>
            
            {/* --- NHÓM QUẢN TRỊ (ADMIN) - BỔ SUNG ĐẦY ĐỦ --- */}
            <Route path="/admin" element={<PrivateRoute allowedRoles={['Admin']}><AdminDashboard /></PrivateRoute>} />
            <Route path="/admin/users" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý người dùng" /></PrivateRoute>} />
            <Route path="/admin/courses" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý khóa học" /></PrivateRoute>} />
            <Route path="/admin/classes" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý lớp học" /></PrivateRoute>} />
            <Route path="/admin/schedules" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý lịch dạy & học" /></PrivateRoute>} />
            <Route path="/admin/content" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý nội dung học" /></PrivateRoute>} />
            <Route path="/admin/finances" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Quản lý học phí & tài chính" /></PrivateRoute>} />
            <Route path="/admin/reports" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Báo cáo & Thống kê" /></PrivateRoute>} />
            <Route path="/admin/notifications" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Thông báo & Liên lạc" /></PrivateRoute>} />
            <Route path="/admin/profile" element={<PrivateRoute allowedRoles={['Admin']}><Placeholder title="Hồ sơ cá nhân" /></PrivateRoute>} />

            {/* --- NHÓM GIÁO VIÊN (TEACHER) --- */}
            <Route path="/teacher" element={<PrivateRoute allowedRoles={['Giao_Vien']}><TeacherDashboard /></PrivateRoute>} />
            <Route path="/teacher/schedule" element={<PrivateRoute allowedRoles={['Giao_Vien']}><TeacherSchedule /></PrivateRoute>} />
            <Route path="/teacher/classes" element={<PrivateRoute allowedRoles={['Giao_Vien']}><Placeholder title="Quản lý lớp học" /></PrivateRoute>} />
            <Route path="/teacher/homework" element={<PrivateRoute allowedRoles={['Giao_Vien']}><Placeholder title="Bài tập & Chấm điểm" /></PrivateRoute>} />
            <Route path="/teacher/materials" element={<PrivateRoute allowedRoles={['Giao_Vien']}><Placeholder title="Kho học liệu" /></PrivateRoute>} />
            <Route path="/teacher/communication" element={<PrivateRoute allowedRoles={['Giao_Vien']}><Placeholder title="Trao đổi phụ huynh" /></PrivateRoute>} />
            <Route path="/teacher/profile" element={<PrivateRoute allowedRoles={['Giao_Vien']}><Placeholder title="Hồ sơ giáo viên" /></PrivateRoute>} />

            {/* --- NHÓM HỌC SINH (STUDENT) --- */}
            <Route path="/student" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><StudentDashboard /></PrivateRoute>} />
            <Route path="/student/schedule" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><SchedulePage /></PrivateRoute>} />
            <Route path="/student/results" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><Placeholder title="Kết quả học tập" /></PrivateRoute>} />
            
            {/* Route mới dành cho danh sách bài tập về nhà được giao */}
            <Route path="/student/homework-list" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><Placeholder title="Danh sách Bài tập về nhà" /></PrivateRoute>} />
            
            {/* Route dành cho phần tự học / luyện tập online */}
            <Route path="/student/homework" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><Placeholder title="Hệ thống Học chủ động" /></PrivateRoute>} />
            
            <Route path="/student/profile" element={<PrivateRoute allowedRoles={['Hoc_Sinh']}><Placeholder title="Hồ sơ cá nhân" /></PrivateRoute>} />

            {/* --- NHÓM PHỤ HUYNH (PARENT) --- */}
            <Route path="/parent" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><ParentDashboard /></PrivateRoute>} />
            <Route path="/parent/schedule" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><ParentSchedule /></PrivateRoute>} />
            <Route path="/parent/progress" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><Placeholder title="Tiến độ học tập" /></PrivateRoute>} />
            <Route path="/parent/results" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><Placeholder title="Kết quả học tập của con" /></PrivateRoute>} />
            <Route path="/parent/feedback" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><Placeholder title="Phản hồi giáo viên" /></PrivateRoute>} />
            <Route path="/parent/profile" element={<PrivateRoute allowedRoles={['Phu_Huynh']}><Placeholder title="Hồ sơ phụ huynh" /></PrivateRoute>} />
          </Route>

          {/* Error Routes */}
          <Route path="/unauthorized" element={<div className="text-center mt-5"><i className="bi bi-shield-lock-fill fs-1 text-danger"></i><h1 className="mt-3">403 - Không có quyền truy cập</h1></div>} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;