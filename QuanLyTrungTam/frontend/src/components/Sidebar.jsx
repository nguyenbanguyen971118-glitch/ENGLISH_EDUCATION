import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Sidebar = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const rolePath = user?.role === 'Giao_Vien' ? 'teacher' 
                   : user?.role === 'Hoc_Sinh' ? 'student' 
                   : user?.role === 'Phu_Huynh' ? 'parent' 
                   : 'admin';

    const handleLogout = () => {
        logout();
        navigate('/login');
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

                {rolePath === 'student' && (
                    <div className="mb-4 px-1">
                        <p className="text-muted fw-bold mb-2 ps-1" style={{ fontSize: '10px', letterSpacing: '1px', opacity: 0.8 }}>LỚP HIỆN TẠI</p>
                        <select className="form-select border-light-subtle bg-light rounded-3 py-2 small fw-bold text-dark shadow-none border-2" style={{ fontSize: '12px', cursor: 'pointer' }}>
                            <option>HNI - PRI4 - 0065</option>
                        </select>
                    </div>
                )}
            </div>

            {/* --- PHẦN GIỮA CÓ THỂ CUỘN (MENU) --- */}
            {/* flex-grow-1 giúp chiếm hết khoảng trống, overflow-y-auto tạo thanh cuộn khi menu Admin quá dài */}
            <div className="flex-grow-1 overflow-y-auto custom-scrollbar px-1">
                <ul className="nav nav-pills flex-column shadow-none">
                    <li className="nav-item">
                        <NavLink to={`/${rolePath}`} className={getNavLinkClass} end>
                            <i className="bi bi-grid-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Trang Chủ</span>
                        </NavLink>
                    </li>
                    
                    {/* Menu cho từng Role (vẫn giữ nguyên logic cũ của bạn) */}
                    {rolePath === 'teacher' && (
                        <>
                            <li className="nav-item"><NavLink to="/teacher/schedule" className={getNavLinkClass}><i className="bi bi-calendar-event me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Lịch dạy</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/teacher/classes" className={getNavLinkClass}><i className="bi bi-people me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý lớp học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/teacher/homework" className={getNavLinkClass}><i className="bi bi-pencil-square me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Bài tập & Chấm điểm</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/teacher/materials" className={getNavLinkClass}><i className="bi bi-folder-symlink me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Kho học liệu</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/teacher/communication" className={getNavLinkClass}><i className="bi bi-chat-quote me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Trao đổi phụ huynh</span></NavLink></li>
                        </>
                    )}

                    {rolePath === 'parent' && (
                        <>
                            <li className="nav-item"><NavLink to="/parent/schedule" className={getNavLinkClass}><i className="bi bi-calendar3 me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Lịch học của con</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/parent/progress" className={getNavLinkClass}><i className="bi bi-graph-up-arrow me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Tiến độ của con</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/parent/results" className={getNavLinkClass}><i className="bi bi-clipboard-data me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Kết quả học tập</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/parent/feedback" className={getNavLinkClass}><i className="bi bi-chat-left-dots me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Phản hồi giáo viên</span></NavLink></li>
                        </>
                    )}

                    {rolePath === 'student' && (
                        <>
                            <li className="nav-item"><NavLink to="/student/schedule" className={getNavLinkClass}><i className="bi bi-calendar3 me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Lịch học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/student/results" className={getNavLinkClass}><i className="bi bi-journal-check me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Kết quả học tập</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/student/homework-list" className={getNavLinkClass}><i className="bi bi-pencil-square me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Bài tập về nhà</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/student/homework" className={getNavLinkClass}><i className="bi bi-laptop me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Học chủ động</span></NavLink></li>
                        </>
                    )}

                    {rolePath === 'admin' && (
                        <>
                            <li className="nav-item"><NavLink to="/admin/users" className={getNavLinkClass}><i className="bi bi-person-badge-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý người dùng</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/courses" className={getNavLinkClass}><i className="bi bi-journal-bookmark-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý khóa học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/classes" className={getNavLinkClass}><i className="bi bi-collection-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Quản lý lớp học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/schedules" className={getNavLinkClass}><i className="bi bi-calendar-check-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Lịch dạy & học</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/content" className={getNavLinkClass}><i className="bi bi-file-earmark-richtext-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Nội dung học tập</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/finances" className={getNavLinkClass}><i className="bi bi-cash-stack me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Học phí & Tài chính</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/reports" className={getNavLinkClass}><i className="bi bi-pie-chart-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Báo cáo & Thống kê</span></NavLink></li>
                            <li className="nav-item"><NavLink to="/admin/notifications" className={getNavLinkClass}><i className="bi bi-send-check-fill me-3 fs-5"></i> <span style={{ fontSize: '14px' }}>Thông báo & Liên lạc</span></NavLink></li>
                        </>
                    )}
                </ul>
            </div>

            {/* --- PHẦN CUỐI CỐ ĐỊNH (LOGOUT) --- */}
            <div className="flex-shrink-0 mt-auto pt-3 border-top">
                <button onClick={handleLogout} className="btn w-100 text-danger fw-bold py-2.5 rounded-4 border-0 shadow-sm d-flex align-items-center justify-content-center btn-logout" style={{ backgroundColor: '#FFF0F0', transition: '0.3s', fontSize: '14px' }}>
                    <i className="bi bi-power me-2 fs-5"></i> Đăng xuất
                </button>
                <p className="text-center text-muted mt-3 mb-0 fw-medium" style={{ fontSize: '10px', opacity: 0.5 }}>Active App v2.10.38</p>
            </div>

            <style>{`
                .custom-scrollbar::-webkit-scrollbar { width: 4px; }
                .custom-scrollbar::-webkit-scrollbar-thumb { background: #eee; border-radius: 10px; }
                .custom-scrollbar::-webkit-scrollbar-track { background: transparent; }
                .hover-bg-light:hover { background-color: #f8f9fa !important; color: #005197 !important; }
                .btn-logout:hover { background-color: #ffe0e0 !important; transform: translateY(-2px); }
                .shadow-hover:hover { box-shadow: 0 4px 12px rgba(0,0,0,0.1) !important; }
                .nav-link.active { border-left-width: 4px !important; }
            `}</style>
        </div>
    );
};

export default Sidebar;