import React, { useState, useEffect } from 'react';
import UserProfileUniversal from '../../components/UserProfileUniversal';

const ParentProfile = () => {
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    const savedUser = localStorage.getItem('user_parent');
    if (savedUser) {
      setUserData(JSON.parse(savedUser));
    } else {
      // Dữ liệu mẫu cho Phụ huynh
      const mockParent = {
        HoTen: 'Phụ huynh Nguyễn Văn A',
        Email: 'parent.a@gmail.com',
        SoDienThoai: '0944.555.666',
        DiaChi: 'Thái Bình, Việt Nam',
        Role: 'Phụ huynh'
      };
      localStorage.setItem('user_parent', JSON.stringify(mockParent));
      setUserData(mockParent);
    }
  }, []);

  const handleSave = (updatedData) => {
    setUserData(updatedData);
    localStorage.setItem('user_parent', JSON.stringify(updatedData));
    alert("Hồ sơ phụ huynh đã được lưu!");
  };

  if (!userData) return <div className="p-5 text-center">Đang tải hồ sơ phụ huynh...</div>;

  return (
    <UserProfileUniversal 
      userData={userData} 
      userRole="Phụ huynh" 
      onBack={() => window.history.back()} 
      onSave={handleSave}
      onDelete={() => alert("Chức năng xóa yêu cầu quyền Admin")}
    />
  );
};

export default ParentProfile;