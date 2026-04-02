import React, { useState, useEffect } from 'react';
import { Book, Plus, Edit, Trash, Settings, Save, CheckCircle } from 'lucide-react';
import AssignAssignmentContent from './AssignAssignmentContent'; 

const AdminAssignment = () => {
  const [baiTaps, setBaiTaps] = useState([]);
  const [khoaHocs, setKhoaHocs] = useState([
    { MaKhoaHoc: 'KH01', TenKhoaHoc: 'Khóa IELTS 6.5' },
    { MaKhoaHoc: 'KH02', TenKhoaHoc: 'Khóa TOEIC 700' }
  ]);
  
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({});
  const [isEdit, setIsEdit] = useState(false);

  // State quản lý trạng thái "Đã lưu"
  const [isSaved, setIsSaved] = useState(false);

  const [viewMode, setViewMode] = useState('LIST'); 
  const [selectedAssignment, setSelectedAssignment] = useState(null);

  useEffect(() => {
    setBaiTaps([
      { MaBaiTap: '1', TenBaiTap: 'Test Reading Unit 1', MaKhoaHoc: 'KH01', TenKhoaHoc: 'Khóa IELTS 6.5', ThoiGianLamBai: 45, DiemToiDa: 10, TrangThai: 1 }
    ]);
  }, []);

  const openAddModal = () => {
    setIsEdit(false);
    setIsSaved(false); // Reset trạng thái nút khi mở modal mới
    setFormData({ TenBaiTap: '', MaKhoaHoc: '', MoTa: '', LoaiBaiTap: 1, ThoiGianLamBai: 0, DiemToiDa: 10 });
    setShowModal(true);
  };

  const openEditModal = (item) => {
    setIsEdit(true);
    setIsSaved(false); // Reset trạng thái nút
    setFormData(item);
    setShowModal(true);
  };

  // Sửa hàm lưu dữ liệu để hiện chữ "Đã lưu"
  const handleSave = () => {
    console.log("Lưu dữ liệu: ", formData);
    
    // Bật trạng thái đã lưu
    setIsSaved(true);

    // Đợi 1.5 giây để Admin thấy chữ "Đã lưu" rồi mới đóng Modal
    setTimeout(() => {
      setShowModal(false);
      setIsSaved(false);
    }, 1500);
  };

  const openManageQuestions = (item) => {
    setSelectedAssignment(item);
    setViewMode('ASSIGN_CONTENT');
  };

  if (viewMode === 'ASSIGN_CONTENT') {
    return (
      <AssignAssignmentContent 
        maBaiTap={selectedAssignment.MaBaiTap} 
        maKhoaHoc={selectedAssignment.MaKhoaHoc}
        tenBaiTap={selectedAssignment.TenBaiTap}
        onBack={() => setViewMode('LIST')} 
      />
    );
  }

  return (
    <div className="container-fluid p-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h4 className="fw-bold text-primary"><Book className="me-2"/> Quản lý Bài tập gốc</h4>
        <button className="btn btn-primary" onClick={openAddModal}>
          <Plus size={18} className="me-1"/> Thêm bài tập
        </button>
      </div>

      <div className="card shadow-sm border-0">
        <table className="table table-hover align-middle mb-0">
          <thead className="table-light">
            <tr>
              <th>Tên Đề Thi/Bài Tập</th>
              <th>Thuộc Khóa Học</th>
              <th className="text-center">Thời Gian (Phút)</th>
              <th className="text-center">Điểm Tối Đa</th>
              <th className="text-center">Thao tác Admin</th>
            </tr>
          </thead>
          <tbody>
            {baiTaps.map((item) => (
              <tr key={item.MaBaiTap}>
                <td className="fw-bold text-dark">{item.TenBaiTap}</td>
                <td><span className="badge bg-secondary">{item.TenKhoaHoc}</span></td>
                <td className="text-center">{item.ThoiGianLamBai}</td>
                <td className="text-center text-danger fw-bold">{item.DiemToiDa}</td>
                <td className="text-center">
                  <div className="btn-group shadow-sm">
                    <button 
                      className="btn btn-sm btn-info text-white d-flex align-items-center" 
                      onClick={() => openManageQuestions(item)}
                    >
                      <Settings size={14} className="me-1"/> Gán Nội Dung
                    </button>
                    <button className="btn btn-sm btn-outline-warning" onClick={() => openEditModal(item)}>
                      <Edit size={14}/>
                    </button>
                    <button className="btn btn-sm btn-outline-danger">
                      <Trash size={14}/>
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal d-block" style={{background: 'rgba(0,0,0,0.5)'}}>
          <div className="modal-dialog modal-lg border-0 shadow">
            <div className="modal-content">
              <div className="modal-header bg-primary text-white">
                <h5 className="modal-title">{isEdit ? 'Sửa thông tin đề thi' : 'Tạo mới đề thi'}</h5>
                <button className="btn-close btn-close-white" onClick={() => setShowModal(false)}></button>
              </div>
              <div className="modal-body row g-3">
                <div className="col-12">
                  <label className="form-label fw-bold">Tên bài tập</label>
                  <input type="text" className="form-control" value={formData.TenBaiTap} onChange={e => setFormData({...formData, TenBaiTap: e.target.value})} />
                </div>
                <div className="col-md-6">
                  <label className="form-label fw-bold">Thuộc khóa học</label>
                  <select className="form-select" value={formData.MaKhoaHoc} onChange={e => setFormData({...formData, MaKhoaHoc: e.target.value})}>
                    <option value="">-- Chọn khóa học --</option>
                    {khoaHocs.map(k => <option key={k.MaKhoaHoc} value={k.MaKhoaHoc}>{k.TenKhoaHoc}</option>)}
                  </select>
                </div>
                <div className="col-md-3">
                  <label className="form-label fw-bold">Thời gian (Phút)</label>
                  <input type="number" className="form-control" value={formData.ThoiGianLamBai} onChange={e => setFormData({...formData, ThoiGianLamBai: Number(e.target.value)})} />
                </div>
                <div className="col-md-3">
                  <label className="form-label fw-bold">Điểm tối đa</label>
                  <input type="number" className="form-control" value={formData.DiemToiDa} onChange={e => setFormData({...formData, DiemToiDa: Number(e.target.value)})} />
                </div>
              </div>
              <div className="modal-footer">
                <button className="btn btn-light" onClick={() => setShowModal(false)}>Hủy</button>
                
                {/* NÚT BẤM BIẾN ĐỔI CHỮ ĐÃ LƯU */}
                <button 
                  className={`btn ${isSaved ? 'btn-success' : 'btn-primary'} px-4 d-flex align-items-center`} 
                  onClick={handleSave}
                  disabled={isSaved}
                >
                  {isSaved ? (
                    <>
                      <CheckCircle size={18} className="me-2"/> Đã lưu thành công!
                    </>
                  ) : (
                    <>
                      <Save size={18} className="me-2"/> Lưu thông tin
                    </>
                  )}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminAssignment;