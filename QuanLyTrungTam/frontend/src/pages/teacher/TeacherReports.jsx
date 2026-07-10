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
      // Default to "all" to show the overall teaching dashboard first
      setSelectedClassId('all');
    } catch (err) {
      console.error(err);
      setError(err.message || 'Không thể tải danh sách lớp học giảng dạy.');
    } finally {
      setLoadingClasses(false);
    }
  };

  // Load report for selected class or all classes
  const loadClassReport = async (classId) => {
    if (!classId) return;
    setLoadingReport(true);
    try {
      let res;
      if (classId === 'all') {
        res = await p0Api.reports.teacherOverview();
      } else {
        res = await p0Api.reports.teacherClassOverview(classId);
      }
      setReport(res);
    } catch (err) {
      console.error(err);
      alert(err.message || 'Không thể tải dữ liệu thống kê.');
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
      const exportUrl = selectedClassId === 'all'
        ? `${baseUrl}/reports/teacher/export`
        : `${baseUrl}/reports/teacher/classes/${selectedClassId}/export`;

      const filename = selectedClassId === 'all'
        ? 'BaoCaoTongQuanGiangDay.csv'
        : `BaoCaoLopHoc_${classes.find(c => c.id === selectedClassId)?.name || 'Export'}.csv`;

      const response = await fetch(exportUrl, {
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
      a.download = filename;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (err) {
      alert(err.message || 'Lỗi khi xuất báo cáo.');
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
          <h2 className="fw-bold text-uppercase text-primary m-0">Thống Kê Giảng Dạy</h2>
          <p className="text-muted m-0">Quản lý chuyên cần, quy mô sĩ số và tiến trình học tập của học viên</p>
        </div>
        <div className="col-12 col-md-6 d-flex gap-2 justify-content-md-end">
          <select
            className="form-select rounded-pill px-3 w-auto"
            value={selectedClassId}
            onChange={(e) => setSelectedClassId(e.target.value)}
          >
            <option value="all">Tất cả các lớp phụ trách</option>
            {classes.map((cls) => (
              <option key={cls.id} value={cls.id}>
                {cls.name}
              </option>
            ))}
          </select>
          <button
            onClick={handleExport}
            className="btn btn-outline-primary fw-bold shadow-sm"
            disabled={exporting || loadingReport || !report}
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
        </div>
      </div>

      {loadingReport ? (
        <div className="d-flex align-items-center justify-content-center p-5" style={{ minHeight: '40vh' }}>
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Đang tải dữ liệu báo cáo...</span>
          </div>
        </div>
      ) : report ? (
        <>
          {/* Top Summary Widgets */}
          <div className="row g-3 mb-4">
            {/* Widget 1 */}
            <div className="col-12 col-sm-6 col-md-4 animate__animated animate__fadeInUp" style={{ animationDelay: '50ms' }}>
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center dashboard-card-hover transition-all">
                <span className="text-muted text-uppercase fw-semibold mb-1" style={{ fontSize: '12px' }}>Sĩ Số Học Sinh</span>
                <h2 className="fw-bold text-primary m-0">{report.totalStudents}</h2>
              </div>
            </div>
            {/* Widget 2 */}
            <div className="col-12 col-sm-6 col-md-4 animate__animated animate__fadeInUp" style={{ animationDelay: '100ms' }}>
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center dashboard-card-hover transition-all">
                <span className="text-muted text-uppercase fw-semibold mb-1" style={{ fontSize: '12px' }}>
                  {selectedClassId === 'all' ? 'Số Lớp Giảng Dạy' : 'Tỷ Lệ Chuyên Cần'}
                </span>
                <h2 className={`fw-bold ${selectedClassId === 'all' ? 'text-info' : 'text-success'} m-0`}>
                  {selectedClassId === 'all' ? report.totalClasses : `${report.attendanceRate.PresentPercent}%`}
                </h2>
              </div>
            </div>
            {/* Widget 3 */}
            <div className="col-12 col-sm-6 col-md-4 animate__animated animate__fadeInUp" style={{ animationDelay: '150ms' }}>
              <div className="card border-0 shadow-sm rounded-4 p-3 bg-white text-center dashboard-card-hover transition-all">
                <span className="text-muted text-uppercase fw-semibold mb-1" style={{ fontSize: '12px' }}>
                  {selectedClassId === 'all' ? 'Tỷ Lệ Chuyên Cần Chung' : 'Tỷ Lệ Nộp Bài Tập'}
                </span>
                <h2 className={`fw-bold ${selectedClassId === 'all' ? 'text-success' : 'text-warning'} m-0`}>
                  {selectedClassId === 'all' ? `${report.attendanceRate.PresentPercent}%` : `${report.homeworkCompletionRate}%`}
                </h2>
              </div>
            </div>
          </div>

          {/* Charts Row */}
          <div className="row g-4 mb-4">
            {/* Attendance Chart */}
            <div className="col-12 col-lg-5 animate__animated animate__fadeInUp" style={{ animationDelay: '200ms' }}>
              <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white chart-card-hover transition-all">
                <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">
                  {selectedClassId === 'all' ? 'Chuyên Cần Chung' : 'Chuyên Cần Lớp'}
                </h5>
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

            {/* Score Distribution Chart */}
            <div className="col-12 col-lg-7 animate__animated animate__fadeInUp" style={{ animationDelay: '250ms' }}>
              <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white chart-card-hover transition-all">
                <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">
                  Phổ Điểm Chung
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

          {/* Bottom Table/Progress list */}
          <div className="animate__animated animate__fadeInUp" style={{ animationDelay: '300ms' }}>
            {selectedClassId === 'all' ? (
              /* Quy mô sĩ số lớp học phụ trách */
              <div className="card border-0 shadow-sm rounded-4 p-4 bg-white">
                <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Quy Mô Lớp Học Giảng Dạy</h5>
                <div className="row g-3">
                  {report.classSizes && report.classSizes.length > 0 ? (
                    report.classSizes.map((cls, idx) => {
                      const pct = cls.capacity > 0 ? Math.min(100, Math.round(cls.studentCount / cls.capacity * 100)) : 0;
                      return (
                        <div key={idx} className="col-12 col-md-6 mb-2">
                          <div className="p-3 border rounded-4 bg-light">
                            <div className="d-flex justify-content-between align-items-center mb-2">
                              <span className="fw-bold text-dark">{cls.className}</span>
                              <span className="badge bg-secondary px-3">{cls.studentCount} / {cls.capacity} Học sinh</span>
                            </div>
                            <div className="progress rounded-pill" style={{ height: '10px' }}>
                              <div 
                                className={`progress-bar rounded-pill ${pct >= 90 ? 'bg-danger' : pct >= 70 ? 'bg-warning' : 'bg-success'}`}
                                role="progressbar" 
                                style={{ width: `${pct}%` }} 
                                aria-valuenow={pct} 
                                aria-valuemin="0" 
                                aria-valuemax="100"
                              ></div>
                            </div>
                          </div>
                        </div>
                      );
                    })
                  ) : (
                    <div className="col-12 text-center p-4 text-muted">
                      Bạn chưa được phân công lớp học nào hoặc lớp trống.
                    </div>
                  )}
                </div>
              </div>
            ) : (
              /* Student Progress Table */
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
                      {report.students && report.students.map((student) => (
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
            )}
          </div>
        </>
      ) : (
        <div className="text-center p-5 text-muted">Vui lòng chọn lớp học để xem thống kê</div>
      )}
    </div>
  );
};

export default TeacherReports;
