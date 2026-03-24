import React, { useState } from "react";

/**
 * Chức năng: Hiển thị và quản lý tình trạng điểm danh các lớp học cho admin
 */
const AdminAttendance = () => {
    // 1. Dữ liệu giả
    const [attendances, setAttendances] = useState([
        {
            id: 1,
            classCode: "IELTS-SPK",
            courseName: "IELTS Speaking: Part 3",
            teacher: "Steven Dang",
            date: "24/03/2026",
            time: "07:55 - 10:35",
            present: 28,
            total: 30,
            status: "Đã điểm danh",
        },
        {
            id: 2,
            classCode: "HNI-PRI4-006",
            courseName: "IELTS Reading: Multiple Choice",
            teacher: "Nguyễn Thị Lan Anh",
            date: "24/03/2026",
            time: "07:00 - 09:40",
            present: 20,
            total: 30,
            status: "Đã điểm danh",
        },
        {
            id: 3,
            classCode: "TEST-002",
            courseName: "IELTS Mock Test: Listening",
            teacher: "Ban Khảo Thí",
            date: "24/03/2026",
            time: "13:30 - 15:10",
            present: 0,
            total: 40,
            status: "Chưa điểm danh",
        },
    ]);

    // State lọc dữ liệu
    const [searchTerm, setSearchTerm] = useState("");
    const [filterStatus, setFilterStatus] = useState("Tất cả trạng thái");

    // 2. Hàm xử lý logic
    const statusColor = (status) => {
        if (status === "Đã điểm danh") return "bg-success";
        if (status === "Chưa điểm danh") return "bg-warning text-dark";
        return "bg-secondary";
    };

    // Lọc dữ liệu trước khi hiển thị
    const filteredAttendances = attendances.filter((item) => {
        const matchSearch =
            item.classCode.toLowerCase().includes(searchTerm.toLowerCase()) ||
            item.courseName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            item.teacher.toLowerCase().includes(searchTerm.toLowerCase());
        const matchStatus =
            filterStatus === "Tất cả trạng thái" || item.status === filterStatus;
        return matchSearch && matchStatus;
    });

    // 3. Render
    return (
        <div className="p-4 animate__animated animate__fadeIn">
            {/* Header */}
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h3 className="fw-bold text-dark">
                    <i className="bi bi-calendar-check me-2"></i>
                    Quản lý điểm danh
                </h3>
                <button className="btn btn-outline-primary fw-bold shadow-sm">
                    <i className="bi bi-download me-2"></i> Xuất báo cáo
                </button>
            </div>

            {/* Filter */}
            <div className="card mb-3 shadow-sm border-0 rounded-4">
                <div className="card-body">
                    <div className="row g-2">
                        <div className="col-md-8">
                            <input
                                className="form-control bg-light border-0 py-2 shadow-none"
                                placeholder="Tìm kiếm theo mã lớp, tên khóa học, giáo viên..."
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                        <div className="col-md-4">
                            <select
                                className="form-select bg-light border-0 py-2 shadow-none"
                                value={filterStatus}
                                onChange={(e) => setFilterStatus(e.target.value)}
                            >
                                <option>Tất cả trạng thái</option>
                                <option>Đã điểm danh</option>
                                <option>Chưa điểm danh</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>

            {/* Table */}
            <div className="card shadow-sm border-0 rounded-4">
                <div className="card-body p-0">
                    <table className="table align-middle table-hover mb-0">
                        <thead className="table-light text-muted small">
                            <tr>
                                <th className="ps-4 border-0">Lớp học / Khóa học</th>
                                <th className="border-0">Giáo viên</th>
                                <th className="border-0">Thời gian học</th>
                                <th className="border-0">Tình trạng (Hiện diện)</th>
                                <th className="border-0">Trạng thái</th>
                                <th className="pe-4 border-0 text-end">Thao tác</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredAttendances.length === 0 ? (
                                <tr>
                                    <td colSpan="6" className="text-center py-5 text-muted">
                                        <i className="bi bi-search fs-1 d-block mb-2"></i>
                                        Không tìm thấy dữ liệu điểm danh nào.
                                    </td>
                                </tr>
                            ) : (
                                filteredAttendances.map((item) => (
                                    <tr key={item.id}>
                                        <td className="ps-4 py-3">
                                            <div className="fw-bold text-primary">
                                                {item.classCode}
                                            </div>
                                            <small className="text-muted fw-medium">
                                                {item.courseName}
                                            </small>
                                        </td>
                                        <td>
                                            <span className="fw-bold text-secondary">
                                                <i className="bi bi-person-fill text-muted me-1"></i>
                                                {item.teacher}
                                            </span>
                                        </td>
                                        <td>
                                            <div className="small fw-bold text-dark">
                                                <i className="bi bi-calendar-event me-1"></i>{" "}
                                                {item.date}
                                            </div>
                                            <small className="text-muted">
                                                <i className="bi bi-clock me-1"></i> {item.time}
                                            </small>
                                        </td>
                                        <td>
                                            <div className="d-flex align-items-center">
                                                <span
                                                    className={`fw-bold fs-6 me-1 ${item.present / item.total < 0.5 ? "text-danger" : "text-success"}`}
                                                >
                                                    {item.present}
                                                </span>
                                                <span className="text-muted fw-medium">
                                                    {" "}
                                                    / {item.total}
                                                </span>
                                            </div>
                                            <div
                                                className="progress mt-1"
                                                style={{ height: "5px", width: "80%" }}
                                            >
                                                <div
                                                    className={`progress-bar ${item.present / item.total < 0.5 ? "bg-danger" : "bg-success"}`}
                                                    role="progressbar"
                                                    style={{
                                                        width: `${item.total > 0 ? (item.present / item.total) * 100 : 0}%`,
                                                    }}
                                                ></div>
                                            </div>
                                        </td>
                                        <td>
                                            <span
                                                className={`badge ${statusColor(item.status)} rounded-pill px-3 py-2 fw-medium shadow-sm`}
                                            >
                                                {item.status}
                                            </span>
                                        </td>
                                        <td className="pe-4 text-end">
                                            <button
                                                className="btn btn-light border btn-sm rounded-circle shadow-sm"
                                                title="Xem chi tiết"
                                            >
                                                <i className="bi bi-eye-fill text-primary"></i>
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default AdminAttendance;
