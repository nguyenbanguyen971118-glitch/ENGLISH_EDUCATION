import React, { useState, useEffect } from 'react';
import UserProfileUniversal from '../../components/UserProfileUniversal';

const StudentProfile = () => {
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    const savedUser = localStorage.getItem('user_student');
    if (savedUser) {
      setUserData(JSON.parse(savedUser));
    } else {
      // Dữ liệu mẫu cho Học sinh nếu chưa có trong Local Storage
      const mockStudent = {
        HoTen: 'Nguyễn Văn Toàn',
        Email: 'toan.student@gmail.com',
        SoDienThoai: '0912.345.678',
        DiaChi: 'Thái Bình, Việt Nam',
        Role: 'Học sinh'
      };
      localStorage.setItem('user_student', JSON.stringify(mockStudent));
      setUserData(mockStudent);
    }
  }, []);

  const handleSave = (updatedData) => {
    setUserData(updatedData);
    localStorage.setItem('user_student', JSON.stringify(updatedData));
    alert("Hồ sơ học sinh đã được cập nhật!");
  };

  if (!userData) return <div className="p-5 text-center">Đang tải hồ sơ học sinh...</div>;

  return (
    <UserProfileUniversal 
      userData={userData} 
      userRole="Học sinh" 
      onBack={() => window.history.back()} 
      onSave={handleSave}
      onDelete={() => alert("Chức năng xóa yêu cầu quyền Admin")}
    />
  );
};

export default StudentProfile;