import React, { useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import toast from 'react-hot-toast';
import Unauthorized from '../components/Unauthorized';

const PrivateRoute = ({
    children,
    allowedRoles = [],
    requiredPermissions = [],
    requireAllPermissions = true
}) => {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    const rolePath = user?.role === 'Giao_Vien'
        ? 'teacher'
        : user?.role === 'Hoc_Sinh'
            ? 'student'
            : user?.role === 'Phu_Huynh'
                ? 'parent'
                : 'admin';

    const defaultRedirect = `/${rolePath}`;

    // Kiểm tra vai trò
    const isRoleDenied = allowedRoles.length > 0 && !allowedRoles.includes(user.role);

    // Kiểm tra quyền hạn chi tiết
    const userPermissions = Array.isArray(user.permissionCodes) ? user.permissionCodes : [];
    const isPermissionDenied = requiredPermissions.length > 0 && (
        requireAllPermissions
            ? !requiredPermissions.every((permission) => userPermissions.includes(permission))
            : !requiredPermissions.some((permission) => userPermissions.includes(permission))
    );

    const isDenied = isRoleDenied || isPermissionDenied;

    useEffect(() => {
        if (isDenied) {
            toast.error('Bạn không có quyền truy cập tài nguyên này.', { id: '403-unauthorized' });
        }
    }, [isDenied]);

    if (isDenied) {
        return <Navigate to="/unauthorized" replace />;
    }

    return children;
};

export default PrivateRoute;