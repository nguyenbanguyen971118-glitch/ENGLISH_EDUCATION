

import React from 'react';

const ResultsPage = () => {
  return (
    <div style={{ backgroundColor: "#F3F4F6", minHeight: "100vh", fontFamily: "'Montserrat', sans-serif", paddingTop: "0px" }}>
      <div style={{ backgroundColor: "white", padding: "24px 48px", borderBottom: "1px solid #E5E7EB" }}>
        <h2 style={{ fontSize: "24px", fontWeight: "700", color: "#111827", marginBottom: "4px" }}>
          Kết quả bài làm
        </h2>
        <p style={{ color: "#6B7280", fontSize: "14px", margin: 0 }}>
          Màn này xuất hiện sau khi học viên đã nộp bài hoặc sau khi giáo viên chấm xong.
        </p>
      </div>

      <div style={{ padding: "32px 48px" }}>
        <div style={{ color: "#3B82F6", fontSize: "14px", fontWeight: "600", marginBottom: "8px" }}>
          KẾT QUẢ
        </div>
        <h2 style={{ fontSize: "32px", fontWeight: "700", marginBottom: "8px", color: "#111827" }}>
          Kết quả bài làm
        </h2>
        <p style={{ color: "#6B7280", marginBottom: "32px", fontSize: "16px" }}>
          Nếu bài có tự luận/việc lại câu, điểm hiện thị là điểm tạm tính và một số câu ở trạng thái chờ giáo viên chấm.
        </p>

        <div style={{ maxWidth: "1000px", margin: "0 auto" }}>
          <div style={{ display: "flex", gap: "48px" }}>
            <div style={{ flex: 1.5 }}>
              <div style={{ backgroundColor: "white", borderRadius: "24px", padding: "32px", boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1)" }}>
                <h3 style={{ fontSize: "24px", fontWeight: "700", marginBottom: "24px" }}>
                  Tổng quan kết quả
                </h3>
                <p style={{ fontSize: "14px", color: "#6B7280", marginBottom: "24px" }}>
                  Một số câu viết là/tự luận đang chờ giáo viên chấm.
                </p>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px", marginBottom: "24px" }}>
                  <div style={{ backgroundColor: "#EFF6FF", borderRadius: "12px", padding: "24px" }}>
                    <div style={{ fontSize: "28px", fontWeight: "700", color: "#2563EB", marginBottom: "4px" }}>
                      78/100
                    </div>
                    <div style={{ fontSize: "12px", color: "#64748B" }}>
                      Điểm tạm tính
                    </div>
                  </div>
                  <div style={{ backgroundColor: "#D1FAE5", borderRadius: "12px", padding: "24px" }}>
                    <div style={{ fontSize: "20px", fontWeight: "700", color: "#065F46", marginBottom: "4px" }}>
                      62
                    </div>
                    <div style={{ fontSize: "12px", color: "#065F46" }}>
                      Câu đúng
                    </div>
                  </div>
                </div>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px" }}>
                  <div style={{ backgroundColor: "#FFE4E6", borderRadius: "12px", padding: "24px" }}>
                    <div style={{ fontSize: "20px", fontWeight: "700", color: "#DC2626", marginBottom: "4px" }}>
                      18
                    </div>
                    <div style={{ fontSize: "12px", color: "#DC2626" }}>
                      Câu sai
                    </div>
                  </div>
                  <div style={{ backgroundColor: "#FFF7ED", borderRadius: "12px", padding: "24px" }}>
                    <div style={{ fontSize: "20px", fontWeight: "700", color: "#EA580C", marginBottom: "4px" }}>
                      52:14
                    </div>
                    <div style={{ fontSize: "12px", color: "#EA580C" }}>
                      Thời gian
                    </div>
                  </div>
                </div>

                <div style={{
                  border: "2px solid #E5E7EB",
                  borderRadius: "12px",
                  padding: "16px",
                  marginTop: "24px",
                  backgroundColor: "#FEF3C7"
                }}>
                  <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                    <span style={{ fontSize: "14px", fontWeight: "600", color: "#92400E" }}>
                      Trạng thái
                    </span>
                    <span style={{
                      backgroundColor: "#FCD34D",
                      color: "#92400E",
                      padding: "4px 12px",
                      borderRadius: "999px",
                      fontSize: "12px",
                      fontWeight: "600"
                    }}>
                      Chờ chấm tay
                    </span>
                  </div>
                  <p style={{ fontSize: "12px", color: "#92400E" }}>
                    Điểm cuối cùng sẽ cập nhật sau khi giáo viên chấm các câu tự luận.
                  </p>
                </div>
              </div>
            </div>

            <div style={{ flex: 1 }}>
              <div style={{ backgroundColor: "white", borderRadius: "24px", padding: "32px", boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1)" }}>
                <h3 style={{ fontSize: "24px", fontWeight: "700", marginBottom: "24px" }}>
                  Review câu sai
                </h3>

                <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                  <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0" }}>
                    <span style={{ fontSize: "14px", fontWeight: "500" }}>Câu 2</span>
                    <div style={{ display: "flex", gap: "12px", alignItems: "center" }}>
                      <span style={{
                        backgroundColor: "#FFE4E6",
                        color: "#DC2626",
                        padding: "4px 12px",
                        borderRadius: "8px",
                        fontSize: "12px",
                        fontWeight: "600"
                      }}>
                        Sai
                      </span>
                      <button style={{
                        padding: "8px 16px",
                        borderRadius: "8px",
                        border: "1px solid #D1D5DB",
                        backgroundColor: "white",
                        color: "#111827",
                        fontWeight: "600",
                        cursor: "pointer",
                        fontSize: "12px"
                      }}>
                        Xem
                      </button>
                    </div>
                  </div>

                  <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0" }}>
                    <span style={{ fontSize: "14px", fontWeight: "500" }}>Câu 5</span>
                    <div style={{ display: "flex", gap: "12px", alignItems: "center" }}>
                      <span style={{
                        backgroundColor: "#FFE4E6",
                        color: "#DC2626",
                        padding: "4px 12px",
                        borderRadius: "8px",
                        fontSize: "12px",
                        fontWeight: "600"
                      }}>
                        Sai
                      </span>
                      <button style={{
                        padding: "8px 16px",
                        borderRadius: "8px",
                        border: "1px solid #D1D5DB",
                        backgroundColor: "white",
                        color: "#111827",
                        fontWeight: "600",
                        cursor: "pointer",
                        fontSize: "12px"
                      }}>
                        Xem
                      </button>
                    </div>
                  </div>

                  <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0" }}>
                    <span style={{ fontSize: "14px", fontWeight: "500" }}>Câu 10</span>
                    <div style={{ display: "flex", gap: "12px", alignItems: "center" }}>
                      <span style={{
                        backgroundColor: "#FFE4E6",
                        color: "#DC2626",
                        padding: "4px 12px",
                        borderRadius: "8px",
                        fontSize: "12px",
                        fontWeight: "600"
                      }}>
                        Sai
                      </span>
                      <button style={{
                        padding: "8px 16px",
                        borderRadius: "8px",
                        border: "1px solid #D1D5DB",
                        backgroundColor: "white",
                        color: "#111827",
                        fontWeight: "600",
                        cursor: "pointer",
                        fontSize: "12px"
                      }}>
                        Xem
                      </button>
                    </div>
                  </div>
                </div>

                <button style={{
                  width: "100%",
                  padding: "12px 24px",
                  borderRadius: "8px",
                  border: "none",
                  backgroundColor: "#2563EB",
                  color: "white",
                  fontWeight: "600",
                  cursor: "pointer",
                  fontSize: "14px",
                  marginTop: "32px"
                }}>
                  Xem toàn bộ bài làm
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ResultsPage;

