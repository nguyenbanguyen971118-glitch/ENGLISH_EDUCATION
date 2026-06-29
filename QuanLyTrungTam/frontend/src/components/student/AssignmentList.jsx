
import React from 'react';

const AssignmentList = ({ assignments, onStartTest, onViewDetails }) => {
  return (
    <div style={{ backgroundColor: "#F3F4F6", minHeight: "100vh", fontFamily: "'Montserrat', sans-serif", padding: "24px" }}>
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="fw-bold mb-0" style={{ fontSize: "24px", color: "#111827" }}>Xin chào, Nguyễn Văn A! 👋</h1>
          <p className="text-muted mb-0" style={{ fontSize: "14px" }}>Lịch làm bài tập của bạn tuần này</p>
        </div>
        <div className="d-flex gap-3 align-items-center">
          <div style={{ width: "40px", height: "40px", borderRadius: "50%", backgroundColor: "#E5E7EB", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <i className="bi bi-bell" style={{ fontSize: "20px", color: "#4B5563" }}></i>
          </div>
        </div>
      </div>

      {/* Stats Overview */}
      <div className="row mb-4">
        <div className="col-md-3 mb-3">
          <div className="bg-white rounded-4 p-4 shadow-sm">
            <div className="d-flex justify-content-between align-items-center mb-3">
              <span className="text-muted" style={{ fontSize: "12px" }}>Đang mở</span>
              <div style={{ backgroundColor: "#DBEAFE", padding: "6px", borderRadius: "8px" }}>
                <i className="bi bi-file-earmark-text" style={{ color: "#2563EB" }}></i>
              </div>
            </div>
            <h3 className="fw-bold mb-0" style={{ fontSize: "32px", color: "#111827" }}>01</h3>
            <p className="text-success mb-0" style={{ fontSize: "12px" }}>+2 so với tuần trước</p>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="bg-white rounded-4 p-4 shadow-sm">
            <div className="d-flex justify-content-between align-items-center mb-3">
              <span className="text-muted" style={{ fontSize: "12px" }}>Sắp hết hạn</span>
              <div style={{ backgroundColor: "#FFF7ED", padding: "6px", borderRadius: "8px" }}>
                <i className="bi bi-clock-history" style={{ color: "#EA580C" }}></i>
              </div>
            </div>
            <h3 className="fw-bold mb-0" style={{ fontSize: "32px", color: "#111827" }}>01</h3>
            <p className="text-danger mb-0" style={{ fontSize: "12px" }}>+2 so với tuần trước</p>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="bg-white rounded-4 p-4 shadow-sm">
            <div className="d-flex justify-content-between align-items-center mb-3">
              <span className="text-muted" style={{ fontSize: "12px" }}>Đã nộp</span>
              <div style={{ backgroundColor: "#D1FAE5", padding: "6px", borderRadius: "8px" }}>
                <i className="bi bi-check2-circle" style={{ color: "#059669" }}></i>
              </div>
            </div>
            <h3 className="fw-bold mb-0" style={{ fontSize: "32px", color: "#111827" }}>08</h3>
            <p className="text-success mb-0" style={{ fontSize: "12px" }}>+2 so với tuần trước</p>
          </div>
        </div>
      </div>

      {/* All Assignments */}
      <div className="bg-white rounded-4 p-4 shadow-sm mb-4">
        <h2 className="fw-bold mb-4" style={{ fontSize: "18px", color: "#111827" }}>Tất cả bài tập</h2>
        {assignments.map((assignment) => (
          <div key={assignment.id} className="d-flex justify-content-between align-items-center mb-3 p-3 border rounded-3" style={{ borderColor: "#E5E7EB" }}>
            <div className="d-flex gap-3 align-items-center">
              <div style={{ backgroundColor: "#EFF6FF", padding: "10px 14px", borderRadius: "8px" }}>
                <i className="bi bi-file-earmark-text" style={{ fontSize: "20px", color: "#2563EB" }}></i>
              </div>
              <div>
                <h4 className="fw-bold mb-1" style={{ fontSize: "16px", color: "#111827" }}>{assignment.title}</h4>
                <p className="text-muted mb-0" style={{ fontSize: "12px" }}>{assignment.classInfo}</p>
              </div>
            </div>
            <div className="d-flex gap-3 align-items-center">
              <span className="px-3 py-1 rounded-2 fw-semibold" style={{ backgroundColor: assignment.statusColor, color: assignment.statusTextColor, fontSize: "12px" }}>
                {assignment.status}
              </span>
              <div className="d-flex gap-3 align-items-center">
                <div className="d-flex gap-2 align-items-center text-muted" style={{ fontSize: "12px" }}>
                  <i className="bi bi-file-text"></i>
                  <span>{assignment.questions} câu</span>
                </div>
                <div className="d-flex gap-2 align-items-center text-muted" style={{ fontSize: "12px" }}>
                  <i className="bi bi-clock"></i>
                  <span>{assignment.duration}</span>
                </div>
              </div>
              <button className="btn btn-light fw-semibold" style={{ fontSize: "12px" }} onClick={() => onViewDetails(assignment)}>
                Xem chi tiết
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default AssignmentList;

