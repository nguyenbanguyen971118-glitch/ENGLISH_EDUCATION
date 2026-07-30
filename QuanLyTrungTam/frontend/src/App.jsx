import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { StudentClassProvider } from './context/StudentClassContext';
import { TestProvider } from './context/TestContext';
import PrivateRoute from './routes/PrivateRoute';
import MainLayout from './components/MainLayout';
import { Toaster } from 'react-hot-toast';
import Unauthorized from './components/Unauthorized';

// Import Pages
import Login from './pages/Login';
import AdminDashboard from './pages/admin/AdminDashboard';
import TeacherDashboard from './pages/teacher/TeacherDashboard';
import TeacherSchedule from './pages/teacher/TeacherSchedule';
import StudentDashboard from './pages/student/Dashboard';
import ParentDashboard from './pages/parent/ParentDashboard';
import ParentSchedule from './pages/parent/ParentSchedule';
import SchedulePage from './pages/student/SchedulePage';

// Import Admin Pages
import AdminCourses from './pages/admin/AdminCourses';
import AdminClasses from './pages/admin/AdminClasses';
import AdminCreateClass from "./pages/admin/AdminCreateClass";
import AdminCreateCourse from "./pages/admin/AdminCreateCourse";
import AdminCreateRoom from "./pages/admin/AdminCreateRoom";
import AdminEditRoom from "./pages/admin/AdminEditRoom";
import AdminAssignStudent from "./pages/admin/AdminAssignStudent";
import AdminMessages from './pages/admin/AdminMessages'; // Đã import component nhắn tin
import AdminStudyContent from './pages/admin/AdminStudyContent'; // Đã import trang nội dung học tập 
import AdminAttendance from './pages/admin/AdminAttendance'; // Đã import trang điểm danh
import AdminUsers from "./pages/admin/AdminUsers";
import AdminCreateUser from "./pages/admin/AdminCreateUser";
import AdminNotifcations from "./pages/admin/AdminNotifications";
import AdminAssignment from "./pages/admin/AdminAssignment";
import AdminFunctions from './pages/admin/AdminFunctions';
import AdminSchedule from './pages/admin/AdminSchedule';
import AdminReports from './pages/admin/AdminReports';

// Import Teacher Pages
import TeacherAttendance from './pages/teacher/TeacherAttendance';
import TeacherClasses from './pages/teacher/TeacherClasses';
import TeacherProfile from './pages/teacher/TeacherProfile';
import TeacherMessages from './pages/teacher/TeacherMessages';
import TeacherExams from './pages/teacher/TeacherExams';
import TeacherStudyContent from './pages/teacher/TeacherStudyContent'; // Đã import trang nội dung  học tập của giáo viên
import TeacherReports from './pages/teacher/TeacherReports';
import ExamGenerationManager from './pages/teacher/ExamGenerationManager';
// Import Parent Pages
import ParentAttendance from './pages/parent/ParentAttendance';
import ParentMessages from './pages/parent/ParentMessages';
import ParentReports from './pages/parent/ParentReports';

