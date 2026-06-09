import React, { useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Sidebar = () => {
    const { user, logout, refreshPermissions } = useAuth();
    const navigate = useNavigate();
    const [refreshing, setRefreshing] = useState(false);

    const permissionCodes = Array.isArray(user?.permissionCodes) ? user.permissionCodes : [];
    const hasPermission = (code) => permissionCodes.includes(code);

    const rolePath = user?.role === 'Giao_Vien' ? 'teacher' 
                   : user?.role === 'Hoc_Sinh' ? 'student' 
                   : user?.role === 'Phu_Huynh' ? 'parent' 
                   : 'admin';

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const handleRefreshPermissions = async () => {
        setRefreshing(true);
        try {
            const result = await refreshPermissions();
            if (result.success) {
                alert('✅ Quyền truy cập đã được cập nhật!');
                window.location.reload();
            } else {
                alert('❌ ' + result.message);
            }
        } catch (error) {
            alert('❌ Lỗi khi cập nhật quyền: ' + error.message);
        } finally {
            setRefreshing(false);
        }
    };

    const goToProfile = () => {
        navigate(`/${rolePath}/profile`);
    };

    const getNavLinkClass = ({ isActive }) => {
        const baseClass = "nav-link py-3 px-4 fw-bold d-flex align-items-center mb-2 rounded-4 transition-all border-start border-4";
        return isActive 
            ? `${baseClass} bg-primary text-white shadow border-primary active` 
            : `${baseClass} text-secondary border-white hover-bg-light`; 
    };

    const permissionMenus = [
        {
            code: 'PAGE_ADMIN_USERS_VIEW',
            to: '/admin/users',
            icon: 'bi bi-person-badge-fill',
            label: 'Quản lý người dùng'
        },
        {
            code: 'PAGE_ADMIN_CLASSES_VIEW',
            to: '/admin/classes',
            icon: 'bi bi-collection-fill',
            label: 'Quản lý lớp học'
        },
        {
            code: 'PAGE_ADMIN_REPORTS_VIEW',
            to: '/admin/reports',
            icon: 'bi bi-pie-chart-fill',
            label: 'Báo cáo và thống kê'
        },
        {
            code: 'PAGE_TEACHER_SCHEDULE_VIEW',
            roles: ['Giao_Vien'],
            to: '/teacher/schedule',
            icon: 'bi bi-calendar-event',
            label: 'Quản lý lịch dạy'
        },
        {
            code: 'PAGE_STUDENT_SCHEDULE_VIEW',
            roles: ['Hoc_Sinh'],
            to: '/student/schedule',
            icon: 'bi bi-calendar3',
            label: 'Quản lý lịch học'
        },
        {
            code: 'PAGE_PARENT_SCHEDULE_VIEW',
            roles: ['Phu_Huynh'],
            to: '/parent/schedule',
            icon: 'bi bi-calendar3',
            label: 'Xem lịch học'
        }
    ].filter((item) => hasPermission(item.code) && (!item.roles || item.roles.includes(user?.role)));

    return (
        <div className="col-md-3 col-lg-2 bg-white d-flex flex-column vh-100 sticky-top border-end shadow-sm p-3" style={{ fontFamily: "'Montserrat', sans-serif" }}>
            
            {/* --- PHẦN ĐẦU CỐ ĐỊNH (PROFILE) --- */}
            <div className="flex-shrink-0">
                <div onClick={goToProfile} className="d-flex align-items-center p-3 mb-4 rounded-4 border bg-white shadow-sm position-relative shadow-hover" style={{ transition: '0.3s', cursor: 'pointer', minHeight: '85px' }}>
                    <div className="position-absolute top-0 start-0 h-100 w-2 bg-primary"></div>
                    {(() => {
                        const displayName = user?.hoTen || user?.fullName || user?.name || 'Người dùng';
                        const displayRole = user?.role?.replace('_', ' ').toUpperCase() || 'USER';
                        return (
                            <>
                                <img 
                                    src={`https://ui-avatars.com/api/?name=${encodeURIComponent(displayName)}&background=005197&color=fff&bold=true`} 
                                    className="rounded-circle me-3 border border-2 border-primary-subtle" 
                                    width="42" height="42" alt="avatar" 
                                />
                                <div className="flex-grow-1" style={{ minWidth: 0 }}>
                                    <h6 className="mb-1 fw-bold text-dark" style={{ fontSize: displayName.length > 15 ? '0.75rem' : '0.85rem', lineHeight: '1.2', wordBreak: 'break-word' }}>
                                        {displayName}
                                    </h6>
                                    <span className="badge bg-info-subtle text-info rounded-pill px-2 py-1" style={{ fontSize: '9px', fontWeight: '800' }}>
                                        {displayRole}
                                    </span>
                                </div>
                            </>
                        );
                    })()}
                </div>

            </div>

            {/* --- PHẦN GIỮA CÓ THỂ CUỘN (MENU) --- */}
            <div className="flex-grow-1 overflow-y-auto custom-scrollbar px-1">
                <ul className="nav nav-pills flex-column shadow-none">
                    <li className="nav-item">
                        <NavLink to={`/${rolePath}`} className={getNavLinkClass} end>
                            <i className="bi bi-grid-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Trang Chủ</span>
                        </NavLink>
                    </li>

                    {permissionMenus.map((menu) => (
                        <li className="nav-item" key={menu.code}>
                            <NavLink to={menu.to} className={getNavLinkClass}>
                                <i className={`${menu.icon} me-3 fs-5`}></i> <span style={{ fontSize: '14px' }}>{menu.label}</span>
                            </NavLink>
                        </li>
                    ))}
                    
                    {/* --- MENU CHO ADMIN --- */}
                    {rolePath === 'admin' && (
                        <>
                            <li className="nav-item"><NavLink to="/admin/functions" className={getNavLinkClass}><i className="bi bi-toggle-on me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý chức năng</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/courses" className={getNavLinkClass}><i className="bi bi-journal-bookmark-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý khóa học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/content" className={getNavLinkClass}><i className="bi bi-file-earmark-richtext-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Nội dung học tập</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/exams" className={getNavLinkClass}><i className="bi bi-journal-code me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Bài tập - Đề thi</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/attendance" className={getNavLinkClass}><i className="bi bi-clipboard-check-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý Điểm danh</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/notifications" className={getNavLinkClass}><i className="bi bi-send-check-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý Thông báo</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/messages" className={getNavLinkClass}><i className="bi bi-chat-dots-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý Nhắn tin</span></NavLink></li>
                        </>
                    )}

                    {/* --- MENU CHO PHỤ HUYNH --- */}
                    {rolePath === 'parent' && (
                        <>
                            <li className="nav-item"><NavLink to="/parent/attendance" className={getNavLinkClass}><i className="bi bi-clipboard2-check me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Xem điểm danh</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/parent/messages" className={getNavLinkClass}><i className="bi bi-chat-dots me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý nhắn tin</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/parent/reports" className={getNavLinkClass}><i className="bi bi-bar-chart-line me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Báo cáo thống kê</span></NavLink></li>
                        </>
                    )}

                    {/* --- MENU CHO HỌC SINH --- */}
                    {rolePath === 'student' && (
                        <>
                            <li className="nav-item"><NavLink to="/student/homework-list" className={getNavLinkClass}><i className="bi bi-pencil-square me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Bài tập/bài thi</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/student/reports" className={getNavLinkClass}><i className="bi bi-pie-chart-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Báo cáo thống kê</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/student/homework" className={getNavLinkClass}><i className="bi bi-laptop me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Học chủ động</span></NavLink></li>
                        </>
                    )}

                    {/* --- MENU CHO GIÁO VIÊN --- */}
                    {rolePath === 'teacher' && (
                    <>
                        <li className="nav-item">
                            <NavLink to="/teacher/classes" className={getNavLinkClass}>
                                <i className="bi bi-people me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Quản lý lớp học</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/exams" className={getNavLinkClass}>
                                <i className="bi bi-file-earmark-text me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Quản lý bài tập - Đề thi</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/grading" className={getNavLinkClass}>
                                <i className="bi bi-pencil-square me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Chấm điểm</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/content" className={getNavLinkClass}>
                                <i className="bi bi-book me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Nội dung học tập</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/messages" className={getNavLinkClass}>
                                <i className="bi bi-chat-dots me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Quản lý nhắn tin</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/attendance" className={getNavLinkClass}>
                                <i className="bi bi-clipboard2-check me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Điểm danh</span>
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink to="/teacher/reports" className={getNavLinkClass}>
                                <i className="bi bi-bar-chart-line me-3 fs-5"></i> 
                                <span style={{ fontSize: '14px' }}>Báo cáo và thống kê</span>
                            </NavLink>
                        </li>
                    </>
                )}
                </ul>
            </div>

            {/* --- DEBUG INFO --- */}
            <div className="px-3 py-2 small text-muted border-top">
                <details style={{ fontSize: '11px' }}>
                    <summary style={{ cursor: 'pointer', fontWeight: 'bold' }}>📋 Debug Info</summary>
                    <div className="mt-2 small" style={{ backgroundColor: '#f5f5f5', padding: '8px', borderRadius: '4px' }}>
                        <div><strong>User:</strong> {user?.fullName}</div>
                        <div><strong>Role:</strong> {user?.role}</div>
                        <div><strong>Permissions ({permissionCodes.length}):</strong></div>
                        {permissionCodes.length > 0 ? (
                            <ul style={{ fontSize: '10px', margin: '4px 0', paddingLeft: '16px' }}>
                                {permissionCodes.map((code, idx) => (
                                    <li key={idx} style={{ wordBreak: 'break-word' }}>✓ {code}</li>
                                ))}
                            </ul>
                        ) : (
                            <div style={{ color: 'red', fontWeight: 'bold' }}>⚠️ Không có quyền!</div>
                        )}
                    </div>
                </details>
            </div>

            {/* --- PHẦN CUỐI CỐ ĐỊNH (REFRESH & LOGOUT) --- */}
            <div className="flex-shrink-0 mt-auto pt-3 border-top d-flex gap-2">
                <button 
                    onClick={handleRefreshPermissions} 
                    disabled={refreshing}
                    className="btn flex-grow-1 text-info fw-bold py-2.5 rounded-4 border-0 shadow-sm d-flex align-items-center justify-content-center btn-refresh" 
                    style={{ backgroundColor: '#E3F2FD', transition: '0.3s', fontSize: '14px' }}
                    title="Cập nhật quyền truy cập từ hệ thống"
                >
                    <i className={`bi bi-arrow-clockwise me-2 fs-5 ${refreshing ? 'spin' : ''}`}></i> 
                    {refreshing ? 'Đang cập nhật...' : 'Làm mới'}
                </button>
                <button onClick={handleLogout} className="btn flex-grow-1 text-danger fw-bold py-2.5 rounded-4 border-0 shadow-sm d-flex align-items-center justify-content-center btn-logout" style={{ backgroundColor: '#FFF0F0', transition: '0.3s', fontSize: '14px' }}>
                    <i className="bi bi-power me-2 fs-5"></i> Đăng xuất
                </button>
            </div>

            <style>{`
                .custom-scrollbar::-webkit-scrollbar { width: 4px; }
                .custom-scrollbar::-webkit-scrollbar-thumb { background: #eee; border-radius: 10px; }
                .custom-scrollbar::-webkit-scrollbar-track { background: transparent; }
                .hover-bg-light:hover { background-color: #f8f9fa !important; color: #005197 !important; }
                .btn-refresh:hover:not(:disabled) { background-color: #bbdefb !important; transform: translateY(-2px); }
                .btn-refresh:disabled { opacity: 0.7; cursor: not-allowed; }
                .btn-refresh .spin { animation: spin 1s linear infinite; }
                .btn-logout:hover { background-color: #ffe0e0 !important; transform: translateY(-2px); }
                .shadow-hover:hover { box-shadow: 0 4px 12px rgba(0,0,0,0.1) !important; }
                .nav-link.active { border-left-width: 4px !important; }
                @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
            `}</style>
        </div>
    );
};

export default Sidebar;
// ngày chỉnh sửa 10:03 ngày 6/3/2026 nội dung chỉnh sửa: Bổ sung các chức năng trên thanh slidebar cho giống vs file excel
