import React from "react";
import StudentClasses from "../../components/StudentClasses";

const StudentDashboard = () => {
    return (
        <div className="p-4 animate__animated animate__fadeIn" style={{ fontFamily: "'Montserrat', sans-serif" }}>
            
            <div className="mb-4">
                <h2 className="fw-bold text-dark mb-2">Lớp học của bạn</h2>
                <p className="text-muted mb-0">Quản lý và xem lịch học của các lớp đang học</p>
            </div>

            <StudentClasses />
        </div>
    );
};

export default StudentDashboard;
