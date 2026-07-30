import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { useStudentClass } from '../../context/StudentClassContext';
import NotificationPanel from '../../components/NotificationPanel';
import BaseApi from '../../api/BaseApi';
import { p0Api } from '../../api/p0Api';
import bannerMot from "../../assets/a1.jpeg";
import bannerHai from "../../assets/a2.jpeg";
import bannerBa from "../../assets/a3.jpeg";
const Dashboard = () => {
    const { user } = useAuth();
    const [showNotifications, setShowNotifications] = useState(false);
    const [unreadCount, setUnreadCount] = useState(0);
    const [attendanceRows, setAttendanceRows] = useState([]);
    const [attendanceLoading, setAttendanceLoading] = useState(true);
    const { classes, currentClass, selectClass, loading, error } = useStudentClass();
    const navigate = useNavigate();

    useEffect(() => {
        fetchUnreadCount();
        fetchAttendanceRows();
    }, []);

    const fetchUnreadCount = async () => {
        try {
            const res = await BaseApi.get('Notification/unread-count');
            const resData = res.data || res;
            const count = typeof resData.data === 'number'
                ? resData.data
                : resData.success && typeof resData.data === 'number'
                    ? resData.data
                    : 0;
            setUnreadCount(count);
        } catch (err) {
            console.error('Error fetching unread count:', err);
            setUnreadCount(0);
        }
    };

    const handleNotificationPanelOpen = (isOpen) => {
        setShowNotifications(isOpen);
        if (!isOpen) {
            fetchUnreadCount();
        }
    };
    const handleSelectClass = (classId) => {
        selectClass(classId);
    };

    const handleViewSchedule = () => {
        navigate('/student/schedule');
    };

    const fetchAttendanceRows = async () => {
        setAttendanceLoading(true);
        try {
            setAttendanceRows(await p0Api.students.attendance());
        } catch (err) {
            console.error('Error fetching student attendance:', err);
            setAttendanceRows([]);
        } finally {
            setAttendanceLoading(false);
        }
    };

    const renderAttendanceBadge = (status, label) => {
        const className = {
            present: 'bg-success',
            absent: 'bg-danger',
            late: 'bg-warning text-dark',
            excused: 'bg-primary',
        }[status] || 'bg-secondary';
        return <span className={`badge ${className}`}>{label || 'Chua diem danh'}</span>;
    };

    return (
        <div className="p-0 animate__animated animate__fadeIn" style={{ fontFamily: "'Montserrat', sans-serif" }}>
            
            {/* TOPBAR */}
            <div className="d-flex justify-content-between align-items-center mb-5">
                <div>
<h2 className="fw-bold mb-1" style={{ color: '#005197', letterSpacing: '-0.5px' }}>
                        Xin chào, {user?.fullName || user?.username || 'Học viên'}
                    </h2>
                    <p className="text-muted fw-500 mb-0">Hãy bắt đầu học!</p>
                </div>
                
                <div className="d-flex align-items-center gap-3">
                    <div 
                        className="position-relative hover-up bg-white shadow-sm border rounded-circle d-flex align-items-center justify-content-center" 
                        style={{ width: '45px', height: '45px', cursor: 'pointer' }}
                        onClick={() => handleNotificationPanelOpen(!showNotifications)}
                        title="Thông báo"
                    >
                        <i className="bi bi-bell-fill text-primary fs-5"></i>
                        {unreadCount > 0 && (
                            <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger border border-light" style={{ fontSize: '10px' }}>
                                {unreadCount > 99 ? '99+' : unreadCount}
                            </span>
                        )}
                    </div>
                    <NotificationPanel isOpen={showNotifications} onClose={() => handleNotificationPanelOpen(false)} onMarkAsRead={fetchUnreadCount} />
                </div>
            </div>
        {/* KHU VUC BANNER SLIDE */}
            <div id="bannerCarousel" className="carousel slide mb-5 shadow-lg rounded-5 overflow-hidden" data-bs-ride="carousel" style={{ minHeight: '450px' }}>
                
                <div className="carousel-indicators">
                    <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="0" className="active" aria-current="true" aria-label="Slide 1"></button>
                    <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
                    <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="2" aria-label="Slide 3"></button>
                </div>

                {/* Sửa minHeight ở đây */}
                <div className="carousel-inner" style={{ minHeight: '450px' }}>
                    <div className="carousel-item active" data-bs-interval="3000">
                        {/* Sửa height ở thẻ img */}
                        <img src={bannerMot} className="d-block w-100" alt="Banner 1" style={{ objectFit: 'cover', height: '450px' }} />
                    </div>
                    <div className="carousel-item" data-bs-interval="3000">
                        <img src={bannerHai} className="d-block w-100" alt="Banner 2" style={{ objectFit: 'cover', height: '450px' }} />
                    </div>
                    <div className="carousel-item" data-bs-interval="3000">
<img src={bannerBa} className="d-block w-100" alt="Banner 3" style={{ objectFit: 'cover', height: '450px' }} />
                    </div>
                </div>

                <button className="carousel-control-prev" type="button" data-bs-target="#bannerCarousel" data-bs-slide="prev">
                    <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span className="visually-hidden">Previous</span>
                </button>
                <button className="carousel-control-next" type="button" data-bs-target="#bannerCarousel" data-bs-slide="next">
                    <span className="carousel-control-next-icon" aria-hidden="true"></span>
                    <span className="visually-hidden">Next</span>
                </button>
            </div>

            {/* STATS SECTION */}
            <div className="row g-4 mb-5">
                {/* the thong ke tham du */}
                <div className="col-md-4">
                    <div className="card bg-white border-0 rounded-4 shadow-sm h-100 overflow-hidden">
                        <div className="card-body p-4 position-relative">
                            <div className="d-flex align-items-center mb-3">
                                <div className="bg-primary bg-opacity-10 p-2 rounded-3 me-3">
                                    <i className="bi bi-patch-check fs-4 text-primary"></i>
                                </div>
                                <span className="fw-bold text-uppercase small text-muted">Tham dự</span>
                            </div>
                            <h2 className="display-5 fw-bold mb-0 text-primary">94%</h2>
                            <i className="bi bi-graph-up-arrow position-absolute bottom-0 end-0 p-3 text-primary opacity-10" style={{ fontSize: '3rem' }}></i>
                        </div>
                    </div>
                </div>

                {/* the thong ke so bai tap giao vien da giao */}
                <div className="col-md-4">
                    <div className="card bg-white border-0 rounded-4 shadow-sm h-100 overflow-hidden">
                        <div className="card-body p-4 position-relative">
                            <div className="d-flex align-items-center mb-3">
                                <div className="p-2 rounded-3 me-3" style={{ backgroundColor: 'rgba(236, 64, 122, 0.1)' }}>
                                    <i className="bi bi-journal-text fs-4" style={{ color: '#ec407a' }}></i>
                                </div>
                                <span className="fw-bold text-uppercase small text-muted" style={{ fontSize: '0.8rem' }}>Số bài tập giáo viên đã giao</span>
                            </div>
                            <h2 className="display-5 fw-bold mb-0" style={{ color: '#ec407a' }}>15</h2>
<i className="bi bi-file-earmark-check position-absolute bottom-0 end-0 p-3 opacity-10" style={{ fontSize: '3rem', color: '#ec407a' }}></i>
                        </div>
                    </div>
                </div>

                {/* the thong ke so buoi hoc trong tuan */}
                <div className="col-md-4">
                    <div className="card bg-white border-0 rounded-4 shadow-sm h-100 overflow-hidden">
                        <div className="card-body p-4 position-relative">
                            <div className="d-flex align-items-center mb-3">
                                <div className="p-2 rounded-3 me-3" style={{ backgroundColor: 'rgba(229, 57, 53, 0.1)' }}>
                                    <i className="bi bi-calendar-week fs-4" style={{ color: '#e53935' }}></i>
                                </div>
                                <span className="fw-bold text-uppercase small text-muted">Số buổi học trong tuần</span>
                            </div>
                            <h2 className="display-5 fw-bold mb-0" style={{ color: '#e53935' }}>3</h2>
                            <i className="bi bi-calendar3 position-absolute bottom-0 end-0 p-3 opacity-10" style={{ fontSize: '3rem', color: '#e53935' }}></i>
                        </div>
                    </div>
                </div>
            </div>
{/* KHU VUC LOP HIEN TAI */}
            <div className="mb-4">
                <div className="d-flex align-items-center mb-3">
                    <div className="bg-primary bg-opacity-10 rounded-3 p-2 me-3">
                        <i className="bi bi-book text-primary fs-5"></i>
                    </div>
                    <h5 className="fw-bold mb-0 text-dark">Lớp hiện tại</h5>
                </div>

                {error && (
                    <div className="alert alert-warning alert-dismissible fade show rounded-4" role="alert">
                        <i className="bi bi-exclamation-triangle me-2"></i>
                        {error}
                    </div>
                )}

                <div className="card bg-white border-0 rounded-4 shadow-sm p-4">
                    {loading ? (
                        <div className="text-center py-4">
                            <div className="spinner-border text-primary" role="status">
                                <span className="visually-hidden">Đang tải...</span>
                            </div>
                        </div>
                    ) : classes.length === 0 ? (
                        <div className="text-center py-4">
                            <div className="bg-light rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '80px', height: '80px' }}>
                                <i className="bi bi-inbox text-muted" style={{ fontSize: '2.5rem' }}></i>
                            </div>
<h5 className="fw-bold text-dark">Chưa có lớp từ database</h5>
                            <p className="text-muted mb-0">Tài khoản học sinh hiện chưa được gán vào lớp nào. Vui lòng liên hệ quản trị viên hoặc giáo viên phụ trách.</p>
                        </div>
                    ) : (
                        <div className="row align-items-center">
                            <div className="col-md-6">
                                <label className="form-label fw-bold text-muted text-uppercase small mb-2">Chọn lớp</label>
                                <select 
                                    className="form-select form-select-lg rounded-4 border-1 shadow-none fw-bold"
                                    value={currentClass?.id || ''}
                                    onChange={(e) => handleSelectClass(e.target.value)}
                                    style={{ color: '#2c3e50', backgroundColor: '#f8f9fa' }}
                                >
                                    {classes.map(cls => (
                                        <option key={cls.id} value={cls.id}>
                                            {cls.classCode} - {cls.teacher}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="col-md-6 mt-3 mt-md-0">
                                {currentClass && (
                                    <div className="p-4 rounded-4 position-relative overflow-hidden" style={{ background: 'linear-gradient(135deg, #1e88e5 0%, #1565c0 100%)', color: 'white' }}>
                                        <i className="bi bi-book-half position-absolute top-50 end-0 translate-middle-y opacity-25 me-n3" style={{ fontSize: '6rem' }}></i>
                                        <div className="position-relative z-index-1">
                                            <p className="mb-1 small text-white-50 text-uppercase fw-bold">Lớp đang học</p>
                                            <h4 className="fw-bold mb-2">{currentClass.classCode}</h4>
                                            <p className="mb-0 small">
                                                <i className="bi bi-person-badge me-2"></i>
                                                {currentClass.teacher}
                                            </p>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                    )}
                </div>
            </div>

            {/* KHU VUC DIEM DANH GAN DAY */}
            <div className="mb-4">
                <div className="d-flex align-items-center mb-3">
                    <div className="bg-success bg-opacity-10 rounded-3 p-2 me-3">
<i className="bi bi-clipboard-check text-success fs-5"></i>
                    </div>
                    <h5 className="fw-bold mb-0 text-dark">Điểm danh gần đây</h5>
                </div>

                <div className="card bg-white border-0 rounded-4 shadow-sm p-4">
                    {attendanceLoading ? (
                        <div className="text-center py-4 text-muted">Đang tải điểm danh...</div>
                    ) : attendanceRows.length === 0 ? (
                        <div className="text-center text-muted py-4">Chưa có dữ liệu điểm danh. Khi giáo viên điểm danh, trạng thái sẽ hiển thị tại đây.</div>
                    ) : (
                        <div className="table-responsive">
                            <table className="table align-middle mb-0 border-white">
                                <thead className="table-light">
                                    <tr>
                                        <th className="rounded-start-3 border-0">Lớp</th>
                                        <th className="border-0">Ngày học</th>
                                        <th className="border-0">Trạng thái</th>
                                        <th className="rounded-end-3 border-0">Ghi chú</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {/* render danh sach diem danh */}
                                    {attendanceRows.slice(0, 5).map((row) => (
                                        <tr key={`${row.classId}-${row.sessionId}`}>
                                            <td className="border-bottom-0 py-3 fw-medium text-dark">{row.className}</td>
                                            <td className="border-bottom-0 py-3 text-muted">{row.sessionDate}</td>
                                            <td className="border-bottom-0 py-3">{renderAttendanceBadge(row.status, row.statusLabel)}</td>
                                            <td className="border-bottom-0 py-3 text-muted">{row.note || '-'}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>

            {/* KHU VUC LOP HOC HOM NAY */}
            <div className="mb-4">
                <div className="d-flex align-items-center mb-3">
                    <div className="bg-danger bg-opacity-10 rounded-3 p-2 me-3">
                        <i className="bi bi-calendar-event text-danger fs-5"></i>
                    </div>
                    <h5 className="fw-bold mb-0 text-dark">Lớp học hôm nay</h5>
                </div>
                
                <div className="card bg-white border-0 rounded-4 shadow-sm p-5 text-center">
<div className="bg-light rounded-circle d-inline-flex align-items-center justify-content-center mx-auto mb-3" style={{ width: '80px', height: '80px' }}>
                        <i className="bi bi-calendar-x text-muted" style={{ fontSize: '2.5rem' }}></i>
                    </div>
                    <h5 className="fw-bold text-dark">Hiện chưa có lớp học nào hôm nay</h5>
                    <p className="text-muted mb-4">Hãy quay lại sau hoặc xem lịch học chi tiết nhé!</p>
                    <div>
                        <button className="btn btn-primary rounded-pill px-4 py-2 fw-bold shadow-sm" onClick={handleViewSchedule}>
                            <i className="bi bi-calendar3 me-2"></i>
                            XEM LỊCH HỌC
                        </button>
                    </div>
                </div>
            </div>

        </div>
    );
};

export default Dashboard;