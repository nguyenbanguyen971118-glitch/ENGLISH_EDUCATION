

import React from 'react';

const AssignmentDetails = ({ assignment, onBack, onStartTest }) => {
  return (
    <div style={{ backgroundColor: "#F3F4F6", minHeight: "100vh", fontFamily: "'Montserrat', sans-serif", padding: "24px" }}>
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <button className="btn btn-light fw-semibold d-flex gap-2 align-items-center" onClick={onBack}>
          <i className="bi bi-arrow-left"></i>
          Quay lại
        </button>
        <div className="d-flex gap-3 align-items-center">
          <div style={{ width: "40px", height: "40px", borderRadius: "50%", backgroundColor: "#E5E7EB", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <i className="bi bi-bell" style={{ fontSize: "20px", color: "#4B5563" }}></i>
          </div>
        </div>
      </div>

      <div className="row g-4">
        {/* Left Column - Assignment Details */}
        <div className="col-md-8">
          <div className="bg-white rounded-4 p-5 shadow-sm mb-4">
            <div className="d-flex gap-3 align-items-center mb-4">
              <div style={{ backgroundColor: "#EFF6FF", padding: "12px 16px", borderRadius: "12px" }}>
                <i className="bi bi-file-earmark-text" style={{ fontSize: "24px", color: "#2563EB" }}></i>
              </div>
              <div>
                <h1 className="fw-bold mb-1" style={{ fontSize: "24px", color: "#111827" }}>
                  {assignment.title}
                </h1>
                <p className="text-muted mb-0" style={{ fontSize: "14px" }}>
                  {assignment.classInfo}
                </p>
              </div>
            </div>

            <h2 className="fw-bold mb-3" style={{ fontSize: "18px", color: "#111827" }}>Nội dung bài làm</h2>
            <p style={{ fontSize: "14px", color: "#4B5563", lineHeight: "1.6" }}>
              Bài kiểm tra này bao gồm các câu hỏi trắc nghiệm và điền khuyết nhằm đánh giá kỹ năng đọc hiểu và ngữ pháp tiếng Anh của học sinh. Thời gian làm bài là 60 phút.
            </p>
            <ul style={{ fontSize: "14px", color: "#4B5563", paddingLeft: "20px", lineHeight: "2" }}>
              <li>Số câu hỏi: {assignment.questions} câu</li>
              <li>Thời gian làm bài: {assignment.duration}</li>
              <li>Loại bài: Bài tập giữa kỳ</li>
            </ul>
          </div>
        </div>

        {/* Right Column - Summary */}
        <div className="col-md-4">
          <div className="bg-white rounded-4 p-4 shadow-sm mb-4">
            <h3 className="fw-bold mb-4" style={{ fontSize: "18px", color: "#111827" }}>Tóm tắt</h3>
            <div className="mb-3">
              <p className="text-muted mb-1" style={{ fontSize: "14px" }}>Trạng thái</p>
              <span className="px-3 py-1 rounded-2 fw-semibold" style={{ backgroundColor: assignment.statusColor, color: assignment.statusTextColor, fontSize: "12px" }}>
                {assignment.status}
              </span>
            </div>
            <div className="mb-3">
              <p className="text-muted mb-1" style={{ fontSize: "14px" }}>Hạn nộp</p>
              <p className="fw-semibold mb-0" style={{ fontSize: "14px", color: "#111827" }}>23:59 - 15/12/2024</p>
            </div>
            <div className="mb-3">
              <p className="text-muted mb-1" style={{ fontSize: "14px" }}>Lần làm tối đa</p>
              <p className="fw-semibold mb-0" style={{ fontSize: "14px", color: "#111827" }}>2 lần</p>
            </div>
            <div className="mb-3">
              <p className="text-muted mb-1" style={{ fontSize: "14px" }}>Thời gian làm bài</p>
              <p className="fw-semibold mb-0" style={{ fontSize: "14px", color: "#111827" }}>{assignment.duration}</p>
            </div>

            <button className="btn btn-primary w-100 fw-bold py-3" style={{ borderRadius: "8px" }} onClick={onStartTest}>
              Bắt đầu làm
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AssignmentDetails;

