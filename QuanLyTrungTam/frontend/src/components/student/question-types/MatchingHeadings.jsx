
import React from 'react';

const MatchingHeadings = ({ question, currentQuestion, answers, onAnswerChange, currentSubIndex }) => {
  return (
    <div style={{ padding: "24px" }}>
      <div style={{ marginBottom: "20px" }}>
        <span style={{
          backgroundColor: "#E0F2FE",
          color: "#0369A1",
          padding: "6px 12px",
          borderRadius: "6px",
          fontSize: "12px",
          fontWeight: 600
        }}>
          Nối / Ghép
        </span>
      </div>

      <h3 style={{
        fontSize: "20px",
        fontWeight: 700,
        color: "#111827",
        marginBottom: "24px"
      }}>
        Câu {currentQuestion + 1}. Chọn tiêu đề phù hợp cho từng đoạn.
      </h3>

      <div style={{ display: "flex", gap: "24px" }}>
        <div style={{ flex: 1 }}>
          <div style={{
            backgroundColor: "#F9FAFB",
            borderRadius: "12px",
            padding: "20px",
            marginBottom: "24px"
          }}>
            <p style={{ fontSize: "14px", fontWeight: 600, color: "#374151", marginBottom: "12px" }}>
              i. Using hunting to stop a worse crime
            </p>
            <p style={{ fontSize: "14px", fontWeight: 600, color: "#374151", marginBottom: "12px" }}>
              ii. Legal hunting has little financial benefit
            </p>
            <p style={{ fontSize: "14px", fontWeight: 600, color: "#374151", marginBottom: "12px" }}>
              iii. Trying to make a living
            </p>
            <p style={{ fontSize: "14px", fontWeight: 600, color: "#374151" }}>
              vi. Emotional reactions may have negative consequences
            </p>
          </div>

          {question.paragraphs?.map((paragraph, idx) => {
            const qIndex = currentQuestion - (currentSubIndex || 0) + idx;
            const answer = answers[qIndex];
            return (
              <div key={idx} style={{ marginBottom: "20px", display: "flex", alignItems: "center", gap: "16px" }}>
                <p style={{ fontSize: "16px", fontWeight: 600, color: "#111827", width: "120px", margin: 0 }}>
                  {paragraph.label}
                </p>
                <select
                  value={answer || ""}
                  onChange={(e) => onAnswerChange(qIndex, e.target.value)}
                  style={{
                    flex: 1,
                    padding: "10px 16px",
                    border: "1px solid #D1D5DB",
                    borderRadius: "10px",
                    fontSize: "14px",
                    backgroundColor: "#FFFFFF"
                  }}
                >
                  <option value="">Chọn heading</option>
                  {question.headings?.map((heading, hIdx) => (
                    <option key={hIdx} value={heading}>{heading}</option>
                  ))}
                </select>
              </div>
            );
          })}
        </div>

        <div style={{ flex: 1 }}>
          <p style={{ fontSize: "14px", color: "#6B7280", lineHeight: "1.6" }}>
            Thao tác: học viên chọn heading từ dropdown cho từng đoạn. Heading đã dùng có thể làm mờ hoặc hiện nhận "Đã dùng".
          </p>
        </div>
      </div>
    </div>
  );
};

export default MatchingHeadings;
