import React, { useEffect, useState } from 'react';
import { Doughnut, Bar } from 'react-chartjs-2';
import { Chart as ChartJS, registerables } from 'chart.js';
import { p0Api } from '../../api/p0Api';
import { getApiBaseUrl } from '../../api/BaseApi';

ChartJS.register(...registerables);

const AdminReports = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [exporting, setExporting] = useState(false);

  const fetchReport = async () => {
    setLoading(true);
    setError('');
    try {
      const res = await p0Api.reports.adminOverview();
      setData(res);
    } catch (err) {
      console.error(err);
      setError(err.message || 'Không thể tải báo cáo thống kê.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchReport();
  }, []);

  const handleExport = async () => {
    setExporting(true);
    try {
      const userData = localStorage.getItem('user');
      const token = userData ? JSON.parse(userData)?.token : null;

      const baseUrl = getApiBaseUrl();
      const response = await fetch(`${baseUrl}/reports/admin/export`, {
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
      a.download = `BaoCaoAdmin_${new Date().toISOString().slice(0, 10)}.csv`;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (err) {
      alert(err.message || 'Lỗi khi xuất báo cáo.');
    } finally {
      setExporting(false);
    }
  };

  if (loading) {
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
        <button onClick={fetchReport} className="btn btn-outline-danger btn-sm ms-3">Thử lại</button>
      </div>
    );
  }

  // Attendance Chart Data
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

  // Score Distribution Chart Data
  const scoreChartData = {
    labels: ['Dưới 5.0 (Yếu/Kém)', '5.0 - 7.0 (Trung bình)', '7.0 - 8.5 (Khá)', 'Trên 8.5 (Giỏi)'],
    datasets: [
      {
        label: 'Số lượng bài nộp',
        data: [
          data?.scoreDistribution?.under5 || 0,
          data?.scoreDistribution?.from5To7 || 0,
          data?.scoreDistribution?.from7To85 || 0,
          data?.scoreDistribution?.above85 || 0
        ],
        backgroundColor: '#ef7d00',
        borderRadius: 8,
      }
    ]
  };

  // Class sizes Chart Data
  const classSizesLabels = data?.classSizes?.map(c => c.className) || [];
  const classSizesStudents = data?.classSizes?.map(c => c.studentCount) || [];
  const classSizesCapacity = data?.classSizes?.map(c => c.capacity) || [];

  const classChartData = {
    labels: classSizesLabels,
    datasets: [
      {
        label: 'Sĩ số hiện tại',
        data: classSizesStudents,
        backgroundColor: '#4361ee',
        borderRadius: 6,
      },
      {
        label: 'Sức chứa tối đa',
        data: classSizesCapacity,
        backgroundColor: '#ccd9ff',
        borderRadius: 6,
      }
    ]
  };

  return (
    <div className="container-fluid py-4 animate__animated animate__fadeIn">
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="fw-bold text-uppercase text-primary m-0">Báo Cáo & Thống Kê</h2>
          <p className="text-muted m-0">Tổng quan tình hình học tập và chuyên cần toàn trung tâm</p>
        </div>
        <button
          onClick={handleExport}
          className="btn btn-outline-primary fw-bold shadow-sm"
          disabled={exporting}
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

      {/* Stats Cards */}
      <div className="row g-3 mb-4">
        <div className="col-12 col-sm-6 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 d-flex flex-row align-items-center justify-content-between" style={{ background: 'linear-gradient(135deg, #eef2f7 0%, #d9e2ec 100%)' }}>
            <div>
              <h6 className="text-secondary text-uppercase fw-semibold mb-1">Tổng Học Sinh</h6>
              <h3 className="fw-bold m-0 text-dark">{data?.totalStudents || 0}</h3>
            </div>
            <div className="rounded-circle bg-white text-primary shadow-sm d-flex align-items-center justify-content-center" style={{ width: '56px', height: '56px', flexShrink: 0 }}><i className="bi bi-people-fill fs-4"></i></div>
          </div>
        </div>

        <div className="col-12 col-sm-6 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 d-flex flex-row align-items-center justify-content-between" style={{ background: 'linear-gradient(135deg, #eef2f7 0%, #d9e2ec 100%)' }}>
            <div>
              <h6 className="text-secondary text-uppercase fw-semibold mb-1">Tổng Giảng Viên</h6>
              <h3 className="fw-bold m-0 text-dark">{data?.totalTeachers || 0}</h3>
            </div>
            <div className="rounded-circle bg-white text-success shadow-sm d-flex align-items-center justify-content-center" style={{ width: '56px', height: '56px', flexShrink: 0 }}><i className="bi bi-person-workspace fs-4"></i></div>
          </div>
        </div>

        <div className="col-12 col-sm-6 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 d-flex flex-row align-items-center justify-content-between" style={{ background: 'linear-gradient(135deg, #eef2f7 0%, #d9e2ec 100%)' }}>
            <div>
              <h6 className="text-secondary text-uppercase fw-semibold mb-1">Số Lớp Học</h6>
              <h3 className="fw-bold m-0 text-dark">{data?.totalClasses || 0}</h3>
            </div>
            <div className="rounded-circle bg-white text-warning shadow-sm d-flex align-items-center justify-content-center" style={{ width: '56px', height: '56px', flexShrink: 0 }}><i className="bi bi-building-fill fs-4"></i></div>
          </div>
        </div>

        <div className="col-12 col-sm-6 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 d-flex flex-row align-items-center justify-content-between" style={{ background: 'linear-gradient(135deg, #eef2f7 0%, #d9e2ec 100%)' }}>
            <div>
              <h6 className="text-secondary text-uppercase fw-semibold mb-1">Số Khóa Học</h6>
              <h3 className="fw-bold m-0 text-dark">{data?.totalCourses || 0}</h3>
            </div>
            <div className="rounded-circle bg-white text-danger shadow-sm d-flex align-items-center justify-content-center" style={{ width: '56px', height: '56px', flexShrink: 0 }}><i className="bi bi-book-half fs-4"></i></div>
          </div>
        </div>
      </div>

      {/* Charts Grid */}
      <div className="row g-4">
        {/* Attendance Rates */}
        <div className="col-12 col-lg-5">
          <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Tỷ Lệ Điểm Danh Chung</h5>
            <div className="d-flex align-items-center justify-content-center p-3" style={{ height: '300px' }}>
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
                <span className="text-muted">Chưa có dữ liệu chuyên cần</span>
              )}
            </div>
          </div>
        </div>

        {/* Score Distribution */}
        <div className="col-12 col-lg-7">
          <div className="card border-0 shadow-sm rounded-4 p-4 h-100 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">
              Phổ Điểm Toàn Trung Tâm
              {data?.scoreDistribution?.total > 0 && (
                <span className="badge bg-primary ms-3 fs-6">ĐTB: {data.scoreDistribution.averageScore}</span>
              )}
            </h5>
            <div style={{ height: '300px' }}>
              {data?.scoreDistribution?.total > 0 ? (
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
                  <span className="text-muted">Chưa có dữ liệu điểm số</span>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Class Sizes Column */}
        <div className="col-12">
          <div className="card border-0 shadow-sm rounded-4 p-4 bg-white">
            <h5 className="fw-bold mb-3 text-secondary border-bottom pb-2">Thống Kê Quy Mô Lớp Học (Học Sinh / Lớp)</h5>
            <div style={{ height: '350px' }}>
              {data?.classSizes?.length > 0 ? (
                <Bar
                  data={classChartData}
                  options={{
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: { y: { beginAtZero: true } }
                  }}
                />
              ) : (
                <div className="d-flex align-items-center justify-content-center h-100">
                  <span className="text-muted">Chưa có dữ liệu lớp học</span>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminReports;
