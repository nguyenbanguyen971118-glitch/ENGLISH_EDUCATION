
import React from 'react';

const Reading = ({ question, currentQuestion, currentSubIndex, answers, onAnswerChange }) => {
  return (
    <div style={{ display: "flex", gap: "24px", padding: "24px" }}>
      {/* Cột trái - Đoạn văn */}
      <div style={{ flex: 1 }}>
        <div style={{
          backgroundColor: "#FFFFFF",
          borderRadius: "16px",
          padding: "24px",
          boxShadow: "0 1px 3px rgba(0,0,0,0.05)"
        }}>
          <h3 style={{
            fontSize: "20px",
            fontWeight: 700,
            color: "#111827",
            marginBottom: "12px"
          }}>
            Island Biogeography Model
          </h3>
          <p style={{
            fontSize: "14px",
            color: "#6B7280",
            lineHeight: "1.8",
            whiteSpace: "pre-line"
          }}>
            {question.passage}
          </p>
        </div>
      </div>

      {/* Cột phải - Câu hỏi */}
      <div style={{ flex: 1 }}>
        <div style={{
          backgroundColor: "#FFFFFF",
          borderRadius: "16px",
          padding: "24px",
          boxShadow: "0 1px 3px rgba(0,0,0,0.05)"
        }}>
          <div style={{ marginBottom: "16px" }}>
            <span style={{
              backgroundColor: "#D1FAE5",
              color: "#047857",
              padding: "4px 12px",
              borderRadius: "6px",
              fontSize: "12px",
              fontWeight: 600
            }}>
              Đọc hiểu
            </span>
          </div>

          {question.questions.map((subQ, idx) => {
            const answer = answers[currentQuestion - (currentSubIndex || 0) + idx];
            return (
              <div key={idx} style={{ marginBottom: idx < question.questions.length - 1 ? "32px" : "0" }}>
                <h4 style={{
                  fontSize: "20px",
                  fontWeight: 700,
                  color: "#111827",
                  marginBottom: "16px"
                }}>
                  Câu {currentQuestion - (currentSubIndex || 0) + idx + 1}. {subQ.question}
                </h4>
                <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                  {subQ.options.map((option, optIdx) => {
                    const isSelected = answer === optIdx;
                    return (
                      <div
                        key={optIdx}
                        onClick={() => onAnswerChange(currentQuestion - (currentSubIndex || 0) + idx, optIdx)}
                        style={{
                          display: "flex",
                          alignItems: "center",
                          gap: "10px",
                          padding: "10px 14px",
                          border: isSelected ? "2px solid #2563EB" : "1px solid #E5E7EB",
                          borderRadius: "10px",
                          backgroundColor: isSelected ? "#EFF6FF" : "#FFFFFF",
                          cursor: "pointer",
                          transition: "all 0.2s"
                        }}
                      >
                        <div style={{
                          width: "20px",
                          height: "20px",
                          borderRadius: "50%",
                          border: `2px solid ${isSelected ? "#2563EB" : "#D1D5DB"}`,
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          backgroundColor: isSelected ? "#2563EB" : "#FFFFFF",
                          flexShrink: 0
                        }}>
                          {isSelected && (
                            <div style={{
                              width: "8px",
                              height: "8px",
                              borderRadius: "50%",
                              backgroundColor: "#FFFFFF"
                            }}></div>
                          )}
                        </div>
                        <span style={{
                          fontSize: "14px",
                          color: "#111827",
                          fontWeight: isSelected ? 600 : 400
                        }}>
                          {String.fromCharCode(65 + optIdx)}. {option}
                        </span>
                      </div>
                    );
                  })}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default Reading;
