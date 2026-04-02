import React, { useState, useEffect } from 'react';
import UserProfileUniversal from '../../components/UserProfileUniversal';

const TeacherProfile = () => {
  const [userData, setUserData] = useState(null);

  // Khởi tạo dữ liệu tạm nếu chưa có
  useEffect(() => {
    const savedUser = localStorage.getItem('user');
    
    if (savedUser) {
      setUserData(JSON.parse(savedUser));
    } else {
      // Dữ liệu giả định ban đầu (Mock Data)
      const mockData = {
        HoTen: 'Nguyễn Thị Lan Anh',
        Email: 'lananh.teacher@gmail.com',
        SoDienThoai: '0988.123.456',
        DiaChi: 'Thái Bình, Việt Nam',
        Role: 'Giảng viên',
        ChuyenMon: 'IELTS Writing & Speaking'
      };
      // Lưu vào máy để lần sau ấn vào là có luôn
      localStorage.setItem('user', JSON.stringify(mockData));
      setUserData(mockData);
    }
  }, []);

  // Chức năng Sửa & Thêm (Cập nhật lại state và bộ nhớ)
  const handleSave = (updatedData) => {
    setUserData(updatedData);
    localStorage.setItem('user', JSON.stringify(updatedData));
    alert("Đã cập nhật dữ liệu thành công!");
  };

  // Chức năng Xóa (Xóa sạch bộ nhớ và reset giao diện)
  const handleDelete = () => {
    if(window.confirm("Bạn có chắc chắn muốn xóa hồ sơ này?")) {
      localStorage.removeItem('user');
      setUserData({
        HoTen: '',
        Email: '',
        SoDienThoai: '',
        DiaChi: '',
        Role: 'Giảng viên',
        ChuyenMon: ''
      });
      alert("Đã xóa dữ liệu tạm thời.");
    }
  };

  if (!userData) return <div className="p-5 text-center">Đang khởi tạo dữ liệu...</div>;

  return (
    <UserProfileUniversal 
      userData={userData} 
      userRole={userData.Role} 
      onBack={() => window.history.back()} 
      onSave={handleSave}
      onDelete={handleDelete}
    />
  );
};

export default TeacherProfile;