
import React from 'react';

const WordBank = ({ question, currentQuestion, currentSubIndex, answers, onAnswerChange }) => {
  return (
    <div style={{ padding: "24px" }}>
      <div style={{ marginBottom: "20px" }}>
        <span style={{
          backgroundColor: "#D1FAE5",
          color: "#047857",
          padding: "6px 12px",
          borderRadius: "6px",
          fontSize: "12px",
          fontWeight: 600
        }}>
          Điền từ trong danh sách
        </span>
      </div>

      <h3 style={{
        fontSize: "20px",
        fontWeight: 700,
        color: "#111827",
        marginBottom: "20px"
      }}>
        Câu {currentQuestion + 1}. Chọn từ trong danh sách để điền vào chỗ trống.
      </h3>

      <div style={{ marginBottom: "28px" }}>
        <p style={{
          fontSize: "14px",
          fontWeight: 600,
          color: "#6B7280",
          marginBottom: "12px"
        }}>
          Từ cho sẵn
        </p>
        <div style={{ display: "flex", flexWrap: "wrap", gap: "8px" }}>
          {question.wordBank.map((word, idx) => (
            <span
              key={`word-${idx}`}
              style={{
                backgroundColor: idx % 2 === 0 ? "#D1FAE5" : "#DBEAFE",
                color: idx % 2 === 0 ? "#047857" : "#1D4ED8",
                padding: "6px 14px",
                borderRadius: "8px",
                fontSize: "14px",
                fontWeight: 600
              }}
            >
              {word}
            </span>
          ))}
        </div>
      </div>

      <div style={{ display: "flex", flexDirection: "column", gap: "20px" }}>
        {question.sentences.map((sentence, idx) => {
          const qIndex = currentQuestion - (currentSubIndex || 0) + idx;
          const answer = answers[qIndex];
          return (
            <div key={idx}>
              <p style={{
                fontSize: "18px",
                color: "#111827",
                lineHeight: "1.6",
                marginBottom: "12px"
              }}>
                {idx + 1}. {sentence.text.split("______").map((part, partIdx, arr) => (
                  <React.Fragment key={partIdx}>
                    {part}
                    {partIdx < arr.length - 1 && (
                      <select
                        value={answer || ""}
                        onChange={(e) => onAnswerChange(qIndex, e.target.value)}
                        style={{
                          padding: "6px 12px",
                          border: "1px solid #D1D5DB",
                          borderRadius: "8px",
                          fontSize: "16px",
                          margin: "0 8px",
                          backgroundColor: "#FFFFFF",
                          minWidth: "140px"
                        }}
                      >
                        <option value="">Chọn từ</option>
                        {question.wordBank.map((word, wordIdx) => (
                          <option key={wordIdx} value={word}>{word}</option>
                        ))}
                      </select>
                    )}
                  </React.Fragment>
                ))}
              </p>
            </div>
          );
        })}
      </div>

      <p style={{
        fontSize: "14px",
        color: "#6B7280",
        lineHeight: "1.6",
        marginTop: "28px"
      }}>
        Thao tác: học viên click vào ô trống để chọn từ, hoặc kéo thả từ từ danh sách. Nếu mỗi từ chỉ dùng một lần, từ đã dùng sẽ được làm mờ.
      </p>
    </div>
  );
};

export default WordBank;
