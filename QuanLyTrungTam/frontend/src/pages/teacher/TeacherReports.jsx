import React, { useEffect, useState } from 'react';
import { Doughnut, Bar } from 'react-chartjs-2';
import { Chart as ChartJS, registerables } from 'chart.js';
import { p0Api } from '../../api/p0Api';
import { getApiBaseUrl } from '../../api/BaseApi';

ChartJS.register(...registerables);

const TeacherReports = () => {
  const [classes, setClasses] = useState([]);
  const [selectedClassId, setSelectedClassId] = useState('');
  const [report, setReport] = useState(null);
  const [loadingClasses, setLoadingClasses] = useState(true);
  const [loadingReport, setLoadingReport] = useState(false);
  const [error, setError] = useState('');
  const [exporting, setExporting] = useState(false);

  // Load teacher assigned classes
  const loadClasses = async () => {
    setLoadingClasses(true);
    setError('');
    try {
      const res = await p0Api.classes.assignedToMe();
      setClasses(res || []);
      if (res && res.length > 0) {
        setSelectedClassId(res[0].id);
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Không thể tải danh sách lớp học giảng dạy.');
    } finally {
      setLoadingClasses(false);
    }
  };

  // Load report for selected class
  const loadClassReport = async (classId) => {
    if (!classId) return;
    setLoadingReport(true);
    try {
      const res = await p0Api.reports.teacherClassOverview(classId);
      setReport(res);
    } catch (err) {
      console.error(err);
      alert(err.message || 'Không thể tải dữ liệu thống kê lớp học.');
    } finally {
      setLoadingReport(false);
    }
  };

  useEffect(() => {
    loadClasses();
  }, []);

  useEffect(() => {
    if (selectedClassId) {
      loadClassReport(selectedClassId);
    }
  }, [selectedClassId]);

  const handleExport = async () => {
    if (!selectedClassId) return;
    setExporting(true);
    try {
      const userData = localStorage.getItem('user');
      const token = userData ? JSON.parse(userData)?.token : null;

      const baseUrl = getApiBaseUrl();
      const response = await fetch(`${baseUrl}/reports/teacher/classes/${selectedClassId}/export`, {
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
      a.download = `BaoCaoLopHoc_${classes.find(c => c.id === selectedClassId)?.name || 'Export'}.csv`;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (err) {
      alert(err.message || 'Lỗi khi xuất báo cáo lớp học.');
    } finally {
      setExporting(false);
    }
  };

  if (loadingClasses) {
    return (
      <div className="d-flex align-items-center justify-content-center p-5" style={{ minHeight: '60vh' }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Đang tải...</span>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger m-4 rounded-4" role="alert">
        <i className="bi bi-exclamation-triangle-fill me-2"></i>
        {error}
        <button onClick={loadClasses} className="btn btn-outline-danger btn-sm ms-3">Thử lại</button>
      </div>
    );
  }

  // Attendance Chart Data
  const attendanceChartData = {
    labels: ['Có mặt', 'Đi muộn', 'Vắng mặt', 'Có phép'],
    datasets: [
      {
        data: [
          report?.attendanceRate?.present || 0,
          report?.attendanceRate?.late || 0,
          report?.attendanceRate?.absent || 0,
          report?.attendanceRate?.excused || 0
        ],
        backgroundColor: ['#2ec4b6', '#ff9f1c', '#e71d36', '#011627'],
        borderWidth: 2,
        borderColor: '#ffffff',
      }
    ]
  };

  // Score Distribution Chart Data
  const scoreChartData = {
    labels: ['Dưới 5.0 (Yếu/Kém)', '5.0 - 7.0 (Trung bình)', '7.0 - 8.5 (Khá)', 'Trên 8.5 (Giỏi)'],
    datasets: [
      {
        label: 'Số bài nộp đạt điểm',
        data: [
          report?.scoreDistribution?.under5 || 0,
          report?.scoreDistribution?.from5To7 || 0,
          report?.scoreDistribution?.from7To85 || 0,
          report?.scoreDistribution?.above85 || 0
        ],
        backgroundColor: '#4361ee',
        borderRadius: 8,
      }
    ]
  };

  return (
    <div className="container-fluid py-4 animate__animated animate__fadeIn">
      {/* Selection Header */}
      <div className="row g-3 align-items-center mb-4 border-bottom pb-3">
        <div className="col-12 col-md-6">
          <h2 className="fw-bold text-uppercase text-primary m-0">Thống Kê Lớp Học</h2>
          <p className="text-muted m-0">Quản lý chuyên cần và tiến trình làm bài của lớp giảng dạy</p>
        </div>
        <div className="col-12 col-md-6 d-flex gap-2 justify-content-md-end">
          {classes.length > 0 ? (
            <>
              <select
                className="form-select rounded-pill px-3 w-auto"
                value={selectedClassId}
                onChange={(e) => setSelectedClassId(e.target.value)}
              >
                {classes.map((cls) => (
                  <option key={cls.id} value={cls.id}>
                    {cls.name}
                  </option>
                ))}
              </select>
              <button
                onClick={handleExport}
                className="btn btn-outline-primary fw-bold shadow-sm"
                disabled={exporting || loadingReport}
              >
                {exporting ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                    Đang xuất...
                  </>
                ) : (
                  <>
                    <i className="bi bi-download me-2"></i> Xuất báo cáo
                  </>
                )}
              </button>
            </>
          ) : (
            <span className="text-warning fw-semibold">Bạn chưa được phân công lớp học nào</span>
          )}
        </div>
      </div>

      {loadingReport ? (
        <div className="d-flex align-items-center justify-content-center p-5" style={{ minHeight: '40vh' }}>
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Đang tải dữ liệu lớp...</span>
          </div>
        </div>
      ) : report ? (
        <>
          {/* Top Summary Widgets */}
          <div className="row g-3 mb-4">
            <div className="col-12 col-md-4">
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
                <span className="text-muted text-uppercase fw-semibold mb-1">Sĩ Số Học Sinh</span>
                <h2 className="fw-bold text-primary m-0">{report.totalStudents}</h2>
              </div>
            </div>
            <div className="col-12 col-md-4">
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
                <span className="text-muted text-uppercase fw-semibold mb-1">Tỷ Lệ Đi Học Đầy Đủ</span>
                <h2 className="fw-bold text-success m-0">{report.attendanceRate.PresentPercent}%</h2>
              </div>
            </div>
            <div className="col-12 col-md-4">
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center">
                <span className="text-muted text-uppercase fw-semibold mb-1">Tỷ Lệ Nộp Bài Tập</span>
                <h2 className="fw-bold text-warning m-0">{report.homeworkCompletionRate}%</h2>
              </div>
            </div>
          </div>

          {/* Charts Row */}
          <div className="row g-4 mb-4">
            {/* Attendance of Selected Class */}
            <div className="col-12 col-lg-5">
              <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
                <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Chuyên Cần Lớp</h5>
                <div style={{ height: '280px' }} className="d-flex align-items-center justify-content-center">
                  {report.attendanceRate.total > 0 ? (
                    <Doughnut
                      data={attendanceChartData}
                      options={{
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { position: 'bottom' } }
                      }}
                    />
                  ) : (
                    <span className="text-muted">Chưa có thông tin điểm danh</span>
                  )}
                </div>
              </div>
            </div>

            {/* Score Distribution of Selected Class */}
            <div className="col-12 col-lg-7">
              <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
                <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">
                  Phổ Điểm Của Lớp
                  {report.scoreDistribution.total > 0 && (
                    <span className="badge bg-primary ms-3">ĐTB: {report.scoreDistribution.averageScore}</span>
                  )}
                </h5>
                <div style={{ height: '280px' }}>
                  {report.scoreDistribution.total > 0 ? (
                    <Bar
                      data={scoreChartData}
                      options={{
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { display: false } },
                        scales: { y: { beginAtZero: true } }
                      }}
                    />
                  ) : (
                    <div className="d-flex align-items-center justify-content-center h-100">
                      <span className="text-muted">Chưa có điểm bài tập nào được chấm</span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>

          {/* Student Progress Table */}
          <div className="card border-0 shadow-sm rounded-4 p-4 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Chi Tiết Từng Học Sinh</h5>
            <div className="table-responsive">
              <table className="table table-hover align-middle">
                <thead className="table-light">
                  <tr>
                    <th scope="col">Họ Tên Học Sinh</th>
                    <th scope="col" className="text-center">Chuyên Cần (%)</th>
                    <th scope="col" className="text-center">Điểm Trung Bình</th>
                    <th scope="col" className="text-center">Bài Tập Hoàn Thành</th>
                  </tr>
                </thead>
                <tbody>
                  {report.students.map((student) => (
                    <tr key={student.studentId}>
                      <td className="fw-bold text-dark">{student.studentName}</td>
                      <td className="text-center fw-semibold text-success">{student.attendanceRate}%</td>
                      <td className="text-center">
                        <span className={`badge ${student.averageScore >= 8.0 ? 'bg-success' : student.averageScore >= 5.0 ? 'bg-primary' : 'bg-danger'} px-3 py-2 fs-6`}>
                          {student.averageScore.toFixed(1)}
                        </span>
                      </td>
                      <td className="text-center fw-semibold">
                        {student.completedHomeworkCount} / {student.totalHomeworkCount}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </>
      ) : (
        <div className="text-center p-5 text-muted">Vui lòng chọn lớp học để xem thống kê</div>
      )}
    </div>
  );
};

export default TeacherReports;
