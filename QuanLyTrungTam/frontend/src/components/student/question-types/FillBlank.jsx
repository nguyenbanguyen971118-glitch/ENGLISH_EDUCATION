
import React from 'react';

const FillBlank = ({ question, currentQuestion, answer, onAnswerChange }) => {
  return (
    <div style={{ padding: "24px" }}>
      <div style={{ marginBottom: "20px" }}>
        <span style={{
          backgroundColor: "#FEF3C7",
          color: "#92400E",
          padding: "6px 12px",
          borderRadius: "6px",
          fontSize: "12px",
          fontWeight: 600
        }}>
          Điền khuyết
        </span>
      </div>

      <h3 style={{
        fontSize: "20px",
        fontWeight: 700,
        color: "#111827",
        marginBottom: "24px"
      }}>
        Câu {currentQuestion + 1}. Điền một từ thích hợp vào chỗ trống.
      </h3>

      <p style={{
        fontSize: "20px",
        color: "#111827",
        lineHeight: "1.6",
        marginBottom: "24px"
      }}>
        {question.question.split("______").map((part, idx, arr) => (
          <React.Fragment key={idx}>
            {part}
            {idx < arr.length - 1 && (
              <input
                type="text"
                value={answer || ""}
                onChange={(e) => onAnswerChange(currentQuestion, e.target.value)}
                style={{
                  width: "120px",
                  padding: "6px 12px",
                  border: "1px solid #D1D5DB",
                  borderRadius: "8px",
                  fontSize: "16px",
                  margin: "0 8px",
                  backgroundColor: "#F9FAFB"
                }}
              />
            )}
          </React.Fragment>
        ))}
      </p>

      <div style={{ marginBottom: "24px" }}>
        <span style={{
          backgroundColor: "#FEE2E2",
          color: "#991B1B",
          padding: "4px 12px",
          borderRadius: "6px",
          fontSize: "12px",
          fontWeight: 600
        }}>
          Giới hạn: 1 từ
        </span>
      </div>

      <p style={{
        fontSize: "14px",
        color: "#6B7280",
        lineHeight: "1.6"
      }}>
        Thao tác: học viên click vào ô trống và nhập đáp án. Nếu nhập quá số từ quy định, hệ thống hiện thị cảnh báo nhưng không xóa nội dung.
      </p>
    </div>
  );
};

export default FillBlank;
