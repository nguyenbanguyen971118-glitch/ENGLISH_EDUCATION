import React, { createContext, useState, useContext, useEffect } from 'react';
import { authApi } from '../api';
import apiClient from '../api/BaseApi';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Kiểm tra xem đã đăng nhập trước đó chưa (trong localStorage)
        const savedUser = localStorage.getItem('user');
        if (savedUser) {
            setUser(JSON.parse(savedUser));
        }
        setLoading(false);

        // Dang ky callback de refresh token khi gap 401
        apiClient.setRefreshTokenProvider(() => refreshAccessToken());
    }, []);

    const login = (userData) => {
        setUser(userData);
        localStorage.setItem('user', JSON.stringify(userData));
    };

    const logout = () => {
        setUser(null);
        localStorage.removeItem('user');
        window.location.href = '/login';
    };

    // - chuc nang: Gui request refresh token de cap new access token khi het han.
    // - nmkhue -31/3/2026
    const refreshAccessToken = async () => {
        if (!user?.sessionId || !user?.refreshToken) {
            return { success: false, message: 'Không tìm thấy refresh token' };
        }

        try {
            const result = await authApi.refresh(user.sessionId, user.refreshToken);

            if (result?.success && result?.data) {
                // Cập nhật user với token mới
                const updatedUser = {
                    ...user,
                    token: result.data.accessToken,
                    refreshToken: result.data.refreshToken || user.refreshToken,
                    sessionId: result.data.sessionId || user.sessionId
                };
                login(updatedUser);
                return { success: true, message: 'Token đã được làm mới' };
            } else {
                return { success: false, message: result?.message || 'Không thể làm mới token' };
            }
        } catch (error) {
            return { success: false, message: 'Lỗi kết nối: ' + error.message };
        }
    };

    const refreshPermissions = async () => {
        if (!user?.id) {
            return { success: false, message: 'Không tìm thấy thông tin người dùng' };
        }

        try {
            const data = await authApi.refreshPermissions(user.id);

            if (data?.success && data?.data) {
                // Update user with new permissions
                const updatedUser = {
                    ...user,
                    permissionCodes: data.data.permissionCodes
                };
                login(updatedUser);
                return { success: true, message: 'Quyền truy cập đã được cập nhật' };
            } else {
                return { success: false, message: data?.message || 'Không thể cập nhật quyền' };
            }
        } catch (error) {
            return { success: false, message: 'Lỗi kết nối: ' + error.message };
        }
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, loading, refreshPermissions, refreshAccessToken }}>
            {!loading && children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);