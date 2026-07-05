import React, { useEffect, useState } from 'react';
import { Doughnut, Line } from 'react-chartjs-2';
import { Chart as ChartJS, registerables } from 'chart.js';
import { p0Api } from '../../api/p0Api';
import { getApiBaseUrl } from '../../api/BaseApi';

ChartJS.register(...registerables);

const StudentReports = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [exporting, setExporting] = useState(false);

  const loadReport = async () => {
    setLoading(true);
    setError('');
    try {
      const res = await p0Api.reports.studentOverview();
      setData(res);
    } catch (err) {
      console.error(err);
      setError(err.message || 'Không thể tải dữ liệu học tập cá nhân.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadReport();
  }, []);

  const handleExport = async () => {
    setExporting(true);
    try {
      const userData = localStorage.getItem('user');
      const token = userData ? JSON.parse(userData)?.token : null;

      const baseUrl = getApiBaseUrl();
      const response = await fetch(`${baseUrl}/reports/student/export`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Yêu cầu xuất Excel thất bại.');
      }

      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `BaoCaoHocTap_${data?.studentName || 'HocSinh'}.csv`;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (err) {
      alert(err.message || 'Lỗi khi xuất phiếu báo cáo.');
    } finally {
      setExporting(false);
    }
  };

  if (loading) {
    return (
      <div className="d-flex align-items-center justify-content-center p-5" style={{ minHeight: '60vh' }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Đang tải báo cáo...</span>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger m-4 rounded-4" role="alert">
        <i className="bi bi-exclamation-triangle-fill me-2"></i>
        {error}
        <button onClick={loadReport} className="btn btn-outline-danger btn-sm ms-3">Thử lại</button>
      </div>
    );
  }

  // Attendance Chart
  const attendanceChartData = {
    labels: ['Có mặt', 'Đi muộn', 'Vắng mặt', 'Có phép'],
    datasets: [
      {
        data: [
          data?.attendanceRate?.present || 0,
          data?.attendanceRate?.late || 0,
          data?.attendanceRate?.absent || 0,
          data?.attendanceRate?.excused || 0
        ],
        backgroundColor: ['#2ec4b6', '#ff9f1c', '#e71d36', '#011627'],
        borderWidth: 2,
        borderColor: '#ffffff',
      }
    ]
  };

  // Score Trend Chart
  const scoreLabels = data?.scoreTrend?.map(t => t.assignmentName) || [];
  const scoreDataPoints = data?.scoreTrend?.map(t => t.score) || [];

  const scoreTrendChartData = {
    labels: scoreLabels,
    datasets: [
      {
        label: 'Điểm số bài tập',
        data: scoreDataPoints,
        fill: true,
        borderColor: '#ef7d00',
        backgroundColor: 'rgba(239, 125, 0, 0.1)',
        tension: 0.3,
        pointBackgroundColor: '#ef7d00',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: '#ef7d00',
        pointRadius: 5,
        pointHoverRadius: 7,
      }
    ]
  };

  // Average Score Calculation
  const validScores = scoreDataPoints.filter(s => s !== null && s !== undefined);
  const avgScore = validScores.length > 0 ? (validScores.reduce((a, b) => a + b, 0) / validScores.length).toFixed(1) : 'Chưa có';

  return (
    <div className="container py-4 animate__animated animate__fadeIn">
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
        <div>
          <h2 className="fw-bold text-uppercase text-primary m-0">Học Bạ Cá Nhân</h2>
          <p className="text-muted m-0">Xin chào, <strong className="text-dark">{data?.studentName}</strong>. Hãy xem tiến trình học tập của bạn.</p>
        </div>
        <button
          onClick={handleExport}
          className="btn btn-outline-primary fw-bold shadow-sm"
          disabled={exporting || validScores.length === 0}
        >
          {exporting ? (
            <>
              <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
              Đang xuất...
            </>
          ) : (
            <>
              <i className="bi bi-download me-2"></i> Tải học bạ
            </>
          )}
        </button>
      </div>

      {/* Widget Cards */}
      <div className="row g-3 mb-4">
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
            <span className="text-muted text-uppercase fw-semibold mb-1">Điểm Trung Bình</span>
            <h2 className="fw-bold text-primary m-0">{avgScore}</h2>
          </div>
        </div>
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
            <span className="text-muted text-uppercase fw-semibold mb-1">Tỷ Lệ Chuyên Cần</span>
            <h2 className="fw-bold text-success m-0">{data?.attendanceRate?.PresentPercent || 0}%</h2>
          </div>
        </div>
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
            <span className="text-muted text-uppercase fw-semibold mb-1">Hoàn Thành Bài Tập</span>
            <h2 className="fw-bold text-warning m-0">{data?.homeworkCompletionRate || 0}%</h2>
          </div>
        </div>
      </div>

      {/* Main Charts */}
      <div className="row g-4">
        {/* Attendance Rates */}
        <div className="col-12 col-lg-4">
          <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Tỷ Lệ Đi Học Cá Nhân</h5>
            <div style={{ height: '260px' }} className="d-flex align-items-center justify-content-center">
              {data?.attendanceRate?.total > 0 ? (
                <Doughnut
                  data={attendanceChartData}
                  options={{
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: { legend: { position: 'bottom' } }
                  }}
                />
              ) : (
                <span className="text-muted">Chưa ghi nhận điểm danh</span>
              )}
            </div>
          </div>
        </div>

        {/* Score Progress Trend */}
        <div className="col-12 col-lg-8">
          <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Tiến Trình Điểm Số Qua Các Bài Tập</h5>
            <div style={{ height: '260px' }}>
              {validScores.length > 0 ? (
                <Line
                  data={scoreTrendChartData}
                  options={{
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                      y: {
                        beginAtZero: true,
                        max: 10,
                        ticks: { stepSize: 1 }
                      }
                    }
                  }}
                />
              ) : (
                <div className="d-flex align-items-center justify-content-center h-100">
                  <span className="text-muted">Bạn chưa có bài nộp nào được chấm điểm</span>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Assignment score detail table */}
        {data?.scoreTrend?.length > 0 && (
          <div className="col-12">
            <div className="card border-0 shadow-sm rounded-4 p-4 bg-white">
              <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Lịch Sử Điểm Bài Tập</h5>
              <div className="table-responsive">
                <table className="table table-hover align-middle">
                  <thead className="table-light">
                    <tr>
                      <th scope="col">Tên Bài Tập</th>
                      <th scope="col" className="text-center">Ngày Nộp</th>
                      <th scope="col" className="text-center">Thang Điểm</th>
                      <th scope="col" className="text-center">Kết Quả Điểm Số</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.scoreTrend.map((trend, index) => (
                      <tr key={index}>
                        <td className="fw-bold text-dark">{trend.assignmentName}</td>
                        <td className="text-center text-muted">
                          {trend.submitTime ? new Date(trend.submitTime).toLocaleString('vi-VN') : 'Chưa nộp'}
                        </td>
                        <td className="text-center fw-semibold text-secondary">{trend.maxScore}</td>
                        <td className="text-center">
                          <span className={`badge ${trend.score >= 8.0 ? 'bg-success' : trend.score >= 5.0 ? 'bg-primary' : 'bg-danger'} px-3 py-2 fs-6`}>
                            {trend.score !== null ? trend.score.toFixed(1) : 'Chưa chấm'}
                          </span>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default StudentReports;