import ParentProfile from './pages/parent/ParentProfile';
// Import Student Pages
import StudentProfile from './pages/student/StudentProfile';
import StudentStudyContent from './pages/student/StudentStudyContent';
import StudentAssignmentPage from './pages/student/StudentAssignmentPage';
import StudentReports from './pages/student/StudentReports';
import StudentMessages from './pages/student/StudentMessages'; // Đã import trang nhắn tin của học sinh
// Component Placeholder để tránh lỗi khi chưa có file trang cụ thể
const Placeholder = ({ title }) => (
  <div className="p-4 animate__animated animate__fadeIn">
    <div className="card border-0 shadow-sm rounded-5 p-5 text-center">
      <i className="bi bi-cone-striped fs-1 text-warning mb-3"></i>
      <h3 className="fw-bold text-uppercase">{title}</h3>
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
      <StudentClassProvider>
        <TestProvider>
          <Toaster position="top-right" />
          <Router future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
            <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<HomeRedirect />} />

          <Route element={<MainLayout />}>

          {/* --- NHÓM QUẢN TRỊ (ADMIN) --- */}
            <Route path="/admin" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_USERS_VIEW']}><AdminDashboard /></PrivateRoute>} />
            <Route path="/admin/functions" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_PERMISSIONS_VIEW']}><AdminFunctions /></PrivateRoute>} />
            <Route path="/admin/users" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_USERS_VIEW']}><AdminUsers /></PrivateRoute>} />
            <Route path="/admin/courses" element={<PrivateRoute requiredPermissions={['COURSES_CREATE']}><AdminCourses /></PrivateRoute>} />
            <Route path="/admin/courses/create" element={<PrivateRoute requiredPermissions={['COURSES_CREATE']}><AdminCreateCourse /></PrivateRoute>} />

            <Route path="/admin/classes" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_CLASSES_VIEW']}><AdminClasses /></PrivateRoute>} />
            <Route path="/admin/classes/create" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_CLASSES_VIEW']}><AdminCreateClass /></PrivateRoute>} />
            <Route path="/admin/rooms/create" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_CLASSES_VIEW']}><AdminCreateRoom /></PrivateRoute>} />
            <Route path="/admin/rooms/:id/edit" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_CLASSES_VIEW']}><AdminEditRoom /></PrivateRoute>} />
            <Route path="/admin/classes/assign-students" element={<PrivateRoute requiredPermissions={['CLASSES_STUDENTS_VIEW']}><AdminAssignStudent /></PrivateRoute>} />

            <Route path="/admin/attendance" element={<PrivateRoute requiredPermissions={['ATTENDANCE_ADMIN_VIEW']}><AdminAttendance /></PrivateRoute>} />
            <Route path="/admin/exams" element={<PrivateRoute requiredPermissions={['ASSIGNMENTS_ADMIN_VIEW']}><AdminAssignment /></PrivateRoute>} />
            <Route path="/admin/exams/generate" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_ADMIN']}><ExamGenerationManager /></PrivateRoute>} />
            <Route path="/admin/reports" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_REPORTS_VIEW']}><AdminReports /></PrivateRoute>} />
            <Route path="/admin/notifications" element={<PrivateRoute requiredPermissions={['NOTIFICATIONS_VIEW_ALL']}><AdminNotifcations /></PrivateRoute>} />
            <Route path="/admin/content" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_ADMIN']}><AdminStudyContent /></PrivateRoute>} />
            {/* ĐÃ TÍCH HỢP TRANG TIN NHẮN TẠI ĐÂY */}
            <Route path="/admin/messages" element={<PrivateRoute requiredPermissions={['MESSAGES_ACCESS']}><AdminMessages /></PrivateRoute>} />

            <Route path="/admin/schedules" element={<PrivateRoute requiredPermissions={['PAGE_ADMIN_CLASSES_VIEW']}><AdminSchedule /></PrivateRoute>} />

            <Route path="/admin/profile" element={<PrivateRoute><Placeholder title="Hồ sơ cá nhân" /></PrivateRoute>} />
            {/* nmkhue: Route để tạo người dùng mới 
              thay vì sử dụng AdminUsers thì dùng User_Create 
              để phân tách chức năng */}
            <Route
              path="/admin/users/create"
              element={
                <PrivateRoute requiredPermissions={['USERS_CREATE']}>
                  <AdminCreateUser />
                </PrivateRoute>
              }
            />


            {/* --- NHÓM GIÁO VIÊN (TEACHER) --- */}
            <Route path="/teacher" element={<PrivateRoute requiredPermissions={['PAGE_TEACHER_SCHEDULE_VIEW']}><TeacherDashboard /></PrivateRoute>} />
            <Route path="/teacher/schedule" element={<PrivateRoute requiredPermissions={['PAGE_TEACHER_SCHEDULE_VIEW']}><TeacherSchedule /></PrivateRoute>} />
            <Route path="/teacher/classes" element={<PrivateRoute requiredPermissions={['CLASSES_STUDENTS_VIEW']}><TeacherClasses /></PrivateRoute>} />
            <Route path="/teacher/exams" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_TEACHER']}><TeacherExams /></PrivateRoute>} />
            <Route path="/teacher/exams/generate" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_TEACHER']}><ExamGenerationManager /></PrivateRoute>} />
            <Route path="/teacher/grading" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_TEACHER']}><Placeholder title="Chấm điểm" /></PrivateRoute>} />
            <Route path="/teacher/content" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_TEACHER']}><TeacherStudyContent /></PrivateRoute>} />
            <Route path="/teacher/notifications" element={<PrivateRoute><Placeholder title="Thông báo" /></PrivateRoute>} />
            <Route path="/teacher/messages" element={<PrivateRoute requiredPermissions={['MESSAGES_ACCESS']}><TeacherMessages /></PrivateRoute>} />
            <Route path="/teacher/attendance" element={<PrivateRoute requiredPermissions={['ATTENDANCE_EDIT']}><TeacherAttendance /></PrivateRoute>} />
            <Route path="/teacher/reports" element={<PrivateRoute requiredPermissions={['PAGE_TEACHER_REPORTS_VIEW']}><TeacherReports /></PrivateRoute>} />
            {/* Trang hồ sơ giảng viên dùng API thật — chỉ cần đăng nhập, bảo vệ bởi userId trong JWT. */}
            <Route path="/teacher/profile" element={<PrivateRoute><TeacherProfile /></PrivateRoute>} />

            {/* --- NHÓM HỌC SINH (STUDENT) --- */}
            <Route path="/student" element={<PrivateRoute requiredPermissions={['PAGE_STUDENT_SCHEDULE_VIEW']}><StudentDashboard /></PrivateRoute>} />
            <Route path="/student/schedule" element={<PrivateRoute requiredPermissions={['PAGE_STUDENT_SCHEDULE_VIEW']}><SchedulePage /></PrivateRoute>} />
            <Route path="/student/homework-list" element={<PrivateRoute requiredPermissions={['ASSIGNMENTS_STUDENT_ACCESS']}><StudentAssignmentPage /></PrivateRoute>} />
            <Route path="/student/reports" element={<PrivateRoute requiredPermissions={['PAGE_STUDENT_SCHEDULE_VIEW']}><StudentReports /></PrivateRoute>} />
            <Route path="/student/homework" element={<PrivateRoute requiredPermissions={['STUDY_CONTENT_STUDENT']}><StudentStudyContent /></PrivateRoute>} />
            <Route path="/student/notifications" element={<PrivateRoute><Placeholder title="Thông báo" /></PrivateRoute>} />
            <Route path="/student/messages" element={<PrivateRoute requiredPermissions={['MESSAGES_ACCESS']}><StudentMessages /></PrivateRoute>} />
            {/* Trang hồ sơ học sinh dùng API thật — chỉ cần đăng nhập, bảo vệ bởi userId trong JWT. */}
            <Route path="/student/profile" element={<PrivateRoute><StudentProfile /></PrivateRoute>} />

            {/* --- NHÓM PHỤ HUYNH (PARENT) --- */}
            <Route path="/parent" element={<PrivateRoute requiredPermissions={['PAGE_PARENT_SCHEDULE_VIEW']}><ParentDashboard /></PrivateRoute>} />
            <Route path="/parent/schedule" element={<PrivateRoute requiredPermissions={['PAGE_PARENT_SCHEDULE_VIEW']}><ParentSchedule /></PrivateRoute>} />
            <Route path="/parent/attendance" element={<PrivateRoute requiredPermissions={['ATTENDANCE_VIEW_PARENT']}><ParentAttendance /></PrivateRoute>} />
            <Route path="/parent/messages" element={<PrivateRoute requiredPermissions={['MESSAGES_ACCESS']}><ParentMessages /></PrivateRoute>} />
            <Route path="/parent/reports" element={<PrivateRoute requiredPermissions={['PAGE_PARENT_SCHEDULE_VIEW']}><ParentReports /></PrivateRoute>} />
            {/* Trang hồ sơ phụ huynh dùng API thật — chỉ cần đăng nhập, bảo vệ bởi userId trong JWT. */}
            <Route path="/parent/profile" element={<PrivateRoute><ParentProfile /></PrivateRoute>} />
            
            {/* Trang 403 hiển thị trong layout có sidebar */}
            <Route path="/unauthorized" element={<PrivateRoute><Unauthorized height="70vh" /></PrivateRoute>} />
          </Route>

          {/* Error Routes & Catch-All */}
          <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Router>
        </TestProvider>
      </StudentClassProvider>
    </AuthProvider>
  );
}

export default App;

// ngày chỉnh sửa 10:03 ngày 6/3/2026 nội dung chỉnh sửa: Bổ sung các chức năng trên thanh slidebar cho giống vs file excel
