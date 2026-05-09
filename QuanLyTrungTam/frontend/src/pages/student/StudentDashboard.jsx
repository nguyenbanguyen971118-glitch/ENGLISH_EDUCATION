import React, { useState, useEffect } from "react";
import { useAuth } from "../../context/AuthContext";
import StudentClasses from "../../components/StudentClasses";
import NotificationPanel from "../../components/NotificationPanel";
import BaseApi from "../../api/BaseApi";
import "../../styles/notification-badge.css";

const StudentDashboard = () => {
    const { user } = useAuth();
    const [showNotifications, setShowNotifications] = useState(false);
    const [unreadCount, setUnreadCount] = useState(0);

    // Fetch unread notification count
    useEffect(() => {
        fetchUnreadCount();
    }, []);

    const fetchUnreadCount = async () => {
        try {
            const res = await BaseApi.get('Notification/unread-count');
            console.log('Full response object:', res);
            const resData = res.data || res;
            console.log('Unread count response:', resData);
            
            // Handle multiple response formats
            let count = 0;
            if (typeof resData === 'number') {
                count = resData;
            } else if (resData && typeof resData === 'object') {
                // Check if it's ApiResponse format with data property
                if ('data' in resData && typeof resData.data === 'number') {
                    count = resData.data;
                } else if ('value' in resData && typeof resData.value === 'number') {
                    count = resData.value;
                }
            }
            
            console.log('Setting unread count to:', count);
            setUnreadCount(count);
        } catch (error) {
            console.error('Error fetching unread count:', error);
            console.error('Error details:', error.response?.data || error.message);
            setUnreadCount(0);
        }
    };

    // Refresh unread count when notification panel opens
    const handleNotificationPanelOpen = (isOpen) => {
        console.log('Notification panel toggle (student):', isOpen);
        setShowNotifications(isOpen);
        if (!isOpen) {
            // Refresh count when panel closes (after marking as read)
            fetchUnreadCount();
        }
    };

    return (
        <div className="p-4 animate__animated animate__fadeIn" style={{ fontFamily: "'Montserrat', sans-serif" }}>
            
            <div className="mb-4 d-flex justify-content-between align-items-center">
                <div>
                    <h2 className="fw-bold text-dark mb-2">Lớp học của bạn</h2>
                    <p className="text-muted mb-0">Quản lý và xem lịch học của các lớp đang học</p>
                </div>
                
                <div className="d-flex align-items-center gap-3">
                    {/* Nút thông báo hình chuông */}
                    <div 
                        className="position-relative hover-up bg-white shadow-sm border rounded-circle d-flex align-items-center justify-content-center" 
                        style={{ width: '45px', height: '45px', cursor: 'pointer' }}
                        onClick={() => handleNotificationPanelOpen(!showNotifications)}
                        title="Thông báo"
                    >
                        <i className="bi bi-bell-fill text-secondary fs-5"></i>
                        {unreadCount > 0 && (
                            <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger border border-light notification-badge" style={{fontSize: '10px'}}>
                                {unreadCount > 99 ? '99+' : unreadCount}
                            </span>
                        )}
                    </div>
                    <NotificationPanel isOpen={showNotifications} onClose={() => handleNotificationPanelOpen(false)} onMarkAsRead={fetchUnreadCount} />
                </div>
            </div>

            <StudentClasses />
        </div>
    );
};

export default StudentDashboard;
