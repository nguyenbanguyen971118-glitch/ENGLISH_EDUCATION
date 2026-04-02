import React, { useState, useEffect } from 'react';
import { ChevronRight, Save, ArrowLeft, Search, Trash2, Info, CheckCircle } from 'lucide-react';

const AssignAssignmentContent = ({ maBaiTap, maKhoaHoc, tenBaiTap, onBack }) => {
  const [bankQuestions, setBankQuestions] = useState([]);
  const [selectedQuestions, setSelectedQuestions] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  
  // 1. Thêm state quản lý trạng thái hiển thị "Đã lưu"
  const [isSaved, setIsSaved] = useState(false);
  
  const TOTAL_SCORE = 10; 

  useEffect(() => {
    const mockBank = [
      { MaCauHoi: 'QS001', NoiDungCauHoi: 'Chọn từ có phần gạch chân phát âm khác: "Finished, Stopped, Worked, Waited"', LoaiCauHoi: 'Trắc nghiệm' },
      { MaCauHoi: 'QS002', NoiDungCauHoi: 'Viết lại câu sau: "I started learning English 5 years ago" -> I have...', LoaiCauHoi: 'Tự luận' },
      { MaCauHoi: 'QS003', NoiDungCauHoi: 'Điền giới từ thích hợp: "She is interested ____ listening to music."', LoaiCauHoi: 'Điền khuyết' },
      { MaCauHoi: 'QS004', NoiDungCauHoi: 'Đọc đoạn văn sau và trả lời câu hỏi về chủ đề Environment...', LoaiCauHoi: 'Đọc hiểu' },
      { MaCauHoi: 'QS005', NoiDungCauHoi: 'Sắp xếp từ thành câu: "yesterday / went / to / They / cinema / the"', LoaiCauHoi: 'Tự luận' },
      { MaCauHoi: 'QS006', NoiDungCauHoi: 'Tìm lỗi sai: "She don\'t like drinking coffee in the morning."', LoaiCauHoi: 'Trắc nghiệm' },
    ];
    setBankQuestions(mockBank);
  }, []);

  const autoCalculateScores = (list) => {
    if (list.length === 0) return [];
    const scorePerQuestion = parseFloat((TOTAL_SCORE / list.length).toFixed(2));
    return list.map((q, index) => ({
      ...q,
      DiemCuaCau: scorePerQuestion,
      ThuTu: index + 1
    }));
  };

  const addToAssignment = (q) => {
    const newSelected = [...selectedQuestions, q];
    setSelectedQuestions(autoCalculateScores(newSelected));
    setBankQuestions(bankQuestions.filter(item => item.MaCauHoi !== q.MaCauHoi));
  };

  const removeFromAssignment = (q) => {
    const newSelected = selectedQuestions.filter(item => item.MaCauHoi !== q.MaCauHoi);
    setSelectedQuestions(autoCalculateScores(newSelected));
    setBankQuestions([...bankQuestions, q]);
  };

  // 2. Cập nhật hàm handleSave để xử lý chữ "Đã lưu"
  const handleSave = () => {
    console.log("Dữ liệu gửi lên API:", selectedQuestions);
    
    // Bật trạng thái đã lưu
    setIsSaved(true);

    // Sau 2 giây thì đổi lại nút bình thường
    setTimeout(() => {
      setIsSaved(false);
    }, 2000);
  };

  return (
    <div className="container-fluid bg-light p-4 rounded min-vh-100">
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4 bg-white p-3 rounded shadow-sm">
        <button className="btn btn-outline-secondary border-0 d-flex align-items-center" onClick={onBack}>
          <ArrowLeft size={18} className="me-2"/> Quay lại
        </button>
        <div className="text-center">
          <h4 className="mb-0 fw-bold text-dark">Thiết lập nội dung</h4>
          <span className="badge bg-info text-dark">{tenBaiTap} (ID: {maBaiTap})</span>
        </div>

        {/* 3. Nút bấm biến đổi linh hoạt */}
        <button 
          className={`btn ${isSaved ? 'btn-outline-success' : 'btn-success'} px-4 d-flex align-items-center transition-all`} 
          onClick={handleSave}
          disabled={isSaved || selectedQuestions.length === 0}
        >
          {isSaved ? (
            <>
              <CheckCircle size={18} className="me-2"/> Đã lưu nội dung
            </>
          ) : (
            <>
              <Save size={18} className="me-2"/> Lưu nội dung
            </>
          )}
        </button>
      </div>

      <div className="row g-4">
        {/* CỘT TRÁI: KHO CÂU HỎI */}
        <div className="col-md-5">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-header bg-white py-3 border-0">
              <div className="input-group">
                <span className="input-group-text bg-light border-0"><Search size={16}/></span>
                <input 
                  type="text" 
                  className="form-control bg-light border-0 shadow-none" 
                  placeholder="Tìm câu hỏi trong kho..." 
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                />
              </div>
            </div>
            <div className="list-group list-group-flush overflow-auto" style={{maxHeight: '600px'}}>
              {bankQuestions.filter(q => q.NoiDungCauHoi.toLowerCase().includes(searchTerm.toLowerCase())).map(q => (
                <div key={q.MaCauHoi} className="list-group-item d-flex justify-content-between align-items-center p-3 border-bottom-0 mb-2 bg-white mx-2 rounded shadow-sm hover-shadow" style={{transition: '0.3s'}}>
                  <div>
                    <span className="badge bg-secondary mb-1 opacity-75 small">{q.LoaiCauHoi}</span>
                    <p className="mb-0 small fw-medium text-dark">{q.NoiDungCauHoi}</p>
                  </div>
                  <button className="btn btn-sm btn-outline-primary rounded-circle p-1" onClick={() => addToAssignment(q)}>
                    <ChevronRight size={20}/>
                  </button>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* CỘT PHẢI: NỘI DUNG ĐỀ */}
        <div className="col-md-7">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-header bg-primary text-white py-3 border-0 d-flex justify-content-between align-items-center">
              <h6 className="mb-0 fw-bold">NỘI DUNG ĐỀ THI ({selectedQuestions.length} câu)</h6>
              <div className="small opacity-75 d-flex align-items-center">
                <Info size={14} className="me-1"/> Điểm tự chia (Tổng = 10)
              </div>
            </div>
            <div className="table-responsive">
              <table className="table table-hover align-middle mb-0">
                <thead className="table-light">
                  <tr className="small text-muted">
                    <th className="text-center" style={{width: '60px'}}>STT</th>
                    <th>Nội dung câu hỏi</th>
                    <th className="text-center" style={{width: '100px'}}>Điểm</th>
                    <th style={{width: '60px'}}></th>
                  </tr>
                </thead>
                <tbody>
                  {selectedQuestions.map((q, index) => (
                    <tr key={q.MaCauHoi}>
                      <td className="text-center fw-bold text-secondary">{index + 1}</td>
                      <td className="small text-dark">{q.NoiDungCauHoi}</td>
                      <td className="text-center">
                        <span className="badge bg-danger-subtle text-danger px-2 py-1" style={{minWidth: '45px'}}>{q.DiemCuaCau}</span>
                      </td>
                      <td className="text-center">
                        <button className="btn btn-link text-danger p-0 hover-scale" onClick={() => removeFromAssignment(q)}>
                          <Trash2 size={18} />
                        </button>
                      </td>
                    </tr>
                  ))}
                  {selectedQuestions.length === 0 && (
                    <tr>
                      <td colSpan="4" className="text-center py-5 text-muted italic">
                        <div className="opacity-50">Chưa có câu hỏi nào được chọn.</div>
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
            {selectedQuestions.length > 0 && (
              <div className="card-footer bg-white border-0 py-3 d-flex justify-content-end align-items-center">
                <span className="text-muted me-2">Tổng điểm dự kiến:</span>
                <span className="text-danger fw-bold fs-4">
                  {Math.round(selectedQuestions.reduce((sum, q) => sum + q.DiemCuaCau, 0))} / 10
                </span>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AssignAssignmentContent;