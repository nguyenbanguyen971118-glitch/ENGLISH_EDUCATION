import React, { useState } from 'react';
import { 
  Folder, FileText, Video, Plus, Edit, Trash2, 
  Eye, Download, Search, Upload, Globe, Link as LinkIcon,
  FileCode, Music, FileSpreadsheet, File 
} from 'lucide-react';
import { useAuth } from '../../context/AuthContext'; 

const AdminStudyContent = () => {
  const { user } = useAuth(); 
  const isAdmin = user?.role === 'Admin';
  const isTeacher = user?.role === 'Giao_Vien';

  // --- QUẢN LÝ STATE ---
  const [selectedCourse, setSelectedCourse] = useState("");
  const [selectedClass, setSelectedClass] = useState("");
  const [isRefreshing, setIsRefreshing] = useState(false);
  
  const [showModal, setShowModal] = useState(false);
  const [modalMode, setModalMode] = useState('add'); 
  const [editDocData, setEditDocData] = useState(null);

  // State cho việc Xem tài liệu (Preview)
  const [previewDoc, setPreviewDoc] = useState(null);
  const [showPreview, setShowPreview] = useState(false);

  // --- DỮ LIỆU GIẢ LẬP ---
  const courses = [{ id: "KH01", ten: "IELTS Academic" }, { id: "KH02", ten: "TOEIC Target 650" }];
  const classes = [{ id: "L01", maKH: "KH01", ten: "IELTS_Sáng_T246" }, { id: "L02", maKH: "KH02", ten: "TOEIC_Tối_T357" }];
  const chapters = [
    { id: "C1", maLop: "L01", ten: "Chương 1: Vocabulary & Grammar Foundation" },
    { id: "C2", maLop: "L01", ten: "Chương 2: Listening Practice" },
  ];

  const [studyData, setStudyData] = useState([
    {
      maChuong: "C1",
      tenChuong: "Chương 1: Vocabulary & Grammar Foundation",
      taiLieu: [
        { id: 1, ten: "Slide bài giảng Unit 1.pdf", loai: "PDF", date: "21/03/2026", url: "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf" },
        { id: 2, ten: "Video hướng dẫn.mp4", loai: "Video", date: "22/03/2026", url: "https://www.w3schools.com/html/mov_bbb.mp4" }
      ]
    }
  ]);

  // --- LOGIC XỬ LÝ XEM VÀ TẢI ---
  const handleView = (doc) => {
    if (!doc.url) {
      alert("Tài liệu này chưa có đường dẫn (URL) để xem!");
      return;
    }
    setPreviewDoc(doc);
    setShowPreview(true);
  };

  const handleDownload = (doc) => {
    if (!doc.url) {
        alert("Không tìm thấy file để tải!");
        return;
    }
    const link = document.createElement('a');
    link.href = doc.url;
    link.setAttribute('download', doc.ten);
    document.body.appendChild(link);
    link.click();
    link.remove();
  };

  const getFileIcon = (loai) => {
    switch (loai) {
      case 'PDF': return <FileText size={16} className="text-danger me-2"/>;
      case 'Word': return <FileCode size={16} className="text-primary me-2"/>;
      case 'Excel': return <FileSpreadsheet size={16} className="text-success me-2"/>;
      case 'Audio': return <Music size={16} className="text-warning me-2"/>;
      case 'Video': return <Video size={16} className="text-info me-2"/>;
      default: return <File size={16} className="text-secondary me-2"/>;
    }
  };

  // --- LOGIC CRUD ---
  const handleOpenAdd = () => { setModalMode('add'); setEditDocData(null); setShowModal(true); };
  const handleOpenEdit = (doc) => { setModalMode('edit'); setEditDocData(doc); setShowModal(true); };
  const handleDelete = (chuongId, docId) => {
    if (window.confirm("Bạn có chắc chắn muốn xóa?")) {
      setStudyData(prev => prev.map(c => c.maChuong === chuongId ? { ...c, taiLieu: c.taiLieu.filter(d => d.id !== docId) } : c));
    }
  };

  const handleSaveDocument = (newDoc) => {
    if (modalMode === 'add') {
      const docToAdd = { id: Date.now(), ...newDoc, date: new Date().toLocaleDateString('en-GB') };
      setStudyData(prev => prev.map(c => c.maChuong === newDoc.maChuong ? { ...c, taiLieu: [...c.taiLieu, docToAdd] } : c));
    } else {
      setStudyData(prev => prev.map(c => ({ ...c, taiLieu: c.taiLieu.map(d => d.id === newDoc.id ? { ...d, ...newDoc } : d) })));
    }
    setShowModal(false);
  };

  return (
    <div className="container-fluid p-4 bg-light min-vh-100">
      {/* HEADER */}
      <div className="d-flex justify-content-between align-items-center mb-4 bg-white p-3 rounded shadow-sm">
        <div>
          <h4 className="fw-bold text-primary mb-1">{isAdmin ? "GIÁM SÁT HỌC LIỆU" : "QUẢN LÝ TÀI LIỆU"}</h4>
          <p className="text-muted mb-0 small text-uppercase">Hệ thống học liệu trực tuyến</p>
        </div>
        {isTeacher && (
          <button className="btn btn-primary d-flex align-items-center gap-2" onClick={handleOpenAdd}>
            <Plus size={18} /> Thêm tài liệu
          </button>
        )}
      </div>

      {/* BỘ LỌC */}
      <div className="card shadow-sm border-0 mb-4 p-3">
        <div className="row g-3">
          <div className="col-md-4">
            <select className="form-select" value={selectedCourse} onChange={(e) => {setSelectedCourse(e.target.value); setSelectedClass("");}}>
              <option value="">-- Chọn Khóa học --</option>
              {courses.map(c => <option key={c.id} value={c.id}>{c.ten}</option>)}
            </select>
          </div>
          <div className="col-md-4">
            <select className="form-select" disabled={!selectedCourse} value={selectedClass} onChange={(e) => setSelectedClass(e.target.value)}>
              <option value="">-- Chọn lớp --</option>
              {classes.filter(l => l.maKH === selectedCourse).map(l => <option key={l.id} value={l.id}>{l.ten}</option>)}
            </select>
          </div>
        </div>
      </div>

      {/* BẢNG DỮ LIỆU */}
      {selectedClass ? (
        <div className="card shadow-sm border-0">
          <table className="table table-hover align-middle mb-0">
            <thead className="table-light">
              <tr>
                <th className="ps-4">Tên tài liệu</th>
                <th>Định dạng</th>
                <th className="text-center">Hành động</th>
              </tr>
            </thead>
            <tbody>
              {studyData.map((chuong) => (
                <React.Fragment key={chuong.maChuong}>
                  <tr className="table-info border-0"><td colSpan="3" className="fw-bold ps-4"><Folder size={18} className="me-2"/>{chuong.tenChuong}</td></tr>
                  {chuong.taiLieu.map((doc) => (
                    <tr key={doc.id}>
                      <td className="ps-5">{getFileIcon(doc.loai)} {doc.ten}</td>
                      <td><span className="badge bg-light text-dark border">{doc.loai}</span></td>
                      <td className="text-center">
                        <div className="btn-group">
                          <button className="btn btn-sm btn-outline-primary" onClick={() => handleView(doc)}><Eye size={14}/></button>
                          <button className="btn btn-sm btn-outline-secondary" onClick={() => handleDownload(doc)}><Download size={14}/></button>
                          {isTeacher && (
                            <>
                              <button className="btn btn-sm btn-outline-warning" onClick={() => handleOpenEdit(doc)}><Edit size={14}/></button>
                              <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(chuong.maChuong, doc.id)}><Trash2 size={14}/></button>
                            </>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </React.Fragment>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div className="text-center py-5 bg-white rounded border border-dashed"><Search size={40} className="text-muted opacity-25 mb-2"/><p>Vui lòng chọn khóa và lớp</p></div>
      )}

      {/* MODAL THÊM/SỬA */}
      {showModal && <DocumentModal mode={modalMode} initialData={editDocData} courses={courses} classes={classes} chapters={chapters} defaultCourse={selectedCourse} defaultClass={selectedClass} onClose={() => setShowModal(false)} onSave={handleSaveDocument} />}

      {/* MODAL XEM TRƯỚC (PREVIEW) */}
      {showPreview && <PreviewModal doc={previewDoc} onClose={() => setShowPreview(false)} />}
    </div>
  );
};

// --- COMPONENT MODAL XEM TRƯỚC ---
const PreviewModal = ({ doc, onClose }) => {
  if (!doc) return null;
  const getPreviewUrl = () => {
    if (doc.loai === 'Word' || doc.loai === 'Excel') {
      return `https://docs.google.com/viewer?url=${encodeURIComponent(doc.url)}&embedded=true`;
    }
    return doc.url;
  };

  return (
    <div className="modal d-block" style={{ backgroundColor: 'rgba(0,0,0,0.8)', zIndex: 1060 }}>
      <div className="modal-dialog modal-xl modal-dialog-centered" style={{ height: '90vh' }}>
        <div className="modal-content h-100">
          <div className="modal-header bg-dark text-white py-2">
            <span className="small">Đang xem: {doc.ten}</span>
            <button type="button" className="btn-close btn-close-white" onClick={onClose}></button>
          </div>
          <div className="modal-body p-0 bg-secondary">
            {doc.loai === 'Video' ? (
              <video controls className="w-100 h-100" autoPlay><source src={doc.url} type="video/mp4" /></video>
            ) : (
              <iframe src={getPreviewUrl()} width="100%" height="100%" style={{ border: 'none' }}></iframe>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

// --- COMPONENT MODAL FORM (CHỌN 3 CẤP) ---
const DocumentModal = ({ mode, initialData, courses, classes, chapters, defaultCourse, defaultClass, onClose, onSave }) => {
  const [formData, setFormData] = useState(initialData || { maKH: defaultCourse || "", maLop: defaultClass || "", maChuong: "", ten: "", loai: "PDF", url: "" });
  const fileAccepts = { 'PDF': '.pdf', 'Word': '.doc,.docx', 'Excel': '.xls,.xlsx', 'Audio': '.mp3', 'Video': '.mp4' };

  return (
    <div className="modal d-block" style={{backgroundColor: 'rgba(0,0,0,0.5)'}}>
      <div className="modal-dialog modal-dialog-centered shadow-lg">
        <div className="modal-content border-0">
          <div className="modal-header bg-primary text-white">
            <h5 className="modal-title fw-bold">{mode === 'add' ? 'Thêm Tài Liệu' : 'Sửa Tài Liệu'}</h5>
            <button type="button" className="btn-close btn-close-white" onClick={onClose}></button>
          </div>
          <div className="modal-body p-4">
            <div className="row g-2 mb-3">
                <div className="col-md-6">
                    <select className="form-select form-select-sm" value={formData.maKH} onChange={(e) => setFormData({...formData, maKH: e.target.value, maLop: "", maChuong: ""})}>
                        <option value="">-- Khóa học --</option>
                        {courses.map(c => <option key={c.id} value={c.id}>{c.ten}</option>)}
                    </select>
                </div>
                <div className="col-md-6">
                    <select className="form-select form-select-sm" disabled={!formData.maKH} value={formData.maLop} onChange={(e) => setFormData({...formData, maLop: e.target.value, maChuong: ""})}>
                        <option value="">-- Lớp học --</option>
                        {classes.filter(l => l.maKH === formData.maKH).map(l => <option key={l.id} value={l.id}>{l.ten}</option>)}
                    </select>
                </div>
                <div className="col-12 mt-2">
                    <select className="form-select form-select-sm border-primary" disabled={!formData.maLop} value={formData.maChuong} onChange={(e) => setFormData({...formData, maChuong: e.target.value})}>
                        <option value="">-- Chọn Chương học (*) --</option>
                        {chapters.filter(ch => ch.maLop === formData.maLop).map(ch => <option key={ch.id} value={ch.id}>{ch.ten}</option>)}
                    </select>
                </div>
            </div>
            <div className="mb-3">
                <label className="form-label small fw-bold">Loại file</label>
                <div className="d-flex gap-2">{Object.keys(fileAccepts).map(t => <button key={t} className={`btn btn-sm ${formData.loai === t ? 'btn-primary' : 'btn-outline-secondary'}`} onClick={() => setFormData({...formData, loai: t})}>{t}</button>)}</div>
            </div>
            <input type="text" className="form-control mb-3" placeholder="Tên hiển thị..." value={formData.ten} onChange={(e) => setFormData({...formData, ten: e.target.value})} />
            <div className="border border-dashed p-3 text-center bg-light position-relative">
                <Upload size={20} className="text-muted"/> <span className="small d-block">Chọn file {formData.loai}</span>
                <input type="file" className="position-absolute top-0 start-0 w-100 h-100 opacity-0 cursor-pointer" accept={fileAccepts[formData.loai]} onChange={(e) => e.target.files[0] && setFormData({...formData, ten: e.target.files[0].name})} />
            </div>
          </div>
          <div className="modal-footer"><button className="btn btn-light" onClick={onClose}>Hủy</button><button className="btn btn-primary px-4" onClick={() => onSave(formData)}>Lưu</button></div>
        </div>
      </div>
    </div>
  );
};

export default AdminStudyContent;