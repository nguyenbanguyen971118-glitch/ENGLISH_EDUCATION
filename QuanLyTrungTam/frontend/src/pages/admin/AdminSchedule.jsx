import React, { useState, useEffect } from 'react';

const AdminSchedule = () => {
    // ==========================================
    // 1. STATE ĐIỀU HƯỚNG & DANH MỤC
    // ==========================================
    const [currentDate, setCurrentDate] = useState(new Date("2026-02-23T00:00:00"));
    const [viewFilter, setViewFilter] = useState('all');
    const [searchTerm, setSearchTerm] = useState('');
    const [weekDays, setWeekDays] = useState([]);
    
    // Toast Notification State
    const [toast, setToast] = useState({ show: false, msg: '', type: 'success' });
    const showToast = (msg, type = 'success') => {
        setToast({ show: true, msg, type });
        setTimeout(() => setToast({ show: false, msg: '', type: '' }), 3000);
    };

    const timeSlots = [
        { id: 1, name: 'Ca 1', time: '07:30 - 09:30' },
        { id: 2, name: 'Ca 2', time: '09:45 - 11:45' },
        { id: 3, name: 'Ca 3', time: '13:30 - 15:30' },
        { id: 4, name: 'Ca 4', time: '15:45 - 17:45' },
        { id: 5, name: 'Ca 5', time: '18:00 - 20:00' },
        { id: 6, name: 'Ca 6', time: '20:15 - 22:15' }
    ];

    const classesList = ['HNI-PRI4-0065', 'IELTS-102', 'TOEIC-500', 'KIDS-STARTER'];
    const teachersList = ['Lan Anh', 'Steven Dang', 'Thu Hà', 'Minh Tuấn'];
    const roomsList = ['P.101', 'P.202', 'P.302', 'P.405', 'Lab 1'];

    // ==========================================
    // 2. STATE DỮ LIỆU & KÉO THẢ
    // ==========================================
    const [schedules, setSchedules] = useState([
        { id: 1, classCode: 'HNI-PRI4-0065', teacher: 'Lan Anh', room: 'P.302', dayIdx: 0, slotId: 5, isConflict: false },
        { id: 2, classCode: 'IELTS-102', teacher: 'Steven Dang', room: 'P.405', dayIdx: 1, slotId: 6, isConflict: false },
        { id: 3, classCode: 'TOEIC-500', teacher: 'Thu Hà', room: 'P.101', dayIdx: 3, slotId: 5, isConflict: true, conflictReason: 'Trùng lịch Giảng viên (Đang dạy lớp khác)' }
    ]);

    const [rescheduleRequests, setRescheduleRequests] = useState([
        { id: 1, teacher: 'Lan Anh', classCode: 'HNI-PRI4-0065', oldDate: '26/02 - Ca 5', newDate: '27/02 - Ca 5', reason: 'Có việc gia đình đột xuất', status: 'pending' },
        { id: 2, teacher: 'Steven Dang', classCode: 'IELTS-102', oldDate: '24/02 - Ca 6', newDate: '25/02 - Ca 6', reason: 'Trùng lịch họp chuyên môn', status: 'pending' }
    ]);

    const [showModal, setShowModal] = useState(false);
    const [modalMode, setModalMode] = useState('add'); 
    const [formData, setFormData] = useState({ id: null, classCode: classesList[0], teacher: teachersList[0], room: roomsList[0], dayIdx: 0, slotId: 1 });
    const [errorMsg, setErrorMsg] = useState('');
    const [draggedItem, setDraggedItem] = useState(null);

    // ==========================================
    // 3. LOGIC XỬ LÝ
    // ==========================================
    useEffect(() => {
        const getDatesOfWeek = (dateInWeek) => {
            const date = new Date(dateInWeek);
            const dayOfWeek = date.getDay(); 
            const diff = date.getDate() - dayOfWeek + (dayOfWeek === 0 ? -6 : 1);
            const monday = new Date(date.setDate(diff));

            const days = [];
            const dayNames = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'CN'];
            for (let i = 0; i < 7; i++) {
                const currentDay = new Date(monday);
                currentDay.setDate(monday.getDate() + i);
                const dd = String(currentDay.getDate()).padStart(2, '0');
                const mm = String(currentDay.getMonth() + 1).padStart(2, '0');
                days.push(`${dayNames[i]} (${dd}/${mm})`);
            }
            return days;
        };
        setWeekDays(getDatesOfWeek(currentDate));
    }, [currentDate]);

    const changeWeek = (amount) => {
        const newDate = new Date(currentDate);
        newDate.setDate(currentDate.getDate() + (amount * 7));
        setCurrentDate(newDate);
    };

    const openAddModal = (dayIdx, slotId) => {
        setModalMode('add');
        setFormData({ id: Date.now(), classCode: classesList[0], teacher: teachersList[0], room: roomsList[0], dayIdx, slotId });
        setErrorMsg('');
        setShowModal(true);
    };

    const openEditModal = (schedule) => {
        setModalMode('edit');
        setFormData({ ...schedule });
        setErrorMsg('');
        setShowModal(true);
    };

    const handleSaveSchedule = () => {
        const dayIdxInt = parseInt(formData.dayIdx);
        const slotIdInt = parseInt(formData.slotId);

        const isSlotOccupied = schedules.find(s => s.id !== formData.id && s.dayIdx === dayIdxInt && s.slotId === slotIdInt && s.room === formData.room);
        const isTeacherBusy = schedules.find(s => s.id !== formData.id && s.dayIdx === dayIdxInt && s.slotId === slotIdInt && s.teacher === formData.teacher);

        if (isTeacherBusy) { setErrorMsg(`⚠️ Lỗi: Giảng viên ${formData.teacher} đang kẹt dạy lớp ${isTeacherBusy.classCode} ở ca này!`); return; }
        if (isSlotOccupied) { setErrorMsg(`⚠️ Lỗi: Phòng ${formData.room} đang được lớp ${isSlotOccupied.classCode} sử dụng ở ca này!`); return; }

        const newSchedule = { ...formData, dayIdx: dayIdxInt, slotId: slotIdInt, isConflict: false };

        if (modalMode === 'add') {
            setSchedules([...schedules, newSchedule]);
            showToast('Đã thêm lịch dạy mới thành công!');
        } else {
            setSchedules(schedules.map(s => s.id === formData.id ? newSchedule : s));
            showToast('Đã cập nhật thông tin lịch dạy!');
        }
        setShowModal(false);
    };

    const handleDeleteSchedule = () => {
        if(window.confirm("Bạn có chắc chắn muốn xóa lịch dạy này?")) {
            setSchedules(schedules.filter(s => s.id !== formData.id));
            setShowModal(false);
            showToast('Đã xóa lịch dạy!', 'danger');
        }
    };

    // --- HÀM DUYỆT YÊU CẦU ĐỔI LỊCH ---
    const handleApproveReq = (reqId) => {
        if(window.confirm("Xác nhận duyệt đổi lịch? Lịch mới sẽ được tự động cập nhật lên hệ thống.")) {
            setRescheduleRequests(rescheduleRequests.filter(r => r.id !== reqId));
            showToast('Đã duyệt yêu cầu đổi lịch thành công!', 'success');
        }
    };

    // --- HÀM TỪ CHỐI YÊU CẦU ĐỔI LỊCH ---
    const handleRejectReq = (reqId) => {
        if(window.confirm("Bạn chắc chắn muốn từ chối yêu cầu đổi lịch này?")) {
            setRescheduleRequests(rescheduleRequests.filter(r => r.id !== reqId));
            showToast('Đã từ chối yêu cầu đổi lịch.', 'danger');
        }
    };

    // --- DRAG & DROP ---
    const handleDragStart = (e, scheduleItem) => {
        setDraggedItem(scheduleItem);
        e.dataTransfer.effectAllowed = 'move';
        setTimeout(() => { e.target.classList.add('dragging-ghost'); }, 0);
    };

    const handleDragEnd = (e) => {
        e.target.classList.remove('dragging-ghost');
        setDraggedItem(null);
    };

    const handleDragOver = (e) => {
        e.preventDefault(); 
        e.dataTransfer.dropEffect = 'move';
    };

    const handleDrop = (e, targetDayIdx, targetSlotId) => {
        e.preventDefault();
        if (!draggedItem) return;

        if (draggedItem.dayIdx === targetDayIdx && draggedItem.slotId === targetSlotId) return;

        const occupyingClass = schedules.find(s => s.dayIdx === targetDayIdx && s.slotId === targetSlotId);
        if (occupyingClass) { showToast(`Lỗi: Ô này đang có lớp ${occupyingClass.classCode} học.`, 'danger'); return; }

        const isRoomBusy = schedules.find(s => s.id !== draggedItem.id && s.dayIdx === targetDayIdx && s.slotId === targetSlotId && s.room === draggedItem.room);
        if (isRoomBusy) { showToast(`Lỗi: Phòng ${draggedItem.room} đã được sử dụng!`, 'danger'); return; }

        const isTeacherBusy = schedules.find(s => s.id !== draggedItem.id && s.dayIdx === targetDayIdx && s.slotId === targetSlotId && s.teacher === draggedItem.teacher);
        if (isTeacherBusy) { showToast(`Lỗi: Giảng viên ${draggedItem.teacher} bị kẹt dạy lớp khác!`, 'danger'); return; }

        setSchedules(prev => prev.map(s => s.id === draggedItem.id ? { ...s, dayIdx: targetDayIdx, slotId: targetSlotId } : s));
        showToast('Đã di chuyển lịch thành công!');
    };

    // ==========================================
    // 4. RENDER UI
    // ==========================================
    const renderTimeSlot = (dayIdx, slotId) => {
        const classInSlot = schedules.find(c => c.dayIdx === dayIdx && c.slotId === slotId);
        
        if (!classInSlot) return (
            <div onClick={() => openAddModal(dayIdx, slotId)} onDragOver={handleDragOver} onDrop={(e) => handleDrop(e, dayIdx, slotId)}
                 className={`h-100 w-100 p-2 text-muted text-center slot-empty d-flex align-items-center justify-content-center transition-all rounded-3 ${draggedItem ? 'bg-light border-primary border-opacity-50' : ''}`} 
                 style={{ minHeight: '85px', border: '1px dashed #dee2e6', cursor: 'pointer' }}>
                <i className="bi bi-plus-lg opacity-25 fs-5"></i>
            </div>
        );

        const isMatchingSearch = searchTerm === '' || classInSlot.classCode.toLowerCase().includes(searchTerm.toLowerCase()) || classInSlot.teacher.toLowerCase().includes(searchTerm.toLowerCase());

        return (
            <div draggable onDragStart={(e) => handleDragStart(e, classInSlot)} onDragEnd={handleDragEnd} onClick={() => openEditModal(classInSlot)}
                 title="Nhấn giữ và kéo để chuyển lịch. Click để sửa."
                 className={`p-2 rounded-3 shadow-sm h-100 position-relative border-start border-4 slot-filled transition-all 
                 ${classInSlot.isConflict ? 'bg-danger-subtle border-danger' : 'bg-primary-subtle border-primary'}
                 ${!isMatchingSearch ? 'opacity-25 grayscale' : ''}`} 
                 style={{ minHeight: '85px', cursor: 'grab' }}>
                
                {classInSlot.isConflict && (
                    <div className="position-absolute top-0 end-0 bg-danger text-white rounded-circle d-flex align-items-center justify-content-center mt-1 me-1 shadow-sm" style={{ width: '20px', height: '20px' }} title={classInSlot.conflictReason}>
                        <i className="bi bi-exclamation-triangle-fill" style={{ fontSize: '11px' }}></i>
                    </div>
                )}
                
                <div className="fw-bold text-dark mb-1 text-truncate pe-3" style={{ fontSize: '13px' }}>{classInSlot.classCode}</div>
                <div className="d-flex justify-content-between align-items-center mt-2">
                    <span className="small text-secondary fw-medium text-truncate" style={{ fontSize: '11px', maxWidth: '65%' }}>
                        <i className="bi bi-person-fill me-1 text-primary"></i>{classInSlot.teacher}
                    </span>
                    <span className="badge bg-white text-dark shadow-sm border border-light px-2 py-1" style={{ fontSize: '10px' }}>{classInSlot.room}</span>
                </div>
            </div>
        );
    };

    if (weekDays.length === 0) return null;

    return (
        <div className="p-4 animate__animated animate__fadeIn h-100 d-flex flex-column position-relative">
            
            {/* THÔNG BÁO NỔI (TOAST) GÓC DƯỚI */}
            {toast.show && (
                <div className={`position-fixed bottom-0 end-0 m-4 p-3 rounded-4 shadow-lg text-white bg-${toast.type} animate__animated animate__fadeInUp`} style={{ zIndex: 1100, minWidth: '280px', border: '1px solid rgba(255,255,255,0.2)' }}>
                    <div className="d-flex align-items-center fw-bold">
                        <i className={`bi ${toast.type === 'danger' ? 'bi-exclamation-triangle-fill fs-4' : 'bi-check-circle-fill fs-5'} me-3`}></i>
                        {toast.msg}
                    </div>
                </div>
            )}

            {/* HEADER & TÌM KIẾM */}
            <div className="d-flex justify-content-between align-items-center mb-4 bg-white p-3 rounded-4 shadow-sm border">
                <div>
                    <h3 className="fw-bold text-dark mb-1 text-uppercase" style={{ letterSpacing: '-0.5px' }}>Điều phối Lịch dạy & Học</h3>
                    <p className="text-muted small mb-0">Quản lý kéo thả lịch học, phòng học và xử lý xung đột.</p>
                </div>
                
                <div className="d-flex gap-3 align-items-center">
                    <div className="input-group shadow-sm rounded-pill overflow-hidden border border-primary border-opacity-25" style={{ width: '280px' }}>
                        <span className="input-group-text bg-white border-0 px-3 text-primary"><i className="bi bi-search"></i></span>
                        <input type="text" className="form-control bg-white border-0 shadow-none fw-medium text-dark" placeholder="Tìm Giảng viên, Mã lớp..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                        {searchTerm && <button className="btn btn-white border-0 text-muted" onClick={() => setSearchTerm('')}><i className="bi bi-x-circle-fill"></i></button>}
                    </div>

                    <select className="form-select rounded-pill shadow-sm bg-light border-0 fw-medium" style={{ width: '150px' }} value={viewFilter} onChange={(e) => setViewFilter(e.target.value)}>
                        <option value="all">Tất cả lịch</option>
                    </select>

                    <button className="btn btn-primary rounded-pill px-4 py-2 fw-bold shadow-sm d-flex align-items-center hover-scale" onClick={() => openAddModal(0, 1)}>
                        <i className="bi bi-plus-lg me-2"></i> Xếp lịch mới
                    </button>
                </div>
            </div>

            <div className="row g-4 flex-grow-1">
                {/* LƯỚI LỊCH (GRID) */}
                <div className="col-lg-9 d-flex flex-column">
                    <div className="card border-0 shadow-sm rounded-4 p-0 flex-grow-1 bg-white d-flex flex-column overflow-hidden">
                        <div className="d-flex justify-content-between align-items-center p-3 border-bottom bg-light">
                            <h5 className="fw-bold mb-0 text-dark">
                                <i className="bi bi-calendar-week text-primary me-2"></i>Lịch trình hệ thống
                                {draggedItem && <span className="badge bg-warning text-dark ms-3 fs-6 animate__animated animate__pulse animate__infinite">Đang kéo: {draggedItem.classCode}</span>}
                            </h5>
                            <div className="bg-white border rounded-pill p-1 shadow-sm d-flex align-items-center">
                                <button className="btn btn-primary rounded-pill px-4 fw-bold shadow-sm" onClick={() => setCurrentDate(new Date("2026-02-23T00:00:00"))} style={{ fontSize: '13px' }}>Hiện tại</button>
                                <div className="d-flex align-items-center px-2">
                                    <button className="btn btn-link text-dark shadow-none hover-scale" onClick={() => changeWeek(-1)}><i className="bi bi-chevron-left fw-bold"></i></button>
                                    <div className="vr mx-1 opacity-25" style={{ height: '20px' }}></div>
                                    <button className="btn btn-link text-dark shadow-none hover-scale" onClick={() => changeWeek(1)}><i className="bi bi-chevron-right fw-bold"></i></button>
                                </div>
                            </div>
                        </div>

                        <div className="table-responsive flex-grow-1 custom-scrollbar">
                            <table className="table table-bordered mb-0 align-middle text-center" style={{ minWidth: '900px', tableLayout: 'fixed' }}>
                                <thead className="bg-light sticky-top shadow-sm" style={{ zIndex: 10 }}>
                                    <tr className="text-muted small text-uppercase">
                                        <th style={{ width: '10%' }} className="border-0 bg-white py-3 shadow-sm">Ca / Giờ</th>
                                        {weekDays.map((day, idx) => <th key={idx} style={{ width: '12.8%' }} className="border-0 bg-white py-3 shadow-sm fw-bold text-dark">{day}</th>)}
                                    </tr>
                                </thead>
                                <tbody>
                                    {timeSlots.map(slot => (
                                        <tr key={slot.id}>
                                            <td className="bg-light border-end py-3">
                                                <div className="fw-bold text-dark">{slot.name}</div>
                                                <div className="text-muted" style={{ fontSize: '11px' }}>{slot.time}</div>
                                            </td>
                                            {weekDays.map((_, dayIdx) => (
                                                <td key={dayIdx} 
                                                    className={`p-2 align-top bg-white hover-bg-light ${draggedItem ? 'border-dashed-hover' : ''}`}
                                                    onDragOver={handleDragOver}
                                                    onDrop={(e) => handleDrop(e, dayIdx, slot.id)}>
                                                    {renderTimeSlot(dayIdx, slot.id)}
                                                </td>
                                            ))}
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                {/* SIDEBAR BÊN PHẢI */}
                <div className="col-lg-3 d-flex flex-column gap-4">
                    
                    {/* ===== KHU VỰC DUYỆT YÊU CẦU ĐỔI LỊCH (CÓ NÚT) ===== */}
                    <div className="card border-0 shadow-sm rounded-4 bg-white d-flex flex-column h-50">
                        <div className="p-3 border-bottom d-flex justify-content-between align-items-center bg-light rounded-top-4">
                            <h6 className="fw-bold mb-0 text-dark"><i className="bi bi-envelope-paper-fill text-warning me-2"></i>Duyệt đổi lịch</h6>
                            <span className="badge bg-danger rounded-pill px-2 shadow-sm">{rescheduleRequests.length}</span>
                        </div>
                        <div className="overflow-y-auto custom-scrollbar p-3 flex-grow-1">
                            {rescheduleRequests.map(req => (
                                <div key={req.id} className="bg-white rounded-4 p-3 mb-3 border shadow-sm position-relative hover-up">
                                    <div className="fw-bold text-primary mb-1" style={{ fontSize: '14px' }}>{req.classCode}</div>
                                    <div className="small text-dark fw-bold mb-2"><i className="bi bi-person text-muted me-1"></i>{req.teacher}</div>
                                    <div className="d-flex align-items-center justify-content-center gap-2 mb-2 bg-light p-2 rounded-3 border" style={{ fontSize: '11px' }}>
                                        <div className="text-decoration-line-through text-muted">{req.oldDate.split(' - ')[0]}<br/>{req.oldDate.split(' - ')[1]}</div>
                                        <i className="bi bi-arrow-right text-primary fs-6"></i>
                                        <div className="fw-bold text-success">{req.newDate.split(' - ')[0]}<br/>{req.newDate.split(' - ')[1]}</div>
                                    </div>
                                    <div className="text-muted mb-3 text-center" style={{ fontSize: '11px', fontStyle: 'italic' }}>"{req.reason}"</div>
                                    
                                    {/* CÁC NÚT TƯƠNG TÁC (Nút này sẽ kích hoạt hàm showToast) */}
                                    <div className="d-flex gap-2 mt-auto">
                                        <button className="btn btn-success btn-sm flex-grow-1 rounded-pill fw-bold shadow-sm" onClick={() => handleApproveReq(req.id)} style={{ fontSize: '12px' }}>
                                            <i className="bi bi-check-lg me-1"></i> Duyệt
                                        </button>
                                        <button className="btn btn-outline-danger btn-sm flex-grow-1 rounded-pill fw-bold bg-white" onClick={() => handleRejectReq(req.id)} style={{ fontSize: '12px' }}>
                                            Từ chối
                                        </button>
                                    </div>
                                </div>
                            ))}
                            {rescheduleRequests.length === 0 && <div className="text-center small text-muted mt-5"><i className="bi bi-check2-circle text-success fs-1 d-block mb-2"></i>Đã xử lý hết yêu cầu</div>}
                        </div>
                    </div>

                    {/* Chú thích */}
                    <div className="card border-0 shadow-sm rounded-4 p-4 bg-dark text-white h-50">
                        <h6 className="fw-bold mb-4 text-warning"><i className="bi bi-info-circle-fill me-2"></i>Chú thích hệ thống</h6>
                        <div className="d-flex align-items-center mb-3">
                            <div className="bg-primary text-white rounded-3 me-3 d-flex justify-content-center align-items-center shadow-sm" style={{ width: '28px', height: '28px' }}><i className="bi bi-check2"></i></div>
                            <span className="small fw-medium">Lịch học bình thường</span>
                        </div>
                        <div className="d-flex align-items-center mb-3">
                            <div className="bg-danger text-white rounded-3 me-3 d-flex justify-content-center align-items-center shadow-sm" style={{ width: '28px', height: '28px' }}>
                                <i className="bi bi-exclamation-triangle-fill"></i>
                            </div>
                            <span className="small text-danger fw-bold">Xung đột lịch (Lỗi)</span>
                        </div>
                        <div className="d-flex align-items-center mb-4">
                            <div className="bg-light border border-dashed rounded-3 me-3 d-flex justify-content-center align-items-center text-muted" style={{ width: '28px', height: '28px' }}><i className="bi bi-plus"></i></div>
                            <span className="small fw-medium opacity-75">Kéo thả để dời lịch</span>
                        </div>
                    </div>
                </div>
            </div>

            {/* MODAL THÊM/SỬA VỚI BÁO LỖI ĐỎ */}
            {showModal && (
                <div className="modal-backdrop-custom d-flex align-items-center justify-content-center animate__animated animate__fadeIn">
                    <div className="bg-white rounded-4 shadow-lg overflow-hidden" style={{ width: '450px', zIndex: 1050 }}>
                        <div className={`p-3 px-4 d-flex justify-content-between align-items-center ${modalMode === 'add' ? 'bg-primary text-white' : 'bg-warning text-dark'}`}>
                            <h5 className="fw-bold mb-0">{modalMode === 'add' ? <><i className="bi bi-calendar-plus me-2"></i>Thêm Lịch Mới</> : <><i className="bi bi-pencil-square me-2"></i>Chỉnh Sửa Lịch</>}</h5>
                            <button className={`btn-close ${modalMode === 'add' ? 'btn-close-white' : ''}`} onClick={() => setShowModal(false)}></button>
                        </div>

                        <div className="p-4">
                            {errorMsg && (
                                <div className="alert alert-danger py-2 px-3 small fw-bold d-flex align-items-center rounded-3 border-danger border-start border-4 shadow-sm mb-4 animate__animated animate__headShake">
                                    {errorMsg}
                                </div>
                            )}

                            <div className="mb-3">
                                <label className="form-label small fw-bold text-dark mb-1">Lớp học</label>
                                <select className="form-select bg-light border-0 py-2 shadow-none fw-medium" value={formData.classCode} onChange={e => setFormData({...formData, classCode: e.target.value})}>
                                    {classesList.map(c => <option key={c} value={c}>{c}</option>)}
                                </select>
                            </div>
                            <div className="mb-3">
                                <label className="form-label small fw-bold text-dark mb-1">Giảng viên phụ trách</label>
                                <select className="form-select bg-light border-0 py-2 shadow-none fw-medium" value={formData.teacher} onChange={e => setFormData({...formData, teacher: e.target.value})}>
                                    {teachersList.map(t => <option key={t} value={t}>{t}</option>)}
                                </select>
                            </div>
                            <div className="row mb-4">
                                <div className="col-5">
                                    <label className="form-label small fw-bold text-dark mb-1">Phòng học</label>
                                    <select className="form-select bg-light border-0 py-2 shadow-none fw-medium" value={formData.room} onChange={e => setFormData({...formData, room: e.target.value})}>
                                        {roomsList.map(r => <option key={r} value={r}>{r}</option>)}
                                    </select>
                                </div>
                                <div className="col-7">
                                    <label className="form-label small fw-bold text-dark mb-1">Ngày / Ca học</label>
                                    <div className="input-group shadow-sm rounded-3 overflow-hidden">
                                        <select className="form-select bg-light border-0 py-2 fw-medium border-end" value={formData.dayIdx} onChange={e => setFormData({...formData, dayIdx: e.target.value})}>
                                            {weekDays.map((d, i) => <option key={i} value={i}>{d.split(' (')[0]}</option>)}
                                        </select>
                                        <select className="form-select bg-light border-0 py-2 fw-medium" value={formData.slotId} onChange={e => setFormData({...formData, slotId: e.target.value})}>
                                            {timeSlots.map(s => <option key={s.id} value={s.id}>C{s.id}</option>)}
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div className="d-flex justify-content-between pt-3 border-top mt-2">
                                {modalMode === 'edit' ? (
                                    <button className="btn btn-outline-danger rounded-pill px-4 fw-bold d-flex align-items-center hover-scale" onClick={handleDeleteSchedule}>
                                        <i className="bi bi-trash-fill me-1"></i> Xóa
                                    </button>
                                ) : <div></div>}
                                
                                <div className="d-flex gap-2">
                                    <button className="btn btn-light rounded-pill px-4 fw-bold text-muted border hover-bg-gray" onClick={() => setShowModal(false)}>Hủy</button>
                                    <button className="btn btn-primary rounded-pill px-4 fw-bold shadow-sm hover-scale" onClick={handleSaveSchedule}>
                                        <i className="bi bi-save2-fill me-1"></i> Lưu lịch
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            <style>{`
                .custom-scrollbar::-webkit-scrollbar { width: 6px; height: 6px; }
                .custom-scrollbar::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 10px; }
                .custom-scrollbar::-webkit-scrollbar-track { background: transparent; }
                
                .transition-all { transition: all 0.2s ease-in-out; }
                .hover-scale:hover { transform: scale(1.05); transition: 0.2s; }
                .hover-up:hover { transform: translateY(-3px); box-shadow: 0 8px 15px rgba(0,0,0,0.1) !important; transition: 0.2s; }
                .hover-bg-light:hover { background-color: #f8f9fa !important; }
                .hover-bg-gray:hover { background-color: #e9ecef !important; }
                
                .slot-empty:hover { background-color: #eef2f7 !important; border-color: #0d6efd !important; }
                .slot-empty:hover i { opacity: 1 !important; color: #0d6efd; transform: scale(1.3); transition: 0.2s; }
                .slot-filled:hover { transform: translateY(-3px); box-shadow: 0 6px 12px rgba(13, 110, 253, 0.15) !important; filter: brightness(0.97); }
                
                /* Kéo thả CSS */
                .slot-filled:active { cursor: grabbing !important; transform: scale(0.95); opacity: 0.8; }
                .dragging-ghost { opacity: 0.4; border: 2px dashed #0d6efd !important; }
                .border-dashed-hover:hover { background-color: #e8f4ff !important; border: 2px dashed #0d6efd !important; }
                
                .grayscale { filter: grayscale(100%); opacity: 0.25; pointer-events: none; }
                .modal-backdrop-custom { position: fixed; top: 0; left: 0; width: 100vw; height: 100vh; background: rgba(0,0,0,0.5); z-index: 1040; backdrop-filter: blur(4px); }
                .border-dashed { border-style: dashed !important; border-width: 2px !important; }
            `}</style>
        </div>
    );
};

export default AdminSchedule;