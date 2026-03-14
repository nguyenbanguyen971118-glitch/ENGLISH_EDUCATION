import React, { useState, useEffect } from 'react';
import ScheduleTable from '../../components/ScheduleTable';
import { useAuth } from '../../context/AuthContext';
import scheduleData from '../../data/schedule.json';

const TeacherSchedule = () => {
    const { user } = useAuth();
    const [currentDate, setCurrentDate] = useState(new Date("2026-02-26"));
    const [classes, setClasses] = useState([]);

    // ==========================================
    // DỮ LIỆU GIẢ LẬP ĐỂ TEST FORM
    // ==========================================
    const mockRooms = ['P.101', 'P.102', 'P.201', 'P.202', 'P.301', 'P.302', 'P.405', 'Lab 1', 'Lab 2', 'Hội trường A'];
    
    // Danh sách lớp học CỐ ĐỊNH để Dropdown luôn có dữ liệu đẹp
    const classOptions = [
        { id: 'IELTS-102', name: 'IELTS 6.5+ Target' },
        { id: 'HNI-PRI4-0065', name: 'Tiếng Anh Trẻ Em' },
        { id: 'TOEIC-500', name: 'TOEIC Căn bản' },
        { id: 'KIDS-STARTER', name: 'Tiếng Anh Mầm non' }
    ];

    // ==========================================
    // STATE CHO TÍNH NĂNG ĐỀ XUẤT ĐỔI LỊCH
    // ==========================================
    const [showModal, setShowModal] = useState(false);
    const [requestType, setRequestType] = useState('time'); // 'time' (đổi giờ) | 'room' (đổi phòng)
    const [formData, setFormData] = useState({ 
        classId: classOptions[0].id, // Mặc định chọn lớp đầu tiên
        newDay: 'Thứ 2', 
        newSlot: 'Ca 1 (07:30 - 09:30)', 
        newRoom: mockRooms[0], 
        reason: 'Có việc gia đình đột xuất'
    });
    
    const [toast, setToast] = useState({ show: false, msg: '', type: 'success' });

    const timeSlots = [
        'Ca 1 (07:30 - 09:30)', 'Ca 2 (09:45 - 11:45)', 'Ca 3 (13:30 - 15:30)', 
        'Ca 4 (15:45 - 17:45)', 'Ca 5 (18:00 - 20:00)', 'Ca 6 (20:15 - 22:15)'
    ];
    const days = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'Chủ Nhật'];
    const reasons = [
        'Có việc gia đình đột xuất', 'Trùng lịch họp chuyên môn', 'Vấn đề sức khỏe (Ốm/Bệnh)', 
        'Phòng học bị hỏng thiết bị (Máy chiếu/Điều hòa)', 'Học viên yêu cầu học bù', 'Lý do khác...'
    ];

    useEffect(() => {
        // Vẫn load dữ liệu cho Bảng lưới lịch (ScheduleTable)
        const currentTeacherId = user?.teacherId || "GV001";
        const filtered = scheduleData.filter(item => item.teacherId === currentTeacherId);
        setClasses(filtered); 
    }, [user]);

    const changeWeek = (amount) => {
        const newDate = new Date(currentDate);
        newDate.setDate(currentDate.getDate() + (amount * 7));
        setCurrentDate(newDate);
    };

    const showToastMsg = (msg, type = 'success') => {
        setToast({ show: true, msg, type });
        setTimeout(() => setToast({ show: false, msg: '', type: '' }), 3500);
    };

    const handleSubmitRequest = (e) => {
        e.preventDefault();
        
        if (!formData.classId || !formData.reason.trim()) {
            showToastMsg('Vui lòng chọn đầy đủ thông tin và lý do!', 'danger');
            return;
        }

        console.log("Đã gửi yêu cầu:", { type: requestType, ...formData });
        showToastMsg('Đã gửi yêu cầu thành công! Vui lòng chờ Admin phê duyệt.', 'success');
        setShowModal(false);
    };

    return (
        <div className="p-4 animate__animated animate__fadeIn position-relative h-100">
            
            {/* THÔNG BÁO NỔI */}
            {toast.show && (
                <div className={`position-fixed bottom-0 end-0 m-4 p-3 rounded-4 shadow-lg text-white bg-${toast.type} animate__animated animate__fadeInUp`} style={{ zIndex: 1100, minWidth: '280px', border: '1px solid rgba(255,255,255,0.2)' }}>
                    <div className="d-flex align-items-center fw-bold">
                        <i className={`bi ${toast.type === 'danger' ? 'bi-exclamation-triangle-fill fs-4' : 'bi-check-circle-fill fs-5'} me-3`}></i>
                        {toast.msg}
                    </div>
                </div>
            )}

            {/* HEADER */}
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h2 className="fw-bold text-dark mb-1 text-uppercase" style={{ fontSize: '24px', letterSpacing: '-0.5px' }}>
                        Lịch dạy cá nhân
                    </h2>
                    <div className="d-flex align-items-center gap-2 mt-2">
                        <span className="badge bg-primary-subtle text-primary border border-primary-subtle rounded-pill px-3 shadow-sm">
                            Mã GV: {user?.teacherId || "GV001"}
                        </span>
                        <p className="text-muted mb-0 small">
                            Giảng viên: <span className="fw-bold text-dark">{user?.hoTen || "Nguyễn Thị Lan Anh"}</span>
                        </p>
                    </div>
                </div>
                
                <div className="d-flex align-items-center gap-3">
                    {/* NÚT YÊU CẦU ĐỔI LỊCH */}
                    <button className="btn btn-warning rounded-pill px-4 fw-bold shadow-sm d-flex align-items-center text-dark hover-scale"
                            onClick={() => setShowModal(true)} style={{ height: '40px' }}>
                        <i className="bi bi-send-plus-fill me-2"></i> Đề xuất đổi lịch
                    </button>

                    <button className="btn btn-primary rounded-pill px-4 fw-bold shadow-sm hover-scale" 
                            onClick={() => setCurrentDate(new Date("2026-02-26"))} style={{ height: '40px' }}>
                        Hiện tại
                    </button>
                    
                    <div className="d-flex align-items-center border rounded-pill bg-white px-2 shadow-sm" style={{ height: '40px' }}>
                        <button className="btn btn-link text-dark p-0 px-2 text-decoration-none small fw-medium hover-scale" onClick={() => changeWeek(-1)}>
                            <i className="bi bi-chevron-left small fw-bold"></i> Trở về
                        </button>
                        <div className="vr mx-1 my-2" style={{ opacity: 0.2 }}></div>
                        <button className="btn btn-link text-dark p-0 px-2 text-decoration-none small fw-medium hover-scale" onClick={() => changeWeek(1)}>
                            Tiếp <i className="bi bi-chevron-right small fw-bold"></i>
                        </button>
                    </div>
                </div>
            </div>

            {/* BẢNG LỊCH DẠY */}
            <div className="card border-0 shadow-sm rounded-5 overflow-hidden bg-white">
                <div className="card-body p-0">
                    <ScheduleTable data={classes} currentViewDate={currentDate} />
                </div>
            </div>

            {/* GHI CHÚ */}
            <div className="mt-4 p-3 bg-light rounded-4 border-start border-primary border-4 shadow-sm">
                <div className="d-flex align-items-center gap-2 mb-1">
                    <i className="bi bi-info-circle-fill text-primary"></i>
                    <small className="text-muted fw-bold text-uppercase">Lưu ý:</small>
                </div>
                <span className="small text-dark fst-italic">
                    * Thầy/Cô vui lòng thực hiện điểm danh trong vòng 15 phút đầu giờ học. Nếu có sự cố trùng lịch hoặc phòng học có vấn đề, vui lòng sử dụng chức năng <b>"Đề xuất đổi lịch"</b> phía trên.
                </span>
            </div>

            {/* ========================================== */}
            {/* MODAL ĐỀ XUẤT ĐỔI LỊCH / PHÒNG (ĐÃ FIX DROPDOWN LỚP) */}
            {/* ========================================== */}
            {showModal && (
                <div className="modal-backdrop-custom d-flex align-items-center justify-content-center animate__animated animate__fadeIn">
                    <div className="bg-white rounded-4 shadow-lg overflow-hidden" style={{ width: '500px', zIndex: 1050 }}>
                        
                        <div className="p-3 px-4 bg-warning d-flex justify-content-between align-items-center">
                            <h5 className="fw-bold mb-0 text-dark">
                                <i className="bi bi-envelope-paper-fill me-2"></i> Phiếu yêu cầu thay đổi
                            </h5>
                            <button className="btn-close" onClick={() => setShowModal(false)}></button>
                        </div>

                        <form onSubmit={handleSubmitRequest} className="p-4">
                            
                            <div className="mb-4">
                                <label className="form-label small fw-bold text-dark mb-2">1. Chọn lớp học cần thay đổi:</label>
                                {/* ĐÃ SỬA THÀNH DANH SÁCH MOCK DATA ĐẸP */}
                                <select className="form-select bg-light border-0 py-2 shadow-sm fw-medium text-primary" 
                                        value={formData.classId} onChange={e => setFormData({...formData, classId: e.target.value})}>
                                    {classOptions.map((c, idx) => (
                                        <option key={idx} value={c.id}>
                                            Lớp: {c.id} - {c.name}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div className="mb-3">
                                <label className="form-label small fw-bold text-dark mb-2">2. Bạn muốn thay đổi gì?</label>
                                <div className="d-flex gap-3">
                                    <div className={`border rounded-3 p-2 flex-grow-1 text-center cursor-pointer transition-all ${requestType === 'time' ? 'bg-primary-subtle border-primary text-primary fw-bold shadow-sm' : 'bg-light text-muted border-light hover-bg-gray'}`}
                                         onClick={() => setRequestType('time')}>
                                        <i className="bi bi-clock-history d-block fs-4 mb-1"></i> Đổi Ngày / Ca học
                                    </div>
                                    <div className={`border rounded-3 p-2 flex-grow-1 text-center cursor-pointer transition-all ${requestType === 'room' ? 'bg-success-subtle border-success text-success fw-bold shadow-sm' : 'bg-light text-muted border-light hover-bg-gray'}`}
                                         onClick={() => setRequestType('room')}>
                                        <i className="bi bi-door-open-fill d-block fs-4 mb-1"></i> Đổi Phòng học
                                    </div>
                                </div>
                            </div>

                            {/* THÔNG TIN CHI TIẾT */}
                            <div className="p-3 bg-light rounded-4 mb-4 border">
                                {requestType === 'time' ? (
                                    <div className="row g-3 animate__animated animate__fadeIn">
                                        <div className="col-6">
                                            <label className="form-label small fw-bold text-muted mb-1">Chuyển sang Ngày mới:</label>
                                            <select className="form-select border-0 py-2 shadow-sm fw-medium" value={formData.newDay} onChange={e => setFormData({...formData, newDay: e.target.value})}>
                                                {days.map(d => <option key={d} value={d}>{d}</option>)}
                                            </select>
                                        </div>
                                        <div className="col-6">
                                            <label className="form-label small fw-bold text-muted mb-1">Chuyển sang Ca mới:</label>
                                            <select className="form-select border-0 py-2 shadow-sm fw-medium" value={formData.newSlot} onChange={e => setFormData({...formData, newSlot: e.target.value})}>
                                                {timeSlots.map(s => <option key={s} value={s}>{s}</option>)}
                                            </select>
                                        </div>
                                    </div>
                                ) : (
                                    <div className="animate__animated animate__fadeIn">
                                        <label className="form-label small fw-bold text-muted mb-1">Đề xuất chuyển sang Phòng mới:</label>
                                        <select className="form-select border-0 py-2 shadow-sm fw-medium" value={formData.newRoom} onChange={e => setFormData({...formData, newRoom: e.target.value})}>
                                            <option value="" disabled>-- Hãy chọn phòng trống --</option>
                                            {mockRooms.map(room => (
                                                <option key={room} value={room}>{room}</option>
                                            ))}
                                        </select>
                                    </div>
                                )}
                            </div>

                            <div className="mb-4">
                                <label className="form-label small fw-bold text-dark mb-2">3. Lý do đề xuất <span className="text-danger">*</span></label>
                                <select className="form-select bg-light border-0 py-2 shadow-sm mb-2 fw-medium" 
                                        value={formData.reason} 
                                        onChange={e => setFormData({...formData, reason: e.target.value})}>
                                    {reasons.map((r, i) => <option key={i} value={r}>{r}</option>)}
                                </select>
                                {formData.reason === 'Lý do khác...' && (
                                    <textarea className="form-control bg-light border-0 p-3 shadow-sm custom-scrollbar animate__animated animate__fadeIn" rows="2" 
                                              placeholder="Vui lòng nhập rõ lý do chi tiết..."
                                              onChange={e => setFormData({...formData, reason: e.target.value})}></textarea>
                                )}
                            </div>

                            <div className="d-flex gap-2 justify-content-end pt-3 border-top">
                                <button type="button" className="btn btn-light rounded-pill px-4 fw-bold text-muted border hover-bg-gray" onClick={() => setShowModal(false)}>Hủy thao tác</button>
                                <button type="submit" className="btn btn-primary rounded-pill px-4 fw-bold shadow-sm hover-scale">
                                    <i className="bi bi-send-fill me-1"></i> Gửi yêu cầu
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            <style>{`
                .cursor-pointer { cursor: pointer; }
                .transition-all { transition: all 0.2s ease-in-out; }
                .hover-scale:hover { transform: scale(1.05); transition: 0.2s; }
                .hover-bg-gray:hover { background-color: #e9ecef !important; }
                .modal-backdrop-custom { position: fixed; top: 0; left: 0; width: 100vw; height: 100vh; background: rgba(0,0,0,0.5); z-index: 1040; backdrop-filter: blur(4px); }
                .custom-scrollbar::-webkit-scrollbar { width: 6px; }
                .custom-scrollbar::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 10px; }
            `}</style>
        </div>
    );
};

export default TeacherSchedule;