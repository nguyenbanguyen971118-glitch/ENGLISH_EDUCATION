
import React from 'react';

const ListeningDictation = ({ question, currentQuestion, answers, onAnswerChange }) => {
  return (
    <div style={{ padding: "24px" }}>
      <div style={{ marginBottom: "20px" }}>
        <span style={{
          backgroundColor: "#DBEAFE",
          color: "#1D4ED8",
          padding: "6px 12px",
          borderRadius: "6px",
          fontSize: "12px",
          fontWeight: 600
        }}>
          Listening điền khuyết
        </span>
      </div>

      <h3 style={{
        fontSize: "20px",
        fontWeight: 700,
        color: "#111827",
        marginBottom: "8px"
      }}>
        Nghe audio và điền từ còn thiếu vào đoạn văn.
      </h3>
      <p style={{
        fontSize: "14px",
        color: "#6B7280",
        marginBottom: "24px"
      }}>
        Nghe đoạn thoại bấm, sau đó điền đúng từ/cụm từ còn thiếu vào các ô trống trong đoạn văn.
      </p>

      <div style={{ display: "flex", gap: "24px", marginBottom: "24px" }}>
        <div style={{
          flex: 1,
          backgroundColor: "#F9FAFB",
          borderRadius: "12px",
          padding: "20px",
          border: "1px solid #E5E7EB"
        }}>
          <div style={{ display: "flex", gap: "16px", alignItems: "center" }}>
            <div style={{
              width: "64px",
              height: "64px",
              backgroundColor: "#2563EB",
              borderRadius: "50%",
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              flexShrink: 0
            }}>
              <i className="bi bi-play-fill" style={{ fontSize: "32px", color: "white" }}></i>
            </div>
            <div style={{ flex: 1 }}>
              <p style={{
                fontSize: "16px",
                fontWeight: 600,
                color: "#111827",
                marginBottom: "8px"
              }}>
                {question.trackName}
              </p>
              <p style={{
                fontSize: "14px",
                color: "#6B7280",
                marginBottom: "12px"
              }}>
                {question.listeningHint}
              </p>
              <div style={{
                width: "100%",
                height: "8px",
                backgroundColor: "#E5E7EB",
                borderRadius: "4px"
              }}>
                <div style={{
                  width: "60%",
                  height: "100%",
                  backgroundColor: "#2563EB",
                  borderRadius: "4px"
                }}></div>
              </div>
            </div>
          </div>
        </div>
        {question.rules && (
          <div style={{
            backgroundColor: "#FEF3C7",
            borderRadius: "12px",
            padding: "16px",
            width: "250px",
            flexShrink: 0
          }}>
            <p style={{
              fontSize: "14px",
              fontWeight: 600,
              color: "#92400E",
              marginBottom: "8px"
            }}>
              Quy định
            </p>
            <ul style={{
              fontSize: "14px",
              color: "#6B7280",
              paddingLeft: "16px",
              margin: 0
            }}>
              {question.rules.map((rule, idx) => (
                <li key={idx} style={{ marginBottom: "4px" }}>
                  {rule}
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>

      <div style={{
        backgroundColor: "#F9FAFB",
        borderRadius: "12px",
        padding: "24px",
        border: "1px solid #E5E7EB"
      }}>
        <p style={{
          fontSize: "14px",
          fontWeight: 600,
          color: "#374151",
          marginBottom: "12px"
        }}>
          Đoạn văn cần hoàn thành
        </p>
        <p style={{
          fontSize: "16px",
          color: "#111827",
          lineHeight: "2"
        }}>
          {question.passage.split("______").map((part, idx, arr) => (
            <React.Fragment key={idx}>
              {part}
              {idx < arr.length - 1 && (
                <input
                  key={`dictation-${currentQuestion}-${idx}`}
                  type="text"
                  value={answers[`${currentQuestion}-${idx}`] || ""}
                  onChange={(e) => onAnswerChange(`${currentQuestion}-${idx}`, e.target.value)}
                  style={{
                    width: "120px",
                    padding: "6px 12px",
                    border: "1px solid #D1D5DB",
                    borderRadius: "8px",
                    fontSize: "14px",
                    margin: "0 6px"
                  }}
                />
              )}
            </React.Fragment>
          ))}
        </p>
      </div>
    </div>
  );
};

export default ListeningDictation;
